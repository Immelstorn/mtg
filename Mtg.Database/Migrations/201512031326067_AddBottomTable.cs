namespace Mtg.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBottomTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bottoms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Card_id = c.String(maxLength: 128),
                        Game_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cards", t => t.Card_id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .Index(t => t.Card_id)
                .Index(t => t.Game_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bottoms", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Bottoms", "Card_id", "dbo.Cards");
            DropIndex("dbo.Bottoms", new[] { "Game_Id" });
            DropIndex("dbo.Bottoms", new[] { "Card_id" });
            DropTable("dbo.Bottoms");
        }
    }
}
