// <copyright file="MigrationStatus.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Core
{
    /// <summary>
    /// Represents the status of a <see cref="MigrationEngine"/>.
    /// </summary>
    public sealed record MigrationStatus
    {
        private MigrationStatus(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the name of the <see cref="MigrationStatus"/>.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a status which represents that the engine in new, and has not been started.
        /// </summary>
        public static MigrationStatus New => new(nameof(New));

        /// <summary>
        /// Gets a status which represents that the engine is running.
        /// </summary>
        public static MigrationStatus Running => new(nameof(Running));

        /// <summary>
        /// Gets a status which represents that the engine has failed.
        /// </summary>
        public static MigrationStatus Failed => new(nameof(Failed));

        /// <summary>
        /// Gets a status which represents that the engine has succeeded.
        /// </summary>
        public static MigrationStatus Succeeded => new(nameof(Succeeded));
    }
}
