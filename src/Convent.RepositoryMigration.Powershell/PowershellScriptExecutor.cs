// <copyright file="PowershellScriptExecutor.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Powershell
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;
    using System.Threading;
    using System.Threading.Tasks;
    using Convent.RepositoryMigration.Core;
    using Microsoft.Extensions.Logging;
    using Microsoft.PowerShell;

    /// <summary>
    /// Implementation of <see cref="IScriptExecutor"/> which runs powershell scripts.
    /// </summary>
    internal class PowershellScriptExecutor : IScriptExecutor
    {
        private readonly IEnumerable<IScriptPreprocessor> scriptPreprocessors;
        private readonly IFileSystem fileSystem;
        private readonly ILogger<PowershellScriptExecutor> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PowershellScriptExecutor"/> class.
        /// </summary>
        /// <param name="scriptPreprocessors">The preprocessors to use.</param>
        /// <param name="fileSystem">The file system to use.</param>
        /// <param name="logger">The logger to write messages to.</param>
        public PowershellScriptExecutor(
            IEnumerable<IScriptPreprocessor> scriptPreprocessors,
            IFileSystem fileSystem,
            ILogger<PowershellScriptExecutor> logger)
        {
            this.scriptPreprocessors = scriptPreprocessors;
            this.fileSystem = fileSystem;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task ExecuteAsync(MigrationScript script, CancellationToken cancellationToken = default)
        {
            InitialSessionState initialSessionState = InitialSessionState.CreateDefault();
            initialSessionState.ExecutionPolicy = ExecutionPolicy.Unrestricted;
            using var powershell = PowerShell.Create(initialSessionState);

            var workingDirectory = Path.Join(Path.GetTempPath(), Guid.NewGuid().ToString());
            this.logger.LogDebug("Creating workingDirectory: {workingDirectory}", workingDirectory);
            this.fileSystem.Directory.CreateDirectory(workingDirectory);

            foreach (var preprocessor in this.scriptPreprocessors.OrderBy(p => p.Order))
            {
                script = await preprocessor.ProcessAsync(script, cancellationToken);
            }

            try
            {
                string scriptPath = Path.Join(workingDirectory, script.Name);
                this.logger.LogDebug("Creating script file: {scriptPath}", scriptPath);
                await this.fileSystem.File.WriteAllTextAsync(scriptPath, script.Contents, cancellationToken);

                powershell.Runspace.SessionStateProxy.Path.SetLocation(workingDirectory);

                this.logger.LogDebug(
                    "powershell.Runspace.SessionStateProxy.Path.CurrentLocation: {CurrentLocation}",
                    powershell.Runspace.SessionStateProxy.Path.CurrentLocation);

                string dotSourcedCommand = $".\\{script.Name}";
                powershell.AddScript(dotSourcedCommand);

                PSDataCollection<object> input = new();
                PSDataCollection<object> output = new();

                this.AddLogging(script, powershell, output);

                await powershell.InvokeAsync(input, output)
                                .WithCancellation(cancellationToken);
            }
            finally
            {
                this.logger.LogDebug("Deleting workingDirectory: {workingDirectory}", workingDirectory);
                Directory.Delete(path: workingDirectory, recursive: true);
            }
        }

        private static LogLevel MapToLogLevel(object? obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

#pragma warning disable SA1503 // Braces should not be omitted.
            if (obj is ErrorRecord) return LogLevel.Error;
            if (obj is InformationRecord) return LogLevel.Information;
            if (obj is DebugRecord || obj is VerboseRecord || obj is object) return LogLevel.Debug;
            if (obj is WarningRecord) return LogLevel.Warning;
#pragma warning restore SA1503 // Braces should not be omitted.

            throw new NotSupportedException($"The type {obj?.GetType()} is not supported");
        }

        private void AddLogging(MigrationScript script, PowerShell powershell, PSDataCollection<object> output)
        {
            // Add logging to all streams.
            powershell.Streams.Information.DataAdded += this.CreateEventHandler<InformationRecord>(script);
            powershell.Streams.Debug.DataAdded += this.CreateEventHandler<DebugRecord>(script);
            powershell.Streams.Error.DataAdded += this.CreateEventHandler<ErrorRecord>(script);
            powershell.Streams.Verbose.DataAdded += this.CreateEventHandler<VerboseRecord>(script);
            powershell.Streams.Warning.DataAdded += this.CreateEventHandler<WarningRecord>(script);
            powershell.Streams.Progress.DataAdded += this.CreateEventHandler<ProgressRecord>(script);

            // Add logging to output.
            output.DataAdded += this.CreateEventHandler<object>(script);
        }

        private EventHandler<DataAddedEventArgs> CreateEventHandler<TRecord>(MigrationScript script)
        {
            return (object? sender, DataAddedEventArgs e) =>
            {
                if (sender is PSDataCollection<TRecord> records)
                {
                    var record = records[e.Index];

                    this.logger.Log(logLevel: MapToLogLevel(record), "{Name} | {record}", script.Name, record);
                }
            };
        }
    }
}
