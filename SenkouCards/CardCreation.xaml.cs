using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace SenkouCards
{
    /// <summary>
    /// Interaction logic for CardCreation.xaml
    /// </summary>
    public partial class CardCreation : Window
    {
        private SenkoucardsConfig dbContext;
        private decks currentDeck = null;
        public CardCreation(decks passedDeck)
        {
            InitializeComponent();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            currentDeck = passedDeck;
            try
            {
                dbContext = new SenkoucardsConfig();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Error reading from database\n" + ex.Message, "Fatal error",
                     MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }

        }
        private void RtbFront_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = RtbFront.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = RtbFront.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = RtbFront.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = RtbFront.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;
            temp = RtbFront.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbFontSize.Text = temp.ToString();
        }

        //Optional: to review
        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
                TextRange rangeFront = new TextRange(RtbFront.Document.ContentStart, RtbFront.Document.ContentEnd);
                rangeFront.Load(fileStream, DataFormats.Rtf);
                TextRange rangeBack = new TextRange(RtbBack.Document.ContentStart, RtbBack.Document.ContentEnd);
                rangeBack.Load(fileStream, DataFormats.Rtf);
            }
        }

        //Also optional
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*"
            };
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
                TextRange rangeFront = new TextRange(RtbFront.Document.ContentStart, RtbFront.Document.ContentEnd);
                rangeFront.Save(fileStream, DataFormats.Rtf);
                TextRange rangeBack = new TextRange(RtbBack.Document.ContentStart, RtbBack.Document.ContentEnd);
                rangeBack.Save(fileStream, DataFormats.Rtf);
            }
        }
        private byte[] _imageBytes = null;
        private void BtnUploadImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //TODO: EXCEPTIONS
                OpenFileDialog dlg = new OpenFileDialog
                {
                    Filter = "Image files (*.jpg;*.png;*.jpeg)|*.jpg;*.png;*.jpeg;|All Files (*.*)|*.*",
                    RestoreDirectory = true
                };
                if (dlg.ShowDialog() == true)
                {
                    string selectedImageName = dlg.FileName;
                    TbxImagePath.Text = selectedImageName;

                    using (var fs = new FileStream(selectedImageName, FileMode.Open, FileAccess.Read))  //IOException, ArgumentException, UnauthorizedAccessException
                    {
                        _imageBytes = new byte[fs.Length];
                        fs.Read(_imageBytes, 0, Convert.ToInt32(fs.Length));
                    }
                }

            }
            catch (Exception ex) when (ex is IOException || ex is ArgumentException || ex is UnauthorizedAccessException)
            {
                MessageBox.Show(this, "Image upload failed\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        private byte[] _audioBytes = null;

        public decks CurrentDeck { get; }

        private void BtnUploadAudio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //TODO: EXCEPTIONS
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "Audio files (*.mp3;*.m4a;*.flac;*.wav)|*.mp3;*.m4a;*.flac;*.wav;|All Files (*.*)|*.*";
                dlg.RestoreDirectory = true;
                if (dlg.ShowDialog() == true)
                {
                    string selectedAudioName = dlg.FileName;
                    TbxAudioPath.Text = selectedAudioName;

                    using (var fs = new FileStream(selectedAudioName, FileMode.Open, FileAccess.Read))
                    {
                        _audioBytes = new byte[fs.Length];
                        fs.Read(_audioBytes, 0, Convert.ToInt32(fs.Length));
                    }

                }

            }
            catch (Exception ex) when (ex is IOException || ex is ArgumentException || ex is UnauthorizedAccessException)
            {
                MessageBox.Show(this, "Image upload failed\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void CmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
                RtbFront.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
        }

        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                RtbFront.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Invalid Font Size: \n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUploadServer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //TODO: EXCEPTIONS

                //Front
                string frontText;
                TextRange tr = new TextRange(RtbFront.Document.ContentStart, RtbFront.Document.ContentEnd); // ArgumentException
                using (MemoryStream ms = new MemoryStream())
                {
                    tr.Save(ms, DataFormats.Rtf);
                    frontText = Encoding.ASCII.GetString(ms.ToArray());
                }

                //Back
                string backText;
                TextRange tr2 = new TextRange(RtbBack.Document.ContentStart, RtbBack.Document.ContentEnd);  //ArgumentException
                using (MemoryStream ms = new MemoryStream())
                {
                    tr2.Save(ms, DataFormats.Rtf);
                    backText = Encoding.ASCII.GetString(ms.ToArray());
                }

                //Points
                //Numbers only exception and validation: TODO
                int.TryParse(TbxPoints.Text, out int points);
                cards newCard = new cards { front = frontText, back = backText, points = points, deckId = currentDeck.id };

                try
                {
                    dbContext.cards.Add(newCard);
                    cardsAudios CA;
                    cardsImages CI;
                    //Image
                    if (!String.IsNullOrEmpty(TbxImagePath.Text))
                    {
                         CI = new cardsImages
                        {
                            image = _imageBytes,
                            cards = newCard
                        };
                        dbContext.cardsImages.Add(CI);

                    }
                    //Audio
                    if (!String.IsNullOrEmpty(TbxAudioPath.Text))
                    {
                        CA = new cardsAudios
                        {
                            audio = _audioBytes,
                            cards = newCard
                        };
                        dbContext.cardsAudios.Add(CA);
                    }
                    dbContext.SaveChanges();

                }
                catch (SystemException ex)
                {
                    MessageBox.Show(this, "Error reading from database:\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, "Invalid info uploaded\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ResetFields();
            CardViewAll cardViewAll = new CardViewAll(currentDeck);
            this.Close();
            cardViewAll.Show();


        }
        private void BtnImportServer_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ResetFields()
        {
            TbxAudioPath.Text = "";
            TbxImagePath.Text = "";
            RtbFront.Document.Blocks.Clear();
            RtbBack.Document.Blocks.Clear();
            TbxPoints.Text = "";
        }

    }
}
