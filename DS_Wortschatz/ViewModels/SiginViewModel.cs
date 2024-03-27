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

namespace DS_Wortschatz.ViewModels
{
    public partial class SiginViewModel : ObservableObject
    {
        Account? _account;
        
        [ObservableProperty]
        private string signiUser = "";
        [ObservableProperty]
        private string signinPassword = "";
        [ObservableProperty]
        private string signiEmail = "";

        private int isAdminOff = 2;

        private const MessageBoxImage exclamation = MessageBoxImage.Exclamation;
        private const MessageBoxButton ok = MessageBoxButton.OK;
        private const string warning = "Warning!";

        
        [RelayCommand]
        private void AddUser()
        {
            if (string.IsNullOrEmpty(SigniUser) || string.IsNullOrEmpty(SigninPassword) || string.IsNullOrEmpty(SigniEmail))
            {
                MessageBox.Show("Pleas enter info: ", "Warning!", MessageBoxButton.OK);
            }
            else 
            { 

             string hash = "";
             // Compute Hash
             using (SHA256 sha256Hash = SHA256.Create())
             {
                 hash = GetHash(sha256Hash, SigninPassword);
                
             }
                using (DS_WortschatzDBContext context = new DS_WortschatzDBContext())
                {

                    if (_account == null)
                    {
                        using (DS_WortschatzDBContext dbContext = new DS_WortschatzDBContext())
                        {
                            bool usernameExists = dbContext.Accounts.Where(a => a.Username.Equals(SigniUser)).Any();
                            bool emailExists = dbContext.Accounts.Where(a => a.Email.Equals(SigniEmail)).Any();

                            if (usernameExists == true)
                            {
                                MessageBox.Show($"Username {SigniUser} Exists", warning, ok, exclamation);

                            }
                            else if (IsEmailValid(SigniEmail) == false)
                            {
                                MessageBox.Show($"Email {SigniEmail} is not valid", warning, ok, exclamation);

                            }
                            else if (emailExists == true)
                            {
                                MessageBox.Show($"Email {SigniEmail} Exists", warning, ok, exclamation);

                            }
                            else
                            {
                                Account account = new Account();

                                account.Username = SigniUser;
                                account.Password = hash;
                                account.Email = SigniEmail;
                                account.IsAdmin = isAdminOff;
                                context.Accounts.Add(account);
                                context.SaveChanges();
                                Back();

                            }
                        }
                    }
                }
             }
        }
        static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        private static bool IsEmailValid(string email)
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

        [RelayCommand]
        private void Back()
        {
            //ToDo
            //Do a chk _isVisible and activ first
            var loginWindow = new LogIn();
            loginWindow.Show();
            Application.Current.Windows[0].Close();
        }
    }
}

