using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
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
using static System.Net.WebRequestMethods;

namespace SenkouCards
{
    /// <summary>
    /// Interaction logic for WindowOfficialTestEn.xaml
    /// </summary>
    public partial class WindowOfficialTestEn : Window
    {
        decks currentDeck = null;
        cards currentCard = null;
        List<cards> cardList = null;
        List<cards> dictionary = null;
        int score = 0;
        public WindowOfficialTestEn(decks passedDeck)
        {
            currentDeck = passedDeck;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Random rdm=new Random();
            cardList = dictionary =currentDeck.cards.OrderBy(card => rdm.Next()).ToList();
            readCard();
            sentenceAPI();
            randomizeButtons();
            Lbl_DeckName.Content = currentDeck.name;
        }

        private void readCard()
        {
            currentCard = cardList[0];
            cardList.RemoveAt(0);
        }

        private string sentenceAPI()
        {
            using (var client = new HttpClient())
            {
                string currWord = currentCard.front.ToLower();

                string uri = "https://od-api.oxforddictionaries.com/api/v2/sentences/en-us/" + currWord;

                var appSettings = ConfigurationManager.AppSettings;
                string key = appSettings["OX_API_KEY"];
                string id = appSettings["OX_API_ID"];

                client.DefaultRequestHeaders.Add("app_id", id);
                client.DefaultRequestHeaders.Add("app_key", key);
                var endpoint = new Uri(uri);
                

                var result = client.GetAsync(endpoint).Result;
                string json = result.Content.ReadAsStringAsync().Result;

                var jo = JObject.Parse(json);
                string sentence = jo["results"][0]["lexicalEntries"][0]["sentences"][0]["text"].ToString();

                string blankedSentence = sentence.Replace(currWord, "______");

                Lbl_GeneratedSentence.Content=blankedSentence;

            }


            return null;
        }

        private void randomizeButtons()
        {
            Random rand = new Random();
            int answerIndex =rand.Next(4);

            string answerString = currentCard.front;
            string wrongAnswer;

            int numOfWords=dictionary.Count();
            string [] choices=new string[4]; 

            for(int i=0;i<4;i++)
            {
                var button = (Button)this.FindName("Btn_Ans" + (i+1));
                
                if (i==answerIndex)
                {
                    button.Content = answerString;
                    button.Tag = "Right";
                }
                else
                {
                    do
                    {
                        wrongAnswer = dictionary[rand.Next(numOfWords)].front;
                    } while (wrongAnswer == answerString);

                    button.Content = wrongAnswer;
                    button.Tag = "Wrong";
                }
            }

        }

        private void Btn_Ans1_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if(button.Tag.ToString()=="Right")
            {
                Lbl_Score.Content = Decimal.Parse(Lbl_Score.Content.ToString()) + currentCard.points;
            }
        }

        private void Btn_Ans2_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Tag.ToString() == "Right")
            {
                Lbl_Score.Content = Decimal.Parse(Lbl_Score.Content.ToString()) + currentCard.points;
            }
        }

        private void Btn_Ans3_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Tag.ToString() == "Right")
            {
                Lbl_Score.Content = Decimal.Parse(Lbl_Score.Content.ToString()) + currentCard.points;
            }
        }

        private void Btn_Ans4_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Tag.ToString() == "Right")
            {
                Lbl_Score.Content = Decimal.Parse(Lbl_Score.Content.ToString()) + currentCard.points;
            }
        }
    }
}
