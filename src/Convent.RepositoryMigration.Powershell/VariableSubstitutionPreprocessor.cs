// <copyright file="VariableSubstitutionPreprocessor.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Powershell
{
    using System.Threading;
    using System.Threading.Tasks;
    using Convent.RepositoryMigration.Core;

    /// <summary>
    /// Replaces variables with values.
    /// </summary>
    public class VariableSubstitutionPreprocessor : IScriptPreprocessor
    {
        private readonly ScriptVariables scriptVariables;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableSubstitutionPreprocessor"/> class.
        /// </summary>
        /// <param name="scriptVariables">The variables to use.</param>
        public VariableSubstitutionPreprocessor(ScriptVariables scriptVariables)
        {
            this.scriptVariables = scriptVariables;
        }

        /// <inheritdoc/>
        public int Order => 100;

        /// <inheritdoc/>
        public Task<MigrationScript> ProcessAsync(MigrationScript migrationScript, CancellationToken cancellationToken = default)
        {
            var newContents = migrationScript.Contents;

            foreach (var variable in this.scriptVariables)
            {
                newContents = newContents.Replace(variable.Token, variable.Value);
            }

            MigrationScript result = migrationScript with
            {
                Contents = newContents,
            };
            return Task.FromResult(result);
        }
    }
}
