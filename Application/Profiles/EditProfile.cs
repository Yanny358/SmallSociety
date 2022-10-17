using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles;

public class EditProfile
{
    public class Command : IRequest<ResponseResult<Unit>>
    {
        public string DisplayName { get; set; } = default!;
    }
    
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.DisplayName).NotEmpty();
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
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == _userNameAccessor.GetUsername());

            user!.DisplayName = request.DisplayName;
            
            var success = await _context.SaveChangesAsync() > 0;
            if (!success) return ResponseResult<Unit>.Failure("Failed to update profile");

            return ResponseResult<Unit>.Success(Unit.Value);
        }
    }
}