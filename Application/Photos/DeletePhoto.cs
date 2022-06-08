using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos;

public class DeletePhoto
{
    public class Command : IRequest<ResponseResult<Unit>>
    {
        public string Id { get; set; } = default!;
    }
    
    public class Handler : IRequestHandler<Command, ResponseResult<Unit>>
    {
        private readonly DataContext _context;
        private readonly IPhotoAccessor _photoAccessor;
        private readonly IUserNameAccessor _userNameAccessor;

        public Handler(DataContext context, IPhotoAccessor photoAccessor, IUserNameAccessor userNameAccessor)
        {
            _context = context;
            _photoAccessor = photoAccessor;
            _userNameAccessor = userNameAccessor;
        }
        
        public async Task<ResponseResult<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.UserName == _userNameAccessor.GetUsername());
            if (user == null) return null!;

            var photo = user.Photos!.FirstOrDefault(x => x.Id == request.Id);
            if (photo == null) return null!;
            if (photo.IsMain) return ResponseResult<Unit>.Failure("You cannot delete your main photo");

            var result = await _photoAccessor.DeletePhoto(photo.Id);
            if (result == null!) return ResponseResult<Unit>.Failure("Problem deleting photo from cloudinary");

            user.Photos!.Remove(photo);

            var success = await _context.SaveChangesAsync() > 0;
            if (success) return ResponseResult<Unit>.Success(Unit.Value);
            
            return ResponseResult<Unit>.Failure("Problem deleting photo from API");
        }
    }
}