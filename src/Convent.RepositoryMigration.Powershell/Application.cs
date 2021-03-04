// <copyright file="Application.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Powershell
{
    using System.Threading;
    using System.Threading.Tasks;
    using Convent.RepositoryMigration.Core;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Represents the application.
    /// </summary>
    public class Application
    {
        private readonly MigrationEngine engine;

        /// <summary>
        /// Initializes a new instance of the <see cref="Application"/> class.
        /// </summary>
        /// <param name="engine">The migration engine to use.</param>
        public Application(MigrationEngine engine)
        {
            this.engine = engine;
        }

        /// <summary>
        /// Asynchronously run the application.
        /// </summary>
        /// <param name="cancellationToken">Used to cancel the running of the application.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task RunAsync(CancellationToken cancellationToken)
        {
            await this.engine.PerformMigrationAsync(cancellationToken);
        }
    }
}
