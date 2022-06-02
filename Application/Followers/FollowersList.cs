using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers;

public class FollowersList
{
    public class Query : IRequest<ResponseResult<List<Profiles.Profile>>>
    {
        public string Predicate { get; set; }
        public string Username { get; set; }
    }
    
    public class Handler : IRequestHandler<Query, ResponseResult<List<Profiles.Profile>>>
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
        
        public async Task<ResponseResult<List<Profiles.Profile>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var profiles = new List<Profiles.Profile>();

            switch (request.Predicate)
            {
                case "followers":
                    profiles = await _context.UserFollowings
                        .Where(x => x.Target.UserName == request.Username)
                        .Select(o => o.Observer)
                        .ProjectTo<Profiles.Profile>(_mapper.ConfigurationProvider, 
                            new {currentUsername = _userNameAccessor.GetUsername()})
                        .ToListAsync();
                    break;
                
                case "following":
                    profiles = await _context.UserFollowings
                        .Where(x => x.Observer.UserName == request.Username)
                        .Select(o => o.Target)
                        .ProjectTo<Profiles.Profile>(_mapper.ConfigurationProvider,
                            new {currentUsername = _userNameAccessor.GetUsername()})
                        .ToListAsync();
                    break;
                
            }

            return ResponseResult<List<Profiles.Profile>>.Success(profiles);
        }
    }
}