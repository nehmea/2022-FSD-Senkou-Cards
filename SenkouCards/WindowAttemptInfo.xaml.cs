using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SenkouCards
{
    /// <summary>
    /// Interaction logic for WindowAttemptInfo.xaml
    /// </summary>
    public partial class WindowAttemptInfo : Window
    {

        private int attemptId { get; set; }
        public WindowAttemptInfo(int passedAttemptId)
        {
            InitializeComponent();
            attemptId = passedAttemptId;
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


                LvResponses.ItemsSource = Globals.SenkouDbAuto.responses.Where(response => attemptId == attemptId).ToList();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Fatal database error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }

        private void LvResponses_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void LvResponses_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void LvHeader_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeckInfo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnExportResponses_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
