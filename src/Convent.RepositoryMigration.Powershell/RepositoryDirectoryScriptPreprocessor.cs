// <copyright file="RepositoryDirectoryScriptPreprocessor.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Powershell
{
    using System.Threading;
    using System.Threading.Tasks;
    using Convent.RepositoryMigration.Core;

    /// <summary>
    /// A preprocessor which injects content to ensure a script starts in a given location.
    /// </summary>
    public class RepositoryDirectoryScriptPreprocessor : IScriptPreprocessor
    {
        private readonly string startingDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryDirectoryScriptPreprocessor"/> class.
        /// </summary>
        /// <param name="startingDirectory">The directory which scripts will be started in.</param>
        public RepositoryDirectoryScriptPreprocessor(string startingDirectory)
        {
            this.startingDirectory = startingDirectory;
        }

        /// <inheritdoc/>
        public int Order => 0;

        /// <inheritdoc/>
        public Task<MigrationScript> ProcessAsync(MigrationScript migrationScript, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(migrationScript with {
                Contents = $"Set-Location '{this.startingDirectory}'\n" + migrationScript.Contents,
            });
        }
    }
}
