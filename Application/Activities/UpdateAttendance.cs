using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class UpdateAttendance
{
    public class Command : IRequest<ResponseResult<Unit>>
    {
        public Guid Id { get; set; }
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
        
        public async Task<ResponseResult<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities
                .Include(a => a.Atendees)
                .ThenInclude(u => u.AppUser)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (activity == null) return null!;

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == _userNameAccessor.GetUsername());

            if (user == null) return null!;

            var hostUsername = activity.Atendees
                .FirstOrDefault(x => x.IsHost)?.AppUser.UserName;

            var atendee = activity.Atendees
                .FirstOrDefault(x => x.AppUser.UserName == user.UserName);

            if (atendee != null && hostUsername == user.UserName)
            {
                activity.IsCancelled = !activity.IsCancelled;
            }
            if (atendee != null && hostUsername != user.UserName)
            {
                activity.Atendees.Remove(atendee);
            }

            if (atendee == null)
            {
                atendee = new ActivityAtendee
                {
                    AppUser = user,
                    Activity = activity,
                    IsHost = false
                };
                activity.Atendees.Add(atendee);
            }

            var result = await _context.SaveChangesAsync() > 0;

            return result
                ? ResponseResult<Unit>.Success(Unit.Value)
                : ResponseResult<Unit>.Failure("Problem updating attendance");
        }
    }
}