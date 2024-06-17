using Microsoft.AspNetCore.Mvc;
using Application.Models;
using Application.Repositories;
using Application.ViewModels;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/workplaces")]
public class WorkplaceController : ControllerBase
{
    private readonly IWorkplaceRepository _workplaceRepository;
    private readonly IRoomRepository _roomRepository;

    public WorkplaceController(IWorkplaceRepository workplaceRepository, IRoomRepository roomRepository)
    {
        _workplaceRepository = workplaceRepository;
        _roomRepository = roomRepository;   
    }


    [HttpGet]
    public IActionResult GetAllWorkplaces()
    {
        try
        {
            var workplaces = _workplaceRepository.GetAllWorkplaces().ToList();
            return Ok(workplaces);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpGet("{id}")]
    public IActionResult GetWorkplaceById(int id)
    {
        var workplace = _workplaceRepository.GetWorkplaceById(id);

        if (workplace == null)
        {
            return NotFound("Workplace not found");
        }
        
        return Ok(workplace);
    }


    [HttpGet("byRoom/{roomId}")]
    public IActionResult GetWorkplacesByRoomId(int roomId)
    {
        try
        {
            var existingRoom = _roomRepository.GetRoomById(roomId);

            if (existingRoom == null)
            {
                return NotFound($"Room with ID {roomId} not found");
            }

            var workplaceRoom = _workplaceRepository.GetWorkplacesByRoomId(roomId).ToList();
            return Ok(workplaceRoom);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error : {ex.Message}");
        }
    }


    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public IActionResult CreateWorkplace([FromBody] WorkplaceView workplaceView)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var room = _roomRepository.GetRoomById(workplaceView.RoomId);

        var workplace = new Workplace
        {
            HourlyRate = workplaceView.HourlyRate,
            Room = room
        };

        _workplaceRepository.CreateWorkplace(workplace);

        return CreatedAtAction(nameof(GetWorkplaceById), new { id = workplace.WorkplaceId }, workplace);
    }



    [HttpPut("edit/{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public IActionResult UpdateWorkplace([FromRoute] int id, [FromBody] WorkplaceView workplaceView)
    {
        try
        {
            var existingWorkplace = _workplaceRepository.GetWorkplaceById(id);
            var room = _roomRepository.GetRoomById(workplaceView.RoomId);

            if (existingWorkplace == null)
            {
                return NotFound();
            }

            existingWorkplace.HourlyRate = workplaceView.HourlyRate;
            existingWorkplace.Room = room;

            _workplaceRepository.UpdateWorkplace(existingWorkplace);
           
            return Ok(existingWorkplace);
        }
        catch (Exception ex)
        {
            // Логування помилки
            return StatusCode(500, $"Внутрішня помилка сервера: {ex.Message}");
        }
    }


    [HttpDelete("delete/{id}")]
    public IActionResult DeleteWorkplace(int id)
    {
        try
        {
            _workplaceRepository.DeleteWorkplace(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Помилка сервера");
        }
    }
}