namespace Mtg.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaveTokensToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tokens",
                c => new
                    {
                        id = c.Guid(nullable: false, identity: true),
                        ImgLink = c.String(),
                        text = c.String(),
                        name = c.String(),
                        Color = c.String(),
                        CardType = c.String(),
                        Power = c.String(),
                        Toughness = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.CardLinks", "Token_id", c => c.Guid());
            CreateIndex("dbo.CardLinks", "Token_id");
            AddForeignKey("dbo.CardLinks", "Token_id", "dbo.Tokens", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CardLinks", "Token_id", "dbo.Tokens");
            DropIndex("dbo.CardLinks", new[] { "Token_id" });
            DropColumn("dbo.CardLinks", "Token_id");
            DropTable("dbo.Tokens");
        }
    }
}
