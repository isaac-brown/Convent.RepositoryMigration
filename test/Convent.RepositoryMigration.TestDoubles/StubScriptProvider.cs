// <copyright file="StubScriptProvider.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.TestDoubles
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Bogus;
    using Convent.RepositoryMigration.Core;

    /// <summary>
    /// Stub implementation of the <see cref="IScriptProvider"/> interface.
    /// </summary>
    public class StubScriptProvider : IScriptProvider
    {
        private readonly IEnumerable<MigrationScript> migrationScripts;

        private readonly Faker faker = new ();

        /// <summary>
        /// Initializes a new instance of the <see cref="StubScriptProvider"/> class.
        /// </summary>
        /// <param name="migrationScriptNames">The names of the scripts which will be provided by this instance.</param>
        public StubScriptProvider(IEnumerable<string> migrationScriptNames)
        {
            this.migrationScripts = migrationScriptNames.Select(name => new MigrationScript(name, this.faker.Random.Words()));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StubScriptProvider"/> class.
        /// </summary>
        /// <param name="migrationScripts">The scripts which will be provided by this instance.</param>
        public StubScriptProvider(IEnumerable<MigrationScript> migrationScripts)
        {
            this.migrationScripts = migrationScripts;
        }

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<MigrationScript>> GetScriptsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(this.GetScripts());
        }

        private IReadOnlyCollection<MigrationScript> GetScripts()
        {
            return this.migrationScripts.ToImmutableList();
        }
    }
}
