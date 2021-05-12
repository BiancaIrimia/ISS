using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TheatreApi.Entities;
using TheatreApi.Persistence;

namespace TheatreApi.Logic.Plays
{
    public class PlaysList
    {
        public class Query : IRequest<List<PlayModel>> { }

        public class Handler : IRequestHandler<Query, List<PlayModel>>
        {
            private readonly DataContext _dataContext;
            public Handler(DataContext dataContext)
            {
                this._dataContext = dataContext;
            }

            public async Task<List<PlayModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                List<Play> playList = await _dataContext.Plays.ToListAsync();
                List<PlayModel> playListFinal = new List<PlayModel>();

                foreach(var play in playList){
                   
                    var auditorium = await _dataContext.Auditoriums.Where(x => x.AuditoriumId == play.AuditoriumId).ToListAsync();
                    Console.WriteLine(auditorium.Count);
                                
                    
                    playListFinal.Add(
                        new PlayModel {
                            Title = play.Title,
                            DateTime = play.DateTime,
                            Actors = play.Actors,
                            Description =  play.Description,
                            AuditoriumName = auditorium[0].Name

                        }
                    );
                }

               return playListFinal;
             throw new System.Exception("There are no plays");
            }
        }
    }
}