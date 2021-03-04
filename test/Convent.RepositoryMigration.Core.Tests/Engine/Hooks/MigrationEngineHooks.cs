// <copyright file="MigrationEngineHooks.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Core.Tests.Engine.Hooks
{
    using Convent.RepositoryMigration.AutoFixture;
    using Convent.RepositoryMigration.TestDoubles;
    using global::AutoFixture;
    using Xunit;

    /// <summary>
    /// Unit tests to ensure that lifecycle hooks provided by <see cref="MigrationEngine"/> are invoked correctly.
    /// </summary>
    public class MigrationEngineHooks
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements must be documented

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public async System.Threading.Tasks.Task Given_postScriptExecution_is_defined_When_Migration_is_performed_Then_should_be_invoked_same_number_of_times_as_scripts(
            int countScripts)
        {
            // Arrange.
            var fixture = new Fixture().WithFakes();
            var scriptProvider = new StubScriptProvider(fixture.CreateMany<MigrationScript>(countScripts));
            fixture.WithSingle<IScriptProvider>(scriptProvider);
            ExecutionCountMockPostScriptExecutor mockPostScriptExecutor = new(expectedExecutionCount: countScripts);
            fixture.Inject<IPostScriptExecutor>(mockPostScriptExecutor);
            var sut = fixture.Create<MigrationEngine>();

            // Act.
            await sut.PerformMigrationAsync();

            // Assert.
            mockPostScriptExecutor.Verify();
        }
    }
}
