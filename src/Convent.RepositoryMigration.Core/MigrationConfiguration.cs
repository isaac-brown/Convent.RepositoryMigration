// <copyright file="MigrationConfiguration.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides configuration for <see cref="MigrationEngine"/>.
    /// </summary>
    public class MigrationConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationConfiguration"/> class.
        /// </summary>
        /// <param name="scriptProviders">The script providers to use.</param>
        /// <param name="journal">The journal to use.</param>
        /// <param name="scriptExecutor">The script executor to use.</param>
        public MigrationConfiguration(
            IReadOnlyCollection<IScriptProvider> scriptProviders,
            IJournal journal,
            IScriptExecutor scriptExecutor)
        {
            this.Journal = journal;
            this.ScriptExecutor = scriptExecutor;
            this.ScriptProviders = scriptProviders;
        }

        /// <summary>
        /// Gets the script providers.
        /// </summary>
        public IReadOnlyCollection<IScriptProvider> ScriptProviders { get; }

        /// <summary>
        /// Gets the journal.
        /// </summary>
        public IJournal Journal { get; }

        /// <summary>
        /// Gets the script executor.
        /// </summary>
        public IScriptExecutor ScriptExecutor { get; }
    }
}
