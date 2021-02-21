using System.Linq;
using AutoFixture;
using Convent.RepositoryMigration.AutoFixture;
using Xunit;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using MELT;

namespace Convent.RepositoryMigration.Core.Tests.Engine.Output
{
    /// <summary>
    /// Unit tests for the <see cref="MigrationEngine"/> which focus specifically on logging output.
    /// </summary>
    public class MigrationEngineLogging
    {
        [Fact]
        public void Given_a_collection_of_scripts_which_all_succeed_When_PerformMigration_is_called_Then_should_have_logged_message_for_each_script_executed()
        {
            // Arrange.
            IFixture fixture = new Fixture().WithFakes();
            var loggerFactory = fixture.Freeze<ITestLoggerFactory>();

            var sut = fixture.Create<MigrationEngine>();

            // Act.
            var migrationResult = sut.PerformMigration();

            // Assert.
            var expectedScriptNames = migrationResult.ScriptsExecuted.Select(script => $"Executing script {script.Name}");
            loggerFactory.Sink.LogEntries.Where(logEntry => logEntry.LogLevel == LogLevel.Information)
                                         .Select(logEntry => logEntry.Message)
                                         .Should()
                                         .BeEquivalentTo(expectedScriptNames);
        }
    }
}
