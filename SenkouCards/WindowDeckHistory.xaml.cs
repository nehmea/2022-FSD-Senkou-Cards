using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SenkouCards
{
    /// <summary>
    /// Interaction logic for WindowDeckHistory.xaml
    /// </summary>
    public partial class WindowDeckHistory : Window
    {
        private decks currentDeck { get; set; }
        public WindowDeckHistory(decks passedDeck)
        {
            InitializeComponent();
            this.currentDeck = passedDeck;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentDeck == null) return;
                List<string> excluded = new List<string>() { "responses", "users", "userId", "decks", "deckId" };
                Globals.AddListViewColumns<attempts>(GvAttempts, excluded);

                //LvAttemptsHistory.ItemsSource = Globals.SenkouDbAuto.attempts.Where(attempt => attempt.userId == Globals.ActiveUser.id && attempt.deckId == currentDeck.id).ToList();
                LvAttemptsHistory.ItemsSource = currentDeck.attempts.ToList();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Unable to access the database:\n" + ex.Message, "Fatal database error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }

        private void BtnExportDeckAttempts_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LvAttemptsHistory_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void LvAttemptsHistory_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            attempts currentlySelectedAttempt = LvAttemptsHistory.SelectedItem as attempts;
            if (currentlySelectedAttempt == null || LvAttemptsHistory.SelectedItems.Count > 1) return;

            //List<responses> currentlySelectedResponses = Globals.SenkouDbAuto.responses.Where(response => response.attemptId == currentlySelectedAttempt.id).ToList();

            WindowAttemptInfo newWindow = new WindowAttemptInfo(currentlySelectedAttempt.id);
            newWindow.ShowDialog();
            LvAttemptsHistory.SelectedItem = null;
        }

        private void BtnAttemptInfo_Click(object sender, RoutedEventArgs e)
        {
            attempts currentlySelectedAttempt = LvAttemptsHistory.SelectedItem as attempts;
            if (currentlySelectedAttempt == null || LvAttemptsHistory.SelectedItems.Count > 1) return;

            //List<responses> currentlySelectedResponses = Globals.SenkouDbAuto.responses.Where(response => response.attemptId == currentlySelectedAttempt.id).ToList();

            WindowAttemptInfo newWindow = new WindowAttemptInfo(currentlySelectedAttempt.id);
            newWindow.ShowDialog();
            LvAttemptsHistory.SelectedItem = null;
        }

        private void BtnExportAttempts_Click(object sender, RoutedEventArgs e)
        {

        }

        private void setButtonStatus()
        {
            attempts currentlySelectedAttempt = LvAttemptsHistory.SelectedItem as attempts;
            BtnAttemptInfo.IsEnabled = (currentlySelectedAttempt != null);
        }
    }
}
