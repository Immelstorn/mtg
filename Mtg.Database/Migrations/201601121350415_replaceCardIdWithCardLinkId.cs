namespace Mtg.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class replaceCardIdWithCardLinkId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bottoms", "Card_id", "dbo.Cards");
            DropForeignKey("dbo.Exiles", "Card_id", "dbo.Cards");
            DropForeignKey("dbo.Graves", "Card_id", "dbo.Cards");
            DropForeignKey("dbo.HandCards", "Card_id", "dbo.Cards");
            DropForeignKey("dbo.Tops", "Card_id", "dbo.Cards");
            DropIndex("dbo.Bottoms", new[] { "Card_id" });
            DropIndex("dbo.Exiles", new[] { "Card_id" });
            DropIndex("dbo.Graves", new[] { "Card_id" });
            DropIndex("dbo.HandCards", new[] { "Card_id" });
            DropIndex("dbo.Tops", new[] { "Card_id" });
            CreateTable(
                "dbo.CardLinks",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Card_id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cards", t => t.Card_id)
                .Index(t => t.Card_id);
            
            AddColumn("dbo.Bottoms", "CardLink_Id", c => c.Guid());
            AddColumn("dbo.Exiles", "CardLink_Id", c => c.Guid());
            AddColumn("dbo.Graves", "CardLink_Id", c => c.Guid());
            AddColumn("dbo.HandCards", "CardLink_Id", c => c.Guid());
            AddColumn("dbo.Tops", "CardLink_Id", c => c.Guid());
            CreateIndex("dbo.Bottoms", "CardLink_Id");
            CreateIndex("dbo.Exiles", "CardLink_Id");
            CreateIndex("dbo.Graves", "CardLink_Id");
            CreateIndex("dbo.HandCards", "CardLink_Id");
            CreateIndex("dbo.Tops", "CardLink_Id");
            AddForeignKey("dbo.Bottoms", "CardLink_Id", "dbo.CardLinks", "Id");
            AddForeignKey("dbo.Exiles", "CardLink_Id", "dbo.CardLinks", "Id");
            AddForeignKey("dbo.Graves", "CardLink_Id", "dbo.CardLinks", "Id");
            AddForeignKey("dbo.HandCards", "CardLink_Id", "dbo.CardLinks", "Id");
            AddForeignKey("dbo.Tops", "CardLink_Id", "dbo.CardLinks", "Id");
            DropColumn("dbo.Bottoms", "Card_id");
            DropColumn("dbo.Exiles", "Card_id");
            DropColumn("dbo.Graves", "Card_id");
            DropColumn("dbo.HandCards", "Card_id");
            DropColumn("dbo.Tops", "Card_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tops", "Card_id", c => c.String(maxLength: 128));
            AddColumn("dbo.HandCards", "Card_id", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Graves", "Card_id", c => c.String(maxLength: 128));
            AddColumn("dbo.Exiles", "Card_id", c => c.String(maxLength: 128));
            AddColumn("dbo.Bottoms", "Card_id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Tops", "CardLink_Id", "dbo.CardLinks");
            DropForeignKey("dbo.HandCards", "CardLink_Id", "dbo.CardLinks");
            DropForeignKey("dbo.Graves", "CardLink_Id", "dbo.CardLinks");
            DropForeignKey("dbo.Exiles", "CardLink_Id", "dbo.CardLinks");
            DropForeignKey("dbo.Bottoms", "CardLink_Id", "dbo.CardLinks");
            DropForeignKey("dbo.CardLinks", "Card_id", "dbo.Cards");
            DropIndex("dbo.Tops", new[] { "CardLink_Id" });
            DropIndex("dbo.HandCards", new[] { "CardLink_Id" });
            DropIndex("dbo.Graves", new[] { "CardLink_Id" });
            DropIndex("dbo.Exiles", new[] { "CardLink_Id" });
            DropIndex("dbo.CardLinks", new[] { "Card_id" });
            DropIndex("dbo.Bottoms", new[] { "CardLink_Id" });
            DropColumn("dbo.Tops", "CardLink_Id");
            DropColumn("dbo.HandCards", "CardLink_Id");
            DropColumn("dbo.Graves", "CardLink_Id");
            DropColumn("dbo.Exiles", "CardLink_Id");
            DropColumn("dbo.Bottoms", "CardLink_Id");
            DropTable("dbo.CardLinks");
            CreateIndex("dbo.Tops", "Card_id");
            CreateIndex("dbo.HandCards", "Card_id");
            CreateIndex("dbo.Graves", "Card_id");
            CreateIndex("dbo.Exiles", "Card_id");
            CreateIndex("dbo.Bottoms", "Card_id");
            AddForeignKey("dbo.Tops", "Card_id", "dbo.Cards", "id");
            AddForeignKey("dbo.HandCards", "Card_id", "dbo.Cards", "id", cascadeDelete: true);
            AddForeignKey("dbo.Graves", "Card_id", "dbo.Cards", "id");
            AddForeignKey("dbo.Exiles", "Card_id", "dbo.Cards", "id");
            AddForeignKey("dbo.Bottoms", "Card_id", "dbo.Cards", "id");
        }
    }
}
