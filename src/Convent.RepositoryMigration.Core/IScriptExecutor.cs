// <copyright file="IScriptExecutor.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Core
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents something which can execute a <see cref="MigrationScript"/>.
    /// </summary>
    public interface IScriptExecutor
    {
        /// <summary>
        /// Asynchronously executes the given <paramref name="script"/>.
        /// </summary>
        /// <param name="script">The script to execute.</param>
        /// <param name="cancellationToken">The cancellation to token to use.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task ExecuteAsync(MigrationScript script, CancellationToken cancellationToken = default);
    }
}
