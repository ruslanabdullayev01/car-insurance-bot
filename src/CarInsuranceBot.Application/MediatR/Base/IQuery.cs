using Domain.Abstractions;
using MediatR;

namespace CarInsuranceBot.Application.MediatR.Base
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
