// <copyright file="Program.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Powershell
{
    using System;
    using System.Collections.Generic;
    using System.IO.Abstractions;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using Convent.RepositoryMigration.Core;
    using Convent.RepositoryMigration.Journals;
    using Convent.RepositoryMigration.ScriptProviders;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using Serilog.Extensions.Logging;

    /// <summary>
    /// Application entry point.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Application entry point.
        /// </summary>
        private static async Task Main(string[] args)
        {
            CancellationTokenSource cts = AddConsoleCancellation();

            using var container = CompositionRoot(args);

            await container.Resolve<Application>().RunAsync(cts.Token);
        }

        /// <summary>
        /// Creates a new cancellation source which cancels when the user presses the cancel key(s).
        /// </summary>
        /// <returns>A new <see cref="CancellationTokenSource"/>.</returns>
        private static CancellationTokenSource AddConsoleCancellation()
        {
            CancellationTokenSource cts = new();

            Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            return cts;
        }

        private static IContainer CompositionRoot(string[] args)
        {
            var builder = new ContainerBuilder();

            // Configuration.
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddCommandLine(args);
            IConfigurationRoot configuration = configurationBuilder.Build();
            builder.RegisterInstance((IConfiguration)configuration);

            // Service options.
            var options = configuration.GetSection(nameof(ScriptProviderOptions))
                                       .Get<ScriptProviderOptions?>() ?? new ScriptProviderOptions();
            builder.RegisterInstance(options);

            var jsonOptions = configuration.GetSection(nameof(JournalOptions))
                                           .Get<JournalOptions?>() ?? new JournalOptions();
            builder.RegisterInstance(jsonOptions);

            // Logging.
            var logger = new LoggerConfiguration()
                                    .MinimumLevel.Debug()
                                    .WriteTo.Console()
                                    .CreateLogger();

            ILoggerFactory factory = new SerilogLoggerFactory(logger);

            builder.RegisterInstance(factory);

            // builder.RegisterInstance<ILoggerFactory>(
            //     LoggerFactory.Create(builder =>
            //     {
            //         builder.AddConsole();
            //     }));
            builder.RegisterGeneric(typeof(Logger<>))
                    .As(typeof(ILogger<>))
                    .SingleInstance();

            // Filesystem.
            builder.RegisterType<FileSystem>().As<IFileSystem>();

            // Application specific.
            builder.RegisterType<Application>();
            builder.RegisterType<MigrationEngine>();
            builder.RegisterType<MigrationConfiguration>();
            builder.RegisterType<JsonJournal>().As<IJournal>();
            builder.RegisterType<DirectoryScriptProvider>().As<IScriptProvider>();
            builder.RegisterType<PowershellScriptExecutor>().As<IScriptExecutor>();
            builder.RegisterType<VariableSubstitutionPreprocessor>().As<IScriptPreprocessor>();
            var variables = new ScriptVariables(new Dictionary<string, string> { });
            builder.RegisterInstance(variables);
            builder.RegisterInstance(new RepositoryDirectoryScriptPreprocessor(jsonOptions.BaseDirectory)).As<IScriptPreprocessor>();

            return builder.Build();
        }
    }
}
