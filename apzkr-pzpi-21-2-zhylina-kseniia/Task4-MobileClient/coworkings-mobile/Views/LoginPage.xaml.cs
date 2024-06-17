namespace coworkings_mobile.Views;

using coworkings_mobile.Resources.Languages;
using coworkings_mobile.ViewModels;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
        BindingContext = new LoginViewModel();
        UpdateLocalization();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateLocalization();
    }

    private void UpdateLocalization()
    {
        Title = AppResources.LoginButtonText;
        LoginEntry.Placeholder = AppResources.LoginPlaceholder;
        PasswordEntry.Placeholder = AppResources.PasswordPlaceholder;
        LoginButton.Text = AppResources.LoginButtonText;
    }
}