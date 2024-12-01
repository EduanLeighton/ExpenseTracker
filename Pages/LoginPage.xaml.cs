using Microsoft.Maui.Controls;
using System;

namespace ExpenseTracker
{
    public partial class LoginPage : ContentPage
    {
        public string? username, password;

        public LoginPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            NavigationPage.SetHasBackButton(this, false);
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var username = usernameEntry.Text;
            var password = passwordEntry.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please enter both username and password.", "OK");
                return;
            }


            var user = await App.Database.GetUserAsync(username);

            if (user == null || user.Password != password) //make it hashed
            {
                await DisplayAlert("Error", "Invalid username or password.", "OK");
                return;
            }

            await DisplayAlert("Success", "Login successful!", "OK");
            await Navigation.PushAsync(new MainPage(user.Id));

        }

        private async void OnCreateAccountClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AccountCreationPage());
        }
    }
}
