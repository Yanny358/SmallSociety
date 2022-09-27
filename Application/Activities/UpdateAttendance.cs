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
                .Include(a => a.Attendees)
                .ThenInclude(u => u.AppUser)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (activity == null) return null!;

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == _userNameAccessor.GetUsername());

            if (user == null) return null!;

            var hostUsername = activity.Attendees
                .FirstOrDefault(x => x.IsHost)?.AppUser.UserName;

            var attendee = activity.Attendees
                .FirstOrDefault(x => x.AppUser.UserName == user.UserName);

            if (attendee != null && hostUsername == user.UserName)
            {
                activity.IsCancelled = !activity.IsCancelled;
            }
            if (attendee != null && hostUsername != user.UserName)
            {
                activity.Attendees.Remove(attendee);
            }

            if (attendee == null)
            {
                attendee = new ActivityAttendee
                {
                    AppUser = user,
                    Activity = activity,
                    IsHost = false
                };
                activity.Attendees.Add(attendee);
            }

            var result = await _context.SaveChangesAsync() > 0;

            return result
                ? ResponseResult<Unit>.Success(Unit.Value)
                : ResponseResult<Unit>.Failure("Problem updating attendance");
        }
    }
}