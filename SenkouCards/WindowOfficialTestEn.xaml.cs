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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

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
        int unweightedScore = 0;
        List<responses> response = new List <responses>();
        bool cleanClose = false;
        
        public WindowOfficialTestEn(decks passedDeck)
        {
            currentDeck = passedDeck;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Random rdm=new Random();
            cardList=currentDeck.cards.OrderBy(card => rdm.Next()).ToList();
            dictionary = currentDeck.cards.ToList();
            DrawCard();
            Lbl_DeckName.Content = currentDeck.name;
        }
        private void DrawCard()
        {
            readCard();
            sentenceAPI();
            randomizeButtons();
        }
        private void readCard()
        {
            if (cardList.Count > 1)
            {
                currentCard = cardList[0];
                cardList.RemoveAt(0);
            }
            else if(cardList.Count == 1)
            {
                Btn_Next.Background = Brushes.Green;
                Btn_Next.Content = "Finish";
                currentCard = cardList[0];
                cardList.RemoveAt(0);
            }
            else
            {
                RegisterAttempt();
                cleanClose = true;
                this.Close();
            }
                
        }

        private void RegisterAttempt()
        {
            SenkoucardsConfig Sc = new SenkoucardsConfig();
            int prevAttempts=(from a in Sc.attempts where a.deckId == currentDeck.id && a.userId == Globals.ActiveUser.id select a).Count();
            decimal finalScore =
                (prevAttempts == 0) ?
                (decimal)unweightedScore :
                (prevAttempts == 1) ?
                (decimal)unweightedScore * (decimal)0.5 :
                (prevAttempts == 2) ?
                (decimal)unweightedScore * (decimal)0.25 :
                0;
            attempts attempt=new attempts { userId=Globals.ActiveUser.id,deckId=currentDeck.id,score=finalScore, attemptDate = DateTime.Now };
            Sc.attempts.Add(attempt);

            foreach(responses r in response)
            {

                r.attempts=attempt;
                Sc.responses.Add(r);
            }



            users user=(from u in Sc.users where u.id == Globals.ActiveUser.id select u).FirstOrDefault<users>();
            user.score= finalScore + Globals.ActiveUser.score;
            Sc.SaveChanges();
        }

        private string sentenceAPI()
        {
            using (var client = new HttpClient())
            {
                try
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
                    string sentence = jo["results"][0]["lexicalEntries"][0]["sentences"][0]["text"].ToString(); //NullReferenceException

                    string blankedSentence = censorWord(sentence, currWord);

                    TB_GeneratedSentence.Text = blankedSentence;
                }
                catch (Exception ex) when (ex is NullReferenceException)
                {
                    if (cardList.Count == 0)
                    {
                        TB_GeneratedSentence.Text = "No sentence could be generated for this word. Click finish to end the quiz.";
                        //Disables buttons
                        for(int i = 1; i <=4; i++)
                        {
                            var button =(Button)this.FindName("Btn_Ans" + i);
                            button.IsEnabled = false;
                        }
                        Btn_Next.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        DrawCard();
                    }
                        
                }
            }


            return null;
        }

        private void randomizeButtons()
        {
            Random rand = new Random();
            int answerIndex =rand.Next(4);

            string answerString = currentCard.front;
            string wrongAnswer;
            string wrongAnswersUsed = "";

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
                        
                    } while (wrongAnswer == answerString || wrongAnswersUsed.Contains(wrongAnswer));
                    wrongAnswersUsed += wrongAnswer;//ensures that no two buttons have the same wrong answer
                    button.Content = wrongAnswer;
                    button.Tag = "Wrong";
                }
            }

        }

        //This function covers edge cases where a sentence might have a modified form of the word (e.g. word is embarrass, but the sentence uses embarrassed) 
        public static string censorWord(string sentence, string censoredWord)
        {
            //string result = null;
            //string[] splits = sentence.Split(' ');
            //for (int i = 0; i < splits.Length; i++)
            //{
            //    if (splits[i].Contains(censoredWord))
            //    {
            //        splits[i] = "______";
            //    }

            //    result += splits[i] + " ";
            //}
            //    return result;
            
            return sentence.Replace(censoredWord, "______");

            
        }

        private void AnswerSelected()
        {
            for (int i = 0; i < 4; i++)
            {
                var button = (Button)this.FindName("Btn_Ans" + (i + 1));

                if (button.Tag.ToString()=="Right")
                {
                    button.Background = Brushes.Green;
                    button.Tag = "Right";
                }
                else
                {
                    button.Background = Brushes.Red;
                }
                button.IsEnabled = false;
            }
            Btn_ShowAns.Visibility = Visibility.Visible;
            Btn_Next.Visibility = Visibility.Visible;
        }

        private void Btn_Ans1_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            responses resp = new responses { cardId = currentCard.id };

            if (button.Tag.ToString()=="Right")
            {
                unweightedScore += int.Parse(Lbl_Score.Content.ToString()) + currentCard.points;
                Lbl_Score.Content = Decimal.Parse(Lbl_Score.Content.ToString()) + currentCard.points;
                resp.isCorrectResponse = true;
            }
            else
            {
                resp.isCorrectResponse = false;
            }
            response.Add(resp);
            AnswerSelected();
        }

        private void Btn_Ans2_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            responses resp = new responses { cardId = currentCard.id };

            if (button.Tag.ToString() == "Right")
            {
                unweightedScore += int.Parse(Lbl_Score.Content.ToString()) + currentCard.points;
                Lbl_Score.Content = Decimal.Parse(Lbl_Score.Content.ToString()) + currentCard.points;
                resp.isCorrectResponse = true;
            }
            else
            {
                resp.isCorrectResponse = false;
            }
            response.Add(resp);
            AnswerSelected();
        }

        private void Btn_Ans3_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            responses resp = new responses { cardId = currentCard.id };

            if (button.Tag.ToString() == "Right")
            {
                unweightedScore += int.Parse(Lbl_Score.Content.ToString()) + currentCard.points;
                Lbl_Score.Content = Decimal.Parse(Lbl_Score.Content.ToString()) + currentCard.points;
                resp.isCorrectResponse = true;
            }
            else
            {
                resp.isCorrectResponse = false;
            }
            response.Add(resp);
            AnswerSelected();
        }

        private void Btn_Ans4_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            responses resp= new responses { cardId=currentCard.id };

            if (button.Tag.ToString() == "Right")
            {
                unweightedScore += int.Parse(Lbl_Score.Content.ToString()) + currentCard.points;
                Lbl_Score.Content = Decimal.Parse(Lbl_Score.Content.ToString()) + currentCard.points;
                resp.isCorrectResponse = true;
            }
            else
            {
                resp.isCorrectResponse = false;
            }
            response.Add(resp);
            AnswerSelected();
        }

        private void Btn_Next_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 4; i++)
            {
                var button = (Button)this.FindName("Btn_Ans" + (i + 1));
                button.ClearValue(TagProperty);
                button.ClearValue(Button.BackgroundProperty);
                button.IsEnabled = true;
            }
            //Hide ShowAnswer and Next Button
            Btn_ShowAns.Visibility = Visibility.Hidden;
            Btn_Next.Visibility = Visibility.Hidden;

            Button nextButton = sender as Button;
            //More efficient than calling DrawCard and letting ReadCard handle the condition that CardList.Count=0 (since it then goes on to call sentenceAPI which will 100% throw an unnecessary error)
            if(nextButton.Content.ToString()=="Finish")
            {
                RegisterAttempt();
                cleanClose = true;
                this.Close();
            }
            else
            {
                DrawCard();
            }
            
        }

        private void Btn_Abort_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!cleanClose)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to abort this deck? Doing so will still count as an attempt", "SenkouCards - Abort", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                    if (result == MessageBoxResult.Yes)
                    {
                        RegisterAttempt();
                    }
                    else
                    {
                        cleanClose = false;
                        e.Cancel = true;
                    }


            }
            
        }
    }
}
