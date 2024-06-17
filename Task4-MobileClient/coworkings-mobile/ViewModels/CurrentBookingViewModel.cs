using coworkings_mobile.Models;
using coworkings_mobile.Services;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace coworkings_mobile.ViewModels
{
    public class CurrentBookingViewModel : INotifyPropertyChanged
    {
        private readonly BookingService _bookingService;
        private readonly AuthService _authService;
        private Booking _booking;
        private string _countdownText;
        private bool _hasCurrentBooking;
        private bool _noCurrentBooking;

        public Booking Booking
        {
            get => _booking;
            set
            {
                _booking = value;
                OnPropertyChanged(nameof(Booking));
            }
        }

        public string CountdownText
        {
            get => _countdownText;
            set
            {
                _countdownText = value;
                OnPropertyChanged(nameof(CountdownText));
            }
        }

        public bool HasCurrentBooking
        {
            get => _hasCurrentBooking;
            set
            {
                _hasCurrentBooking = value;
                OnPropertyChanged(nameof(HasCurrentBooking));
            }
        }

        public bool NoCurrentBooking
        {
            get => _noCurrentBooking;
            set
            {
                _noCurrentBooking = value;
                OnPropertyChanged(nameof(NoCurrentBooking));
            }
        }

        public ICommand LoadCurrentBookingCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CurrentBookingViewModel()
        {
            _bookingService = new BookingService();
            _authService = new AuthService();
            LoadCurrentBookingCommand = new Command(async () => await LoadCurrentBooking());
        }

        private async Task LoadCurrentBooking()
        {
            var user = _authService.GetUser();
            if (user != null)
            {
                Booking = await _bookingService.GetCurrentBooking(user.UserId);
                if (Booking != null)
                {
                    HasCurrentBooking = true;
                    NoCurrentBooking = false;
                    StartCountdown();
                }
                else
                {
                    HasCurrentBooking = false;
                    NoCurrentBooking = true;
                }
            }
        }

        private void StartCountdown()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if(Booking == null)
                {
                    return false;
                }
                var timeRemaining = Booking.EndDateTime - DateTime.Now;
                if (timeRemaining > TimeSpan.Zero)
                {
                    CountdownText = $"Time left: {timeRemaining:hh\\:mm\\:ss}";
                    return true;
                }
                else
                {
                    CountdownText = "Expired";
                    HasCurrentBooking = false;
                    NoCurrentBooking = true;
                    return false;
                }
            });
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
