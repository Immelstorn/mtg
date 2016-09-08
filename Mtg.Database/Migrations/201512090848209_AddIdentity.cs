namespace Mtg.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIdentity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HandCards", "Card_id", "dbo.Cards");
            DropForeignKey("dbo.HandCards", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Players", "Game_Id", "dbo.Games");
            DropIndex("dbo.HandCards", new[] { "Card_id" });
            DropIndex("dbo.HandCards", new[] { "Game_Id" });
            DropIndex("dbo.Players", new[] { "Game_Id" });
            DropPrimaryKey("dbo.Players");
            CreateTable(
                "dbo.IdentityUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        Player_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Players", t => t.Player_Id)
                .Index(t => t.Player_Id);
            
            CreateTable(
                "dbo.IdentityUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        Player_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Players", t => t.Player_Id)
                .Index(t => t.Player_Id);
            
            CreateTable(
                "dbo.IdentityUserRoles",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Player_Id = c.String(maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.Players", t => t.Player_Id)
                .ForeignKey("dbo.IdentityRoles", t => t.IdentityRole_Id)
                .Index(t => t.Player_Id)
                .Index(t => t.IdentityRole_Id);
            
            CreateTable(
                "dbo.IdentityRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlayerGames",
                c => new
                    {
                        Player_Id = c.String(nullable: false, maxLength: 128),
                        Game_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Player_Id, t.Game_Id })
                .ForeignKey("dbo.Players", t => t.Player_Id, cascadeDelete: true)
                .ForeignKey("dbo.Games", t => t.Game_Id, cascadeDelete: true)
                .Index(t => t.Player_Id)
                .Index(t => t.Game_Id);
            
            AddColumn("dbo.Players", "Email", c => c.String());
            AddColumn("dbo.Players", "EmailConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Players", "PasswordHash", c => c.String());
            AddColumn("dbo.Players", "SecurityStamp", c => c.String());
            AddColumn("dbo.Players", "PhoneNumber", c => c.String());
            AddColumn("dbo.Players", "PhoneNumberConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Players", "TwoFactorEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.Players", "LockoutEndDateUtc", c => c.DateTime());
            AddColumn("dbo.Players", "LockoutEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.Players", "AccessFailedCount", c => c.Int(nullable: false));
            AddColumn("dbo.Players", "UserName", c => c.String());
            AlterColumn("dbo.Players", "Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Players", "Id");
            DropColumn("dbo.Players", "Name");
            DropColumn("dbo.Players", "SessionId");
            DropColumn("dbo.Players", "Game_Id");
            DropTable("dbo.HandCards");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.HandCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Card_id = c.String(maxLength: 128),
                        Game_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Players", "Game_Id", c => c.Guid());
            AddColumn("dbo.Players", "SessionId", c => c.String());
            AddColumn("dbo.Players", "Name", c => c.String());
            DropForeignKey("dbo.IdentityUserRoles", "IdentityRole_Id", "dbo.IdentityRoles");
            DropForeignKey("dbo.IdentityUserRoles", "Player_Id", "dbo.Players");
            DropForeignKey("dbo.IdentityUserLogins", "Player_Id", "dbo.Players");
            DropForeignKey("dbo.PlayerGames", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.PlayerGames", "Player_Id", "dbo.Players");
            DropForeignKey("dbo.IdentityUserClaims", "Player_Id", "dbo.Players");
            DropIndex("dbo.PlayerGames", new[] { "Game_Id" });
            DropIndex("dbo.PlayerGames", new[] { "Player_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "IdentityRole_Id" });
            DropIndex("dbo.IdentityUserRoles", new[] { "Player_Id" });
            DropIndex("dbo.IdentityUserLogins", new[] { "Player_Id" });
            DropIndex("dbo.IdentityUserClaims", new[] { "Player_Id" });
            DropPrimaryKey("dbo.Players");
            AlterColumn("dbo.Players", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Players", "UserName");
            DropColumn("dbo.Players", "AccessFailedCount");
            DropColumn("dbo.Players", "LockoutEnabled");
            DropColumn("dbo.Players", "LockoutEndDateUtc");
            DropColumn("dbo.Players", "TwoFactorEnabled");
            DropColumn("dbo.Players", "PhoneNumberConfirmed");
            DropColumn("dbo.Players", "PhoneNumber");
            DropColumn("dbo.Players", "SecurityStamp");
            DropColumn("dbo.Players", "PasswordHash");
            DropColumn("dbo.Players", "EmailConfirmed");
            DropColumn("dbo.Players", "Email");
            DropTable("dbo.PlayerGames");
            DropTable("dbo.IdentityRoles");
            DropTable("dbo.IdentityUserRoles");
            DropTable("dbo.IdentityUserLogins");
            DropTable("dbo.IdentityUserClaims");
            AddPrimaryKey("dbo.Players", "Id");
            CreateIndex("dbo.Players", "Game_Id");
            CreateIndex("dbo.HandCards", "Game_Id");
            CreateIndex("dbo.HandCards", "Card_id");
            AddForeignKey("dbo.Players", "Game_Id", "dbo.Games", "Id");
            AddForeignKey("dbo.HandCards", "Game_Id", "dbo.Games", "Id", cascadeDelete: true);
            AddForeignKey("dbo.HandCards", "Card_id", "dbo.Cards", "id");
        }
    }
}
