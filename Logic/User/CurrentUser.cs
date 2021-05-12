using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TheatreApi.Entities;
using TheatreApi.Logic.Interfaces;

namespace TheatreApi.Logic.User
{
    public class CurrentUser
    {
        public class Query : IRequest<UserModel> { }

        public class Handler : IRequestHandler<Query, UserModel>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IUserAccessor _userAccessor;
            public Handler(UserManager<AppUser> userManager, IJwtGenerator jwtGenerator, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _jwtGenerator = jwtGenerator;
                _userManager = userManager;
            }

            public async Task<UserModel> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());
                IList<string> role = await _userManager.GetRolesAsync(user);

                return new UserModel
                {
                    Mail = user.Email,
                    Username = user.UserName,
                    Token = _jwtGenerator.CreateToken(user),
                    Role = role[0],
 
                };
            }
        }
    }
    
}