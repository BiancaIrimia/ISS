using System;
using System.Collections.Generic;

namespace TheatreApi.Entities
{
    public class Auditorium
    {
        public Guid AuditoriumId {get; set;}
        public string Name {get; set;}
        public int TotalSeats {get; set;}
        public List<Seat> Seats {get; set;}
        public List<Play> Plays {get; set;}
        
       
    }
}