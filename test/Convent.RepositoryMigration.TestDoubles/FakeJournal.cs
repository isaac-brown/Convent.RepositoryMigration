// <copyright file="FakeJournal.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.TestDoubles
{
    using System.Collections.Generic;
    using Convent.RepositoryMigration.Core;

    /// <summary>
    /// Fake implementation of the <see cref="IJournal"/> interface.
    /// </summary>
    public class FakeJournal : IJournal
    {
        private readonly List<string> executedScripts = new ();

        /// <inheritdoc/>
        public IReadOnlyCollection<string> GetExecutedScripts()
        {
            return this.executedScripts;
        }

        /// <inheritdoc/>
        public void MarkScriptAsExecuted(MigrationScript migrationScript)
        {
            this.executedScripts.Add(migrationScript.Name);
        }
    }
}
