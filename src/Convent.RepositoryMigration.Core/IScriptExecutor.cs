// <copyright file="IScriptExecutor.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Core
{
    /// <summary>
    /// Represents something which can execute a <see cref="MigrationScript"/>.
    /// </summary>
    public interface IScriptExecutor
    {
        /// <summary>
        /// Executes the given <paramref name="script"/>.
        /// </summary>
        /// <param name="script">The script to execute.</param>
        void Execute(MigrationScript script);
    }
}
