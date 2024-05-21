using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DS_Wortschatz.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DS_Wortschatz.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        Worter? _words;

        // Properties to bind to UI for new and edited words and articles
        [ObservableProperty]
        private string newSWord = string.Empty;
       
        [ObservableProperty]
        private string newDWord = string.Empty;

        [ObservableProperty]
        private string tajTaTo = string.Empty;
        [ObservableProperty]
        private string derDieDas = string.Empty;

        [ObservableProperty]
        private string tajTaToNew = string.Empty;

        [ObservableProperty]
        private string derDieDasNew = string.Empty;

        public int IdD { get; set; }
        public int IdS { get; set; }

        // Command that updates article properties based on which button is pressed
        [RelayCommand]
        private void ShowArtikleFromButton(Button button)
        {
            if (button == null) return;

            switch (button.Name)
            {
                case "buttonDer":
                    DerDieDas = button.Content.ToString();
                    break;
                case "buttonDerNew":
                    DerDieDasNew = button.Content.ToString();
                    break;
                case "buttonDie":
                    DerDieDas = button.Content.ToString();
                    break;
                case "buttonDieNew":
                    DerDieDasNew = button.Content.ToString();
                    break;
                case "buttonDas":
                    DerDieDas = button.Content.ToString();
                    break;
                case "buttonDasNew":
                    DerDieDasNew = button.Content.ToString();
                    break;
                case "buttonTaj":
                    TajTaTo = button.Content.ToString();
                    break;
                case "buttonTajNew":
                    TajTaToNew = button.Content.ToString();
                    break;
                case "buttonTa":
                    TajTaTo = button.Content.ToString();
                    break;
                case "buttonTaNew":
                    TajTaToNew = button.Content.ToString();
                    break;
                case "buttonTo":
                    TajTaTo = button.Content.ToString();
                    break;
                case "buttonToNew":
                    TajTaToNew = button.Content.ToString();
                    
                    break;

                default:
                    break;
            }
        }
        // Fetches ID based on article words from the database
        public void GetWordId(string artikleD, string artikleS)
        {

            using (DS_WortschatzDBContext dbContext = new DS_WortschatzDBContext())
            {
                if (!artikleD.IsNullOrEmpty() && !artikleS.IsNullOrEmpty())
                {
                    var artikleIdDFinde = dbContext.ArtikelDs.Where(a => a.DerDieDas == artikleD).
                        Select(x => new { x.Id }).ToList();
                    foreach (var item in artikleIdDFinde)
                    {
                        IdD = item.Id;
                    }
                    var artikleIdSFinde = dbContext.Artikels.Where(a => a.TajTaTo == artikleS).
                        Select(x => new { x.IdS }).ToList();
                    foreach (var item in artikleIdSFinde)
                    {
                        IdS = item.IdS;
                    }
                }
            }
        }
        // Command to delete a word, includes confirmation dialog
        [RelayCommand]
        public void DeleteWord()
        {
            GetWordId(DerDieDas, TajTaTo);

            if (ShowDword != null && ShowSword != null && IdD != 0 && IdS != 0)
            {
                MessageBoxResult warningDel = MessageBox.Show("Willst du wirklich Wort löschen?", "Frage", MessageBoxButton.YesNo);
                if (warningDel == MessageBoxResult.Yes)
                {
                    using (DS_WortschatzDBContext dbContext = new DS_WortschatzDBContext())
                    {
                        Worter? wordToDelete = dbContext.Worters.Find(WordId);

                        if (wordToDelete != null)
                        {
                            dbContext.Worters.Remove(wordToDelete);
                            dbContext.SaveChanges();
                            ResetFields();
                            MessageBox.Show("Wort erfolgreich gelöscht.", "OK", MessageBoxButton.OK);
                        }
                    }

                }
            }

        }
        // Command to edit a word, includes confirmation dialog
        [RelayCommand]
        private void EditWord()
        {
            GetWordId(DerDieDas, TajTaTo);

            if (ShowDword != null && ShowSword != null && IdD != 0 && IdS != 0)
            {
                MessageBoxResult worningEdit = MessageBox.Show("Willst du das Wort wirklich ändern??", "Frage", MessageBoxButton.YesNo);
                if (worningEdit == MessageBoxResult.Yes)
                {
                    using (DS_WortschatzDBContext dbContext = new DS_WortschatzDBContext())
                    {
                        Worter? wordToEdit = dbContext.Worters.Find(WordId);
                        if (wordToEdit != null)
                        {
                            wordToEdit.DartikelId = IdD;
                            wordToEdit.SartikelId = IdS;
                            wordToEdit.Deutsch = ShowDword;
                            wordToEdit.Serbisch = ShowSword;
                            dbContext.SaveChanges();
                            ResetFields();
                            MessageBox.Show("Wort erfolgreich geändert.", "OK", MessageBoxButton.OK);
                        }
                    }
                }
            }
        }
        // Command to add a new word, includes validation for existing words
        [RelayCommand]
        private void AddNewWord()
        {
            GetWordId(DerDieDasNew, TajTaToNew);

            if (NewDWord != null && NewSWord != null && IdD != 0 && IdS != 0)
            {
                using (DS_WortschatzDBContext dbContext = new DS_WortschatzDBContext())
                {
                    if (_words == null)
                    {
                        bool wordDExists = dbContext.Worters.Where(a => a.Deutsch.Equals(NewDWord)).Any();
                        bool wordSExists = dbContext.Worters.Where(a => a.Serbisch.Equals(NewSWord)).Any();

                        if (wordDExists == true && wordSExists == true)
                        {
                            MessageBox.Show("Wort existiert bereits.");
                        }
                        else
                        {
                            Worter worter = new Worter();

                            worter.Deutsch = NewDWord;
                            worter.Serbisch = NewSWord;
                            worter.DartikelId = IdD;
                            worter.SartikelId = IdS;
                            dbContext.Worters.Add(worter);
                            dbContext.SaveChanges();
                            ResetFields();
                            MessageBoxResult InfoAddWord = MessageBox.Show("Word erfolgreich hinzugefügt.", "Erfolg", MessageBoxButton.OK);

                        }   

                    }
                }

            }
            else
            {
                MessageBox.Show("Ungültige Eingabe zum Hinzufügen eines Wortes.");
            }
        }
        // Helper method to reset fields after operations
        private void ResetFields()
        {
            NewSWord = string.Empty;
            TajTaTo = string.Empty;
            DerDieDas = string.Empty;
            TajTaToNew = string.Empty;
            DerDieDasNew = string.Empty;
            NewDWord = string.Empty;
            ShowDword = string.Empty;
            ShowSword = string.Empty;
        }

    }
}
    

