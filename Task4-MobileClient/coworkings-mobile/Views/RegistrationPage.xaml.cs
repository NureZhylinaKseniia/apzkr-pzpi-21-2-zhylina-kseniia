namespace coworkings_mobile.Views;

using coworkings_mobile.Resources.Languages;
using coworkings_mobile.ViewModels;
using Microsoft.Extensions.Logging.Abstractions;

public partial class RegistrationPage : ContentPage
{
	public RegistrationPage()
	{
		InitializeComponent();
        BindingContext = new RegistrationViewModel();
        UpdateLocalization();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateLocalization();
    }

    private void UpdateLocalization()
    {
        Title = AppResources.RegisterButtonText;
        PasswordEntry.Placeholder = AppResources.PasswordPlaceholder;
        EmailEntry.Placeholder = AppResources.EmailPlaceholder;
        FullNameEntry.Placeholder = AppResources.FullNamePlaceholder;
        RegisterButton.Text = AppResources.RegisterButtonText;
    }
}