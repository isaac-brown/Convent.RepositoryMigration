// <copyright file="MigrationEngine.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Orchestrats the migration process.
    /// </summary>
    [ImmutableObject(immutable: true)]
    public class MigrationEngine
    {
        private readonly MigrationConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationEngine"/> class.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        public MigrationEngine(MigrationConfiguration configuration)
        {
            this.configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Performs the migration.
        /// </summary>
        /// <returns>A new <see cref="MigrationResult"/> instance.</returns>
        public MigrationResult PerformMigration()
        {
            var scriptsPreviouslyExecuted = this.configuration.Journal.GetExecutedScripts();
            var scriptsProvided = this.configuration.ScriptProviders.SelectMany(provider => provider.GetScripts())
                                                                    .OrderBy(script => script.Name);
            var scriptsToExecute = scriptsProvided.Where(script => !scriptsPreviouslyExecuted.Contains(script.Name));

            var scriptsExecuted = new List<MigrationScript>();

            try
            {
                foreach (var migrationScript in scriptsToExecute)
                {
                    this.configuration.ScriptExecutor.Execute(migrationScript);
                    scriptsExecuted.Add(migrationScript);
                    this.configuration.Journal.MarkScriptAsExecuted(migrationScript);
                }
            }
            catch (Exception exception)
            {
                return new MigrationResult(scriptsExecuted, exception);
            }

            return new MigrationResult(scriptsExecuted);
        }
    }
}
