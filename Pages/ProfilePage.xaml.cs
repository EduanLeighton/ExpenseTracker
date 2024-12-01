using Microsoft.Maui.Controls;
using ExpenseTracker.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Services;
using SQLite;

namespace ExpenseTracker;

public partial class ProfilePage : ContentPage
{
    private int _userId;
    private User user = new User();

    public ProfilePage(int userId)
    {
        InitializeComponent();
        _userId = userId;

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadProfile();

        NavigationPage.SetHasBackButton(this, false);
    }

    public async Task<User> LoadProfile()
    {
        user = await App.Database.GetUserName(_userId);


        ProfileName.Placeholder = user.Username;
        ProfilePassword.Placeholder = user.Password;

        return user;
    }

    public async void OnUpdateAccountClicked(object sender, EventArgs e)
    {
        var result = await DisplayAlert("Update Account", "Are you sure you want to update your account?", "Yes", "No");

        if (result)
        {
            await App.Database.UpdateUserAsync(_userId, ProfileName.Text, ProfilePassword.Text);

            ProfileName.Placeholder = ProfileName.Text;
            ProfilePassword.Placeholder = ProfilePassword.Text;

            ProfileName.Text = "";
            ProfilePassword.Text = "";


        }
    }

    public async void OnDeleteAccountClicked(object sender, EventArgs e)
    {

        var result = await DisplayAlert("Delete Account", "Are you sure you want to delete your account?", "Yes", "No");

        if (result)
        {
            var user = await App.Database.GetUserName(_userId);
            await App.Database.SaveUserAsync(user);

            await Navigation.PopAsync();
        }
    }

    private async void GoBackButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}