using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheatreApi.Entities;
using TheatreApi.Logic.Interfaces;
using TheatreApi.Persistence;

namespace Backend.Logic.Reservations
{
    public class EditReservation
    {
        public class Command : IRequest
        {
            public Guid ReservationId {get; set;}
            public List<int> Row { get; set; }
            public List<int> Column { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                //RuleFor(x => x.ReservationId).NotEmpty();
                RuleFor(x => x.Row).NotEmpty();
                RuleFor(x => x.Column).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly DataContext _dataContext;
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<AppUser> _userManager;
            public Handler(DataContext dataContext, IUserAccessor userAccessor, UserManager<AppUser> userManager)
            {
                this._userManager = userManager;
                this._userAccessor = userAccessor;
                this._dataContext = dataContext;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var currentUser = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());
                var reservation = await _dataContext.Reservations.Where(x => x.AppUserId.ToString() == currentUser.Id).SingleOrDefaultAsync();
                var seats = await _dataContext.Seats.Where(x => x.ReservationId == reservation.ReservationId).ToListAsync();
                Console.WriteLine(request.ReservationId);
                reservation.SeatsReserved = request.Row.Count;
                if(reservation == null) Console.WriteLine("nullll");
                else Console.WriteLine(reservation.ReservationId);

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

                var play = await _dataContext.Plays.Where(x => x.PlayId == reservation.PlayId).SingleOrDefaultAsync();
                var auditorium = await _dataContext.Auditoriums.Where(x => x.AuditoriumId == play.AuditoriumId).SingleOrDefaultAsync();

                if(success){
                    for(var index = 0; index < request.Row.Count; index++){
                    var seat = await _dataContext.Seats.Where(x => (x.Row == request.Row[index] && x.Column == request.Column[index] && x.AuditoriumId == auditorium.AuditoriumId)).SingleOrDefaultAsync();

                        Console.WriteLine(seat.SeatId);
                    
                        Seat newSeat = new Seat{
                            SeatId = seat.SeatId,
                            Reservation = reservation,
                            ReservationId = reservation.ReservationId,
                            Column = seat.Column,
                            Row = seat.Row,
                            AuditoriumId = seat.AuditoriumId,
                            Auditorium = seat.Auditorium
                        };
                    
                    _dataContext.Remove(seat);
                    _dataContext.Seats.Add(newSeat); 

                    }

                    var success2 = await _dataContext.SaveChangesAsync() > 0;
                    if(success2) return Unit.Value;

                }
                return Unit.Value;
                throw new Exception("Problem saving changes");
            }
        }
    }
}