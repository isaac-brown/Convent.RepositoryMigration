// <copyright file="GitCommitPostScriptExecutor.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Powershell
{
    using System;
    using System.Management.Automation;
    using System.Management.Automation.Runspaces;
    using System.Threading;
    using System.Threading.Tasks;
    using Convent.RepositoryMigration.Core;
    using Microsoft.Extensions.Logging;
    using Microsoft.PowerShell;

    /// <summary>
    /// An <see cref="IPostScriptExecutor"/> which commits all changes in a git repo.
    /// </summary>
    public class GitCommitPostScriptExecutor : IPostScriptExecutor
    {
        private readonly ILogger<GitCommitPostScriptExecutor> logger;
        private readonly GitOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitCommitPostScriptExecutor"/> class.
        /// </summary>
        /// <param name="logger">The logger to output messages to.</param>
        /// <param name="options">The options to use.</param>
        public GitCommitPostScriptExecutor(ILogger<GitCommitPostScriptExecutor> logger, GitOptions options)
        {
            this.options = options;
            this.logger = logger;
        }

        /// <inheritdoc/>
        public async Task ExecuteAsync(MigrationScript script, CancellationToken cancellationToken = default)
        {
            this.logger.LogInformation("Commiting staged and un-staged files.");

            InitialSessionState initialSessionState = InitialSessionState.CreateDefault();
            initialSessionState.ExecutionPolicy = ExecutionPolicy.Unrestricted;
            using var powershell = PowerShell.Create(initialSessionState);

            powershell.Runspace.SessionStateProxy.Path.SetLocation(this.options.TargetDirectory);

            powershell.AddScript("git status");
            powershell.AddScript("git stage *");
            powershell.AddScript("git status");
            powershell.AddScript($"git commit -m 'chore: apply migration script `{script.Name}`'");

            PSDataCollection<object> input = new();
            PSDataCollection<object> output = new();

            this.AddLogging(powershell, output);

            await powershell.InvokeAsync(input, output)
                            .WithCancellation(cancellationToken);
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

        private void AddLogging(PowerShell powershell, PSDataCollection<object> output)
        {
            // Add logging to all streams.
            powershell.Streams.Information.DataAdded += this.CreateEventHandler<InformationRecord>();
            powershell.Streams.Debug.DataAdded += this.CreateEventHandler<DebugRecord>();
            powershell.Streams.Error.DataAdded += this.CreateEventHandler<ErrorRecord>();
            powershell.Streams.Verbose.DataAdded += this.CreateEventHandler<VerboseRecord>();
            powershell.Streams.Warning.DataAdded += this.CreateEventHandler<WarningRecord>();
            powershell.Streams.Progress.DataAdded += this.CreateEventHandler<ProgressRecord>();

            // Add logging to output.
            output.DataAdded += this.CreateEventHandler<object>();
        }

        private EventHandler<DataAddedEventArgs> CreateEventHandler<TRecord>()
        {
            return (object? sender, DataAddedEventArgs e) =>
            {
                if (sender is PSDataCollection<TRecord> records)
                {
                    var record = records[e.Index];

                    this.logger.Log(logLevel: MapToLogLevel(record), "{record}", record);
                }
            };
        }
    }
}
