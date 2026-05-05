using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketAPI.Domain.Enums;
using TicketAPI.Domain.UseCases.CancelTicket;
using TicketAPI.Domain.UseCases.CreateTicket;
using TicketAPI.Domain.UseCases.GetTicketById;
using TicketAPI.Domain.UseCases.GetTickets;
using TicketAPI.Domain.UseCases.GetTicketValidations;
using TicketAPI.Domain.UseCases.ValidateTicket;
using System.Security.Claims;

namespace TicketAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/tickets")]
    public class TicketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] Guid? eventId = null,
            [FromQuery] Guid? orderItemId = null,
            [FromQuery] Guid? seatId = null,
            [FromQuery] ETicketStatus? status = null,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetTicketsRequest
            {
                Page = page,
                PageSize = pageSize,
                EventId = eventId,
                OrderItemId = orderItemId,
                SeatId = seatId,
                Status = status
            }, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{ticket_id}")]
        public async Task<IActionResult> GetById(Guid ticket_id, CancellationToken cancellationToken)
        {
            var ticket = await _mediator.Send(new GetTicketByIdRequest { TicketId = ticket_id }, cancellationToken);
            return Ok(ticket);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTicketRequest request, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(request, cancellationToken);
            return Ok(new { ticketId = id });
        }

        [HttpDelete("{ticket_id}")]
        public async Task<IActionResult> Cancel(Guid ticket_id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new CancelTicketRequest { TicketId = ticket_id }, cancellationToken);
            return NoContent();
        }

        [HttpPost("{ticket_id}/validate")]
        public async Task<IActionResult> Validate(
            Guid ticket_id,
            [FromBody] ValidateTicketRequest request,
            CancellationToken cancellationToken)
        {
            request.TicketId = ticket_id;
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid.TryParse(userIdStr, out var userId);
            request.ScannedBy = userId;
            request.ValidatedBy = User.FindFirstValue(ClaimTypes.Name) ?? userId.ToString();

            var validationId = await _mediator.Send(request, cancellationToken);
            return Ok(new { validationId });
        }

        [HttpGet("{ticket_id}/validations")]
        public async Task<IActionResult> GetValidations(
            Guid ticket_id,
            [FromQuery] Guid? scannerId = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetTicketValidationsRequest
            {
                TicketId = ticket_id,
                ScannerId = scannerId,
                Page = page,
                PageSize = pageSize
            }, cancellationToken);
            return Ok(result);
        }
    }
}
