using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RepositoryLayer.Entitys.AuthorizationEntity;
using RepositoryLayer.Entitys.SubscriptionEntity;
using RepositoryLayer.Entitys.UserEntity;

namespace RepositoryLayer
{
    public class DbApp : DbContext
    {
        public DbApp(DbContextOptions options) : base(options) { }

        public DbSet<AuthorizationToken> authorizationTokens { get; set; }
        public DbSet<Admin> admins { get; set; }
        public DbSet<Writer> writers { get; set; }
        public DbSet<FinancialAccounts> financialAccounts { get; set; }
        public DbSet<PaymentTransaction> paymentTransactions { get; set; }
        public DbSet<Subscription> subscriptions { get; set; }




    }
}
