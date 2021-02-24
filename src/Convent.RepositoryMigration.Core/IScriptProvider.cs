// <copyright file="IScriptProvider.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Core
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides <see cref="MigrationScript"/> objects to interested parties.
    /// </summary>
    public interface IScriptProvider
    {
        /// <summary>
        /// Asynchronously gets all scripts that should be executed.
        /// </summary>
        /// <param name="cancellationToken">Used to cancel fetching of scripts.</param>
        /// <returns>A collection of <see cref="MigrationScript"/> objects.</returns>
        Task<IReadOnlyCollection<MigrationScript>> GetScriptsAsync(CancellationToken cancellationToken = default);
    }
}
