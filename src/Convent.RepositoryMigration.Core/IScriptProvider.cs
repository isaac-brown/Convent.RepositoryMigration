// <copyright file="IScriptProvider.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides <see cref="MigrationScript"/> objects to interested parties.
    /// </summary>
    public interface IScriptProvider
    {
        /// <summary>
        /// Gets all scripts that should be executed.
        /// </summary>
        /// <returns>A collection of <see cref="MigrationScript"/> objects.</returns>
        IReadOnlyCollection<MigrationScript> GetScripts();
    }
}
