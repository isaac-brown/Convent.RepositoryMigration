// <copyright file="FakeJournal.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.TestDoubles
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Convent.RepositoryMigration.Core;

    /// <summary>
    /// Fake implementation of the <see cref="IJournal"/> interface.
    /// </summary>
    public class FakeJournal : IJournal
    {
        private readonly List<string> executedScripts = new ();

        /// <inheritdoc/>
        public Task MarkScriptAsExecutedAsync(MigrationScript migrationScript, CancellationToken cancellationToken = default)
        {
            this.MarkScriptAsExecuted(migrationScript);
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<string>> GetExecutedScriptsAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(this.GetExecutedScripts());
        }

        private IReadOnlyCollection<string> GetExecutedScripts()
        {
            return this.executedScripts;
        }

        private void MarkScriptAsExecuted(MigrationScript migrationScript)
        {
            this.executedScripts.Add(migrationScript.Name);
        }
    }
}
