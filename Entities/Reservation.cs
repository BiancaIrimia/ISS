using System;
using System.Collections.Generic;

namespace TheatreApi.Entities
{
    public class Reservation
    {
        public Guid ReservationId {get;set;}
        public int SeatsReserved {get; set;}
        public AppUser AppUser {get; set;}
        public Play Play {get; set;}
        public Guid PlayId {get; set;}
        public Guid AppUserId {get; set;}
    
        
    }
}