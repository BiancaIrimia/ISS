using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheatreApi.Logic.Plays;

namespace TheatreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
     [Authorize]
    public class PlayController
    {
        private readonly IMediator _mediator;
        public PlayController(IMediator mediator)
        {
            this._mediator = mediator;
        }

       
        [HttpGet]
         public async Task<ActionResult<List<PlayModel>>> GetPlays()
        {
            return await _mediator.Send(new PlaysList.Query());
        }

    
        [HttpPost]
        public async Task<ActionResult<Unit>> CreatePlay(CreatePlay.Command command)
        {

            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Delete(Guid id)
        {
            return await _mediator.Send(new DeletePlay.Command{Id = id});
        }
    }
}