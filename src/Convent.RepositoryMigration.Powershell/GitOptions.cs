// <copyright file="GitOptions.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Powershell
{
    using System;

    /// <summary>
    /// Options to configure git.
    /// </summary>
    public class GitOptions
    {
        /// <summary>
        /// Gets or sets the directory to which migrations will be applied.
        /// </summary>
        public string TargetDirectory { get; set; } = Environment.CurrentDirectory;
    }
}
