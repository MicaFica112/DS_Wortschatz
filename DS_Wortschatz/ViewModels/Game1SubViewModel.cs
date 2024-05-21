using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DS_Wortschatz.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace DS_Wortschatz.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public List<string> Options { get; set; } = new List<string> { "Standardspiel", "Admin-Spiel" };

        public ObservableCollection<Worter> Words { get; } = new ObservableCollection<Worter>();

        // Properties decorated with [ObservableProperty] will notify the UI of changes automatically
        [ObservableProperty]
        private Worter? currentWord;

        [ObservableProperty]
        private int numberOfWords = 5;

        private int currentIndex = -1;

        [ObservableProperty]
        private Brush feedbackColor = Brushes.Transparent;

        [ObservableProperty]
        private bool isGameActive;

        [ObservableProperty]
        private string startStopButtonLabel = "Spiel starten";

        [ObservableProperty]
        private Visibility extraButtonsVisibility = Visibility.Visible;

        [ObservableProperty]
        private Visibility scoreButtonsVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private int correctCount;

        [ObservableProperty]
        private int incorrectCount;

        [ObservableProperty]
        private int? selectedGameMode = 0; // 0 for random words, 1 for Admins words

        // Commands that are bound to UI elements (buttons) to invoke methods
        [RelayCommand]
        private void IncreseNummberOfWord()
        {
            if (NumberOfWords < 25)
                NumberOfWords += 5;

        }
        [RelayCommand]
        private void DecreseNummberOfWord()
        {
            if (NumberOfWords > 5)
                NumberOfWords -= 5;
        }
        
        // Command that toggles the state of the game
        [RelayCommand]
        public void ToggleGame()
        {
            if (IsGameActive)
            {
                StopGame();
            }
            else
            {
                StartGame();
            }
        }
        // Method to start the game based on the selected mode
        private void StartGame()
            {
            if (SelectedGameMode == 0) // Random words
            {
                using (DS_WortschatzDBContext dbContext = new DS_WortschatzDBContext())
                {
                    Words.Clear();
                    var getWords = dbContext.Worters.OrderBy(r => Guid.NewGuid()).
                                                     Take(NumberOfWords).
                                                     ToList();

                    foreach (var word in getWords)
                    {
                        Words.Add(word);
                    }
                    GetOtherSettings();
                }
            }
             else // Admins words 
            {
                using (DS_WortschatzDBContext dbContext = new DS_WortschatzDBContext())
                {
                    Words.Clear();
                    var getWords = dbContext.AdminGames.Where(x => x.UserId == Id).ToList();

                    foreach (var word in getWords)
                    {
                        Worter? wordForPlay = dbContext.Worters.Find(word.IdW);
                        if (wordForPlay != null)
                        {

                            Words.Add(wordForPlay);
                        }
                        GetOtherSettings();
                    }
                }
            }
        }
        // Helper method to initialize settings for a new game
        private void GetOtherSettings()
        {
            currentIndex = 0;
            CurrentWord = Words.FirstOrDefault();
            FeedbackColor = Brushes.Transparent;
            IsGameActive = true;
            StartStopButtonLabel = "Spiel stoppen";
            ExtraButtonsVisibility = Visibility.Collapsed;
            ScoreButtonsVisibility = Visibility.Visible;
            CorrectCount = 0;
            IncorrectCount = 0;
        }
        // Method to stop the game and show final score
        private void StopGame()
        {
            UpdateStats();
            Words.Clear();
            CurrentWord = null;
            FeedbackColor = Brushes.Transparent;
            IsGameActive = false;
            StartStopButtonLabel = "Spiel starten";
            ExtraButtonsVisibility = Visibility.Visible;
            ScoreButtonsVisibility = Visibility.Collapsed;
            MessageBox.Show($"Ihre Punktzahl Richtig: {CorrectCount} Falsch: {IncorrectCount}", "Spiel vorbei", MessageBoxButton.OK);
        }

        // Command to check the user's answer and provide feedback
        [RelayCommand]
        public void CheckArticle(string article)
        {
            if (!IsGameActive) return;


            int articleParse = int.Parse(article);
            if (CurrentWord != null && articleParse == CurrentWord.DartikelId)
            {
                FeedbackColor = Brushes.Green;
                CorrectCount++;
                currentIndex++;
                if (currentIndex < Words.Count)
                {
                    CurrentWord = Words[currentIndex];
                    FeedbackColor = Brushes.Transparent;
                }
                else 
                {
                    StopGame();
                }        
            }
            else
            {
                FeedbackColor = Brushes.Red;
                IncorrectCount++;
            }
        }
            
        

    }
}
