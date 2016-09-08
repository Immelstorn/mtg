namespace Mtg.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBattlefeildCards_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BattlefieldCards",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        State = c.Int(nullable: false),
                        Top = c.Double(nullable: false),
                        Left = c.Double(nullable: false),
                        CardLink_Id = c.Guid(),
                        Player_Id = c.String(maxLength: 128),
                        Game_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CardLinks", t => t.CardLink_Id)
                .ForeignKey("dbo.Players", t => t.Player_Id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .Index(t => t.CardLink_Id)
                .Index(t => t.Player_Id)
                .Index(t => t.Game_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BattlefieldCards", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.BattlefieldCards", "Player_Id", "dbo.Players");
            DropForeignKey("dbo.BattlefieldCards", "CardLink_Id", "dbo.CardLinks");
            DropIndex("dbo.BattlefieldCards", new[] { "Game_Id" });
            DropIndex("dbo.BattlefieldCards", new[] { "Player_Id" });
            DropIndex("dbo.BattlefieldCards", new[] { "CardLink_Id" });
            DropTable("dbo.BattlefieldCards");
        }
    }
}
