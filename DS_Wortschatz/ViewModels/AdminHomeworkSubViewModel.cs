using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DS_Wortschatz.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DS_Wortschatz.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public ObservableCollection<Worter> ListOfWords { get; private set; } = new ObservableCollection<Worter>();
        public ObservableCollection<Worter> ListOfSelectedWords { get; private set; } = new ObservableCollection<Worter>();
        public ObservableCollection<Account> ListOfUsers { get; private set; } = new ObservableCollection<Account>();

        [ObservableProperty]
        private Worter? selectedAvailableWord;

        [ObservableProperty]
        private Account? selectedUser;

        // Fetches a list of users from the database
        public void GetListOfUsers()
        {
            using (DS_WortschatzDBContext dbContext = new DS_WortschatzDBContext())
            {
                ListOfUsers.Clear();
                var getUsers = dbContext.Accounts.ToList();// .Limited to 30Take(30)


                foreach (var user in getUsers)
                {
                    ListOfUsers.Add(user);
                }
            }

        }
        // Fetches a list of words from the database
        public void GetListOfWords()
        {
            using (DS_WortschatzDBContext dbContext = new DS_WortschatzDBContext())
            {
                ListOfWords.Clear();
                var getWords = dbContext.Worters.ToList();// Limited to 5 .Take(5)


                foreach (var word in getWords)
                {
                    ListOfWords.Add(word);
                }
            }
        }
        // Command to move a selected word to the list of selected words
        [RelayCommand]
        private void MoveToSelected()
        {
           if (SelectedAvailableWord != null && !ListOfSelectedWords.Contains(SelectedAvailableWord))
            {
                ListOfSelectedWords.Add(SelectedAvailableWord);
                ListOfWords.Remove(SelectedAvailableWord);
        }
            }
        // Command to remove a word from the selected list back to the available list
        [RelayCommand]
        private void RemoveFromSelected()
        {
            if (ListOfWords != null && SelectedAvailableWord != null && !ListOfWords.Contains(SelectedAvailableWord))
            {
                ListOfWords.Add(SelectedAvailableWord);
                ListOfSelectedWords.Remove(SelectedAvailableWord);
            }
        }
        // Command to send the selected words to a user, creating an AdminGame entry in the database
        [RelayCommand]
         private void SendToUser()
        {
          
             if (SelectedUser != null && ListOfSelectedWords != null)
             {
                
                using (DS_WortschatzDBContext dbContext = new DS_WortschatzDBContext())
                {
                    // Check if there are existing game for the selected user and delete them
                    var existingAdminGame = dbContext.AdminGames.Where(x => x.UserId == SelectedUser.Id).ToList();
                    if (existingAdminGame.Any())
                    {
                        dbContext.AdminGames.RemoveRange(existingAdminGame);
                        dbContext.SaveChanges(); // Ensure deletion is committed before adding new entries
                    }
                    // Add new words as new games for the user
                    foreach (Worter word in ListOfSelectedWords)
                    {
                        var adminGame = new AdminGame();

                        adminGame.UserId = SelectedUser.Id;
                        adminGame.IdW = word.Idw;
                        adminGame.DartikelId = word.DartikelId;
                        adminGame.SartikelId = word.SartikelId;
                        dbContext.AdminGames.Add(adminGame); 
                        
                    }
                    dbContext.SaveChanges();
                    ListOfSelectedWords.Clear();
                    GetListOfWords();    
                   
                    MessageBox.Show($"Neues Admin game fur Benuzer {SelectedUser.Username} erfolgreich erstellt.", "OK", MessageBoxButton.OK);
                }   
            }
        }
    }
}
