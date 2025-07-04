using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entitys.UserEntity;

namespace RepositoryLayer.Entitys.AuthorizationEntity
{
    [Index(nameof(Code))]
    public class OtpCode
    {
        public int OtpCodeId{ get; set; }
        public int UserId { get; set; }

        public string Code { get; set; }

        public DateTime ExpiryDate { get; set; }

        public bool IsUsed { get; set; }=false;

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
