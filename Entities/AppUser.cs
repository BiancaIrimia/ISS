using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace TheatreApi.Entities
{
    public class AppUser : IdentityUser
    {
       // public string DisplayName {get; set;}
        public List<Reservation> Reservations {get; set;}
    }
}