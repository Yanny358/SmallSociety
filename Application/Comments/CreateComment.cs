using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments;

public class CreateComment
{
    public class Command : IRequest<ResponseResult<CommentDTO>>
    {
        public string Body { get; set; }
        public Guid ActivityId { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Body).NotEmpty();
        }
    }
    
    public class Handler : IRequestHandler<Command, ResponseResult<CommentDTO>>
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
        
        public async Task<ResponseResult<CommentDTO>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities.FindAsync(request.ActivityId);
            if (activity == null) return null;

            var user = await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == _userNameAccessor.GetUsername());

            var comment = new Comment
            {
                Author = user,
                Activity = activity,
                Body = request.Body
            };
            
            activity.Comments.Add(comment);

            var success = await _context.SaveChangesAsync() > 0;
            
            if(success) return ResponseResult<CommentDTO>.Success(_mapper.Map<CommentDTO>(comment));
            
            return ResponseResult<CommentDTO>.Failure("Failed to add comment");
        }
    }
}