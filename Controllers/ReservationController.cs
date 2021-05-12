using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TheatreApi.Logic.Reservations;

namespace TheatreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController
    { 
        private readonly IMediator _mediator;
        public ReservationController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        //[AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<ReservationModel>>> Reservations(){
            return await _mediator.Send(new ReservationsList.Query());
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> CreatePlay(CreateReservation.Command command)
        {

            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> DeleteReservation(Guid id)
        {
            return await _mediator.Send(new DeleteReservation.Command{Id = id});
        }

    }
}