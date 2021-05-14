using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheatreApi.Persistence;


namespace TheatreApi.Logic.Reservations
{
    public class SeatsList
    {
        
        public class Query : IRequest<List<SeatModel>>
        {
            public string AuditoriumName { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.AuditoriumName).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, List<SeatModel>>
        {
            private readonly DataContext _dataContext;
            public Handler(DataContext dataContext)
            {
                this._dataContext = dataContext;
            }

            public async Task<List<SeatModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                var seatsList = new List<SeatModel>();
                var auditorium = await _dataContext.Auditoriums.Where(x => x.Name == request.AuditoriumName).SingleOrDefaultAsync();
                if(auditorium != null) Console.WriteLine(auditorium.Name);
                if(auditorium == null) Console.WriteLine("nu");
                bool free = true;
               
                var seats = await _dataContext.Seats.Where(x => x.AuditoriumId == auditorium.AuditoriumId).ToListAsync();
                Console.WriteLine(seats.Count);

                foreach(var seat in seats){
                    if(seat.ReservationId != null) free = false; 
                    seatsList.Add(
                        new SeatModel{
                            Row = seat.Row,
                            Column = seat.Column,
                            Free = free
                        }
                    );
                    free = true;
                }
                return seatsList;
                
            }
        }
    }
    
}