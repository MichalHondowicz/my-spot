﻿using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Models;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("reservations")]
public class ReservationController : ControllerBase
{
    private static int _id = 1;
    private static readonly List<Reservation> Reservations = new();

    private static readonly List<string> ParkingSpotNames = new()
    {
        "P1", "P2", "P3", "P4", "P5"
    };

    [HttpGet]
    public ActionResult<IEnumerable<Reservation>> Get() => Ok(Reservations);

    [HttpGet("{id:int}")]
    public ActionResult<Reservation> Get(int id)
    {
        var reservation = Reservations.SingleOrDefault(x => x.Id == id);
        if (reservation is null)
        {
            return NotFound();
        }
        return Ok(reservation);
    }

    [HttpPost]
    public ActionResult Post(Reservation reservation)
    {
        if (ParkingSpotNames.All(x => x != reservation.ParkingSpotName))
        {
            return BadRequest();
        }

        var ifAlreadyReserved= Reservations.Any(x=>x.ParkingSpotName == reservation.ParkingSpotName && x.Date == reservation.Date);
        if (ifAlreadyReserved)
        {
            return BadRequest();
        }
        
        reservation.Id = _id;
        reservation.Date = DateTime.UtcNow.AddDays(1).Date;
        _id++;
        Reservations.Add(reservation);
        return CreatedAtAction(nameof(Get), new {id = reservation.Id}, null);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id,Reservation reservation)
    {
        var existingReservation = Reservations.SingleOrDefault(x => x.Id == id);
        if (existingReservation is null)
        {
            return NotFound();
        }

        existingReservation.LicensePlate = reservation.LicensePlate;
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var existingReservation = Reservations.SingleOrDefault(x => x.Id == id);
        if (existingReservation is null)
        {
            return NotFound();
        }

        Reservations.Remove(existingReservation);
        return NoContent();
    }
}