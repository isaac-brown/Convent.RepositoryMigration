// <copyright file="TestLoggerRelay.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.AutoFixture
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Convent.RepositoryMigration.Core;
    using global::AutoFixture.Kernel;
    using MELT;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Builds <see cref="ILogger{TCategoryName}"/> instances.
    /// </summary>
    /// <remarks>
    /// For this to work you'll want to ensure that <see cref="ILoggerFactory"/> has been provided to AutoFixture.
    /// </remarks>
    internal class TestLoggerRelay : ISpecimenBuilder
    {
        /// <inheritdoc/>
        public object Create(object request, ISpecimenContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (request is not Type t)
            {
                return new NoSpecimen();
            }

            var typeArguments = t.GetGenericArguments();
            if (typeArguments.Length != 1
                || typeof(ILogger<>) != t.GetGenericTypeDefinition())
            {
                return new NoSpecimen();
            }

            if (context.Resolve(typeof(ILoggerFactory)) is not ILoggerFactory loggerFactory)
            {
                return new NoSpecimen();
            }

            ILoggerFactory loggerFactory1 = TestLoggerFactory.Create();
            loggerFactory1.CreateLogger<MigrationEngine>();

            // All of these lines boil down to: ILoggerFactory.Create<T>();

            // Find the extension method LoggerFactoryExtensions.CreateLogger<T>(ILoggerFactory).
            MethodInfo? methodInfo = typeof(LoggerFactoryExtensions).GetMethods()
                                                                    .SingleOrDefault(m => m.Name == "CreateLogger"
                                                                                       && m.IsGenericMethod);

            // Construct the generic method so we can invoke it.
            MethodInfo? genericMethod = methodInfo?.MakeGenericMethod(t.GetGenericArguments().Single());

            // Invoke the method.
            object? genericLogger = genericMethod?.Invoke(null, new object?[] { loggerFactory });

            if (genericLogger is null)
            {
                return new NoSpecimen();
            }

            return genericLogger;
        }
    }
}
