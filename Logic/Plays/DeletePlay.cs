using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using TheatreApi.Persistence;

namespace TheatreApi.Logic.Plays
{
    public class DeletePlay
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }


        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _dataContext;
            public Handler(DataContext dataContext)
            {
                this._dataContext = dataContext;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                
                var play = await _dataContext.Plays.FindAsync(request.Id);

                if (play == null)
                    throw new Exception("Could not find play");

                _dataContext.Remove(play);              

                var success = await _dataContext.SaveChangesAsync() > 0;

                if (success) return Unit.Value;
                

                throw new Exception("Problem saving changes");

            }


        }
    
    }
}