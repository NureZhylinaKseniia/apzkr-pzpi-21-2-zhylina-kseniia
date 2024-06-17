using coworkings_mobile.Resources.Languages;
using coworkings_mobile.ViewModels;

namespace coworkings_mobile.Views;

public partial class BookingsPage : ContentPage
{
    private readonly BookingsViewModel _viewModel;

    public BookingsPage()
    {
        InitializeComponent();
        _viewModel = new BookingsViewModel();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadBookingsCommand.Execute(null);
        UpdateLocalization();
    }

    private void UpdateLocalization()
    {
        PageTitleLabel.Text = AppResources.BookingTitle;
        PageTitleTextLabel.Text = AppResources.BookingTitle;
        StartDateLabelTitle.Text = AppResources.StartDateLabel;
        StartTimeLabelTitle.Text = AppResources.StartTimeLabel;
        EndDateLabelTitle.Text = AppResources.EndDateLabel;
        EndTimeLabelTitle.Text = AppResources.EndTimeLabel;
        BookingCodeLabelTitle.Text = AppResources.BookingCodeLabel;
    }

    private async void NavigateToLanguageSelection(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LanguageSelectionPage());
    }
}
