using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Models;
using MySpot.Api.Services;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("reservations")]
public class ReservationController : ControllerBase
{
    private readonly ReservationsService _reservationsService = new();

    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> Get() => Ok(_reservationsService.GetAll());

    [HttpGet("{id:int}")]
    public ActionResult<Reservation> Get(int id)
    {
        var reservation = _reservationsService.Get(id);
        if (reservation is null)
        {
            return NotFound();
        }

        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult Post(Reservation reservation)
    {
        int? id = _reservationsService.Create(reservation);
        if (id == null)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(Get), new {id}, value: null);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id,Reservation reservation)
    {
        reservation.Id = id; 
        if (_reservationsService.Update(reservation))
        {
            return NoContent();
        }

        return NotFound();
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        if (_reservationsService.Delete(id))
        {
            return NoContent();
        }

        return NotFound();
    }
}