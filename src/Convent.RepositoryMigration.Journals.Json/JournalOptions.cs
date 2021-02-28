// <copyright file="JournalOptions.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Journals
{
    using System;
    using System.IO;

    /// <summary>
    /// Options for journaling.
    /// </summary>
    public class JournalOptions
    {
        /// <summary>
        /// Gets or sets the directory to which migrations will be applied.
        /// </summary>
        public string BaseDirectory { get; set; } = AppContext.BaseDirectory;

        /// <summary>
        /// Gets or sets the name of the journal file.
        /// Default is "journal.json".
        /// </summary>
        public string JournalFileName { get; set; } = "journal.json";

        /// <summary>
        /// Gets the full file path to the journal file.
        /// </summary>
        public string JournalFilePath => Path.Join(this.BaseDirectory, this.JournalFileName);
    }
}
