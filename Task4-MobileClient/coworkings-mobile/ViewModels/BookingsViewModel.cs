using coworkings_mobile.Models;
using coworkings_mobile.Resources.Languages;
using coworkings_mobile.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;

namespace coworkings_mobile.ViewModels
{
    public class BookingsViewModel
    {
        private readonly BookingService _bookingService;
        private readonly AuthService _authService;

        public ObservableCollection<Booking> Bookings { get; }
        public ICommand LoadBookingsCommand { get; }

        public BookingsViewModel()
        {
            _bookingService = new BookingService();
            _authService = new AuthService();
            Bookings = new ObservableCollection<Booking>();
            LoadBookingsCommand = new Command(async () => await LoadBookings());
        }

        private async Task LoadBookings()
        {
            var user = _authService.GetUser();
            if (user != null)
            {
                var history = await _bookingService.GetUserBookings(user.UserId);
                Bookings.Clear();

                foreach (var item in history)
                {
                    Bookings.Add(item);
                }
            }
        }
    }
}