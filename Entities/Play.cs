using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheatreApi.Entities
{
    public class Play
    {
        public Guid PlayId {get; set;}
        public string Title {get; set;}
        public DateTime DateTime {get; set;}
     
        public string Actors {get; set;}
        public string Description {get; set;}

        public List<Reservation> Reservations {get; set;}
        public Auditorium Auditorium {get; set;}
        public Guid AuditoriumId {get; set;}

        
    }
}