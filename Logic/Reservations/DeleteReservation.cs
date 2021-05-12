using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheatreApi.Entities;
using TheatreApi.Persistence;

namespace TheatreApi.Logic.Reservations
{
    public class DeleteReservation
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
                
                var reservation = await _dataContext.Reservations.FindAsync(request.Id);
                if (reservation == null)
                    throw new Exception("Could not find play");

                var seats = await _dataContext.Seats.Where(x => x.ReservationId == request.Id).ToListAsync();
                Console.WriteLine(seats.Count);

                foreach(var seat in seats){
                    Seat newSeat = new Seat{
                        SeatId = seat.SeatId,
                        Reservation = null,
                        ReservationId = null,
                        Column = seat.Column,
                        Row = seat.Row,
                        AuditoriumId = seat.AuditoriumId,
                        Auditorium = seat.Auditorium
                       };
                  

                   _dataContext.Remove(seat);
                   _dataContext.Seats.Add(newSeat); 
                }

                var success = await _dataContext.SaveChangesAsync() > 0;

                if(success){
                     _dataContext.Remove(reservation); 
                     var successDelete = await _dataContext.SaveChangesAsync() > 0;
                     if(successDelete) return Unit.Value;
                }

                throw new Exception("Problem saving changes");

            }


        }
    }
}