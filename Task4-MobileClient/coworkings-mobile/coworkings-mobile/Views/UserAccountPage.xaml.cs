using coworkings_mobile.Resources.Languages;
using coworkings_mobile.Services;

namespace coworkings_mobile.Views;

public partial class UserAccountPage : ContentPage
{
    private readonly AuthService _authService;

    public UserAccountPage()
    {
        InitializeComponent();
        _authService = new AuthService();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadUserData();
        UpdateLocalization();
    }

    private void LoadUserData()
    {
        var user = _authService.GetUser();
        if (user != null)
        {
            FullNameLabel.Text = user.FullName;
            EmailLabel.Text = user.Email;
        }
    }

    private void UpdateLocalization()
    {
        Title = AppResources.UserAccountTitle;
        PageTitleLabel.Text = AppResources.UserAccountTitle;
        PageTitleTextLabel.Text = AppResources.UserAccountTitle;
        EditButton.Text = AppResources.EditButtonText;
        LogoutButton.Text = AppResources.LogoutButtonText;
        FullNameLabelTitle.Text = AppResources.FullNameLabelTitle;
        EmailLabelTitle.Text = AppResources.EmailLabelTitle;
    }

    private async void EditButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///EditUserPage");
    }

    private async void LogoutButton_Clicked(object sender, EventArgs e)
    {
        await _authService.LogoutAsync();
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void NavigateToLanguageSelection(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LanguageSelectionPage());
    }
}