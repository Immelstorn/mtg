namespace Mtg.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGameInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HandCards",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Card_id = c.String(maxLength: 128),
                        Game_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cards", t => t.Card_id)
                .ForeignKey("dbo.Games", t => t.Game_Id, cascadeDelete: true)
                .Index(t => t.Card_id)
                .Index(t => t.Game_Id);
            
            AddColumn("dbo.Games", "MyHp", c => c.Int(nullable: false, defaultValue: 20));
            AddColumn("dbo.Games", "EnemyHp", c => c.Int(nullable: false, defaultValue: 20));
            AddColumn("dbo.Games", "EnemyCards", c => c.Int(nullable: false, defaultValue: 7));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HandCards", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.HandCards", "Card_id", "dbo.Cards");
            DropIndex("dbo.HandCards", new[] { "Game_Id" });
            DropIndex("dbo.HandCards", new[] { "Card_id" });
            DropColumn("dbo.Games", "EnemyCards");
            DropColumn("dbo.Games", "EnemyHp");
            DropColumn("dbo.Games", "MyHp");
            DropTable("dbo.HandCards");
        }
    }
}
