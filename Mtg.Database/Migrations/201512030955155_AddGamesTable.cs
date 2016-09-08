namespace Mtg.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGamesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Exiles",
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
            
            CreateTable(
                "dbo.Graves",
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
            
            CreateTable(
                "dbo.Tops",
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
            DropForeignKey("dbo.Tops", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Tops", "Card_id", "dbo.Cards");
            DropForeignKey("dbo.Graves", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Graves", "Card_id", "dbo.Cards");
            DropForeignKey("dbo.Exiles", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Exiles", "Card_id", "dbo.Cards");
            DropIndex("dbo.Tops", new[] { "Game_Id" });
            DropIndex("dbo.Tops", new[] { "Card_id" });
            DropIndex("dbo.Graves", new[] { "Game_Id" });
            DropIndex("dbo.Graves", new[] { "Card_id" });
            DropIndex("dbo.Exiles", new[] { "Game_Id" });
            DropIndex("dbo.Exiles", new[] { "Card_id" });
            DropTable("dbo.Tops");
            DropTable("dbo.Graves");
            DropTable("dbo.Exiles");
            DropTable("dbo.Games");
        }
    }
}
