using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheatreApi.Entities;
using TheatreApi.Persistence;

namespace TheatreApi.Logic.Plays
{
    public class CreatePlay
    {
        public class Command : IRequest
        {
            public string Title { get; set; }
            public string Actors { get; set; }
            public string Description { get; set; }
            public DateTime DateTime { get; set; }
            public string AuditoriumName { get; set; }

        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title).NotEmpty();
                RuleFor(x => x.Description).NotEmpty();
                RuleFor(x => x.Actors).NotEmpty();
                RuleFor(x => x.DateTime).NotEmpty();
                RuleFor(x => x.AuditoriumName).NotEmpty();

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
                var auditorium = await _dataContext.Auditoriums.Where(x => x.Name == request.AuditoriumName).SingleOrDefaultAsync();
                
                if(auditorium != null) {
                Play play = new Play{
                    Title = request.Title,
                    DateTime = request.DateTime,
                    Actors = request.Actors, 
                    Description = request.Description,
                    Auditorium = auditorium,
                    AuditoriumId = auditorium.AuditoriumId
                };

                _dataContext.Plays.Add(play);

                var success = await _dataContext.SaveChangesAsync() > 0;

                if (success) {
                    
                    return Unit.Value;
                }
                }
                throw new Exception("Problem saving changes");
                
            }
        }
    }
}