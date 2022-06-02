using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        private readonly IUserNameAccessor _userNameAccessor;

        public Handler(DataContext context, IMapper mapper, IUserNameAccessor userNameAccessor)
        {
            _context = context;
            _mapper = mapper;
            _userNameAccessor = userNameAccessor;
        }
        
        public async Task<ResponseResult<List<ActivityDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activities = await _context.Activities
                .ProjectTo<ActivityDTO>(_mapper.ConfigurationProvider,
                    new {currentUsername = _userNameAccessor.GetUsername()})
                .ToListAsync(cancellationToken);  
            
            return ResponseResult<List<ActivityDTO>>.Success(activities);
        }
    }
}