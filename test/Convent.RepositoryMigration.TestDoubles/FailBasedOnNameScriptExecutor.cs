// <copyright file="FailBasedOnNameScriptExecutor.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.TestDoubles
{
    using System;
    using Convent.RepositoryMigration.Core;

    /// <summary>
    /// Implementation of the <see cref="IScriptExecutor"/> interface which fails when executing
    /// a script name which has contains the string "fail".
    /// </summary>
    public class FailBasedOnNameScriptExecutor : IScriptExecutor
    {
        /// <inheritdoc/>
        public void Execute(MigrationScript script)
        {
            if (script.Name.Contains("fail", StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception($"Failed because script name: {script.Name} contains \"fail\"");
            }
        }
    }
}
