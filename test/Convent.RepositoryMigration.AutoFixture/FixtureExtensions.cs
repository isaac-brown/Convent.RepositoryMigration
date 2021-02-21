// <copyright file="FixtureExtensions.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.AutoFixture
{
    using System.Collections.Generic;
    using global::AutoFixture;

    /// <summary>
    /// Extension methods for <see cref="IFixture"/> instances.
    /// </summary>
    public static class FixtureExtensions
    {
        /// <summary>
        /// Customizes the given <paramref name="fixture"/> with fake implementations of interfaces in the domain.
        /// </summary>
        /// <param name="fixture">The fixture to customize.</param>
        /// <returns>The current instance for chaining.</returns>
        public static IFixture WithFakes(this IFixture fixture)
        {
            return fixture.Customize(new DomainFixtureCustomization())
                          .Customize(new MELTFixtureCustomization());
        }

        /// <summary>
        /// Injects a collection of <typeparamref name="T"/> which contains <paramref name="instance"/> as the only item.
        /// </summary>
        /// <param name="fixture">The fixture to inject into.</param>
        /// <param name="instance">The instance to inject.</param>
        /// <typeparam name="T">The type to be injected.</typeparam>
        /// <returns>The current instance for chaining.</returns>
        public static IFixture WithSingle<T>(this IFixture fixture, T instance)
        {
            fixture.Inject<IEnumerable<T>>(new[] { instance });
            return fixture;
        }
    }
}
