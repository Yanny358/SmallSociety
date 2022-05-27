using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Activities;

public class ActivitiesDetails
{
    public class Query : IRequest<ResponseResult<Activity>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, ResponseResult<Activity>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<ResponseResult<Activity>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities.FindAsync(request.Id);
            return ResponseResult<Activity>.Success(activity);
        }
    }
    
}