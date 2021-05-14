using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TheatreApi.Logic.Reservations;

namespace TheatreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController
    {
        private readonly IMediator _mediator;
        public SeatController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<List<SeatModel>>> Seats(SeatsList.Query query){
            return await _mediator.Send(query);
        }
    }
}