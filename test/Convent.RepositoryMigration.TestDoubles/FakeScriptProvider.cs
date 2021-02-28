// <copyright file="FakeScriptProvider.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.TestDoubles
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Bogus;
    using Convent.RepositoryMigration.Core;

    /// <summary>
    /// Fake implementation of the <see cref="IScriptProvider"/> interface.
    /// </summary>
    public class FakeScriptProvider : IScriptProvider
    {
        private static readonly Faker<MigrationScript> ScriptFaker = new();

        static FakeScriptProvider()
        {
            ScriptFaker.CustomInstantiator(f =>
            {
                return new MigrationScript(Name: f.System.FileName(), Contents: f.Random.Words());
            });
        }

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<MigrationScript>> GetScriptsAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(GetScripts());
        }

        private static IReadOnlyCollection<MigrationScript> GetScripts()
        {
            return ScriptFaker.Generate(count: 3);
        }
    }
}
