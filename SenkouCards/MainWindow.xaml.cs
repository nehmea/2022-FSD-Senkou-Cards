using System.Windows;

namespace SenkouCards
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var cardCreation = new CardCreation();
            cardCreation.Show();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.ShowDialog();
        }
    }
}
