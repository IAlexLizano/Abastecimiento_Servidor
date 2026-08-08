using AutoMapper;
using MediatR;
using PreOpeningApplication.DTOs;
using PreOpeningApplication.Interfaces;
using Shared.Application.Wrappers;

namespace PreOpeningApplication.Features.Commands
{
    public class CreateLabeledBoxesAsyncCommand : IRequest<Response<IEnumerable<LabeledBoxCreatedResponseDto>>>
    {
        public int CkdLotId { get; set; }
    }

    public class CreateLabeledBoxesAsyncCommandHandler : IRequestHandler<CreateLabeledBoxesAsyncCommand, Response<IEnumerable<LabeledBoxCreatedResponseDto>>>
    {
        private readonly IPreOpeningRepository _repository;
        private readonly IMapper _mapper;

        public CreateLabeledBoxesAsyncCommandHandler(IPreOpeningRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<LabeledBoxCreatedResponseDto>>> Handle(CreateLabeledBoxesAsyncCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var requestDto = _mapper.Map<CreateLabeledBoxesRequestDto>(request);
                var result = await _repository.CreateCardboardsAsync(requestDto);
                return new Response<IEnumerable<LabeledBoxCreatedResponseDto>>(result);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<LabeledBoxCreatedResponseDto>> { Message = ex.Message, Succeeded = false };
            }
        }
    }
}
