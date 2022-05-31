using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class AllActivitiesList
{
    public class Query : IRequest<ResponseResult<List<ActivityDTO>>> {}

    public class Handler : IRequestHandler<Query, ResponseResult<List<ActivityDTO>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<ResponseResult<List<ActivityDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activities = await _context.Activities
                .ProjectTo<ActivityDTO>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);  
            
            return ResponseResult<List<ActivityDTO>>.Success(activities);
        }
    }
}