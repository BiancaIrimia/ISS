using System;
using System.Collections.Generic;

namespace TheatreApi.Logic.Reservations
{
    public class ReservationModel
    {
        public string Username {get; set;}
        public string Mail {get; set;}
        public string Title {get; set;}
        public DateTime DateTime {get; set;}
        public int NrOfSeatsReserved {get; set;}
        public string AuditoriumName {get; set;}
        public List<string> SeatsReserved {get; set;}
    }
}