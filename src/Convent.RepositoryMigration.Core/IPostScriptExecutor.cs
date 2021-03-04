// <copyright file="IPostScriptExecutor.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Core
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an action which will be run after each <see cref="IScriptExecutor"/>.
    /// </summary>
    /// <remarks>
    /// Useful for defining actions such as commiting changes generated by a script to source control.
    /// </remarks>
    public interface IPostScriptExecutor
    {
        /// <summary>
        /// Asynchronously executes a post-script execution action.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        /// <param name="migrationScript">The migration script which has already been executed.</param>
        /// <param name="cancellationToken">Used to cancel execution.</param>
        Task ExecuteAsync(MigrationScript migrationScript, CancellationToken cancellationToken = default);
    }
}
