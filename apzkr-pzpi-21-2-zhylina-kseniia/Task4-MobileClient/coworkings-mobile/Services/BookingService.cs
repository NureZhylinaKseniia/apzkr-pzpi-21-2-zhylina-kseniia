using coworkings_mobile.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace coworkings_mobile.Services
{
    public class BookingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public BookingService()
        {
            _httpClient = new HttpClient();
            _apiBaseUrl = "http://though-forget.gl.at.ply.gg:9088";
        }

        public async Task<List<Booking>> GetUserBookings(int userId)
        {
            var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/booking/byUser/{userId}");
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var history = JsonConvert.DeserializeObject<List<Booking>>(responseContent);
                return history;
            }
            else
            {
                return null;
            }
        }

        public async Task<Booking> GetCurrentBooking(int userId)
        {
            var bookings = await GetUserBookings(userId);
            return bookings?.Find(booking => booking.IsValid);
        }
    }
}
