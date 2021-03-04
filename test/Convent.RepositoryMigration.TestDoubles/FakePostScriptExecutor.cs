// <copyright file="FakePostScriptExecutor.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.TestDoubles
{
    using System.Threading;
    using System.Threading.Tasks;
    using Convent.RepositoryMigration.Core;

    /// <summary>
    /// Fake implementation of <see cref="IPostScriptExecutor"/>.
    /// </summary>
    public class FakePostScriptExecutor : IPostScriptExecutor
    {
        /// <inheritdoc/>
        public Task ExecuteAsync(MigrationScript migrationScript, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
