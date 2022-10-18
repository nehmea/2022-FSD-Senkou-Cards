using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System;
using System.Drawing.Imaging;
using System.Text;

namespace SenkouCards
{
    /// <summary>
    /// Interaction logic for CardCreation.xaml
    /// </summary>
    public partial class CardCreation : Window
    {
        private SenkoucardsConfig dbContext;
        public CardCreation()
        {
            InitializeComponent();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
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
                TextRange range = new TextRange(RtbFront.Document.ContentStart, RtbFront.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);
            }
        }

        //Also optional
        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
                TextRange range = new TextRange(RtbFront.Document.ContentStart, RtbFront.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Rtf);
            }
        }
        private byte[] _imageBytes = null;
        private void btnUploadImage_Click(object sender, RoutedEventArgs e)
        {
            //TODO: EXCEPTIONS
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image files (*.jpg;*.png;*.jpeg)|*.jpg;*.png;*.jpeg;|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog() == true)
            {
                string selectedImageName = dlg.FileName;
                TbxImagePath.Text = selectedImageName;
                
                using (var fs = new FileStream(selectedImageName, FileMode.Open, FileAccess.Read))
                {
                    _imageBytes = new byte[fs.Length];
                    fs.Read(_imageBytes, 0, Convert.ToInt32(fs.Length));
                }
            }

        }


        private byte[] _audioBytes = null;
        private void btnUploadAudio_Click(object sender, RoutedEventArgs e)
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

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private void btnUploadServer_Click(object sender, RoutedEventArgs e)
        {
            //TODO: EXCEPTIONS

            //Front
            string frontText;
            TextRange tr = new TextRange(RtbFront.Document.ContentStart, RtbFront.Document.ContentEnd);
            using (MemoryStream ms = new MemoryStream())
            {
                tr.Save(ms, DataFormats.Rtf);
                frontText = Encoding.ASCII.GetString(ms.ToArray());
            }
            
            //Back
            string backText;
            TextRange tr2 = new TextRange(RtbBack.Document.ContentStart, RtbBack.Document.ContentEnd);
            using (MemoryStream ms = new MemoryStream())
            {
                tr2.Save(ms, DataFormats.Rtf);
                backText = Encoding.ASCII.GetString(ms.ToArray());
            }

            //Points
            //Numbers only exception and validation: TODO
            int.TryParse(TbxPoints.Text, out int points);
            cards newCard = new cards { front = frontText, back = backText, points = points };
            
            dbContext.cards.Add(newCard);
            dbContext.SaveChanges();

            //Image
            if (!String.IsNullOrEmpty(TbxImagePath.Text))
            {
                var db = new SenkoucardsConfig();
                var cardsImages = new cardsImages()
                {
                    //TO FIGURE OUT: HOW TO ADD ID
                    cardId = newCard.id,
                    image = _imageBytes
                };
            }
            //Audio
            if (!String.IsNullOrEmpty(TbxAudioPath.Text))
            {
                var db = new SenkoucardsConfig();
                var cardsAudio = new cardsAudios()
                {
                    //TO FIGURE OUT: HOW TO ADD ID
                    cardId = newCard.id,
                    audio = _audioBytes
                };
            }


        }
        private void btnImportServer_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
