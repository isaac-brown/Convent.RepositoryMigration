// <copyright file="DomainFixtureCustomization.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.AutoFixture
{
    using Convent.RepositoryMigration.Core;
    using Convent.RepositoryMigration.TestDoubles;
    using global::AutoFixture;
    using global::AutoFixture.Kernel;

    /// <summary>
    /// Domain specific customization for the <see cref="Convent.RepositoryMigration"/> namespace.
    /// By default fake implementations of all interfaces will be provided.
    /// </summary>
    public class DomainFixtureCustomization : ICustomization
    {
        /// <inheritdoc/>
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(
                new TypeRelay(
                    from: typeof(IJournal),
                    to: typeof(FakeJournal)));

            fixture.Customizations.Add(
                new TypeRelay(
                    typeof(IScriptExecutor),
                    typeof(FakeScriptExecutor)));

            fixture.Customizations.Add(
                new TypeRelay(
                    typeof(IScriptProvider),
                    typeof(FakeScriptProvider)));
        }
    }
}
