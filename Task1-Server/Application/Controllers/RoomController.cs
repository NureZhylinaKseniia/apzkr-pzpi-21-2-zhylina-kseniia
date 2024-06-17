using Microsoft.AspNetCore.Mvc;
using Application.Repositories;
using Application.Models;
using Application.ViewModels;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/rooms")]
public class RoomController : ControllerBase
{
    private readonly IRoomRepository _roomRepository;

    private readonly IUserRepository _userRepository;
    private readonly IWorkplaceRepository _animalRepository;
    private readonly ICoworkingSpaceRepository _coworkingSpaceRepository;

    public RoomController(IRoomRepository roomRepository, IUserRepository userRepository, IWorkplaceRepository animalRepository, 
        ICoworkingSpaceRepository coworkingSpaceRepository)
    {
        _roomRepository = roomRepository;
        _userRepository = userRepository;
        _animalRepository = animalRepository;
        _coworkingSpaceRepository = coworkingSpaceRepository;
    }


    [HttpGet]
    public IActionResult GetAllRooms([FromQuery] RoomParamView searchParams)
    {
        try
        {
            IQueryable<Application.Models.Room> rooms = _roomRepository.GetAllRooms();

            if (!string.IsNullOrWhiteSpace(searchParams.RoomName))
            {
                rooms = rooms.Where(a => EF.Functions.ILike(a.RoomName, "%" + searchParams.RoomName + "%"));
            }

            if (searchParams.CoworkingSpaceName != null)
            {
                rooms = rooms.Where(a => a.CoworkingSpace.Name.Equals(searchParams.CoworkingSpaceName));
            }

            return Ok(rooms.ToList());
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Error");
        }
    }


    [HttpGet("{id}")]
    public IActionResult GetRoomById(int id)
    {
        try
        {
            var room = _roomRepository.GetRoomById(id);

            if (room == null)
            {
                return NotFound($"Room with ID {id} not found");
            }

            return Ok(room);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error getting room: {ex.Message}");
        }
    }


    [HttpGet("byCoworkingSpace/{coworkingSpaceId}")]
    public IActionResult GetRoomsByCoworkingSpaceId(int coworkingSpaceId)
    {
        try
        {
            var existingCoworkingSpace = _coworkingSpaceRepository.GetCoworkingSpaceById(coworkingSpaceId);

            if (existingCoworkingSpace == null)
            {
                return NotFound($"CoworkingSpace with ID {coworkingSpaceId} not found");
            }

            var roomCoworkingSpace = _roomRepository.GetRoomsByCoworkingSpaceId(coworkingSpaceId).ToList();
            return Ok(roomCoworkingSpace);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error : {ex.Message}");
        }
    }


    [HttpPost]
    [ProducesResponseType(201)] // Успішно створений запит
    [ProducesResponseType(400)] // Помилковий запит
    public IActionResult CreateRoom([FromBody] RoomView roomView)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingCoworkingSpace = _coworkingSpaceRepository.GetCoworkingSpaceById(roomView.CoworkingSpaceId);

        var room = new Room
        {
            RoomName = roomView.RoomName,
            Description = roomView.Description,
            CoworkingSpace = existingCoworkingSpace
        };

        _roomRepository.CreateRoom(room);

        return CreatedAtAction(nameof(GetRoomById), new { id = room.RoomId }, room);
    }


    [HttpPut("edit/{id}")]
    public IActionResult UpdateRoom([FromRoute] int id, [FromBody] RoomView roomView)
    {
        try
        {
            var existingRoom = _roomRepository.GetRoomById(id);
            var existingCoworkingSpace = _coworkingSpaceRepository.GetCoworkingSpaceById(roomView.CoworkingSpaceId);

            if (existingRoom == null)
            {
                return NotFound();
            }

            existingRoom.RoomName = roomView.RoomName;
            existingRoom.Description = roomView.Description;
            existingRoom.CoworkingSpace = existingCoworkingSpace;

            _roomRepository.UpdateRoom(existingRoom);

            return Ok(existingRoom);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpDelete("delete/{id}")]
    public IActionResult DeleteRoom(int id)
    {
        try
        {
            _roomRepository.DeleteRoom(id);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest($"Error deleting room: {ex.Message}");
        }
    }
}
