// <copyright file="ExecutionCountMockPostScriptExecutor.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.TestDoubles
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Convent.RepositoryMigration.Core;

    /// <summary>
    /// A mock implementation of <see cref="IPostScriptExecutor"/> which can verify the number of times it's
    /// <see cref="ExecuteAsync"/> method was called.
    /// </summary>
    public class ExecutionCountMockPostScriptExecutor : IPostScriptExecutor
    {
        private readonly int expectedExecutionCount;
        private int actualExecutionCount = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecutionCountMockPostScriptExecutor"/> class.
        /// </summary>
        /// <param name="expectedExecutionCount">The number of times execute is expected ot be called.</param>
        public ExecutionCountMockPostScriptExecutor(int expectedExecutionCount)
        {
            this.expectedExecutionCount = expectedExecutionCount;
        }

        /// <inheritdoc/>
        public Task ExecuteAsync(MigrationScript migrationScript, CancellationToken cancellationToken = default)
        {
            this.actualExecutionCount++;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Verifies that <see cref="ExecuteAsync"/> was called the requisite number of times.
        /// </summary>
        /// <exception cref="Exception">
        /// Throws if <see cref="ExecuteAsync"/> was not called the requisite number of times.
        /// </exception>
        public void Verify()
        {
            if (this.actualExecutionCount != this.expectedExecutionCount)
            {
                throw new Exception($"Expected the method ExecuteAsync to have been invoked {this.expectedExecutionCount} time(s)"
                + $" but it was actually invoked {this.actualExecutionCount} time(s).");
            }
        }
    }
}
