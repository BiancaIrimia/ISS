using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TheatreApi.Entities;
using TheatreApi.Errors;
using TheatreApi.Logic.Interfaces;

namespace TheatreApi.Logic.User
{
    public class Login
    {
        public class Query : IRequest<UserModel>
        {
            public string Mail { get; set; }
            public string Password {get; set;}
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Mail).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, UserModel>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly SignInManager<AppUser> _signInManager;
           private readonly IJwtGenerator _jwtGenerator;
            public Handler(UserManager<AppUser> userManager, IJwtGenerator jwtGenerator, SignInManager<AppUser> signInManager)
            {
                this._jwtGenerator = jwtGenerator;
                this._signInManager = signInManager;
                this._userManager = userManager;
            }

            public async Task<UserModel> Handle(Query request, CancellationToken cancellationToken)
            {
                // throw new System.NotImplementedException();

                var user = await _userManager.FindByEmailAsync(request.Mail);


                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized);

                var result = await _signInManager
                    .CheckPasswordSignInAsync(user, request.Password, false);

                 if (result.Succeeded)
                 {
                    // IList<string> role = await _userManager.GetRolesAsync(user);
                    return new UserModel
                    {
                        Mail = user.Email,
                        Token = _jwtGenerator.CreateToken(user),
                        Username = user.UserName,
                        Role = null,
                        
                    };
                 }
                // throw new RestException(HttpStatusCode.Unauthorized);

                return new UserModel{};

            }
        }
    }
}