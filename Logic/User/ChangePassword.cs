using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TheatreApi.Entities;
using TheatreApi.Errors;
using TheatreApi.Logic.Interfaces;
using TheatreApi.Validators;

namespace Backend.Logic.User
{
    public class ChangePassword
    {
         public class Command : IRequest
        {
            
            public string CurrentPassword { get; set; }

            public string NewPassword { get; set; }
            public string ConfirmNewPassword { get; set; }
            
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.CurrentPassword).NotEmpty();
                RuleFor(x => x.NewPassword).NotEmpty().Password();
                RuleFor(x => x.ConfirmNewPassword).NotEmpty().Password();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IUserAccessor _userAccessor;
            public Handler(UserManager<AppUser> userManager, IUserAccessor userAccessor)
            {
                this._userAccessor = userAccessor;
                this._userManager = userManager;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                AppUser user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                if (user == null)
                    throw new Exception("This user doesn't exist...");

                bool passwordCheck = await _userManager.CheckPasswordAsync(user, request.CurrentPassword);

                if (passwordCheck == false)
                    
                    throw new RestException(System.Net.HttpStatusCode.BadRequest);
                
                if(request.NewPassword == request.ConfirmNewPassword){
                    IdentityResult result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
                    if (result.Succeeded)
                        return Unit.Value;
                     else throw new RestException(System.Net.HttpStatusCode.BadRequest, "Password could not be changed");
                }
                else throw new RestException(System.Net.HttpStatusCode.BadRequest, "The confirmation of the new password does not match the new password");

            }
        }
    }
    
}