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
    public class Query : IRequest<ResponseResult<PagedList<ActivityDTO>>>
    {
        public ActivityParams Params { get; set; }
    }

    public class Handler : IRequestHandler<Query, ResponseResult<PagedList<ActivityDTO>>>
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
        
        public async Task<ResponseResult<PagedList<ActivityDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query =  _context.Activities
                .Where(d => d.Date >= request.Params.StartDate)
                .OrderBy(d => d.Date)
                .ProjectTo<ActivityDTO>(_mapper.ConfigurationProvider,
                    new { currentUsername = _userNameAccessor.GetUsername() })
                .AsQueryable();

            if (request.Params.IsGoing && !request.Params.IsHost)
            {
                query = query
                    .Where(x => x.Atendees.
                        Any(a => a.Username == _userNameAccessor.GetUsername()));
            }
            
            if (!request.Params.IsGoing && request.Params.IsHost)
            {
                query = query.Where(x => x.HostUsername == _userNameAccessor.GetUsername());
            }
            
            return ResponseResult<PagedList<ActivityDTO>>.Success(
                await PagedList<ActivityDTO>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize)
                );
        }
    }
}