// <copyright file="MigrationEngineObservables.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Core.Tests.Engine.Output
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using Convent.RepositoryMigration.AutoFixture;
    using Convent.RepositoryMigration.TestDoubles;
    using FluentAssertions;
    using global::AutoFixture;
    using Microsoft.Reactive.Testing;
    using Xunit;

    /// <summary>
    /// Unit tests for the <see cref="MigrationEngine"/> which focus specifically on output from observables.
    /// </summary>
    public class MigrationEngineObservables : ReactiveTest
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements must be documented

        [Fact]
        public void Given_engine_has_not_started_When_subscribing_to_MigrationStatuses_Then_status_should_be_New()
        {
            // Arrange.
            IFixture fixture = new Fixture().WithFakes();

            var sut = fixture.Create<MigrationEngine>();

            List<MigrationStatus> actualStatuses = new();

            using (sut.MigrationStatuses.Subscribe(status => actualStatuses.Add(status)))
            {
                // Assert.
                actualStatuses.Should()
                              .HaveCount(1).And
                              .Contain(MigrationStatus.New);
            }
        }

        [Fact]
        public async Task Given_a_valid_configuration_When_engine_starts_performing_the_migration_Then_one_migration_started_event_should_be_published()
        {
            // Arrange.
            IFixture fixture = new Fixture().WithFakes();

            var sut = fixture.Create<MigrationEngine>();
            var statuses = new List<MigrationStatus>();

            using (sut.MigrationStatuses.Subscribe(onNext: value => statuses.Add(value)))
            {
                // Act.
                await sut.PerformMigrationAsync();

                // Assert.
                statuses.Should()
                        .Contain(MigrationStatus.Running);
            }
        }

        [Fact]
        public async Task Given_provided_scripts_fail_When_engine_performs_the_migration_Then_a_migration_failed_event_should_be_published()
        {
            // Arrange.
            IFixture fixture = new Fixture().WithFakes();

            Exception expectedException = new("Something went wrong!");
            fixture.Inject<IScriptExecutor>(new AlwaysFailingScriptExecutor(expectedException));

            var sut = fixture.Create<MigrationEngine>();

            var statuses = new List<MigrationStatus>();

            using (sut.MigrationStatuses.Subscribe(onNext: value => statuses.Add(value)))
            {
                // Act.
                await sut.PerformMigrationAsync();

                // Assert.
                statuses.Should()
                        .EndWith(MigrationStatus.Failed);
            }
        }

        [Fact]
        public async Task Given_all_scripts_succeed_When_engine_performs_the_migration_Then_a_migration_succeeded_event_should_be_published()
        {
            // Arrange.
            IFixture fixture = new Fixture().WithFakes();

            var sut = fixture.Create<MigrationEngine>();

            var statuses = new List<MigrationStatus>();

            using (sut.MigrationStatuses.Subscribe(onNext: value => statuses.Add(value)))
            {
                // Act.
                await sut.PerformMigrationAsync();

                // Assert.
                statuses.Should()
                        .EndWith(MigrationStatus.Succeeded);
            }
        }
    }
}
