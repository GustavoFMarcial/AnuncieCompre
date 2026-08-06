using AnuncieCompre.Application.Interfaces;
using AnuncieCompre.Domain.Aggregates.FlowAggregate;
using AnuncieCompre.Domain.Aggregates.ValueObjects;
using AnuncieCompre.Domain.DTO;
using AnuncieCompre.Infra.Repositories;

namespace AnuncieCompre.Application.UseCases.Flows;

public class CreateFlow(IFlowRepository _flowRepository, IUnitOfWork _unitOfWork)
{
    private readonly IFlowRepository flowRepository = _flowRepository;
    private readonly IUnitOfWork unitOfWork = _unitOfWork;

    public async Task<Flow> Handle(CreateFlowRequest request)
    {
        Flow flow = Flow.Create(Name.Create(request.Name).Value, request.Description, request.Status);
        flowRepository.Add(flow);
        await unitOfWork.SaveChangesAsync();

        return flow;
    }
}