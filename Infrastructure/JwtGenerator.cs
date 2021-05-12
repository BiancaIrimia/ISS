using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TheatreApi.Entities;
using TheatreApi.Logic.Interfaces;

namespace TheatreApi.Infrastructure
{
    public class JwtGenerator : IJwtGenerator
    {
         private readonly SymmetricSecurityKey _key;
        //private readonly UserManager<AppUser> _userManager;
        public JwtGenerator(IConfiguration config, UserManager<AppUser> userManager)
        {
           // this._userManager = userManager;
            _key =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        // public async Task<string> getUserRole(AppUser user)
        // {
        //     IList<string> role =await _userManager.GetRolesAsync(user);
        //     return role[0];
        // }

        public string CreateToken(AppUser user){
           

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
               // new Claim(ClaimTypes.Role, getUserRole(user).Result)
                
            };

            //generate signing credentials
             //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Token Key heyhey"));
            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                //Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
    
}