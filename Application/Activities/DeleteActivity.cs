using Application.Core;
using AutoMapper;
using MediatR;
using Persistence;

namespace Application.Activities;

public class DeleteActivity
{
    public class Command : IRequest<ResponseResult<Unit>>
    {
        public Guid Id { get; set; }
    }
    
    public class Handler : IRequestHandler<Command, ResponseResult<Unit>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;

        }
        
        public async Task<ResponseResult<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities.FindAsync(request.Id);
            if (activity == null) return null!;
            _context.Remove(activity);
            var result = await _context.SaveChangesAsync() > 0;
            if (!result) return ResponseResult<Unit>.Failure("Failed to delete activity");

            return ResponseResult<Unit>.Success(Unit.Value);
        }
    }
}