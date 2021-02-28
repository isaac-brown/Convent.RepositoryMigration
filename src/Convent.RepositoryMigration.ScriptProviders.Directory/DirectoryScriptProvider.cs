// <copyright file="DirectoryScriptProvider.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.ScriptProviders
{
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Convent.RepositoryMigration.Core;

    /// <summary>
    /// Implementation of <see cref="IScriptProvider"/> which provides scripts from a file system.
    /// </summary>
    public class DirectoryScriptProvider : IScriptProvider
    {
        private readonly string directory;
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryScriptProvider"/> class.
        /// </summary>
        /// <param name="options">The options to use.</param>
        /// <param name="fileSystem">The filesystem to use.</param>
        public DirectoryScriptProvider(ScriptProviderOptions options, IFileSystem fileSystem)
        {
            this.directory = options.ScriptsDirectory;
            this.fileSystem = fileSystem;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<MigrationScript>> GetScriptsAsync(CancellationToken cancellationToken = default)
        {
            var filePaths = this.fileSystem.Directory.EnumerateFiles(this.directory);

            var getScriptTasks = filePaths.Select(async (filePath) =>
            {
                var relativePath = Path.GetRelativePath(this.directory, filePath);
                var fileContents = await this.fileSystem.File.ReadAllTextAsync(filePath, cancellationToken);
                var script = new MigrationScript(Name: relativePath, Contents: fileContents);
                return script;
            });

            return await Task.WhenAll(getScriptTasks);
        }
    }
}
