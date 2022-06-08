using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos;

public class SetMainPhoto
{
    public class Command : IRequest<ResponseResult<Unit>>
    {
        public string Id { get; set; } = default!;
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
            var user = await _context.Users.Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.UserName == _userNameAccessor.GetUsername());
            if (user == null) return null!;

            var photo = user.Photos!.FirstOrDefault(x => x.Id == request.Id);
            if (photo == null) return null!;

            var currentMain = user.Photos!.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            var success = await _context.SaveChangesAsync() > 0;
            if (success) return ResponseResult<Unit>.Success(Unit.Value);
            
            return ResponseResult<Unit>.Failure("Problem setting main photo");
        }
    }
}