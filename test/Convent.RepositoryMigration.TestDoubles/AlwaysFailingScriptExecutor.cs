// <copyright file="AlwaysFailingScriptExecutor.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.TestDoubles
{
    using System;
    using Convent.RepositoryMigration.Core;

    /// <summary>
    /// An implementation of <see cref="IScriptExecutor"/> which will always throw an exception when trying to execute a script.
    /// </summary>
    public class AlwaysFailingScriptExecutor : IScriptExecutor
    {
        private readonly Exception exceptionToThrow;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlwaysFailingScriptExecutor"/> class.
        /// </summary>
        /// <param name="exceptionToThrow">The exception to throw.</param>
        public AlwaysFailingScriptExecutor(Exception exceptionToThrow)
        {
            this.exceptionToThrow = exceptionToThrow;
        }

        /// <inheritdoc/>
        public void Execute(MigrationScript script)
        {
            throw this.exceptionToThrow;
        }
    }
}
