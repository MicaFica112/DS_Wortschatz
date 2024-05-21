using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DS_Wortschatz.Models;
using DS_Wortschatz.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net.Mail;
using Humanizer;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DS_Wortschatz.ViewModels
{
    public partial class SiginViewModel : ObservableObject
    {
        Account? _account;
        
        [ObservableProperty]
        private string signiUser = string.Empty;
        [ObservableProperty]
        private string signinPassword = string.Empty;
        [ObservableProperty]
        private string signiEmail = string.Empty;

        private int isAdminSetToFalse = 2;

        private const MessageBoxImage exclamation = MessageBoxImage.Exclamation;
        private const MessageBoxButton ok = MessageBoxButton.OK;
        private const string warning = "Warnung!";

        // RelayCommand attribute for the AddUser method which handles new user registration
        [RelayCommand]
        private void AddUser()
        {
            if (string.IsNullOrEmpty(SigniUser) || string.IsNullOrEmpty(SigninPassword) || string.IsNullOrEmpty(SigniEmail))
            {
                MessageBox.Show("Bitte geben Sie Informationen ein: ", "Warnung!", MessageBoxButton.OK);
            }
            else 
            {
                // Compute Hash for the password
                string hash = string.Empty;
             // Compute Hash
             using (SHA256 sha256Hash = SHA256.Create())
             {
                 hash = GetHash(sha256Hash, SigninPassword);
                
             }
                using (DS_WortschatzDBContext context = new DS_WortschatzDBContext())
                {
                    // Check for an existing account
                    if (_account == null)
                    {
                        using (DS_WortschatzDBContext dbContext = new DS_WortschatzDBContext())
                        {
                            bool usernameExists = dbContext.Accounts.Where(a => a.Username.Equals(SigniUser)).Any();
                            bool emailExists = dbContext.Accounts.Where(a => a.Email.Equals(SigniEmail)).Any();

                            if (usernameExists == true)
                            {
                                MessageBox.Show($"Benutzername {SigniUser} Existiert", warning, ok, exclamation);

                            }
                            else if (IsEmailValid(SigniEmail) == false)
                            {
                                MessageBox.Show($"Email {SigniEmail} ist ungültig", warning, ok, exclamation);

                            }
                            else if (emailExists == true)
                            {
                                MessageBox.Show($"Email {SigniEmail} Existiert", warning, ok, exclamation);

                            }
                            else
                            {
                                Account account = new Account();

                                account.Username = SigniUser;
                                account.Password = hash;
                                account.Email = SigniEmail;
                                account.IsAdmin = isAdminSetToFalse;
                                context.Accounts.Add(account);
                                context.SaveChanges();

                                // Check if user has been added and handle related statistics
                                bool userExists = dbContext.Accounts.Where(a => a.Username.Equals(SigniUser)).Any();
                                if (userExists == true)
                                {
                                    var getUserId = dbContext.Accounts.Where(a => a.Username.Equals(SigniUser)).
                                                                       Select(x => new {x.Id}).ToList();
                                    foreach (var userId in getUserId)
                                    {
                                        Stat stat = new Stat();
                                        stat.Uid = userId.Id;
                                        stat.PlayLast = DateTime.Today;
                                        context.Stats.Add(stat);
                                        context.SaveChanges();
                                    }
                                }
                                 MessageBox.Show("Neues Konto erfolgreich erstellt.", "OK", MessageBoxButton.OK);
                                 Back();
                            }
                            
                        }
                    }
                }
             }
        }
        // Generates a hash from a password
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
        // Validates the email format
        public static bool IsEmailValid(string email)
        {
            var valid = true;

            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch
            {
                valid = false;
            }

            return valid;
        }
        // Command to navigate back to the login window
        [RelayCommand]
        public static void Back()
        {
            var siginWindow = Application.Current.Windows.OfType<SignIn>().FirstOrDefault();
            var loginWindow = new LogIn();
               
            if (siginWindow != null)
            {
                loginWindow.Left = siginWindow.Left;
                loginWindow.Top = siginWindow.Top;
                loginWindow.Show();
                siginWindow.Close();
            }
        }
    }
}

