using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLayer.Constants.Messages
{
    public static class ArabicMessages
    {
        public static readonly Dictionary<string, string> Messages = new()
        {
            { MessageKeys.UserNotFound, "المستخدم غير موجود." },
            { MessageKeys.InvalidPassword, "كلمة المرور غير صحيحة." },
            { MessageKeys.SubscriptionExpired, "انتهت صلاحية الاشتراك." },
            { MessageKeys.LoginSuccess, "تم تسجيل الدخول بنجاح." },
            { MessageKeys.RegisterSuccess, "تم تسجيل مستخدم جديد بنجاح." },
            { MessageKeys.EmailNotFound, "لم يتم العثور على هذا البريد الإلكتروني." },
            { MessageKeys.IsEmailVerified, "لم يتم توثيق الحساب." },
            { MessageKeys.FoundEmail, "هذا المستخدم موجود." },
            { MessageKeys.ResetPassword, "اعادة ضبط كلمة المرور." },
            { MessageKeys.CheckEmail, "التحقق من البريد الإلكتروني." },
            { MessageKeys.SendCode, "رمز التحقق الخاص بك هو: {0}." },
            { MessageKeys.ExpiryDateCode, "هذا الكود انتهت صَّلاحِيَته." },
            { MessageKeys.CodeNotFound, "هذا الكد غير موجود." },
            { MessageKeys.UsedCode, "هذا الكود استخدم." },
            { MessageKeys.AccessCode, "هذا الكود صالح." },
        };
    }
}
