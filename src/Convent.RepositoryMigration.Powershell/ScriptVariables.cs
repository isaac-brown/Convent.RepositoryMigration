// <copyright file="ScriptVariables.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Powershell
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a set of script variables.
    /// </summary>
    public class ScriptVariables : IReadOnlySet<ScriptVariable>
    {
        private readonly HashSet<ScriptVariable> set;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptVariables"/> class.
        /// </summary>
        /// <param name="variables">The variables to use.</param>
        public ScriptVariables(IDictionary<string, string> variables)
        {
            this.set = variables.Select(v => new ScriptVariable(v.Key, v.Value)).ToHashSet();
        }

        /// <inheritdoc/>
        public int Count => this.set.Count;

        /// <inheritdoc/>
        public bool Contains(ScriptVariable item) => this.set.Contains(item);

        /// <inheritdoc/>
        public IEnumerator<ScriptVariable> GetEnumerator() => this.set.GetEnumerator();

        /// <inheritdoc/>
        public bool IsProperSubsetOf(IEnumerable<ScriptVariable> other) => this.set.IsProperSubsetOf(other);

        /// <inheritdoc/>
        public bool IsProperSupersetOf(IEnumerable<ScriptVariable> other) => this.set.IsProperSupersetOf(other);

        /// <inheritdoc/>
        public bool IsSubsetOf(IEnumerable<ScriptVariable> other) => this.set.IsSubsetOf(other);

        /// <inheritdoc/>
        public bool IsSupersetOf(IEnumerable<ScriptVariable> other) => this.set.IsSupersetOf(other);

        /// <inheritdoc/>
        public bool Overlaps(IEnumerable<ScriptVariable> other) => this.set.Overlaps(other);

        /// <inheritdoc/>
        public bool SetEquals(IEnumerable<ScriptVariable> other) => this.set.SetEquals(other);

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => this.set.GetEnumerator();
    }
}
