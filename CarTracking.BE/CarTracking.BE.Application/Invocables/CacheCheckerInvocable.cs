using CarTracking.BE.Application.Statuses.Commands;
using Coravel.Invocable;
using MediatR;

namespace CarTracking.BE.Application.Invocables;

public class CacheCheckerInvocable(IMediator mediator) : IInvocable
{
    public async Task Invoke()
    {
        await mediator.Send(new CacheCheckCommand());
    }
}