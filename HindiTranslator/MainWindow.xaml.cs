using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Translator.Models;

namespace Translator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void InputTextBox_KeyUp(object sender, KeyEventArgs e)
        {

            TextBox txtBox = sender as TextBox;
            if ((txtBox == null) || (txtBox.CaretIndex == 0))
                return;


            bool isSpaceKey = System.Windows.Input.Key.Space == e.Key;
            
            string lastWord = GetLastWord(isSpaceKey);

            if (e.Key == Key.Back)
            {
                if (lastWord.Any() && lastWord.Last() == ' ')
                    return;
                else if (HindiProcessor.IsHindiWord(lastWord))
                {
                    lastWord = HindiProcessor.GetReadableWord(lastWord.TrimStart());
                }

            }

            if (e.Key == Key.OemPipe)
            {
                string hindiWord = HindiProcessor.GetHindiWord(lastWord);
                ReplaceLastWordWithSelection(lastWord, hindiWord);

                string trans = GetTranslatedText(txtBox.Text);

                return;
            }

            if (isSpaceKey)
            {
                if (!HindiProcessor.IsHindiWord(lastWord))
                {
                    string hindiWord = HindiProcessor.GetHindiWord(lastWord);
                    ReplaceLastWordWithSelection(lastWord, hindiWord);
                }
                else
                {
                    string selectedItem = SuggestionsListBox.SelectedItem?.ToString();
                    ReplaceLastWordWithSelection(lastWord, selectedItem);
                }

                PopupSuggestions.IsOpen = false;
                e.Handled = true;

                return;
            }

            if (System.Windows.Input.Key.Escape == e.Key)
            {

                PopupSuggestions.IsOpen = false;
                return;
            }

            if (PopupSuggestions.IsOpen)
            {
                
                if (System.Windows.Input.Key.Enter == e.Key)
                {
                    string selectedItem = SuggestionsListBox.SelectedItem?.ToString();
                    ReplaceLastWordWithSelection(lastWord, selectedItem);
                    PopupSuggestions.IsOpen = false;
                    e.Handled = true;
                    return;
                }
                else if (System.Windows.Input.Key.Down == e.Key && !SuggestionsListBox.IsFocused)
                {
                    SuggestionsListBox.SelectionChanged -= SuggestionsListBox_SelectionChanged;

                    SuggestionsListBox.Focus();
                    //SuggestionsListBox.SelectedIndex = 0;

                    SuggestionsListBox.SelectionChanged += SuggestionsListBox_SelectionChanged;
                    return;
                }
            }
            else
            {
                
            }

            // InputTextBox.KeyUp -= InputTextBox_KeyUp;

            if (!string.IsNullOrEmpty(lastWord)  )
            {
                var allSuggestions = Shabdkosh.GetSuggestions(lastWord);

                if(allSuggestions.Count > 0)
                {
                    PopupSuggestions.PlacementTarget = InputTextBox;
                    PopupSuggestions.PlacementRectangle = InputTextBox.GetRectFromCharacterIndex(InputTextBox.CaretIndex, true);
                    PopupSuggestions.IsOpen = true;
                    PopulateSuggestions(lastWord);
                }
                    
            }

            SuggestionsListBox.SelectedIndex = 0;
            //SuggestionsListBox.Focus();

        }

        private string GetTranslatedText(string hindiText)
        {
            hindiText = @"रामन का जन्म तिरुचिरापल्ली में तमिलनाडु में हुआ । रामन का जन्म तिरुचिरापल्ली में तमिलनाडु में हुआ ";

            try
            {
                using (WebClient wc = new WebClient())
                {
                    var url = "https://translate.google.com/translate_a/single?client=t&sl=hi&tl=en&hl=en&dt=bd&dt=ex&dt=ld&dt=md&dt=qca&dt=rw&dt=rm&dt=ss&dt=t&dt=at&ie=UTF-8&oe=UTF-8&source=clks&trs=1&inputm=1&srcrom=1&ssel=0&tsel=0&kc=1&tk=719917.843757&q=" + System.Web.HttpUtility.UrlEncode(hindiText);
                    string results = wc.DownloadString(url);
                    //var allSuggestions = JObject.Parse(JObject.Parse(results)["query"]["results"]["body"].ToString())["suggestions"].ToObject<List<string>>();
                    
                    return string.Empty;
                }
            }
            catch (Exception)
            {


            }

            return string.Empty;

        }

        private string GetLastWord(bool isSpaceKey)
        {
            string txt = InputTextBox.Text;

            int offset = 1;

            if (isSpaceKey && txt[InputTextBox.CaretIndex - 1] == ' ')
                offset = 2;

            int wordStart = txt.LastIndexOf(' ', InputTextBox.CaretIndex - offset);

            if (wordStart == -1)
                wordStart = 0;

            int wordEnd = txt.IndexOf(' ', wordStart+1);

            if (wordEnd == -1)
            {
                if (InputTextBox.Text.Length > InputTextBox.CaretIndex)
                    wordEnd = InputTextBox.Text.Length;
                else
                    wordEnd = InputTextBox.CaretIndex;
            }
            else if (isSpaceKey)
                wordEnd++;

            string lastWord = txt.Substring(wordStart, wordEnd - wordStart);

            if (!isSpaceKey)
                lastWord = lastWord.TrimStart();

            int i = 0;

            for (; i < lastWord.Length; i++)
            {
                if (lastWord[i] < 256)
                    break;
            }
            
            if (i > 0)
            {
                return HindiProcessor.GetReadableWord(lastWord.Substring(0, i)) + lastWord.Substring(i);
            }

            return lastWord;
        }

        private void PopulateSuggestions(string lastWord)
        {

            if (!string.IsNullOrEmpty(lastWord))
            {
                SuggestionsListBox.SelectionChanged -= SuggestionsListBox_SelectionChanged;

                SuggestionsListBox.Items.Clear();

                foreach (var suggestion in Shabdkosh.GetSuggestions(lastWord))
                {
                    SuggestionsListBox.Items.Add(suggestion);
                }

                SuggestionsListBox.SelectionChanged += SuggestionsListBox_SelectionChanged;
            }
        }
     
        private void SuggestionsListBox_KeyDown(object sender, KeyEventArgs e)
        {
            string selectedItem = SuggestionsListBox.SelectedItem?.ToString();
            switch (e.Key)
            {
                case System.Windows.Input.Key.Left:
                case System.Windows.Input.Key.Right:
                case System.Windows.Input.Key.Up:
                case System.Windows.Input.Key.Down:
                   
                    e.Handled = false;
                    break;

                case System.Windows.Input.Key.Enter:
                    PopupSuggestions.IsOpen = false;
                    
                    ReplaceLastWordWithSelection(GetLastWord(false), selectedItem);
                    e.Handled = true;
                    break;

                case System.Windows.Input.Key.Space:
                    PopupSuggestions.IsOpen = false;

                    ReplaceLastWordWithSelection(GetLastWord(true), selectedItem);
                    e.Handled = true;
                    break;

                case System.Windows.Input.Key.Escape:
                    // Hide the Popup
                    PopupSuggestions.IsOpen = false;
                    e.Handled = true;
                    break;


            }
            

        }

        private void ReplaceLastWordWithSelection(string lastWord, string selection)
        {
            
            if (selection == null)
                return;
            
            InputTextBox.KeyUp -= InputTextBox_KeyUp;

            // Save the Caret position
            int i = InputTextBox.CaretIndex;

            string textBoxContent = InputTextBox.Text;


            lastWord = lastWord.TrimStart();
            i -= lastWord.Length;

        

            textBoxContent = textBoxContent.Remove(i , lastWord.Length);
            
            if (textBoxContent.Length < i)
                i = 0;

            // Add text to the text
            InputTextBox.Text = textBoxContent.Insert(i, selection + " ");

            SuggestionsListBox.Items.Clear();
            SuggestionsListBox.SelectedItem = "";

            // Move the caret to the end of the added text
            InputTextBox.CaretIndex = i + selection.Length + 1;
            
            // Move focus back to the text box. This will auto-hide the PopUp due to StaysOpen="false"
            InputTextBox.Focus();

            InputTextBox.KeyUp += InputTextBox_KeyUp;

        }
        

        private void SuggestionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void SuggestionsListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string selectedItem = SuggestionsListBox.SelectedItem?.ToString();
            ReplaceLastWordWithSelection(GetLastWord(false), selectedItem);
            PopupSuggestions.IsOpen = false;

        }
    }
}
