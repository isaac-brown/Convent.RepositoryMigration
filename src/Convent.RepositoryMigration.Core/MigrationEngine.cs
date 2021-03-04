// <copyright file="MigrationEngine.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Orchestrats the migration process.
    /// </summary>
    [ImmutableObject(immutable: true)]
    public class MigrationEngine
    {
        private readonly MigrationConfiguration configuration;
        private readonly ILogger<MigrationEngine> logger;
        private readonly ISubject<MigrationStatus> migrationStatuses = new BehaviorSubject<MigrationStatus>(MigrationStatus.New);

        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationEngine"/> class.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        /// <param name="logger">The logger to write messages to.</param>
        public MigrationEngine(MigrationConfiguration configuration, ILogger<MigrationEngine> logger)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.logger = logger;
        }

        /// <summary>
        /// Gets an observable stream of <see cref="MigrationStatus"/>.
        /// </summary>
        public IObservable<MigrationStatus> MigrationStatuses => this.migrationStatuses;

        /// <summary>
        /// Asynchronously performs the migration.
        /// </summary>
        /// <param name="cancellationToken">Used to cancel the migration.</param>
        /// <returns>A new <see cref="MigrationResult"/> instance.</returns>
        public async Task<MigrationResult> PerformMigrationAsync(CancellationToken cancellationToken = default)
        {
            this.migrationStatuses.OnNext(MigrationStatus.Running);

            var scriptsPreviouslyExecuted = await this.configuration.Journal.GetExecutedScriptsAsync(cancellationToken);

            // Get all scripts from all providers, flatten them and then order by the script name.
            var provideScriptTasks = this.configuration.ScriptProviders.Select(provider => provider.GetScriptsAsync(cancellationToken));
            var providedScripts = (await Task.WhenAll(provideScriptTasks)).SelectMany(scripts => scripts)
                                                                          .OrderBy(script => script.Name);

            var scriptsToExecute = providedScripts.Where(script => !scriptsPreviouslyExecuted.Contains(script.Name));

            var scriptsExecuted = new List<MigrationScript>();

            try
            {
                foreach (var migrationScript in scriptsToExecute)
                {
                    this.logger.LogInformation("Executing script {Name}", migrationScript.Name);
                    await this.configuration.ScriptExecutor.ExecuteAsync(migrationScript, cancellationToken);
                    scriptsExecuted.Add(migrationScript);
                    await this.configuration.Journal.MarkScriptAsExecutedAsync(migrationScript, cancellationToken);
                    await this.configuration.PostScriptExecutor.ExecuteAsync(migrationScript, cancellationToken);
                }
            }
            catch (Exception exception)
            {
                this.migrationStatuses.OnNext(MigrationStatus.Failed);
                this.logger.LogError(exception, "Failed to execute scripts");
                return new MigrationResult(scriptsExecuted, exception);
            }

            this.migrationStatuses.OnNext(MigrationStatus.Succeeded);
            return new MigrationResult(scriptsExecuted);
        }
    }
}
