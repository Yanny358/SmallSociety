using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos;

public class AddPhoto
{
    public class Command : IRequest<ResponseResult<Photo>>
    {
        public IFormFile File { get; set; }  = default!;
    }
    
    public class Handler : IRequestHandler<Command, ResponseResult<Photo>>
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
        
        public async Task<ResponseResult<Photo>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.UserName == _userNameAccessor.GetUsername());
            if (user == null) return null!;

            var photoUploadResult = await _photoAccessor.AddPhoto(request.File);
            var photo = new Photo
            {
                Url = photoUploadResult!.Url,
                Id = photoUploadResult.PublicId
            };

            if (!user.Photos!.Any(x => x.IsMain)) photo.IsMain = true;
            
            user.Photos!.Add(photo);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return ResponseResult<Photo>.Success(photo);
            return ResponseResult<Photo>.Failure("Problem with adding photo");
        }
    }
}