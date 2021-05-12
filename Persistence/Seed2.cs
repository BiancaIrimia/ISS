using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheatreApi.Entities;

namespace TheatreApi.Persistence
{
    public class Seed2
    {
        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager){

            if(!roleManager.Roles.Any()){
            await roleManager.CreateAsync(new IdentityRole("admin"));
            await roleManager.CreateAsync(new IdentityRole("client"));
        }
        

           if(!userManager.Users.Any())
           {
               var users = new List<AppUser>
               {
                   new AppUser
                   {
                      
                       UserName = "ana",
                       Email = "ana@test.com"
                   },
                    new AppUser
                   {
                    
                       UserName = "ica",
                       Email = "ica@test.com"
                   },
                    new AppUser
                   {
                     
                       UserName = "jim",
                       Email = "jim@test.com"
                   }
               };

               foreach (var user in users){
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                    await userManager.AddToRoleAsync(user, "client");
               }
           
                    var admin = new AppUser
                   {
                    
                       UserName = "admin",
                       Email = "admin@test.com"
                   };

                    await userManager.CreateAsync(admin, "Pa$$w0rd");
                    await userManager.AddToRoleAsync(admin, "admin");

                    context.SaveChanges();
           }





        
       if(!context.Auditoriums.Any())
          {
               var a = new List<Auditorium>
              {
                  new Auditorium
                  {
                      Name = "Sala Mare",
                      TotalSeats = 100,
                  },

                  new Auditorium
                  {
                      Name = "Sala Mica",
                      TotalSeats = 40,
                  },

                    new Auditorium
                  {
                      Name = "Sala Studio",
                      TotalSeats = 25,
                  },

              };

            foreach (var aud in a){
               context.Auditoriums.Add(aud);
                context.SaveChanges();
               }
          }


        

          if(!context.Seats.Any()){
               var salaMica = await context.Auditoriums.Where(x => x.Name == "Sala Mica").SingleOrDefaultAsync();
               var salaMare = await context.Auditoriums.Where(x => x.Name == "Sala Mare").SingleOrDefaultAsync();
               var salaStudio = await context.Auditoriums.Where(x => x.Name == "Sala Studio").SingleOrDefaultAsync();

                context.Seats.Add(
                      new Seat{
                        
                          Row = 1,
                          Column = 1,
                          Auditorium = salaMica
                          //AuditoriumId = salaMica.Id,
                          //Reservation = null,
                          
                      }
                  );

              for(int i = 2; i<=4; i++){
                  for(int j = 2; j <= 10; j++)
                 
                  context.Seats.Add(
                      new Seat{
                          Row = i,
                          Column = j,
                          Auditorium = salaMica,
                          // AuditoriumId = salaMica.Id
                          
                      }
                  );
              }


                for(int i = 1; i<=3; i++){
                  for(int j = 1; j <=10; j++)
                  context.Seats.Add(
                      new Seat{
                          Row = i,
                          Column = j,
                          Auditorium = salaMare,
                        //AuditoriumId = salaMare.Id
                      }
                  );
              }

                for(int i = 1; i<=5; i++){
                  for(int j = 1; j <= 5; j++)
                  context.Seats.Add(
                      new Seat{
                          Row = i,
                          Column = j,
                          Auditorium = salaStudio,
                          //AuditoriumId = salaStudio.Id
                      }
                  );
             }
               context.SaveChanges();
          }



        

              
        }

    }

}
        
    
