using DS_Wortschatz.ViewModels;
using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DS_Wortschatz.Views
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
       private LoginViewModel loginViewModel;
        public LogIn()
        {
            // Prevents the window from being resizable
            this.ResizeMode = ResizeMode.NoResize;
            // Call method to initialize all components in the XAML file
            InitializeComponent();
            // Create an instance of LoginViewModel
            loginViewModel = new LoginViewModel();
            // Set this instance as the DataContext for data binding
            this.DataContext = loginViewModel;


        }
        /// <summary>
        /// Event handler for password change in the PasswordBox.
        /// Updates the ViewModel's password property when the user types in the PasswordBox.
        /// </summary>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Ensure that the DataContext is not null to avoid null reference exceptions
            // Safely cast sender to PasswordBox and dynamically update the DataContext's Password property
            // Using dynamic to avoid explicit cast which would require including ViewModel's specific type or assembly
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).Password = ((PasswordBox)sender).Password; }
        }
    }
}
