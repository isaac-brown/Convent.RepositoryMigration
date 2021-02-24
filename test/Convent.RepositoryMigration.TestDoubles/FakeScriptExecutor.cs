// <copyright file="FakeScriptExecutor.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.TestDoubles
{
    using System.Threading;
    using System.Threading.Tasks;
    using Convent.RepositoryMigration.Core;

    /// <summary>
    /// Fake implementation of the <see cref="IScriptExecutor"/> interface.
    /// </summary>
    public class FakeScriptExecutor : IScriptExecutor
    {
        /// <inheritdoc/>
        public Task ExecuteAsync(MigrationScript script, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
