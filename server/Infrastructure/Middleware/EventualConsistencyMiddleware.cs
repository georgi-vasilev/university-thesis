using Domain.Common;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Middleware
{
    public class EventualConsistencyMiddleware
    {
        public const string DomainEventsKey = "DomainEventsKey";

        private readonly RequestDelegate _next;

        public EventualConsistencyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IPublisher publisher, ApplicationDbContext dbContext)
        {
            var transaction = await dbContext.Database.BeginTransactionAsync();
            context.Response.OnCompleted(async () =>
            {
                try
                {
                    if (context.Items.TryGetValue(DomainEventsKey, out var value) && value is Queue<IDomainEvent> domainEvents)
                    {
                        while (domainEvents.TryDequeue(out var nextEvent))
                        {
                            await publisher.Publish(nextEvent);
                        }
                    }

                    await transaction.CommitAsync();
                }
                catch (Exception e)
                {
                    // Handle eventual consistency exceptions
                }
                finally
                {
                    await transaction.DisposeAsync();
                }
            });

            await _next(context);
        }
    }
}
