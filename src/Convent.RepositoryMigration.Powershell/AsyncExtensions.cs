// <copyright file="AsyncExtensions.cs" company="Isaac Brown">
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Convent.RepositoryMigration.Powershell
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for asynchronous tasks.
    /// </summary>
    public static class AsyncExtensions
    {
        /// <summary>
        /// Allows an asynchronous method which does not accept a <see cref="CancellationToken"/> to be cancelled.
        /// </summary>
        /// <param name="task">The task to add cancellation to.</param>
        /// <param name="cancellationToken">Used to cancel the operation.</param>
        /// <typeparam name="T">The return type of the task.</typeparam>
        /// <returns>A task which can be cancelled.</returns>
        /// <exception cref="OperationCanceledException">When the operation is cancelled.</exception>
        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);

            // This disposes the registration as soon as one of the tasks trigger
            using (cancellationToken.Register(
                state =>
                {
                    if (state is not null)
                    {
                        ((TaskCompletionSource<object?>)state).TrySetResult(null);
                    }
                },
                tcs))
            {
                var resultTask = await Task.WhenAny(task, tcs.Task);
                if (resultTask == tcs.Task)
                {
                    // Operation cancelled
                    throw new OperationCanceledException(cancellationToken);
                }

                return await task;
            }
        }
    }
}
