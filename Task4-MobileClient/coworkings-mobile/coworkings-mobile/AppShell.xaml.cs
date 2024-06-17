using coworkings_mobile.Resources.Languages;
using coworkings_mobile.Views;


namespace coworkings_mobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            UpdateLocalization();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateLocalization();
        }

        private async void NavigateToLanguageSelection(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LanguageSelectionPage());
        }

        private void UpdateLocalization()
        {
            CurrentBookingTab.Title = AppResources.CurrentBookingTitle;
            BookingsTab.Title = AppResources.BookingsTitle;
            UserAccountTab.Title = AppResources.UserAccountTitle;
        }
    }
}
