using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entitys.UserEntity;

namespace RepositoryLayer.Entitys.AuthorizationEntity
{
    public class AuthorizationToken
    {
        [Key]
        public int AuthorizationId { get; set; }
        public int UserId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User{ get; set; }
    }

}
