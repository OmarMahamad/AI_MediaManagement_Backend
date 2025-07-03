using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLayer.Constants.Messages
{
    public static class EnglishMessages
    {
        public static readonly Dictionary<string, string> Messages = new()
        {
            { MessageKeys.UserNotFound, "User not found." },
            { MessageKeys.InvalidPassword, "Invalid password." },
            { MessageKeys.SubscriptionExpired, "Subscription expired." },
            { MessageKeys.LoginSuccess, "Login successful." },
            { MessageKeys.EmailNotFound, "This Emial not Found." },
            { MessageKeys.FoundEmail, "This Emial Exit." }
        };
    }
}
