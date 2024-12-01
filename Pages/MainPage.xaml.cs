using Microsoft.Maui.Controls;
using ExpenseTracker.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;



namespace ExpenseTracker
{
    public partial class MainPage : ContentPage
    {
        private int _userId;
        private decimal totalSalary = 0;
        private decimal totalExpenses = 0;

        public MainPage(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadExpenseItems();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void LoadExpenseItems()
        {
            var items = await App.Database.GetExpenseItemsAsync(_userId);
            totalSalary = CalculateSalary(items);
            totalExpenses = CalculateTotalExpenses(items);

            foreach (var item in items)
            {
                AddExpenseItemToView(item);
            }
        }

        private void AddExpenseItemToView(ExpenseItem item)
        {


            var expenseLabel = new Label
            {
                Text = $"Expense: R{item.AmountExpense:F2}, Description: {item.Description}",
                FontSize = 16,
                HorizontalOptions = LayoutOptions.Start
            };

            if (item.AmountExpense != 0)
            {
                ExpenseItemsContainer.Children.Add(expenseLabel);
            }
        }


        private decimal CalculateSalary(List<ExpenseItem> items)
        {
            decimal salary = 0;
            foreach (var item in items)
            {
                salary = item.Salary;
            }
            return salary;
        }

        private decimal CalculateTotalExpenses(List<ExpenseItem> items)
        {
            decimal expenses = 0;
            foreach (var item in items)
            {
                expenses += item.AmountExpense;
            }
            return expenses;
        }



        private async void OnAddSalary(object sender, EventArgs e)
        {
            if (decimal.TryParse(salaryEntry.Text, out decimal income))
            {
                var expenseItem = new ExpenseItem
                {
                    Salary = income,
                    UserId = _userId
                };
                await App.Database.SaveExpenseItemAsync(expenseItem);
                totalSalary = income;
                AddExpenseItemToView(expenseItem);

                salaryEntry.Text = string.Empty;
                salaryLabel.Text = "Total Salary: R" + totalSalary.ToString();
                salaryLeft.Text = "Savings Left: R" + (totalSalary).ToString();

                int deletedCount = await App.Database.DeleteAllExpenseItemsAsync(_userId);
                ExpenseItemsContainer.Children.Clear();
                totalExpenses = 0;

                await DisplayAlert("Salary Updated", $"Total Salary: R{totalSalary:F2}", "OK");
            }
            else
            {
                await DisplayAlert("Invalid Input", "Please enter a valid income amount.", "OK");
            }
        }

        private async void OnAddExpense(object sender, EventArgs e)
        {
            if (decimal.TryParse(expenseEntry.Text, out decimal expense))
            {
                var expenseItem = new ExpenseItem
                {
                    AmountExpense = expense,
                    Description = expenseDescriptionEntry.Text,
                    UserId = _userId
                };
                await App.Database.SaveExpenseItemAsync(expenseItem);
                totalExpenses += expense;
                UpdateBalance();
                AddExpenseItemToView(expenseItem);
                expenseEntry.Text = string.Empty;

                await DisplayAlert("Expense Added", $"Total Expenses: R{totalExpenses:F2}", "OK");
            }
            else
            {
                await DisplayAlert("Invalid Input", "Please enter a valid expense amount.", "OK");
            }
        }


        private void UpdateBalance()
        {
            decimal savingsLeft = totalSalary - totalExpenses;
            salaryLeft.Text = $"Savings Left: R{savingsLeft:F2}";
        }

        private async void OnProfileButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProfilePage(_userId));
        }
    }
}