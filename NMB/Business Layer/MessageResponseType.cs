using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace NMB.Business_Layer
{
    public class MessageResponseType
    {
        protected string rawMessage = "";

        public string RawMessage
        {
            get
            {
                return rawMessage;
            }
        }

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

        //virtual means function can be overitten in our classes inheriting from 
        public virtual List<string> ProcessMessage()
        {
            return null;
        }

        public virtual string Serialise()
        {

            return null;
            //MessageResponseType thingy = this;
            //try
            //{
            //    return "";
            //}
            //catch(Exception e)
            //{
            //    return e.Message;
            //}  
        } 
    }
}
