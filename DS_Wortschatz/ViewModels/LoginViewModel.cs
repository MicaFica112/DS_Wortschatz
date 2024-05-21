using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DS_Wortschatz.Models;
using DS_Wortschatz.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DS_Wortschatz.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        // Properties bound to user input fields on the UI
        [ObservableProperty]
        private string user = string.Empty;
        [ObservableProperty]
        private string password = string.Empty;

        // Model to hold user data upon successful login
        User userData = new User();

        private const MessageBoxImage exclamation = MessageBoxImage.Exclamation;
        private const MessageBoxButton ok = MessageBoxButton.OK;
        private const string warning = "Warnung!";

        /// <summary>
        /// Command executed when the login button is pressed.
        /// </summary>
        [RelayCommand]
        private void LogInChk()
        {
            if (User.Equals(""))
            {
                MessageBox.Show("Geben Sie den Benutzernamen ein", warning, ok, exclamation);
            }
            else if (Password.Equals(""))
            {
                MessageBox.Show("Geben Sie Pass ein", warning, ok, exclamation);
            }

            else
            {
                // Hash the password input using SHA256
                string hash = string.Empty;
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    hash = GetHash(sha256Hash, Password);
                }
                // Check the hashed password against the database
                using (DS_WortschatzDBContext dbContext = new DS_WortschatzDBContext())
                {

                    int number = dbContext.Accounts.Where(a => a.Username == User && a.Password == hash).Count();
                    if (number > 0)
                    {
                        var getUserData = dbContext.Accounts.Where(a => a.Username == User).
                                                             Select(x => new{x.Id,x.Username,
                                                                             x.IsAdmin, x.Email}).ToList();
                        if (getUserData != null)
                        {
                            foreach (var item in getUserData)
                            {
                                userData.Id = item.Id;
                                userData.UserName = item.Username;
                                userData.IsAdmin = item.IsAdmin;
                                userData.Email = item.Email;
                            }
                        }
                        LoginOK(userData);    
                    }
                    else
                    {
                        MessageBox.Show("Benutzername oder Passwort falsch", warning, ok, exclamation);
                    }
                }
            }
        }
        /// <summary>
        /// Helper method to hash strings using the specified hash algorithm.
        /// </summary>
        public static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        private static void LoginOK(User user)
        {
            // Instantiate the view model and load user data
            var mainWindowViewModel = new MainWindowViewModel();
            mainWindowViewModel.GetUserData(user);

            // Create the main window with the initialized view model
            var mainWindow = new MainWindow(mainWindowViewModel);
            //LogIn window chk
            var loginWindow = Application.Current.Windows.OfType<LogIn>().FirstOrDefault();
            // Close the current login window safely
            if (loginWindow != null)
            {
                // Set the position of the MainWindow to the same as the LoginWindow
               mainWindow.Left = loginWindow.Left;
               mainWindow.Top = loginWindow.Top;
               // Show the main window
               mainWindow.Show();
               loginWindow.Close();
            }
        }

        [RelayCommand]
        private static void GoToSignin()
        {   
            var loginWindow = Application.Current.Windows.OfType<LogIn>().FirstOrDefault();
            var signinWindow = new SignIn();
  
            if (loginWindow != null)
            {  
                signinWindow.Left = loginWindow.Left; 
                signinWindow.Top = loginWindow.Top;

                signinWindow.Show();
                loginWindow.Close();
            }
            
        }
    }
}
        

    



