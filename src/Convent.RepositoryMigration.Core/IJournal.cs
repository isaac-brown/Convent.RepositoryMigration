// <copyright file="IJournal.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Core
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Used to store and retrieve scripts migrated.
    /// </summary>
    public interface IJournal
    {
        /// <summary>
        /// Provides the names of scripts that have already been executed.
        /// </summary>
        /// <param name="cancellationToken">Used to cancel fetching.</param>
        /// <returns>A collection of script names.</returns>
        Task<IReadOnlyCollection<string>> GetExecutedScriptsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously arks a script as having been executed.
        /// </summary>
        /// <param name="migrationScript">The script which was executed.</param>
        /// <param name="cancellationToken">Used to cancel execution of the script.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task MarkScriptAsExecutedAsync(MigrationScript migrationScript, CancellationToken cancellationToken = default);
    }
}
