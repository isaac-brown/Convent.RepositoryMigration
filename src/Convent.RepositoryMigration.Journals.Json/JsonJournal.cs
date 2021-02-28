// <copyright file="JsonJournal.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Journals
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Convent.RepositoryMigration.Core;

    /// <summary>
    /// Implementation of <see cref="IJournal"/> which uses a json file for persistence.
    /// </summary>
    public class JsonJournal : IJournal
    {
        private readonly string fileName;
        private readonly IFileSystem fileSystem;
        private readonly JsonSerializerOptions jsonReadOptions = new(defaults: JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true,
        };

        private readonly JsonSerializerOptions jsonWriteOptions = new(defaults: JsonSerializerDefaults.Web)
        {
            WriteIndented = true,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonJournal"/> class.
        /// </summary>
        /// <param name="options">The options to use.</param>
        /// <param name="fileSystem">The filesystem to use.</param>
        public JsonJournal(JournalOptions options, IFileSystem fileSystem)
        {
            this.fileName = options.JournalFilePath;
            this.fileSystem = fileSystem;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<string>> GetExecutedScriptsAsync(CancellationToken cancellationToken = default)
        {
            await EnsureJournalExistsAsync(this.fileName, this.fileSystem, cancellationToken);
            var journalEntries = await this.GetJournalEntriesAsync(cancellationToken);

            return journalEntries.Select(journalEntry => journalEntry.ScriptName).ToList();
        }

        /// <inheritdoc/>
        public async Task MarkScriptAsExecutedAsync(MigrationScript migrationScript, CancellationToken cancellationToken = default)
        {
            await EnsureJournalExistsAsync(this.fileName, this.fileSystem, cancellationToken);

            var journalEntries = await this.GetJournalEntriesAsync(cancellationToken);
            var journalEntryToAdd = MyJournalEntry.FromMigrationScript(migrationScript);
            var newJournalEntries = journalEntries.Append(journalEntryToAdd);

            var json = JsonSerializer.Serialize(newJournalEntries, options: this.jsonWriteOptions);

            await this.fileSystem.File.WriteAllTextAsync(this.fileName, json, cancellationToken);
        }

        private static async Task EnsureJournalExistsAsync(string fileName, IFileSystem fileSystem, CancellationToken cancellationToken)
        {
            if (fileSystem.File.Exists(fileName))
            {
                var contents = await fileSystem.File.ReadAllTextAsync(fileName, cancellationToken);

                if (string.IsNullOrWhiteSpace(contents))
                {
                    fileSystem.File.WriteAllText(fileName, "[]");
                }

                return;
            }

            fileSystem.File.Create(fileName).Dispose();
            fileSystem.File.WriteAllText(fileName, "[]");
        }

        private async Task<IReadOnlyCollection<MyJournalEntry>> GetJournalEntriesAsync(CancellationToken cancellationToken)
        {
            byte[] utf8Json = await this.fileSystem.File.ReadAllBytesAsync(this.fileName, cancellationToken);
            var journalEntries = JsonSerializer.Deserialize<List<MyJournalEntry>>(utf8Json, options: this.jsonReadOptions);

            if (journalEntries is null)
            {
                throw new Exception($"Unable to parse journal file ({this.fileName}) to valid JSON."
                                   + " Expected to get an instance of MyJournalEntry[], instead got <null>.");
            }

            return journalEntries;
        }

        private class MyJournalEntry
        {
            /// <summary>
            /// Gets or sets the name of the script.
            /// </summary>
            public string ScriptName { get; set; } = string.Empty;

            /// <summary>
            /// Creates a new <see cref="MyJournalEntry"/> instance based on the given <paramref name="migrationScript"/>.
            /// </summary>
            /// <param name="migrationScript">The migration script to use.</param>
            /// <returns>A new <see cref="MyJournalEntry"/> instance.</returns>
            public static MyJournalEntry FromMigrationScript(MigrationScript migrationScript)
            {
                return new MyJournalEntry
                {
                    ScriptName = Path.GetFileName(migrationScript.Name),
                };
            }
        }
    }
}
