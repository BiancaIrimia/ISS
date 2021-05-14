using System;
using System.Collections.Generic;

namespace Backend.Logic.Reservations.Models
{
    public class MyReservationModel
    {
        
        public string Title {get; set;}
        public DateTime DateTime {get; set;}
        public int NrOfSeatsReserved {get; set;}
        public string AuditoriumName {get; set;}
        public List<string> SeatsReserved {get; set;}
    }
}