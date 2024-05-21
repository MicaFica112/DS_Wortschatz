using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DS_Wortschatz.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Security.Cryptography;
using System.Windows.Documents;
using System.Net.Mail;

namespace DS_Wortschatz.ViewModels
{

    public partial class MainWindowViewModel : ObservableObject
    {
        // Observable properties that will notify the UI when changes are made.
        [ObservableProperty]
        private string oldPassword = string.Empty;
        [ObservableProperty]
        private string newPassword = string.Empty;

        private int isAdminSetToFalse = 2;

        private bool passChk;

        // Command bound to UI that triggers the verification and potential update of user data.
        [RelayCommand]
        private void ChkNewUserData()
        {
             using (DS_WortschatzDBContext dbContext = new DS_WortschatzDBContext())
             {
                // Querying the database for the password of the currently logged-in user.
                var getUserPassword = dbContext.Accounts.Where(a => a.Id == Id).Select(x => new { x.Password }).ToList();

                 if (getUserPassword != null)
                 {
                     foreach (var userPasword in getUserPassword)
                     {
                         string hashedPassword = userPasword.Password;
                        // Using SHA256 hash algorithm to verify if the input old password matches the stored hash.
                        using (SHA256 sha256Hash = SHA256.Create())
                         {
                             passChk = VerifyHash(sha256Hash, OldPassword, hashedPassword);
                         }
                     }
                     if (passChk)
                     {
                        // Additional check to validate the format of the email.
                        bool eMeilChk = SiginViewModel.IsEmailValid(Email);
                         if (eMeilChk)
                         {
                            // If valid, hash the new password.
                            SHA256 sha256Hash = SHA256.Create();
                             string newHashPassword = SiginViewModel.GetHash(sha256Hash, NewPassword);
                             WriteNewUserData(newHashPassword);
                         }

                         else
                         {
                             MessageBox.Show("Bitte schreiben Sie eine gute E-Mail", "Warnung", MessageBoxButton.OK);
                         }
                     }
                     else
                     {
                         MessageBox.Show("Falsches Passwort!", "Warnung", MessageBoxButton.OK);
                     }
                 }
                 else
                 {
                     MessageBox.Show("Etwas ist schief gelaufen. Versuchen Sie es erneut.", "Warnung", MessageBoxButton.OK);
                 }
             }
        }
        // Static method to verify if the hashed version of the input matches the stored hash.
        static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
        {
            var hashOfInput = SiginViewModel.GetHash(hashAlgorithm, input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashOfInput, hash) == 0;
        }
        // Writes new user data to the database and informs the user of success.
        private void WriteNewUserData(string newHashPassword)
        {
            MessageBoxResult worningEdit = MessageBox.Show("Willst du das new Datie wirklich speiheren?", "Frage", MessageBoxButton.YesNo);
            if (worningEdit == MessageBoxResult.Yes)
            {
                using (DS_WortschatzDBContext dbContext = new DS_WortschatzDBContext())
                {
                    Account? userAcount = dbContext.Accounts.Where(a => a.Id == Id).FirstOrDefault();
                    if (userAcount != null)
                    {
                        userAcount.Username = UserName;
                        userAcount.Email = Email;
                        userAcount.Password = newHashPassword;//Hashed
                        userAcount.IsAdmin = isAdminSetToFalse;
                        dbContext.SaveChanges();
                        NewPassword = string.Empty;
                        OldPassword = string.Empty; 
                        MessageBox.Show("Daten erfolgreich geändert", "OK", MessageBoxButton.OK);
                    }
                }
               
            }
        }
        // Command to delete the user account and navigate away upon confirmation.
        [RelayCommand]
        private void DeleteUser()
        {
            MessageBoxResult worningEdit = MessageBox.Show("Möchten Sie Ihr Konto löschen?", "Frage", MessageBoxButton.YesNo);
            if (worningEdit == MessageBoxResult.Yes)
            {
                using (DS_WortschatzDBContext dbContext = new DS_WortschatzDBContext())
                {
                    Account? curnetUser = dbContext.Accounts.Find(Id);
                    if (curnetUser != null)
                    {
                        // Deleting the user account from the database.
                        dbContext.Accounts.Remove(curnetUser);
                        dbContext.SaveChanges();
                        // Navigate back to a previous view or to the login page.
                        SiginViewModel.Back();
                    }
                }
            }
        }

    }
}
