using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend.Logic.Reservations;
using Backend.Logic.Reservations.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TheatreApi.Logic.Reservations;
using TheatreApi.Logic.User;

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

        [HttpGet]
        public async Task<ActionResult<List<ReservationModel>>> Reservations(){
            return await _mediator.Send(new ReservationsList.Query());
        }

        [HttpGet("my-reservations")]
        public async Task<ActionResult<List<MyReservationModel>>> MyReservations(){
            return await _mediator.Send(new MyReservations.Query());
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

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Edit(Guid id, EditReservation.Command command)
        {
            command.ReservationId = id;
            Console.WriteLine(id);
            Console.WriteLine(command.ReservationId);
            return await _mediator.Send(command);
        }

    }
}