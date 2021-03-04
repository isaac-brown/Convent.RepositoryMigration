// <copyright file="ScriptProviderOptions.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.ScriptProviders
{
    using System;
    using System.IO;

    /// <summary>
    /// Options for finding scripts.
    /// </summary>
    public class ScriptProviderOptions
    {
        /// <summary>
        /// Gets or sets the directory in which script files are held.
        /// </summary>
        public string ScriptsDirectory { get; set; } = Path.Join(Environment.CurrentDirectory, "scripts");
    }
}
