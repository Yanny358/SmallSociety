using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments;

public class CommentsList
{
    public class Query : IRequest<ResponseResult<List<CommentDTO>>>
    {
        public Guid ActivityId { get; set; }
    }
    
    public class Handler : IRequestHandler<Query, ResponseResult<List<CommentDTO>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }        
        public async Task<ResponseResult<List<CommentDTO>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var comments = await _context.Comments
                .Where(x => x.Activity.Id == request.ActivityId)
                .OrderBy(x => x.CreatedAt)
                .ProjectTo<CommentDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return ResponseResult<List<CommentDTO>>.Success(comments);
        }
    }
}