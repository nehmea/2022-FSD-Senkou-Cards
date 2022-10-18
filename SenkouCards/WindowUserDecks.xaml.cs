using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SenkouCards
{
    /// <summary>
    /// Interaction logic for UserDecks.xaml
    /// </summary>
    public partial class UserDecks : Window
    {
        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        public UserDecks()
        {

            InitializeComponent();
            if (Globals.ActiveUser == null)
            {
                Globals.ActiveUser = Globals.SenkouDbAuto.users.Find(2);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> excluded = new List<string>() { "responses", "users", "userId", "decks", "deckId" };
                Globals.AddListViewColumns<attempts>(GvDecks, excluded);

                LvUserDecks.ItemsSource = Globals.SenkouDbAuto.attempts.Where(attempt => attempt.userId == Globals.ActiveUser.id).ToList();
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

        private void LvHeader_Click(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader; //get the clicked header by the RoutedEventArgs
            ListSortDirection direction;

            if (headerClicked != null && headerClicked.Role != GridViewColumnHeaderRole.Padding) //check if header is clicked
            {
                if (headerClicked != _lastHeaderClicked) //set direction to ascending if new column clicked
                {
                    direction = ListSortDirection.Ascending;
                }
                else
                {
                    if (_lastDirection == ListSortDirection.Ascending) // else set direction to opposite from previous click if smae column clicked
                    {
                        direction = ListSortDirection.Descending;
                    }
                    else
                    {
                        direction = ListSortDirection.Ascending;
                    }
                }

                var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding; // get the binding of the clicked column
                var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string; // get the first not null value of binding path or column header

                Sort(sortBy, direction, LvUserDecks); //sort 


                _lastHeaderClicked = headerClicked;
                _lastDirection = direction;
            }
        }

        private void Sort(string sortBy, ListSortDirection direction, ListView LvFoo)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(LvFoo.ItemsSource);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        private void TbxSearchDecks_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchString = TbxSearchDecks.Text;
            if (searchString == "")
            {
                LvUserDecks.ItemsSource = Globals.SenkouDbAuto.attempts.Where(attempt => attempt.userId == Globals.ActiveUser.id).ToList();
            }
            else
            {
                LvUserDecks.ItemsSource = Globals.SenkouDbAuto.attempts
                    .Include("decks")
                    .Where(attempt => attempt.userId == Globals.ActiveUser.id && attempt.decks.name.Contains(searchString))
                    .ToList();

            }

        }
        private void setButtonsStatus()
        {
            decks currentlySelectedDeck = LvUserDecks.SelectedItem as decks;
            BtnCreateDeck.IsEnabled = (Globals.ActiveUser != null);
            BtnExportDeck.IsEnabled = (currentlySelectedDeck != null && LvUserDecks.SelectedItems.Count == 1);
            BtnDeckInfo.IsEnabled = (currentlySelectedDeck != null && LvUserDecks.SelectedItems.Count == 1);
        }

    }
}
