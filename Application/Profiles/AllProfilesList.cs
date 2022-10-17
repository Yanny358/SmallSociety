using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles;

public class AllProfilesList
{
    public class Query : IRequest<ResponseResult<List<Profile>>>
    {
        
    }
    
    public class Handler : IRequestHandler<Query, ResponseResult<List<Profile>>>
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
        
        public async Task<ResponseResult<List<Profile>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var users = await _context.Users.ProjectTo<Profile>(_mapper.ConfigurationProvider,
                new { currentUsername = _userNameAccessor.GetUsername() }).ToListAsync();
            return ResponseResult<List<Profile>>.Success(users);
        }
    }
}