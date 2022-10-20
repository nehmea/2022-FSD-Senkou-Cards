using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SenkouCards
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class CardViewAll : Window
    {
        Decks allDecks;

        private decks currentDeck = null;
        public CardViewAll(decks passedDeck, Decks decksFromLoaded=null)
        {

            InitializeComponent();
            currentDeck = passedDeck;
            allDecks = decksFromLoaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                //ListView
                List<string> excluded = new List<string>() { "responses", "cardsImages", "cardsAudios", "decks", "deckId" };
                Globals.AddListViewColumns<cards>(GvCards, excluded);


                List<cards> cardsList = Globals.SenkouDbAuto.cards.Where(cards => cards.deckId == currentDeck.id).ToList();
                foreach (cards c in cardsList)
                {
                    c.front = Globals.convertedRtf(c.front);
                    c.back = Globals.convertedRtf(c.back);
                }
                LvCards.ItemsSource = cardsList;

                //Deck Info
                TbxName.Text = currentDeck.name;
                TbxDescription.Text = currentDeck.description;
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Fatal database error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }

        }

        private void BtnCreateCard_Click(object sender, RoutedEventArgs e)
        {
            CardCreation cardCreation = new CardCreation(currentDeck);
            this.Close();
            cardCreation.Show();
        }

        private void BtnDeleteCard_Click(object sender, RoutedEventArgs e)
        {

            cards currSelectedCard = LvCards.SelectedItem as cards;
            if (currSelectedCard == null) return;
            var result = MessageBox.Show(this, "Are you sure you want to delete this entry?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            try
            {
                //remove cardImage
                List<cardsImages> cardsImagesList = Globals.SenkouDbAuto.cardsImages.Where(cardsImages => cardsImages.cardId == currSelectedCard.id).ToList();
                foreach (cardsImages ci in cardsImagesList)
                {
                    Globals.SenkouDbAuto.cardsImages.Remove(ci);
                }

                //remove cardAudio
                List<cardsAudios> cardsAudiosList = Globals.SenkouDbAuto.cardsAudios.Where(cardsAudios => cardsAudios.cardId == currSelectedCard.id).ToList();
                foreach (cardsAudios ca in cardsAudiosList)
                {
                    Globals.SenkouDbAuto.cardsAudios.Remove(ca);
                }

                //remove card
                Globals.SenkouDbAuto.cards.Remove(currSelectedCard);
                Globals.SenkouDbAuto.SaveChanges();
                List<cards> cardsList = Globals.SenkouDbAuto.cards.Where(cards => cards.deckId == currentDeck.id).ToList();
                foreach (cards c in cardsList)
                {
                    c.front = Globals.convertedRtf(c.front);
                    c.back = Globals.convertedRtf(c.back);
                }
                LvCards.ItemsSource = cardsList;
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Error reading from database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TbxSearchCards_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void LvUserCards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnDeleteCard.IsEnabled = LvCards.SelectedItem != null;
        }

        private void LvUserCards_MouseDoubleClick(object sender, SelectionChangedEventArgs e)
        {

        }



        private void BtnUpdateDeckInfo_Click(object sender, RoutedEventArgs e)
        {
            if (currentDeck == null) return;
            try
            {
                currentDeck.name = TbxName.Text;                    // ArgumentException
                currentDeck.description = TbxDescription.Text;
                Globals.SenkouDbAuto.SaveChanges();
                allDecks.LvDecks.ItemsSource = Globals.SenkouDbAuto.decks.ToList();
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
