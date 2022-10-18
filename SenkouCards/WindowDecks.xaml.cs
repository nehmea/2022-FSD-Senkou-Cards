using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Window = System.Windows.Window;

namespace SenkouCards
{
    /// <summary>
    /// Interaction logic for Decks.xaml
    /// </summary>
    public partial class Decks : Window
    {
        public Decks()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            setButtonsStatus();

            try
            {
                List<string> excluded = new List<string>() { "attempts", "cards", "users" };
                Globals.AddListViewColumns<decks>(GvDecks, excluded);
                LvDecks.ItemsSource = Globals.SenkouDbAuto.decks.ToList(); // equivalent of SELECT * FROM People
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Fatal database error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }

        /**
         * 
         */
        private void LvUserDecks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            setButtonsStatus();
        }

        /**
         * 
         */
        private void LvUserDecks_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            decks currentlySelectedDeck = LvDecks.SelectedItem as decks;
            if (currentlySelectedDeck == null) return;
            if (LvDecks.SelectedItems.Count > 1) return;

            DeckInfo newWindow = new DeckInfo(currentlySelectedDeck);
            newWindow.ShowDialog();
            LvDecks.SelectedItem = null;
        }

        /**
         * 
         */
        private void BtnCreateDeck_Click(object sender, RoutedEventArgs e)
        {

        }

        /**
         * 
         */
        private void BtnImportDeck_Click(object sender, RoutedEventArgs e)
        {

        }

        /**
         * 
         */
        private void BtnDeckInfo_Click(object sender, RoutedEventArgs e)
        {
            decks currentlySelectedDeck = LvDecks.SelectedItem as decks;
            if (currentlySelectedDeck == null) return;
            if (LvDecks.SelectedItems.Count > 1) return;

            DeckInfo newWindow = new DeckInfo(currentlySelectedDeck);
            newWindow.ShowDialog();
        }

        /**
         * 
         */
        private void BtnExportDeck_Click(object sender, RoutedEventArgs e)
        {
            SaveToCSV();
        }

        private void BtnViewUserDecks_Click(object sender, RoutedEventArgs e)
        {
            UserDecks userDecks = new UserDecks();
            userDecks.ShowDialog();
        }

        private void TbxSearchDecks_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchString = TbxSearchDecks.Text;
            if (searchString == "")
            {
                LvDecks.ItemsSource = Globals.SenkouDbAuto.decks.ToList();
            }
            else
            {
                LvDecks.ItemsSource = Globals.SenkouDbAuto.decks.Where(deck => deck.name.Contains(searchString) || deck.description.Contains(searchString)).ToList();

            }
        }

        private void setButtonsStatus()
        {
            decks currentlySelectedDeck = LvDecks.SelectedItem as decks;
            BtnViewUserDecks.IsEnabled = (Globals.ActiveUser != null);
            BtnCreateDeck.IsEnabled = (Globals.ActiveUser != null);
            BtnExportDeck.IsEnabled = (currentlySelectedDeck != null && LvDecks.SelectedItems.Count == 1);
            BtnDeckInfo.IsEnabled = (currentlySelectedDeck != null && LvDecks.SelectedItems.Count == 1);
        }

        private void SaveToCSV()
        {
            decks currentlySelectedDeck = LvDecks.SelectedItem as decks;
            if (currentlySelectedDeck == null || LvDecks.SelectedItems.Count > 1) return;

            int deckId = currentlySelectedDeck.id;

            IList<cards> cardsList = Globals.SenkouDbAuto.cards
                .Where(card => card.deckId == deckId)
                .ToList();

            List<string> excluded = new List<string>() { "responses", "decks", "cardsAudios", "cardsImages" };

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            if (saveFileDialog.ShowDialog() == true)
            {
                List<string> lines = new List<string>();
                lines.Add(Globals.CreateFileHeaderFromProperties<cards>(excluded));
                foreach (cards card in cardsList)
                {
                    string newline = Globals.CreateStringFromProperties<cards>(card, excluded);
                    //Console.WriteLine(newline);
                    lines.Add(newline);
                }
                try
                {
                    File.WriteAllLines(saveFileDialog.FileName, lines); // ex IO/Sys
                    MessageBox.Show(this, "Export complete!", "Export Status", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex) when (ex is IOException || ex is SystemException)
                {
                    MessageBox.Show(this, "Export failed\n" + ex.Message, "Export Status", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
