using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DS_Wortschatz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DS_Wortschatz.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        // Observable properties that notify UI about changes. These are statistics for a game or application.
        [ObservableProperty]
        private int playedTotal;
        [ObservableProperty]
        private DateTime lastPlayed;
        [ObservableProperty]
        private int numberOfCorrectWord;
        [ObservableProperty]
        private int numberOfWrongWords;

        // Method to retrieve game statistics from the database.
        public void GetStats()
        {
            using (DS_WortschatzDBContext dbContext = new DS_WortschatzDBContext())
            {
                // Fetching statistics for a specific user identified by 'Id'.
                var stats = dbContext.Stats.
                            Where(x => x.Uid == Id).
                            Select(x => new {
                                x.PlayTotal,
                                x.PlayLast,
                                x.CorrectWords,
                                x.WrongWords }).ToList();


                if (stats != null)
                {
                    // Update the local properties with the data fetched from the database.
                    foreach (var item in stats)
                    {
                        PlayedTotal = item.PlayTotal;
                        LastPlayed = item.PlayLast;
                        NumberOfCorrectWord = item.CorrectWords;
                        NumberOfWrongWords = item.WrongWords;
                    }
                }
            }
        }
        // Method to update the player's game statistics
        private void UpdateStats()
        {
            // Increment and update statistics based on the latest game round.
            PlayedTotal++;
            LastPlayed = DateTime.Now;
            NumberOfCorrectWord += CorrectCount;
            NumberOfWrongWords += IncorrectCount;
            using (DS_WortschatzDBContext dbContext = new DS_WortschatzDBContext())
            {
                // Find the specific statistics record by 'Id'.
                Stat? statUpdate = dbContext.Stats.Find(Id);
                if (statUpdate != null)
                {
                    // Update the record with new values.
                    statUpdate.PlayTotal = PlayedTotal;
                    statUpdate.PlayLast = LastPlayed;
                    statUpdate.CorrectWords = NumberOfCorrectWord;
                    statUpdate.WrongWords = NumberOfWrongWords;
                    dbContext.SaveChanges();
                }
                

            }

        }
    }
}
