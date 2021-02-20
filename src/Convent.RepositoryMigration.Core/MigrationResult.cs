// <copyright file="MigrationResult.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents the result of performing a migration, whether it was successfull or not.
    /// </summary>
    public class MigrationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationResult"/> class.
        /// </summary>
        /// <param name="scriptsExecuted">The scripts which were executed.</param>
        /// <param name="exception">The exception which caused the failure.</param>
        public MigrationResult(IEnumerable<MigrationScript> scriptsExecuted, Exception? exception = null)
        {
            this.ScriptsExecuted = scriptsExecuted;
            this.Exception = exception;
        }

        /// <summary>
        /// Gets the collection of <see cref="MigrationScript"/> objects which were executed during the migration.
        /// </summary>
        public IEnumerable<MigrationScript> ScriptsExecuted { get; }

        /// <summary>
        /// Gets a value indicating whether the migration was successful.
        /// </summary>
        public bool HasSucceeded => this.Exception is null;

        /// <summary>
        /// Gets the exception which caused the migration to fail.
        /// </summary>
        /// <remarks>
        /// Null indicates that no exception was thrown and the migration was successful.
        /// </remarks>
        public Exception? Exception { get; }
    }
}
