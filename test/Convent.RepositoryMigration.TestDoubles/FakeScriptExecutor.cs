// <copyright file="FakeScriptExecutor.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.TestDoubles
{
    using Convent.RepositoryMigration.Core;

    /// <summary>
    /// Fake implementation of the <see cref="IScriptExecutor"/> interface.
    /// </summary>
    public class FakeScriptExecutor : IScriptExecutor
    {
        /// <inheritdoc/>
        public void Execute(MigrationScript script)
        {
            // No-op.
        }
    }
}
