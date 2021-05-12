using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheatreApi.Entities;
using TheatreApi.Errors;
using TheatreApi.Logic.Interfaces;
using TheatreApi.Persistence;
using TheatreApi.Validators;

namespace TheatreApi.Logic.User
{
    public class Register
    {
         public class Command : IRequest<UserModel>
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Role {get; set;}
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Username).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotEmpty().Password();
                RuleFor(x => x.Role).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, UserModel>
        {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            public Handler(DataContext context, UserManager<AppUser> userManager, IJwtGenerator jwtGenerator)
            {
                _jwtGenerator = jwtGenerator;
                _userManager = userManager;
                _context = context;
            }

            public async Task<UserModel> Handle(Command request, CancellationToken cancellationToken)
            {
                Console.WriteLine(request.Role);
                if (await _context.Users.Where(x => x.Email == request.Email).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new {Email = "Email already exists"});

                if (await _context.Users.Where(x => x.UserName == request.Username).AnyAsync())
                    throw new RestException(HttpStatusCode.BadRequest, new {Username = "Username already exists"});

                var user = new AppUser
                {
                    Email = request.Email,
                    UserName = request.Username
                };

                var resultPassword = await _userManager.CreateAsync(user, request.Password);
                var resultRole = await _userManager.AddToRoleAsync(user, request.Role);

                if (resultRole.Succeeded && resultPassword.Succeeded)
                {
                    return new UserModel
                    {
                        Mail = user.Email,
                        Token = _jwtGenerator.CreateToken(user),
                        Username = user.UserName,
                        Role = request.Role,

                    };
                }

                throw new Exception("Problem creating user");
            }


        }
    }
}
    
