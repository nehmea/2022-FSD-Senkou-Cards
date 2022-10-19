using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace SenkouCards
{
    /// <summary>
    /// Interaction logic for WindowAttemptInfo.xaml
    /// </summary>
    public partial class WindowAttemptInfo : Window
    {

        private attempts currentAttempt { get; set; }
        public WindowAttemptInfo(attempts passedAttempt)
        {
            InitializeComponent();
            currentAttempt = passedAttempt;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                /*
                List<string> excluded = new List<string>() { "responses", "decks", "cardsAudios", "cardsImages", "front", "points", "deckId" };
                Globals.AddListViewColumns<cards>(GvResponses, excluded);

                excluded = new List<string>() { "decks", "users", "userId", "score", "attemptDate", "responses" };
                Globals.AddListViewColumns<attempts>(GvResponses, excluded);

                */
                List<string> excluded = new List<string>() { "attempts", "cards", "attemptId", "cardId" };
                Globals.AddListViewColumns<responses>(GvResponses, excluded);

                if (currentAttempt == null) return;

                LvResponses.ItemsSource = currentAttempt.responses.ToList();
                TbkDeckName.Text = currentAttempt.decks.name;
                TbkDeckDescription.Text = currentAttempt.decks.description;
                //LvResponses.ItemsSource = Globals.SenkouDbAuto.responses.Where(response => attemptId == attemptId).ToList();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Fatal database error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }
        private void LvHeader_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnExportResponses_Click(object sender, RoutedEventArgs e)
        {
            //attempts currentlySelectedAttempt = LvUserDecks.SelectedItem as attempts;
            //if (currentlySelectedAttempt == null || LvUserDecks.SelectedItems.Count > 1) return;

            //int deckId = currentlySelectedAttempt.deckId;

            //List<cards> cardsList = Globals.SenkouDbAuto.cards
            //.Where(card => card.deckId == deckId)
            //.ToList();

            List<responses> responsesList = currentAttempt.responses.ToList();
            List<string> excluded = new List<string>() { "attempts" };

            try
            {
                Globals.SaveToCSV<responses>(excluded, responsesList);
                MessageBox.Show(this, "Export complete!", "Export Status", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) when (ex is IOException || ex is SystemException)
            {
                MessageBox.Show(this, "Export failed\n" + ex.Message, "Export Status", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
