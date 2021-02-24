// <copyright file="MigrationEngineTests.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Core.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Convent.RepositoryMigration.AutoFixture;
    using Convent.RepositoryMigration.TestDoubles;
    using FluentAssertions;
    using FluentAssertions.Execution;
    using global::AutoFixture;
    using Microsoft.Extensions.Logging.Abstractions;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="MigrationEngine"/> class.
    /// </summary>
    public class MigrationEngineTests
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements must be documented

        [Fact]
        public void Given_configuration_is_null_When_ctor_is_invoked_Then_should_throw_an_ArgumentNullException()
        {
            // Arrange.
            MigrationConfiguration configuration = null!;

            // Act.
            Action constructInstance = () => _ = new MigrationEngine(configuration, new NullLogger<MigrationEngine>());

            // Assert.
            constructInstance.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Given_configuration_has_no_script_providers_When_PerformMigrationAsync_is_called_Then_result_should_be_success_with_no_scripts_run()
        {
            // Arrange.
            IFixture fixture = new Fixture().WithFakes();

            fixture.Inject(Enumerable.Empty<IScriptProvider>());

            MigrationEngine sut = fixture.Create<MigrationEngine>();

            // Act.
            MigrationResult actualResult = await sut.PerformMigrationAsync();

            // Assert.
            using (new AssertionScope())
            {
                actualResult.HasSucceeded.Should().BeTrue();
                actualResult.ScriptsExecuted.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task Given_configuration_has_one_or_more_script_providers_When_PerformMigrationAsync_is_called_Then_result_and_journal_should_contain_scripts_executed()
        {
            // Arrange.
            IFixture fixture = new Fixture().WithFakes();

            var firstSetOfMigrationScripts = fixture.CreateMany<MigrationScript>();
            var secondSetOfMigrationScripts = fixture.CreateMany<MigrationScript>();

            var scriptProviders = new[]
            {
                new StubScriptProvider(firstSetOfMigrationScripts),
                new StubScriptProvider(secondSetOfMigrationScripts),
            };

            fixture.Inject<IEnumerable<IScriptProvider>>(scriptProviders);

            var journal = fixture.Freeze<IJournal>();
            (await journal.GetExecutedScriptsAsync())
                          .Should()
                          .BeEmpty();

            MigrationEngine sut = fixture.Create<MigrationEngine>();

            // Act.
            var actualResult = await sut.PerformMigrationAsync();

            // Assert.
            var expectedScriptsExecuted = firstSetOfMigrationScripts.Concat(secondSetOfMigrationScripts);
            var expectedJournalEntries = expectedScriptsExecuted.Select(script => script.Name);

            using (new AssertionScope())
            {
                actualResult.ScriptsExecuted.Should()
                                            .BeEquivalentTo(expectedScriptsExecuted);

                journal.GetExecutedScriptsAsync()
                       .Should()
                       .BeEquivalentTo(expectedJournalEntries);
            }
        }

        [Fact]
        public async Task Given_journal_contains_all_scripts_provided_When_PerformMigrationAsync_is_called_Then_executed_scripts_should_be_empty()
        {
            // Arrange.
            IFixture fixture = new Fixture().WithFakes();

            var migrationScripts = fixture.CreateMany<MigrationScript>();

            var scriptProviders = new[]
            {
                new StubScriptProvider(migrationScripts),
            };

            fixture.Inject<IEnumerable<IScriptProvider>>(scriptProviders);

            IJournal journal = new StubJournal(migrationScripts);
            fixture.Inject<IJournal>(journal);

            MigrationEngine sut = fixture.Create<MigrationEngine>();

            // Act.
            var actualResult = await sut.PerformMigrationAsync();

            // Assert.
            actualResult.ScriptsExecuted.Should()
                                        .BeEmpty();
        }

        [Fact]
        public async Task Given_a_script_which_fails_to_execute_When_PerformMigrationAsync_is_called_Then_result_should_have_failed_with_expected_exception()
        {
            // Arrange.
            IFixture fixture = new Fixture().WithFakes();

            Exception expectedException = new ("Something went wrong!");
            fixture.Inject<IScriptExecutor>(new AlwaysFailingScriptExecutor(expectedException));

            var sut = fixture.Create<MigrationEngine>();

            // Act.
            var actualResult = await sut.PerformMigrationAsync();

            // Assert.
            using (new AssertionScope())
            {
                actualResult.HasSucceeded.Should()
                                         .BeFalse();
                actualResult.Exception.Should()
                                      .Be(expectedException);
            }
        }

        [Theory]
        [InlineData(new[] { "001-pass", "002-fail" }, new[] { "001-pass" })]
        [InlineData(new[] { "001-pass", "003-pass", "002-fail" }, new[] { "001-pass" })]
        public async Task Given_a_some_scripts_which_succeed_before_failing_When_PerformMigrationAsync_is_called_Then_executed_scripts_should_contain_scripts_which_succeeded(string[] providedScripts, string[] expectedScripts)
        {
            // Arrange.
            IFixture fixture = new Fixture().WithFakes()
                                            .WithSingle<IScriptProvider>(new StubScriptProvider(providedScripts));

            fixture.Inject<IScriptExecutor>(new FailBasedOnNameScriptExecutor());

            var sut = fixture.Create<MigrationEngine>();

            // Act.
            var actualResult = await sut.PerformMigrationAsync();

            // Assert.
            actualResult.ScriptsExecuted.Select(script => script.Name)
                                        .Should()
                                        .BeEquivalentTo(expectedScripts);
        }
    }
}
