// <copyright file="StubJournal.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.TestDoubles
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using Convent.RepositoryMigration.Core;

    /// <summary>
    /// Stub implementation of the <see cref="IJournal"/> interface.
    /// </summary>
    public class StubJournal : IJournal
    {
        private readonly IEnumerable<MigrationScript> migrationScripts;

        /// <summary>
        /// Initializes a new instance of the <see cref="StubJournal"/> class.
        /// </summary>
        /// <param name="migrationScripts">The collection of scripts that will be used for responses.</param>
        public StubJournal(IEnumerable<MigrationScript> migrationScripts)
        {
            this.migrationScripts = migrationScripts;
        }

        /// <inheritdoc/>
        public IReadOnlyCollection<string> GetExecutedScripts()
        {
            return this.migrationScripts.Select(script => script.Name)
                                        .ToImmutableList();
        }

        /// <inheritdoc/>
        public void MarkScriptAsExecuted(MigrationScript migrationScript)
        {
            throw new System.NotImplementedException();
        }
    }
}
