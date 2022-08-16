using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace NMB.Business_Layer
{
    //Inheriting from MessageResponseType class
    public class Tweet : MessageResponseType
    {

        //Declare dictionary for text speak
        Dictionary<string, string> TextSpeechDict;

        //constructor
        public Tweet(string rawMsg, Dictionary<string, string> textSpeechDict, int maxLength = 140) : base(rawMsg)
        {
            //declare variables
            this.TextSpeechDict = textSpeechDict;
            //Find where message section starts of message string
            int messageStart = rawMessage.LastIndexOf("-") + 1;
            //Find the message section length by subtracting messageSTart from full message
            int messageLength = rawMessage.Length - 1 - messageStart;
            //If the message length is longer than 140 character, cut it at that length
            if (messageLength > maxLength)
            {
                rawMessage = rawMessage.Substring(0, messageStart + maxLength) + ";";
            }
        }

        //overiding function
        //Here we take in the string and look for any text speak, if found, add in expanded meaning
        public override List<string> ProcessMessage()
        {
            //change message to upper case
            string upperCaseRawMessage = rawMessage.ToUpper();

            //foreach loop to check for any keys present (abbreviations) are found
            foreach (KeyValuePair<string, string> word in TextSpeechDict)
            {
                //If the message contains one of the keys, check how many times it is present
                if (upperCaseRawMessage.Contains(word.Key))
                {
                    List<int> indexes = Utility.GetAllInstancesInString(upperCaseRawMessage, word.Key);

                    //foreach time the key is present, insert the meaning (value) after the abbreviation (key)
                    foreach (int index in indexes)
                    {
                        upperCaseRawMessage = upperCaseRawMessage.Insert(index + word.Key.Length, " <" + word.Value + "> ");
                    }
                }
            }

            processedMessage = upperCaseRawMessage;
            return null;
        }

        public override string Serialise()
        {

            //store the processMessage as variable
            string message = processedMessage;

            //turn the processedMessage string into an array so the data can be split
            string[] messageList = message.Split('-');

            //assign varibales to the data from the array
            string ID = messageList[0];
            string twitterID = messageList[1];
            string messageBody = messageList[2];

            //create object 
            var data = new
            {
                ID,
                twitterID,
                messageBody,
            };

            //transform to json object
            string jsonData = JsonConvert.SerializeObject(data);

            //filepath
            string path = @"C:\Users\User\source\repos\NMB\NMB\JSON Output\Tweet\" + ID + ".json";

            File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented) + Environment.NewLine);

            return null;
        }

    }
}
