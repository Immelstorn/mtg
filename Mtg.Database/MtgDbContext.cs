using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using Microsoft.AspNet.Identity.EntityFramework;
using Mtg.Database.Models;
using Mtg.Models.CardModels;
using Mtg.Models.GameModels;

namespace Mtg.Database
{
    public class MtgDbContext : IdentityDbContext<Player>
    {
        public MtgDbContext()
                : base("name=MtgDatabase")
        {
            DbInterception.Add(new EFCommandInterceptor());
        }

        public static MtgDbContext Create()
        {
            return new MtgDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().HasMany(g => g.Grave).WithOptional().WillCascadeOnDelete(true);
            modelBuilder.Entity<Game>().HasMany(g => g.Exile).WithOptional().WillCascadeOnDelete(true);
            modelBuilder.Entity<Game>().HasMany(g => g.Bottom).WithOptional().WillCascadeOnDelete(true);
            modelBuilder.Entity<Game>().HasMany(g => g.Top).WithOptional().WillCascadeOnDelete(true);
            modelBuilder.Entity<Game>().HasMany(g => g.HandCards).WithOptional().WillCascadeOnDelete(true);

            modelBuilder.Entity<HandCard>().HasRequired(h => h.Player);

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
        }

        public DbSet<Set> Sets { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardLink> CardLinks { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Top> Tops { get; set; }
        public DbSet<Bottom> Bottoms { get; set; }
        public DbSet<Grave> Graves { get; set; }
        public DbSet<Exile> Exiles { get; set; }
        public DbSet<HandCard> HandCards{ get; set; }
        public DbSet<BattlefieldCard> BattlefieldCards{ get; set; }
        public DbSet<Token> Tokens{ get; set; }
    }
}