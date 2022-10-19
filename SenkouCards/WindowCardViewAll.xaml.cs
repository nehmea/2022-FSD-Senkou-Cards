using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SenkouCards
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class CardViewAll : Window
    {
        private decks currentDeck = null;
        public CardViewAll(decks passedDeck)
        {
            try
            {

            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Error reading from database\n" + ex.Message, "Fatal error",
                     MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }

            InitializeComponent();
            currentDeck = passedDeck;
        }

        private void BtnCreateCard_Click(object sender, RoutedEventArgs e)
        {
            CardCreation cardCreation = new CardCreation(currentDeck);
            this.Close();
            cardCreation.Show();
        }

        private void BtnDeleteCard_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TbxSearchCards_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void LvUserCards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void LvUserCards_MouseDoubleClick(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> excluded = new List<string>() { "responses", "cardsImages", "cardsAudios", "decks", "deckId" };
                Globals.AddListViewColumns<cards>(GvCards, excluded);


                List<cards> cardsList = Globals.SenkouDbAuto.cards.Where(cards => cards.deckId == currentDeck.id).ToList();
                foreach (cards c in cardsList)
                {
                    var document = new FlowDocument();

                    var textRange = new TextRange(document.ContentStart, document.ContentEnd);
                    using (var sm = new MemoryStream(Encoding.ASCII.GetBytes(c.front)))
                    {
                        textRange.Load(sm, DataFormats.Rtf);
                    }
                    c.front = textRange.Text;
                }
                LvCards.ItemsSource = cardsList;



            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Fatal database error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }

        }

        private void BtnUpdateDeckInfo_Click(object sender, RoutedEventArgs e)
        {
            if (currentDeck == null) return;
            try
            {
                currentDeck.name = TbxName.Text;                    // ArgumentException
                currentDeck.description = TbxDescription.Text;
                Globals.SenkouDbAuto.SaveChanges();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, ex.Message, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
