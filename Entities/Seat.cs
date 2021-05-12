using System;

namespace TheatreApi.Entities
{
    public class Seat
    {
        public Guid SeatId {get; set;}
        public Reservation Reservation {get; set;}
        public Guid? ReservationId {get; set;}
        public int Row {get; set;}
        public int Column {get; set;}
        public Auditorium Auditorium {get; set;}
        public Guid AuditoriumId {get; set;}

    }
}