using System;
using System.Collections.Generic;
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
            try
            {
                List<string> excluded = new List<string>() { "attempts", "cards" };
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
        private void TbxSearchDecks_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // fetch decks where title or description includes search text

        }

        /**
         * 
         */
        private void LvUserDecks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /**
         * 
         */
        private void LvUserDecks_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

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

        }

        private void BtnAddSelected_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnViewUserDecks_Click(object sender, RoutedEventArgs e)
        {
            UserDecks userDecks = new UserDecks();
            userDecks.ShowDialog();
        }
    }
}
