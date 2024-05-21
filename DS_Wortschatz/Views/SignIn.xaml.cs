using DS_Wortschatz.ViewModels;
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

namespace DS_Wortschatz.Views
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : Window
    {
        private SiginViewModel siginViewModel;
        public SignIn()
        {
            this.ResizeMode = ResizeMode.NoResize;
            InitializeComponent();
            siginViewModel = new SiginViewModel();
            this.DataContext = siginViewModel;
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).SigninPassword = ((PasswordBox)sender).Password; }
        }
    }
}
