using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheatreApi.Logic.Plays;
using TheatreApi.Logic.User;

namespace TheatreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            this._mediator = mediator;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserModel>> Login(Login.Query query)
        {
            return await _mediator.Send(query);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserModel>> Register(Register.Command command)
        {
            if(command.Role == null)
                command.Role = "client";
            return await _mediator.Send(command);
        }

        [Authorize]
        [HttpGet]
         public async Task<ActionResult<UserModel>> CurrentUser()
        {
            return await _mediator.Send(new CurrentUser.Query());
        }

    }
}