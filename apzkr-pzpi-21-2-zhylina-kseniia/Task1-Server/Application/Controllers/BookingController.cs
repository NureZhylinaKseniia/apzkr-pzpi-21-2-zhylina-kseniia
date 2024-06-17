using Microsoft.AspNetCore.Mvc;
using Application.Repositories;
using Application.Models;
using System;
using System.Linq;
using Application.Controllers;
using Application.ViewModels;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Application.DBContext;
using Application.ModelsView;

[ApiController]
[Route("api/booking")]
public class BookingController : ControllerBase
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserRepository _userRepository;
    private readonly IWorkplaceRepository _workplaceRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly AppDbContext _context;

    public BookingController(Application.DBContext.AppDbContext context, IBookingRepository bookingRepository, IUserRepository userRepository, IWorkplaceRepository workplaceRepository, IRoomRepository roomRepository)
    {
        _context = context;
        _bookingRepository = bookingRepository;
        _userRepository = userRepository;
        _workplaceRepository = workplaceRepository;
        _roomRepository = roomRepository;
    }


    [HttpGet]
    public IActionResult GetAllBookings()
    {
        try
        {
            var bookings = _bookingRepository.GetAllBookings().ToList();
            return Ok(bookings);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpGet("{id}")]
    public IActionResult GetBookingById(int id)
    {
        try
        {
            var booking = _bookingRepository.GetBookingById(id);

            if (booking == null)
            {
                return NotFound($"Booking with ID {id} not found");
            }

            return Ok(booking);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpGet("byUser/{userId}")]
    public IActionResult GetBookingsByUserId(int userId)
    {
        try
        {
            var existingUser = _userRepository.GetUserById(userId);

            if (existingUser == null)
            {
                return NotFound($"User with ID {userId} not found");
            }

            var userBookings = _bookingRepository.GetBookingsByUserId(userId).ToList();
            return Ok(userBookings);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error : {ex.Message}");
        }
    }


    [NonAction]
    private async Task<Booking> FindBookingByCodeAsync(int code)
    {
        var booking = await _context.Booking.FirstOrDefaultAsync(u => u.BookingCode == code);
        return booking;
    }


    [HttpPost]
    [ProducesResponseType(201)] // Успішно створений запит
    [ProducesResponseType(400)] // Помилковий запит
    public IActionResult CreateBooking([FromBody] BookingView bookingView)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingUser = _userRepository.GetUserById(bookingView.UserId);

        List<Workplace> workplaces = new List<Workplace>();

        foreach (int workplaceId in bookingView.WorkplacesId)
        {
            Workplace workplace = _workplaceRepository.GetWorkplaceById(workplaceId);
            if (workplace != null)
            {
                workplaces.Add(workplace);
            }
        }

        int bookingCode = GenerateRandomCode();

        while(true)
        {
            var existingBooking = FindBookingByCodeAsync(bookingCode);

            if (existingBooking != null)
            {
                bookingCode = GenerateRandomCode();
            }
           else
            {
                break;
            }
        }

        var booking = new Booking
        {
            StartDateTime = bookingView.StartDateTime,
            EndDateTime = bookingView.EndDateTime,
            User = existingUser,
            Workplaces = workplaces,
            BookingCode = bookingCode
        };

        _bookingRepository.CreateBooking(booking);

        return CreatedAtAction(nameof(GetBookingById), new { id = booking.BookingId }, booking);
    }


    static int GenerateRandomCode()
    {
        Random random = new Random();
        int code = random.Next(100000, 999999);
        return code;
    }

    [HttpPut("edit/{id}")]
    public IActionResult UpdateBooking([FromRoute] int id, [FromBody] BookingView bookingView)
    {
        try
        {
            var existingBooking = _bookingRepository.GetBookingById(id);
            var existingUser = _userRepository.GetUserById(bookingView.UserId);

            List<Workplace> workplaces = new List<Workplace>();

            foreach (int workplaceId in bookingView.WorkplacesId)
            {
                Workplace workplace = _workplaceRepository.GetWorkplaceById(workplaceId);
                if (workplace != null)
                {
                    workplaces.Add(workplace);
                }
            }

            if (existingBooking == null)
            {
                return NotFound();
            }

            existingBooking.StartDateTime = bookingView.StartDateTime;
            existingBooking.EndDateTime = bookingView.EndDateTime;
            existingBooking.User = existingUser;
            existingBooking.Workplaces = workplaces;

            _bookingRepository.UpdateBooking(existingBooking);

            return Ok(existingBooking);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }


    [HttpDelete("delete/{id}")]
    public IActionResult DeleteBooking(int id)
    {
        try
        {
            var existingAdoption = _bookingRepository.GetBookingById(id);

            if (existingAdoption == null)
            {
                return NotFound($"Booking with ID {id} not found");
            }

            _bookingRepository.DeleteBooking(id);

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    static bool IsDateTimeInInterval(DateTime date, DateTime startDate, DateTime endDate)
    {
        return date >= startDate && date <= endDate;
    }

    [HttpPost("checkCode/{roomId}")]
    public IActionResult CheckValidBooking([FromRoute] int roomId, BookingVerification bookingVerification)
    {
        try
        {
            var room = _roomRepository.GetRoomById(roomId);

            var workplaces = _workplaceRepository.GetWorkplacesByRoomId(roomId);

            int roomCode = 0;
            Boolean marker = false;

            foreach (var workplace in workplaces)
            {
                var bookings = _bookingRepository.GetBookingByWorkplaceId(workplace.WorkplaceId);

                foreach (var booking in bookings)
                {
                    if (booking.BookingCode == bookingVerification.code && IsDateTimeInInterval(bookingVerification.timestamp, booking.StartDateTime, booking.EndDateTime))
                    {
                        roomCode = booking.BookingCode;
                        marker = true;
                        break;
                    }
                }

                if (marker)
                {
                    break;
                }
            }

            var bookingVerify = new BookingVerification
            {
                code = roomCode,
                timestamp = bookingVerification.timestamp
            };

            if (roomCode == 0)
            {
                return NotFound();
            }

            return Ok(bookingVerify);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
        
    }
}