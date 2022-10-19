using System;
using System.Collections.Generic;
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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class CardViewAll : Window
    {
        decks currentDeck = null;
        public CardViewAll(decks passedDeck)
        {
            InitializeComponent();
            currentDeck = passedDeck;
        }

        private void BtnCreateCard_Click(object sender, RoutedEventArgs e)
        {
            CardCreation cardCreation = new CardCreation();
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
            if (currentDeck == null) return;
            
        }
    }
}
