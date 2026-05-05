using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketAPI.Domain.Enums;
using TicketAPI.Domain.UseCases.CreateScanner;
using TicketAPI.Domain.UseCases.CreateScannerEvent;
using TicketAPI.Domain.UseCases.GetScannerById;
using TicketAPI.Domain.UseCases.GetScannerEvents;
using TicketAPI.Domain.UseCases.GetScanners;
using TicketAPI.Domain.UseCases.RemoveScanner;
using TicketAPI.Domain.UseCases.RemoveScannerEvent;
using TicketAPI.Domain.UseCases.UpdateScannerStatus;

namespace TicketAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/scanners")]
    public class ScannersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ScannersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] EScannerStatus? status = null,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetScannersRequest
            {
                Page = page,
                PageSize = pageSize,
                Status = status
            }, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{scanner_id}")]
        public async Task<IActionResult> GetById(Guid scanner_id, CancellationToken cancellationToken)
        {
            var scanner = await _mediator.Send(new GetScannerByIdRequest { ScannerId = scanner_id }, cancellationToken);
            return Ok(scanner);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateScannerRequest request, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(request, cancellationToken);
            return Ok(new { scannerId = id });
        }

        [HttpPut("{scanner_id}")]
        public async Task<IActionResult> UpdateStatus(
            Guid scanner_id,
            [FromBody] UpdateScannerStatusRequest request,
            CancellationToken cancellationToken)
        {
            request.ScannerId = scanner_id;
            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{scanner_id}")]
        public async Task<IActionResult> Remove(Guid scanner_id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new RemoveScannerRequest { ScannerId = scanner_id }, cancellationToken);
            return NoContent();
        }

        [HttpGet("{scanner_id}/events")]
        public async Task<IActionResult> GetEvents(
            Guid scanner_id,
            [FromQuery] Guid? eventId = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetScannerEventsRequest
            {
                ScannerId = scanner_id,
                EventId = eventId,
                Page = page,
                PageSize = pageSize
            }, cancellationToken);
            return Ok(result);
        }

        [HttpPost("{scanner_id}/events")]
        public async Task<IActionResult> CreateEvent(
            Guid scanner_id,
            [FromBody] CreateScannerEventRequest request,
            CancellationToken cancellationToken)
        {
            request.ScannerId = scanner_id;
            var id = await _mediator.Send(request, cancellationToken);
            return Ok(new { scannerEventId = id });
        }

        [HttpDelete("events/{scanner_event_id}")]
        public async Task<IActionResult> RemoveEvent(Guid scanner_event_id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new RemoveScannerEventRequest { ScannerEventId = scanner_event_id }, cancellationToken);
            return NoContent();
        }
    }
}
