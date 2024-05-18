using PasswordResetApp.Classes;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordResetApp
{
    public partial class MainWindow : Window
    {
        private PasswordManager passwordManager;
        private string filePath = "encryptedPassword.txt";

        public MainWindow()
        {
            InitializeComponent();
            passwordManager = new PasswordManager();
        }

        private void CreatePassword_Click(object sender, RoutedEventArgs e)
        {
            string password = PasswordInput.Text;
            passwordManager.CreateAndStorePassword(password, filePath);
            MessageBox.Show("Password created and encrypted.");
        }

        private async void StartAttack_Click(object sender, RoutedEventArgs e)
        {
            int maxThreads;
            if (!int.TryParse(MaxThreadsInput.Text, out maxThreads) || maxThreads <= 0)
            {
                MessageBox.Show("Please enter a valid number of threads.");
                return;
            }

            string encryptedPassword = File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(encryptedPassword))
            {
                MessageBox.Show("Please create and store a password first.");
                return;
            }

            BruteForceAttack bruteForce = new BruteForceAttack(encryptedPassword, maxThreads);

            var (password, duration) = await bruteForce.StartAttack();

            if (!string.IsNullOrEmpty(password))
            {
                ResultTextBlock.Text = $"Password found: {password}";
                TimeTextBlock.Text = $"Elapsed time: {duration.TotalSeconds:F1} seconds";
            }
            else
            {
                ResultTextBlock.Text = "Password not found.";
            }
        }
    }
}
