// <copyright file="MigrationScript.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Core
{
    using System.ComponentModel;

// These can be removed once on compiler version 3.9.0 preview 3 or higher.
// This can be checked using the compiler directive #error version
// See: https://github.com/dotnet/roslyn/issues/44571#issuecomment-767726984
#pragma warning disable CS1573 // Parameter <parameter> has no matching param tag in the XML comment for <member> (but other parameters do)
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1572 // XML comment has a param tag for <parameter>, but there is no parameter by that name

    /// <summary>
    /// Represents a single script which can be run.
    /// </summary>
    /// <param name="Name">Gets the name of the script.</param>
    /// <param name="Contents">Gets the contents of the script.</param>
    [ImmutableObject(immutable: true)]
    public record MigrationScript(string Name, string Contents);
}
