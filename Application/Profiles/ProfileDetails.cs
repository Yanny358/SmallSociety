using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles;

public class ProfileDetails
{
    public class Query : IRequest<ResponseResult<Profile>>
    {
        public string Username { get; set; }
    }
    
    public class Handler : IRequestHandler<Query, ResponseResult<Profile>>
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
        
        public async  Task<ResponseResult<Profile>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .ProjectTo<Profile>(_mapper.ConfigurationProvider,
                    new {currentUsername = _userNameAccessor.GetUsername()})
                .SingleOrDefaultAsync(x => x.Username == request.Username);
            
            
            return ResponseResult<Profile>.Success(user);
        }
    }
}