using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DS_Wortschatz.ViewModels;



namespace DS_Wortschatz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        private MainWindowViewModel _viewModel;

        //Konstractor
        public MainWindow(MainWindowViewModel viewModel)
        {      
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
            _viewModel = viewModel; 
            this.DataContext = _viewModel;
            _viewModel.AdminTab();
            _viewModel.GetStats();
        }
        /// <summary>
        /// Event handler for when the selection changes in any TabControl within this window.
        /// </summary>
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handles logic when the selected tab changes in a specific TabControl
            if (sender is TabControl StatTab && StatTab.SelectedIndex == 2)
            {
                _viewModel.GetStats();
            }
            if (sender is TabControl derDieDasTab && derDieDasTab.SelectedIndex == 0)
            {
               _viewModel.ClearSearchFields();
            }
            if (sender is TabControl AdminTab && AdminTab.SelectedIndex == 4)
            {
               _viewModel.ClearSearchFields();
            }
        }
    }
}
