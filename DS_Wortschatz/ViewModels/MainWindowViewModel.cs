using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DS_Wortschatz.Models;
using DS_Wortschatz.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DS_Wortschatz.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public int Id { get; private set; }
        public int IsAdmin { get; private set;}

        [ObservableProperty]
        public string? userName = string.Empty;
        [ObservableProperty]
        public string? email = string.Empty;

        [ObservableProperty]
        public string? adminTabVis = "Visible";

        [ObservableProperty]
        public int selectedIndex;

        /// <summary>
        /// Determines the visibility of the Admin tab based on user's administrative status.
        /// </summary>
        public void AdminTab()
        {
            if (IsAdmin == 1)
            {
                AdminTabVisible();
            }
            else
            {
                AdminTabNotVisible();
            }
        }
        private void AdminTabVisible() 
        {
            AdminTabVis = "Visible";
            GetListOfWords();
            GetListOfUsers();
        }
        private void AdminTabNotVisible()
        {
            AdminTabVis = "Hidden";
        }
        /// <summary>
        /// Updates ViewModel properties based on data from the UserModel.
        /// </summary>
        public void GetUserData(User user)
        {
            this.Id = user.Id;
            this.UserName = user.UserName;
            this.Email = user.Email;
            this.IsAdmin = user.IsAdmin;
        }

        [RelayCommand]
        public static void LogOut()
        {
            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            var loginWindow = new LogIn();


            if (mainWindow != null)
            {
                loginWindow.Left = mainWindow.Left;
                loginWindow.Top = mainWindow.Top;
                loginWindow.Show();
                mainWindow.Close();
            }
        }
        [RelayCommand]
        private void CloseApp()
        {
            Application.Current.Shutdown();
        }
    }
}
