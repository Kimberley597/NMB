using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace NMB.Business_Layer
{

    //Parent class that declares the methods that will be used for the different message types
    public class MessageResponseType
    {
        //declare message content variable
        protected string rawMessage = "";

        public string RawMessage
        {
            get
            {
                return rawMessage;
            }
        }

        //declare processed Message string
        protected string processedMessage = "";

        public string GetProcessedMessage
        {
            get
            {
                ProcessMessage();
                return processedMessage;
            }
        }

        //constructor
        public MessageResponseType(string rawMsg)
        {
            //declare
            this.rawMessage = rawMsg;
        }

        //Method where any processing of the message takes place
        public virtual List<string> ProcessMessage()
        {
            return null;
        }

        //Method for serilaising message and outputting in JSON format
        public virtual string Serialise()
        {
            return null;
        } 
    }
}
