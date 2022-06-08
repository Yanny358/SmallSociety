using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Followers;

public class FollowToggle
{
    public class Command : IRequest<ResponseResult<Unit>>
    {
        public string TargetUsername { get; set; }  = default!;
    }
    
    public class Handler : IRequestHandler<Command, ResponseResult<Unit>>
    {
        private readonly DataContext _context;
        private readonly IUserNameAccessor _userNameAccessor;

        public Handler(DataContext context, IUserNameAccessor userNameAccessor)
        {
            _context = context;
            _userNameAccessor = userNameAccessor;
        }
        
        public async  Task<ResponseResult<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var observer = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == _userNameAccessor.GetUsername());
            
            var target = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == request.TargetUsername);

            if (target == null) return null!;

            var following = await _context.UserFollowings.FindAsync(observer!.Id, target.Id);

            if (following == null)
            {
                following = new UserFollowing
                {
                    Observer = observer,
                    Target = target
                };

                _context.UserFollowings.Add(following);
            }
            else
            {
                _context.UserFollowings.Remove(following);
            }

            var success = await _context.SaveChangesAsync() > 0;
            if (success) return ResponseResult<Unit>.Success(Unit.Value);
            
            return ResponseResult<Unit>.Failure("Failed to update following");
        }
    }
}