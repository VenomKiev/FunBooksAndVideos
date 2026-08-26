using MediatR;

namespace FunBooksAndVideos.Application.Interfaces;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : IRequest<TResponse>
{
}
