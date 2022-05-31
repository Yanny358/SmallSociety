using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class CreateActivity
{
    public class Command : IRequest<ResponseResult<Unit>>
    {
        public Activity Activity { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
        }
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
            var user = await _context.Users.FirstOrDefaultAsync(
                x => x.UserName == _userNameAccessor.GetUsername());

            var atendee = new ActivityAtendee
            {
                AppUser = user,
                Activity = request.Activity,
                IsHost = true
            };
            
            request.Activity.Atendees.Add(atendee);

            _context.Activities.Add(request.Activity);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return ResponseResult<Unit>.Failure("Failed to create activity");
            return ResponseResult<Unit>.Success(Unit.Value);
        }
    }
}