using coworkings_mobile.Resources.Languages;
using coworkings_mobile.ViewModels;
using Microsoft.Maui.Controls;

namespace coworkings_mobile.Views
{
    public partial class CurrentBookingPage : ContentPage
    {
        private readonly CurrentBookingViewModel _viewModel;

        public CurrentBookingPage()
        {
            InitializeComponent();
            _viewModel = (CurrentBookingViewModel)BindingContext;
            UpdateLocalization();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadCurrentBookingCommand.Execute(null);
            UpdateLocalization();
        }

        private async void NavigateToLanguageSelection(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LanguageSelectionPage());
        }

        private void UpdateLocalization()
        {
            CurrentBookingTitle.Text = AppResources.CurrentBookingTitle;
            StartDateLabel.Text = AppResources.StartDateLabel;
            StartTimeLabel.Text = AppResources.StartTimeLabel;
            EndDateLabel.Text = AppResources.EndDateLabel;
            EndTimeLabel.Text = AppResources.EndTimeLabel;
            BookingCodeLabel.Text = AppResources.BookingCodeLabel;
            NoCurrentBookingMessage.Text = AppResources.NoCurrentBookingMessage;
        }
    }
}
