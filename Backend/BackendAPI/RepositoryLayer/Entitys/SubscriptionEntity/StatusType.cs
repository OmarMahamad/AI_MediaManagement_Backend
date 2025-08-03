using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entitys.SubscriptionEntity
{
    public enum StatusType
    {
        ApprovalPending, // بعد إنشاء الاشتراك وقبل أن يوافق المستخدم على الدفع
        Approved,         // المستخدم وافق لكن لم يُفعل بعد
        Active,           // الاشتراك مفعل
        Suspended,        // توقف مؤقت (بسبب فشل الدفع أو إجراء يدوي)
        Cancelled,        // تم إلغاؤه نهائيًا
        Expired,          // انتهت المدة المحددة
        PaymentFailed,    // فشل في محاولة الدفع
        Refunded          // تم رد المبلغ
    }
}
