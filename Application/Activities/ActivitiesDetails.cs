using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class ActivitiesDetails
{
    public class Query : IRequest<ResponseResult<ActivityDTO>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, ResponseResult<ActivityDTO>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<ResponseResult<ActivityDTO>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities
                .ProjectTo<ActivityDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == request.Id);
            return ResponseResult<ActivityDTO>.Success(activity);
        }
    }
    
}