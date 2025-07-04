using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelperLayer.Constants.Messages;

namespace HelperLayer.Constants.Services
{
    public class MessageService
    {
        public string GetMessage(string key, Language language, params object[] args)
        {
            string template = language switch
            {
                Language.Arabic => ArabicMessages.Messages.ContainsKey(key)
                    ? ArabicMessages.Messages[key]
                    : "رسالة غير معروفة",
                Language.English => EnglishMessages.Messages.ContainsKey(key)
                    ? EnglishMessages.Messages[key]
                    : "Unknown message",
                _ => "Language not supported"
            };

            return string.Format(template, args);
        }


    }
}
