// <copyright file="MELTFixtureCustomization.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.AutoFixture
{
    using global::AutoFixture;
    using MELT;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Customizations specific to <see cref="MELT"/>.
    /// </summary>
    internal class MELTFixtureCustomization : ICustomization
    {
        /// <inheritdoc/>
        public void Customize(IFixture fixture)
        {
            var testLoggerFactory = TestLoggerFactory.Create();

            fixture.Register<ILoggerFactory>(() => testLoggerFactory);
            fixture.Register<ITestLoggerFactory>(() => testLoggerFactory);

            fixture.Customizations.Add(new TestLoggerRelay());
        }
    }
}
