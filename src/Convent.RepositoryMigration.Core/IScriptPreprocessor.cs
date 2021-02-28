// <copyright file="IScriptPreprocessor.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Core
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Used to modify a <see cref="MigrationScript"/>, usually before execution.
    /// </summary>
    public interface IScriptPreprocessor
    {
        /// <summary>
        /// Gets a number indicating which order this preprocessor should run.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Performs processing step(s) on the given script.
        /// </summary>
        /// <param name="migrationScript">The script to apply processing to.</param>
        /// <param name="cancellationToken">Used to cancel the operation.</param>
        /// <returns>A new <see cref="MigrationScript"/> with any processing applied.</returns>
        Task<MigrationScript> ProcessAsync(MigrationScript migrationScript, CancellationToken cancellationToken = default);
    }
}
