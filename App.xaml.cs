using ExpenseTracker.Services;

namespace ExpenseTracker
{
    public partial class App : Application
    {

        static DatabaseService? database;

        public static DatabaseService Database
        {
            get
            {
                if (database == null)
                {
                    try
                    {
                        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ExpenseTracker.db3");
                        database = new DatabaseService(dbPath);
                    }
                    catch (Exception ex)
                    {
                        // Log the exception for debugging
                        System.Diagnostics.Debug.WriteLine($"Error initializing DatabaseService: {ex.Message}");
                        throw; // Optionally rethrow or handle gracefully
                    }
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
        }
    }
}
