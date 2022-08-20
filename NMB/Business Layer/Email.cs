using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace NMB.Business_Layer
{
    //Inheriting from MessageResponseType class
    public class Email : MessageResponseType
    {
        //constructor
        //Takes in the raw message and a maximum length for the message body
        //If the message body is longer than maxLength, it trims it then outputs the trimmed message
        public Email(string rawMsg, int maxLength = 1028) : base(rawMsg)
        {
            //Find where message section starts of message string
            int messageStart = rawMessage.LastIndexOf("-") + 1;
            //Find the message section length by subtracting messageSTart from full message
            int messageLength = rawMessage.Length - 1 - messageStart;
            //If the message length is longer than 1028 character, cut it at that length
            if (messageLength > maxLength)
            {
                rawMessage = rawMessage.Substring(0, messageStart + maxLength) + ";";
            }
        }

        //overiding function
        //Here we take in the string and look for any embedded urls, if found then extarct, add to url quarantined list and replace with placeholder
        public override List<string> ProcessMessage()
        {
            //create list to hold any urls
            List<string> urlList = new List<string>();

            //search for any urls, if found, add to list and replace with placeholder
            foreach (Match url in Regex.Matches(rawMessage, @"(http|ftp|https):\/\/([\w\-_]+(?:(?:\.[\w\-_]+)+))([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?"))
            {
                //add to list
                urlList.Add(url.ToString());

                //foreach url in the url list, add a new entry to the Quarantined URL file, then replace with placeholder 
                foreach (var item in urlList)
                {
                    //write to Quarantined URL file
                    System.IO.File.AppendAllText(@"C:\Users\User\source\repos\NMB\NMB\QuarantinedURLs.txt", item + Environment.NewLine);

                    //replace with placeholder
                    rawMessage = rawMessage.Replace(url.ToString(), "<URL Quarenatined>");
                }

            }

            processedMessage = rawMessage;
            return null;
        }

        //Method that takes in the processedMessage, turns it into an object, then outputs to a file in JSON format
        public override string Serialise()
        {

            //store the processMessage as variable
            string message = processedMessage;

            //turn the processedMessage string into an array so the data can be split
            string[] messageList = message.Split('-');

            //assign varibales to the data from the array
            string ID = messageList[0];
            string emailAddress = messageList[1];
            string subject = messageList[2];
            string messageBody = messageList[3];
            
            //create object 
            var data = new
            {
                ID,
                emailAddress,
                subject,
                messageBody,
            };

            //transform to json object
            //string jsonData = JsonConvert.SerializeObject(data);

            //declare filepath and name the file using the message ID
            string path = @"C:\Users\User\source\repos\NMB\NMB\JSON Output\Email\" + ID + ".json";

            //write the file using Formatting.Indented to place the data on new lines
            File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented) + Environment.NewLine);

            return null;
        }

    }
}
