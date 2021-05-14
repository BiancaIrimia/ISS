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

namespace TheatreApi.Logic.Reservations
{
    public class CreateReservation
    {
        public class Command : IRequest
        {

            public DateTime DateTime { get; set; }
            public string Title { get; set; }
            public List<int> Row { get; set; }
            public List<int> Column { get; set; }

        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Title).NotEmpty();
                RuleFor(x => x.Row).NotEmpty();
                RuleFor(x => x.Column).NotEmpty();
                RuleFor(x => x.DateTime).NotEmpty();

            }
        }

        public class Handler : IRequestHandler<Command>
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
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());
               var play = await _dataContext.Plays.Where(x => x.Title == request.Title).SingleOrDefaultAsync();
                
               if(play != null) Console.WriteLine(play.Title);
               if(user != null) Console.WriteLine(user.UserName);
               if(user == null) Console.WriteLine("nu");

               if(user != null && play != null){
                    
                    
                    Reservation reservation = new Reservation
                    {
                        ReservationId = Guid.NewGuid(),
                        SeatsReserved = request.Column.Count,
                        AppUser = user,
                        AppUserId = Guid.Parse(user.Id),
                        Play = play,
                        PlayId = play.PlayId
                    };
                   

                 _dataContext.Reservations.Add(reservation);

                for(var index = 0; index < request.Row.Count; index++){
                   var seat = await _dataContext.Seats.Where(x => (x.Row == request.Row[index] && x.Column == request.Column[index] && x.Auditorium.AuditoriumId == play.AuditoriumId)).SingleOrDefaultAsync();

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
               

                 var success = await _dataContext.SaveChangesAsync() > 0;
                 Console.WriteLine(success);

                if (success) {
                    return Unit.Value;
                }
             
            throw new Exception("Problem saving changes");
            }

            return Unit.Value;
                throw new Exception("user or play does not exist");
            }
        }
    }
}
