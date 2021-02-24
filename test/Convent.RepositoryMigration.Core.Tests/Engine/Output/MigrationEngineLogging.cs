// <copyright file="MigrationEngineLogging.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Core.Tests.Engine.Output
{
    using System.Linq;
    using System.Threading.Tasks;
    using Convent.RepositoryMigration.AutoFixture;
    using FluentAssertions;
    using global::AutoFixture;
    using MELT;
    using Microsoft.Extensions.Logging;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="MigrationEngine"/> which focus specifically on logging output.
    /// </summary>
    public class MigrationEngineLogging
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements must be documented

        [Fact]
        public async Task Given_a_collection_of_scripts_which_all_succeed_When_PerformMigrationAsync_is_called_Then_should_have_logged_message_for_each_script_executed()
        {
            // Arrange.
            IFixture fixture = new Fixture().WithFakes();
            var loggerFactory = fixture.Freeze<ITestLoggerFactory>();

            var sut = fixture.Create<MigrationEngine>();

            // Act.
            var migrationResult = await sut.PerformMigrationAsync();

            // Assert.
            var expectedScriptNames = migrationResult.ScriptsExecuted.Select(script => $"Executing script {script.Name}");
            loggerFactory.Sink.LogEntries.Where(logEntry => logEntry.LogLevel == LogLevel.Information)
                                         .Select(logEntry => logEntry.Message)
                                         .Should()
                                         .BeEquivalentTo(expectedScriptNames);
        }
    }
}
