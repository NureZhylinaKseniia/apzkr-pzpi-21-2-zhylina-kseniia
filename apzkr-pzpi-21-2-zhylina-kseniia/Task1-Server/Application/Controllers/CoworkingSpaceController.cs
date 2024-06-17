using Microsoft.AspNetCore.Mvc;
using Application.Models;
using Application.Repositories;
using Application.ViewModels;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Owin.BuilderProperties;
using System.Xml.Linq;
using Application.Util;

[ApiController]
[Route("api/coworkingSpaces")]
public class CoworkingSpaceController : ControllerBase
{
    private readonly ICoworkingSpaceRepository _coworkingSpaceRepository;
    private readonly IManagerRepository _managerRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IWorkplaceRepository _workplaceRepository;
    private readonly IBookingRepository _bookingRepository;

    public CoworkingSpaceController(ICoworkingSpaceRepository coworkingSpaceRepository, IManagerRepository managerRepository, IRoomRepository roomRepository, IWorkplaceRepository workplaceRepository, IBookingRepository bookingRepository)
    {
        _coworkingSpaceRepository = coworkingSpaceRepository;
        _managerRepository = managerRepository;
        _roomRepository = roomRepository;
        _workplaceRepository = workplaceRepository;
        _bookingRepository = bookingRepository;
    }


    [HttpGet]
    public IActionResult GetAllCoworkingSpaces()
    {
        var coworkingSpaces = _coworkingSpaceRepository.GetAllCoworkingSpaces().ToList();
        return Ok(coworkingSpaces);
    }


    [HttpGet("{id}")]
    public IActionResult GetCoworkingSpaceById(int id)
    {
        var coworkingSpace = _coworkingSpaceRepository.GetCoworkingSpaceById(id);

        if (coworkingSpace == null)
        {
            return NotFound();
        }

        return Ok(coworkingSpace);
    }


    [HttpPost]
    public IActionResult AddCoworkingSpace([FromBody] CoworkingSpaceAddView coworkingSpaceAddView)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var coworkingSpace = new CoworkingSpace
        {
            Name = coworkingSpaceAddView.Name,
            Address = coworkingSpaceAddView.Address,
            City = coworkingSpaceAddView.City,
            Manager = null
        };

        _coworkingSpaceRepository.AddCoworkingSpace(coworkingSpace);

        return CreatedAtAction(nameof(GetCoworkingSpaceById), new { id = coworkingSpace.CoworkingSpaceId }, coworkingSpace);
    }


    [HttpPut("addManager")]
    public IActionResult AddCoworkingSpaceManager([FromBody] CoworkingSpaceAddManagerView coworkingSpaceAddManagerView)
    {
        try
        {
            var existingCoworkingSpace = _coworkingSpaceRepository.GetCoworkingSpaceById(coworkingSpaceAddManagerView.CoworkingSpaceId);
            var existingManager = _managerRepository.GetManagerById(coworkingSpaceAddManagerView.ManagerId);

            if (existingCoworkingSpace == null && existingManager == null)
            {
                return NotFound();
            }

            existingCoworkingSpace.Manager = existingManager;

            _coworkingSpaceRepository.UpdateCoworkingSpace(existingCoworkingSpace);

            return Ok(existingCoworkingSpace);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpPut("edit/{id}")]
    public IActionResult UpdateCoworkingSpace([FromRoute] int id, [FromBody] CoworkingSpaceView coworkingSpaceView)
    {
        try
        {
            var existingCoworkingSpace = _coworkingSpaceRepository.GetCoworkingSpaceById(id);
            var existingManager = _managerRepository.GetManagerById(coworkingSpaceView.ManagerId);

            if (existingCoworkingSpace == null)
            {
                return NotFound();
            }

            existingCoworkingSpace.Name = coworkingSpaceView.Name;
            existingCoworkingSpace.Address = coworkingSpaceView.Address;
            existingCoworkingSpace.City = coworkingSpaceView.City;
            existingCoworkingSpace.Manager = existingManager;

            _coworkingSpaceRepository.UpdateCoworkingSpace(existingCoworkingSpace);

            return Ok(existingCoworkingSpace);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpDelete("delete/{id}")]
    public IActionResult DeleteCoworkingSpace(int id)
    {
        var existingCoworkingSpace = _coworkingSpaceRepository.GetCoworkingSpaceById(id);

        if (existingCoworkingSpace == null)
        {
            return NotFound();
        }

        _coworkingSpaceRepository.DeleteCoworkingSpace(id);

        return NoContent();
    }


    [HttpPost("mostPopularBookingsTime/{id}")]
    public IActionResult GetMostPopularTimePeriod(int id)
    {
        try
        {
            var bookingAnalyzer = new BookingAnalyzer(_roomRepository, _workplaceRepository, _bookingRepository);
            var (start, end) = bookingAnalyzer.GetMostPopularTimePeriod(id);

            var result = new
            {
                Start = start,
                End = end,
                Duration = (end - start).TotalMinutes
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
