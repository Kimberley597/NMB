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
using System.Windows.Navigation;
using System.Windows.Shapes;
using NMB.Business_Layer;
using System.Text.RegularExpressions;
using System.IO;

namespace NMB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //declare default value
        MessageType messageType = MessageType.Email;

        //Declare Dictionary for text speak of type string, string
        Dictionary<string, string> TextSpeakDict = new Dictionary<string, string>();

        //declare variables
        int maxSubjectLength = 20;
        int maxTwitterIdLength = 15;

        public MainWindow()
        {
            InitializeComponent();

            cmbMessageType.SelectedIndex = 0;

            //Create a string array called lines, take in the text speak file and split it by adding a new line after each entry
            string[] lines = Properties.Resources.textwords.Split(
                new string[] { Environment.NewLine },
                StringSplitOptions.None);

            //foreach line in lines, split at comma
            foreach (string line in lines)
            {
                string[] split = line.Split(',');

                if (split.Count() < 2)
                {
                    continue;
                }

                //If the line isn't already stroed, add it
                if (!TextSpeakDict.ContainsKey(split[0].ToUpper()))
                {
                    TextSpeakDict.Add(split[0].ToUpper(), split[1]);
                }
            }
        }

        //Event handler for selection of message type
        private void cmbMessageType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem choice = (ComboBoxItem)cmbMessageType.SelectedItem;

            switch (choice.Content)
            {
                case "Email":
                    messageType = MessageType.Email;
                    break;
                case "Text":
                    messageType = MessageType.Text;
                    break;
                case "Tweet":
                    messageType = MessageType.Tweet;
                    break;
            }
        }

        //Decide which message type has ben entered, create a string with the info
        private void btnSend_Click_1(object sender, RoutedEventArgs e)
        {
            //declare and intialise result vairable
            string result = "";

            //Generate 9 digit ID
            Random rand = new Random();
            int messageId = rand.Next(111111111, 999999999);

            //Build string using switch statement
            switch (messageType)
            {
                case MessageType.Email:
                    //If message type is email, add E at the start
                    result += "E" + messageId + "-";

                    //Create string for the email address
                    string email = txtboxSender.Text;

                    //Validate using regex
                    if (Regex.IsMatch(txtboxSender.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                    {
                        result += email + "-";
                    }
                    else
                    {
                        MessageBox.Show("Email address is not valid");
                        return;
                    }

                    //Add Subject info
                    if (txtboxSubject.Text.Length <= maxSubjectLength)
                    {
                        result += txtboxSubject.Text + "-";
                    }
                    else
                    {
                        MessageBox.Show("Email Subject length is too long.");
                        return;
                    }

                    //Add Message Body info
                    result += txtboxMessageBody.Text;
                    break;

                case MessageType.Text:
                    //If message type is text, add S at the start
                    result += "S" + messageId + "-";

                    //Create string for the phone number
                    string text = txtboxSender.Text;

                    ////Validate using regex for UK phone number
                    //if (Regex.IsMatch(txtboxSender.Text, @"^((\+44\s?\d{4}|\(?\d{5}\)?)\s?\d{6})|((\+44\s?|0)7\d{3}\s?\d{6})$"))
                    //{
                    //    result += text + "-";
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Phone number is not a valid UK phone number");
                    //    return;
                    //}

                    //Validate using regex for international number
                    if (Regex.IsMatch(txtboxSender.Text, @"^([\+]?33[-]?|[0])?[1-9][0-9]{8}$"))
                    {
                        result += text + "-";
                    }
                    else
                    {
                        MessageBox.Show("Phone number is not a valid international number.");
                        return;
                    }

                    //Add Message Body info
                    result += txtboxMessageBody.Text + ";";

                    //
                    MessageResponseType SMSResponse = new SMS(result, TextSpeakDict);
                    SMSResponse.ProcessMessage();
                    SMSResponse.Serialise();
                    //clearData();

                    break;

                case MessageType.Tweet:
                    //If message type is tweet, add T at the start
                    result += "T" + messageId + "-";

                    string tweet = txtboxSender.Text;

                    //Validate by checking starts with an @ symbol and less than max length
                    if (txtboxSender.Text.Substring(0, 1) == "@" && txtboxSender.Text.Length <= maxTwitterIdLength)
                    {
                        result += tweet + "-";
                    }
                    else
                    {
                        MessageBox.Show("Twitter ID is not valid");
                        return;
                    }

                    //Add Message Body info
                    result += txtboxMessageBody.Text + ";";

                    //
                    MessageResponseType TweetResponse = new Tweet(result, TextSpeakDict);
                    TweetResponse.ProcessMessage();
                    TweetResponse.Serialise();

                    break;

            }

            //For testing purposes - paste result string into text box
            //txtboxMessageString.Text = result;

            //clear entered data, ready for a new message
            clearData();

        }
        private void clearData()
        {
            txtboxSender.Clear();
            txtboxSubject.Clear();
            txtboxMessageBody.Clear();
            //txtboxMessageString.Clear();
        }
    }
}
