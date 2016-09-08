namespace Mtg.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        artist = c.String(),
                        cmc = c.Double(nullable: false),
                        flavor = c.String(),
                        imageName = c.String(),
                        layout = c.String(),
                        manaCost = c.String(),
                        multiverseid = c.Int(nullable: false),
                        name = c.String(),
                        number = c.String(),
                        originalText = c.String(),
                        originalType = c.String(),
                        power = c.String(),
                        rarity = c.String(),
                        text = c.String(),
                        toughness = c.String(),
                        type = c.String(),
                        starter = c.Boolean(),
                        ImgLink = c.String(),
                        Set_magicCardsInfoCode = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Sets", t => t.Set_magicCardsInfoCode)
                .Index(t => t.Set_magicCardsInfoCode);
            
            CreateTable(
                "dbo.ForeignNames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        language = c.String(),
                        name = c.String(),
                        multiverseid = c.Int(nullable: false),
                        Card_id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cards", t => t.Card_id)
                .Index(t => t.Card_id);
            
            CreateTable(
                "dbo.Legalities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        format = c.String(),
                        legality = c.String(),
                        Card_id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cards", t => t.Card_id)
                .Index(t => t.Card_id);
            
            CreateTable(
                "dbo.Rulings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        date = c.String(),
                        text = c.String(),
                        Card_id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cards", t => t.Card_id)
                .Index(t => t.Card_id);
            
            CreateTable(
                "dbo.Sets",
                c => new
                    {
                        magicCardsInfoCode = c.String(nullable: false, maxLength: 128),
                        name = c.String(),
                        code = c.String(),
                        oldCode = c.String(),
                        releaseDate = c.String(),
                        border = c.String(),
                        type = c.String(),
                        block = c.String(),
                    })
                .PrimaryKey(t => t.magicCardsInfoCode);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cards", "Set_magicCardsInfoCode", "dbo.Sets");
            DropForeignKey("dbo.Rulings", "Card_id", "dbo.Cards");
            DropForeignKey("dbo.Legalities", "Card_id", "dbo.Cards");
            DropForeignKey("dbo.ForeignNames", "Card_id", "dbo.Cards");
            DropIndex("dbo.Rulings", new[] { "Card_id" });
            DropIndex("dbo.Legalities", new[] { "Card_id" });
            DropIndex("dbo.ForeignNames", new[] { "Card_id" });
            DropIndex("dbo.Cards", new[] { "Set_magicCardsInfoCode" });
            DropTable("dbo.Sets");
            DropTable("dbo.Rulings");
            DropTable("dbo.Legalities");
            DropTable("dbo.ForeignNames");
            DropTable("dbo.Cards");
        }
    }
}
