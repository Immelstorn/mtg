namespace Mtg.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHands : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HandCards",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Card_id = c.String(nullable: false, maxLength: 128),
                        Player_Id = c.String(nullable: false, maxLength: 128),
                        Game_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cards", t => t.Card_id, cascadeDelete: true)
                .ForeignKey("dbo.Players", t => t.Player_Id, cascadeDelete: true)
                .ForeignKey("dbo.Games", t => t.Game_Id, cascadeDelete: true)
                .Index(t => t.Card_id)
                .Index(t => t.Player_Id)
                .Index(t => t.Game_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HandCards", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.HandCards", "Player_Id", "dbo.Players");
            DropForeignKey("dbo.HandCards", "Card_id", "dbo.Cards");
            DropIndex("dbo.HandCards", new[] { "Game_Id" });
            DropIndex("dbo.HandCards", new[] { "Player_Id" });
            DropIndex("dbo.HandCards", new[] { "Card_id" });
            DropTable("dbo.HandCards");
        }
    }
}
