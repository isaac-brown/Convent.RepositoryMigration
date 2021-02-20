// <copyright file="IJournal.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Used to store and retrieve scripts migrated.
    /// </summary>
    public interface IJournal
    {
        /// <summary>
        /// Provides the names of scripts that have already been executed.
        /// </summary>
        /// <returns>A collection of script names.</returns>
        IReadOnlyCollection<string> GetExecutedScripts();

        /// <summary>
        /// Marks a script as having been executed.
        /// </summary>
        /// <param name="migrationScript">The script which was executed.</param>
        void MarkScriptAsExecuted(MigrationScript migrationScript);
    }
}
