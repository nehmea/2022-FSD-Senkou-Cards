using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SenkouCards
{
    /// <summary>
    /// Interaction logic for WindowCreateDeck.xaml
    /// </summary>
    public partial class WindowCreateDeck : Window
    {
        Decks decks;
        public WindowCreateDeck(Decks wd=null)
        {
            decks = wd;
            InitializeComponent();
        }

        private void Btn_Create_Click(object sender, RoutedEventArgs e)
        {
            bool deckOfficial = Rbn_Yes.IsChecked == true;
            string deckName = Tbx_Name.Text;
            string deckDescription = Tbx_Description.Text;

            try
            {
                SenkoucardsConfig Sc = new SenkoucardsConfig();
                decks newDeck = new decks { ownerId = Globals.ActiveUser.id, name = deckName, description = deckDescription, isOfficial = deckOfficial };
                Sc.decks.Add(newDeck);
                Sc.SaveChanges();
                decks.LvDecks.ItemsSource = Globals.SenkouDbAuto.decks.ToList(); //updates the list with new deck
                this.DialogResult = true;
            }
            catch (Exception ex) when (ex is DataException || ex is SystemException)
            {
                MessageBox.Show("Unexpected Error... Who are we kidding, we expected all sorts of errors. "+ex.Message,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
    }
}
