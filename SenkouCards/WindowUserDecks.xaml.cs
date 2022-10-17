using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SenkouCards
{
    /// <summary>
    /// Interaction logic for UserDecks.xaml
    /// </summary>
    public partial class UserDecks : Window
    {
        public UserDecks()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> excluded = new List<string>() { "responses", "users", "userId" };
                Globals.AddListViewColumns<attempts>(GvDecks, excluded);

                // get list of unique deckIds
                // for each of these deckIds get the count of attempts, and the top score (max of score) and its date
                // 
                List<attempts> listOfAttempts = Globals.SenkouDbAuto.attempts.Where(attempt => attempt.userId == Globals.userId).ToList();
                Console.WriteLine(listOfAttempts);
                foreach (attempts attempt in listOfAttempts)
                {
                    Console.WriteLine(attempt.ToString());
                }
                //create a list of lists
                //run foreach attempt in listOfAttempts, get deckId
                //LvUserDecks.ItemsSource = Globals.SenkouDbAuto.attempts.Where(attempt => attempt.userId == Globals.userId).decks.ToList(); // equivalent of SELECT * FROM People
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

        }

        /**
         * 
         */
        private void BtnExportDeck_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
