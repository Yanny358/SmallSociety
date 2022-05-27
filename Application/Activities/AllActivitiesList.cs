using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities;

public class AllActivitiesList
{
    public class Query : IRequest<ResponseResult<List<Activity>>> {}

    public class Handler : IRequestHandler<Query, ResponseResult<List<Activity>>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }
        
        public async Task<ResponseResult<List<Activity>>> Handle(Query request, CancellationToken cancellationToken)
        {
            return ResponseResult<List<Activity>>.Success(await _context.Activities.ToListAsync(cancellationToken));
        }
    }
}