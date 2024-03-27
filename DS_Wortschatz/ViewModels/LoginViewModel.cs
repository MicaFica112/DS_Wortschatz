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

namespace DS_Wortschatz.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {

        [ObservableProperty]
        private string user = "";
        [ObservableProperty]
        private string password = "";

        private int UserId { get; set; }
        private int IsAdmin { get; set; }


        private const MessageBoxImage exclamation = MessageBoxImage.Exclamation;
        private const MessageBoxButton ok = MessageBoxButton.OK;
        private const string warning = "Warning!";


        [RelayCommand]
        private void LogInChk()
        {
            if (User.Equals(""))
            {
                MessageBox.Show("Enter username ", warning, ok, exclamation);
            }
            else if (Password.Equals(""))
            {
                MessageBox.Show("Enter Pass", warning, ok, exclamation);
            }

            else
            {
                string hash = "";
                // Compute Hash
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    hash = GetHash(sha256Hash, Password);
                }
                using (DS_WortschatzDBContext dbContext = new DS_WortschatzDBContext())
                {

                    int number = dbContext.Accounts.Where(a => a.Username == User && a.Password == hash).Count();
                    if (number > 0)
                    {
                        //?????????????????????????????????????????
                        
                        var getUserId = dbContext.Accounts.Where(x => x.Username == User).Select(x => x.Id).ToList();
                        var getIsAdmin = dbContext.Accounts.Where(x => x.Username == User).Select(x => x.IsAdmin).ToList();

                        if (getUserId != null && getIsAdmin != null)
                        {
                            foreach (var id in getUserId)
                            {
                                UserId = id;
                            }
                            foreach (var x in getIsAdmin)
                            {
                               IsAdmin = x;
                            }
                        }
                    
                       LoginOK(UserId, IsAdmin);    
                    }
                    else
                    {
                        MessageBox.Show("Wrong Username or Password", warning, ok, exclamation);
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

        private static void LoginOK(int userId, int isAdmin)
        {
            MessageBox.Show($"Korisnik ID {userId} Korisnik je Admin {isAdmin}" , warning, ok, exclamation);
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Application.Current.Windows[0].Close();
        }
        [RelayCommand]
        private static void GoToSignin()
        {
            //ToDo
            //Do a chk _isVisible and activ first
            var signinWindow = new SignIn();
            signinWindow.Show();
            Application.Current.Windows[0].Close();
        }


        // Verify a hash against a string.
        /* static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
         {
             // Hash the input.
             var hashOfInput = GetHash(hashAlgorithm, input);

             // Create a StringComparer an compare the hashes.
             StringComparer comparer = StringComparer.OrdinalIgnoreCase;

             return comparer.Compare(hashOfInput, hash) == 0;
         }*/
    }
}
        

    



