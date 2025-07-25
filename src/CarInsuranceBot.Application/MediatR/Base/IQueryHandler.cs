using Domain.Abstractions;
using MediatR;

namespace CarInsuranceBot.Application.MediatR.Base
{
    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
        where TQuery : IQuery<TResponse>
    {
    }
}
