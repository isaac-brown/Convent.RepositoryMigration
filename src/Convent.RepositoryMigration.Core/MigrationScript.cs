// <copyright file="MigrationScript.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Core
{
    /// <summary>
    /// Represents a single script which can be run.
    /// </summary>
    public class MigrationScript
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationScript"/> class.
        /// </summary>
        /// <param name="name">The name of the script.</param>
        /// <param name="contents">The contents of the script.</param>
        public MigrationScript(string name, string contents)
        {
            this.Name = name;
            this.Contents = contents;
        }

        /// <summary>
        /// Gets the name of the script.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the contents of the script.
        /// </summary>
        public string Contents { get; }
    }
}
