using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles;

public class ListActivities
{
    public class Query : IRequest<ResponseResult<List<UserActivityDto>>>
    {
        public string Username { get; set; } = default!;
        public string Predicate { get; set; } = default!;
    }
    
    public class Handler : IRequestHandler<Query, ResponseResult<List<UserActivityDto>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<ResponseResult<List<UserActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _context.ActivityAttendees
                .Where(u => u.AppUser.UserName == request.Username)
                .OrderBy(a => a.Activity.Date)
                .ProjectTo<UserActivityDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            query = request.Predicate switch
            {
                "past" => query.Where(a => a.Date <= DateTime.Now),
                "hosting" => query.Where(a => a.HostUsername == request.Username),
                _  => query.Where(a => a.Date >= DateTime.Now)
            };

            var activities = await query.ToListAsync();

            return ResponseResult<List<UserActivityDto>>.Success(activities);
        }
    }
}