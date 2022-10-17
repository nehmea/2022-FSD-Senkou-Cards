using System.Linq;
using System.Windows;

namespace SenkouCards
{
    /// <summary>
    /// Interaction logic for DeckInfo.xaml
    /// </summary>
    public partial class DeckInfo : Window
    {
        public static decks currentDeck { get; set; }
        public DeckInfo(decks passedDeck)
        {
            InitializeComponent();
            currentDeck = passedDeck;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (currentDeck == null) return;
            LblDeckName.Content = currentDeck.name;
            TblCardsNumber.Text = currentDeck.cards.ToList().Count.ToString();
            TblDescription.Text = currentDeck.description;
        }
    }
}
