using Microsoft.Maui.Controls;
using ExpenseTracker.Model;
using System;

#if WINDOWS
using Windows.System;
#endif

namespace ExpenseTracker
{
    public partial class AccountCreationPage : ContentPage
    {
        public AccountCreationPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Disable the back button
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void OnCreateAccountClicked(object sender, EventArgs e)
        {
            var username = usernameEntry.Text;
            var password = passwordEntry.Text;
            var confirmPassword = confirmPasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                await DisplayAlert("Error", "Please enter both a username and password, and choose what type of account you want.", "OK");
                return;
            }

            if (password != confirmPassword)
            {
                await DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            var existingUser = await App.Database.GetUserAsync(username);
            if (existingUser != null)
            {
                await DisplayAlert("Error", "Username already exists.", "OK");
                return;
            }

            var newUser = new ExpenseTracker.Model.User
            {
                Username = username,
                Password = password,
            };

            await App.Database.SaveUserAsync(newUser);

            await DisplayAlert("Success", "Account created successfully!", "OK");

            var createdUser = await App.Database.GetUserAsync(username);
            await Navigation.PushAsync(new MainPage(createdUser.Id));
            // Fetch the user again to get the ID and navigate to the MainPage with the UserId


        }
    }
}
