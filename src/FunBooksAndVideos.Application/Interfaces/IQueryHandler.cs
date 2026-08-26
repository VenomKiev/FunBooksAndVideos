using MediatR;

namespace FunBooksAndVideos.Application.Interfaces;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IRequest<TResponse>
{
}
