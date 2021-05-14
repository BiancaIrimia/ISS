using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Backend.Logic.Reservations.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheatreApi.Entities;
using TheatreApi.Logic.Interfaces;
using TheatreApi.Logic.Reservations;
using TheatreApi.Persistence;

namespace TheatreApi.Logic.User
{
    public class MyReservations
    {
        public class Query : IRequest<List<MyReservationModel>> { }

        public class Handler : IRequestHandler<Query, List<MyReservationModel>>
        {
            private readonly DataContext _dataContext;
            private readonly UserManager<AppUser> _userManager;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext dataContext, UserManager<AppUser> userManager, IUserAccessor userAccessor)
            {
                this._userAccessor = userAccessor;
                this._userManager = userManager;
                this._dataContext = dataContext;
            }

            public async Task<List<MyReservationModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                var currentUser = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());
                var reservations = await _dataContext.Reservations.Where(x => x.AppUserId.ToString() == currentUser.Id).ToListAsync();
                
                List<MyReservationModel> reservationsList = new List<MyReservationModel>();
                List<string> seatsReserved = new List<string>();

                foreach(var reservation in reservations){

                    var play = await _dataContext.Plays.Where(x => x.PlayId == reservation.PlayId).SingleOrDefaultAsync();
                    var auditorium = await _dataContext.Auditoriums.Where(x => x.AuditoriumId == play.AuditoriumId).SingleOrDefaultAsync();
                    var seats = await _dataContext.Seats.Where(x => x.ReservationId == reservation.ReservationId).ToListAsync();

                    foreach(var seat in seats){
                        seatsReserved.Add("R"+seat.Row+"S"+seat.Column);
                    }

                    reservationsList.Add(
                        new MyReservationModel{
                            NrOfSeatsReserved = reservation.SeatsReserved,
                            Title = play.Title,
                            DateTime = play.DateTime,
                            AuditoriumName = auditorium.Name,
                            SeatsReserved = seatsReserved
                        }
                    );
                }

                return reservationsList;
            }
        }
    }
}