using System.Linq;
using System.Windows;

namespace SenkouCards
{
    /// <summary>
    /// Interaction logic for DeckInfo.xaml
    /// </summary>
    public partial class DeckInfo : Window
    {
        Decks decks;
        public static decks currentDeck { get; set; }
        public DeckInfo(decks passedDeck, Decks allDecks=null)
        {
            InitializeComponent();
            currentDeck = passedDeck;
            decks = allDecks;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (currentDeck == null) return;
            LblDeckName.Content = currentDeck.name;
            TblCardsNumber.Text = currentDeck.cards.ToList().Count.ToString();
            TblDescription.Text = currentDeck.description;
        }


        private void Btn_Attempt_Click(object sender, RoutedEventArgs e)
        {
            WindowOfficialTestEn windowOfficialTestEn = new WindowOfficialTestEn(currentDeck);
            this.Close();
            windowOfficialTestEn.Show();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            CardViewAll cardViewAll = new CardViewAll(currentDeck, decks);
            this.Close();
            cardViewAll.Show();
        }

        private void BtnDeckHistory_Click(object sender, RoutedEventArgs e)
        {
            WindowDeckHistory deckHistory = new WindowDeckHistory(currentDeck);
            deckHistory.Show();
        }
    }
}
