using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheatreApi.Entities;
using TheatreApi.Errors;
using TheatreApi.Persistence;

namespace TheatreApi.Logic.Reservations
{
    public class ReservationsList
    {
        public class Query : IRequest<List<ReservationModel>> {}

        public class Handler : IRequestHandler<Query, List<ReservationModel>>
        {
            private readonly DataContext _dataContext;
            public Handler(DataContext dataContext)
            {
                this._dataContext = dataContext;
            }

            public async Task<List<ReservationModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<string> seatsReserved = new List<string>();
                List<Reservation> result = await _dataContext.Reservations.ToListAsync();
                List<ReservationModel> reservations = new List<ReservationModel>();
                foreach(var rez in result){
                    var user = await _dataContext.Users.Where(x => x.Id == rez.AppUserId.ToString()).SingleOrDefaultAsync();
                    var play = await _dataContext.Plays.Where(x => x.PlayId == rez.PlayId).SingleOrDefaultAsync();
                    var auditorium = await _dataContext.Auditoriums.Where(x => x.AuditoriumId == play.AuditoriumId).SingleOrDefaultAsync();
                    var seats = await _dataContext.Seats.Where(x => x.ReservationId == rez.ReservationId).ToListAsync();

                    foreach(var seat in seats){
                        seatsReserved.Add("R"+seat.Row+"S"+seat.Column);
                    }

                    reservations.Add(
                        new ReservationModel{
                            Username = user.UserName,
                            Mail = user.Email,
                            NrOfSeatsReserved = rez.SeatsReserved,
                            Title = play.Title,
                            DateTime = play.DateTime,
                            AuditoriumName = auditorium.Name,
                            SeatsReserved = seatsReserved
                        }
                    );

                }
                return reservations;
            }
        }
    }
}