using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DS_Wortschatz.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DS_Wortschatz.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        // Properties bound to UI elements for capturing and displaying data
        [ObservableProperty]
        private string serachWord = string.Empty;

        [ObservableProperty]
        private string showDartikl = string.Empty;
        [ObservableProperty]
        private string showDword = string.Empty;
        [ObservableProperty]
        private string showSartikl = string.Empty;
        [ObservableProperty]
        private string showSword = string.Empty;
        public int WordId { get; private set; }

        [ObservableProperty]
        private Visibility dividingLine = Visibility.Collapsed;

        /// <summary>
        /// Command bound to a search action, retrieves word details from the database.
        /// </summary>
        [RelayCommand]
        private void SearchForWord()
        {
            if (!string.IsNullOrEmpty(SerachWord))
            {
                using (DS_WortschatzDBContext dbContext = new DS_WortschatzDBContext())
                {
                    // Check if the word exists in the database
                    bool wordExists = dbContext.Worters.Where(a => a.Deutsch.Equals(SerachWord)).Any();
                    if (wordExists == true)
                    {
                        // Retrieve word details using a projection to avoid fetching unnecessary data
                        var getWords = dbContext.Worters.
                            Where(x => x.Deutsch == SerachWord).
                            Select(x => new {x.Idw,
                                             x.Deutsch, x.Serbisch, 
                                             x.Dartikel.DerDieDas,
                                             x.Sartikel.TajTaTo} ).ToList();

                        foreach (var item in getWords)
                        {
                            WordId = item.Idw;
                            ShowDword = item.Deutsch;
                            ShowSword = item.Serbisch;
                            ShowDartikl = item.DerDieDas;
                            ShowSartikl = item.TajTaTo;
                            DerDieDas = item.DerDieDas;
                            TajTaTo = item.TajTaTo;
                            DividingLine = Visibility.Visible;
                           
                        }
                         SerachWord = string.Empty;
                        
                    }
                    else
                    {
                        MessageBox.Show("Das Wort existiert nicht.");
                    }
                }
            }            
            else
                {
                    MessageBox.Show("Geben Sie ein deutsches Wort ein");

                }
        }
        /// <summary>
        ///Helper funktion clears all the fields related to the word search results.
        /// </summary>
        public void ClearSearchFields()
        {
            SerachWord = string.Empty;
            DerDieDas = string.Empty;
            TajTaTo = string.Empty;
            ShowDartikl = string.Empty;
            ShowSartikl = string.Empty;
            ShowDword = string.Empty;
            ShowSword = string.Empty;
            DividingLine = Visibility.Collapsed;
        }
    }
}





