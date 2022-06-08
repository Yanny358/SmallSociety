using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities;

public class EditActivity
{
    public class Command : IRequest<ResponseResult<Unit>>
    {
        public Activity Activity { get; set; } = default!;
    }
    
    public class CommandValidator : AbstractValidator<CreateActivity.Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
        }
    }
    
    public class Handler : IRequestHandler<Command,ResponseResult<Unit>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<ResponseResult<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _context.Activities.FindAsync(request.Activity.Id);
            if (activity == null) return null!;
            _mapper.Map(request.Activity, activity);
            var result = await _context.SaveChangesAsync() > 0;
            if (!result) return ResponseResult<Unit>.Failure("Failed to update activity");

            return ResponseResult<Unit>.Success(Unit.Value);
        }
    }
}