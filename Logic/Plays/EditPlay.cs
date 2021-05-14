using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheatreApi.Persistence;

namespace Backend.Logic.Plays
{
    public class EditPlay
    {
        public class Command : IRequest
        {
            public Guid PlayId { get; set; }
            public DateTime? DateTime { get; set; }
            #nullable enable
            public string? Actors { get; set; }
            public string? Description { get; set; }
            public string? AuditoriumName { get; set; }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly DataContext _dataContext;
            public Handler(DataContext dataContext)
            {
                this._dataContext = dataContext;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var play = await _dataContext.Plays.Where(x => x.PlayId == request.PlayId).SingleOrDefaultAsync();

                if(request.DateTime != null){
                    var auditorium = await _dataContext.Auditoriums.Where(x => x.Name == request.AuditoriumName).SingleOrDefaultAsync();
                }
                play.DateTime = request.DateTime ?? play.DateTime;
                play.Actors = request.Actors ?? play.Actors;
                play.Description = request.Description ?? play.Description;

                throw new NotImplementedException();
            }
        }
    }
}