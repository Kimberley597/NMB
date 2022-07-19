using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public MessageResponseType(string rawMessage)
        {
            //declare
            this.rawMessage = rawMessage;
        }

        public virtual List<string> ProcessMessage()
        {
            return null;
        }

        public virtual string Serialise()
        {
            return null;
        }
    }
}
