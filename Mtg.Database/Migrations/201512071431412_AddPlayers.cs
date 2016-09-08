namespace Mtg.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPlayers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(defaultValue: "Random Player"),
                        SessionId = c.String(),
                        Game_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.Game_Id)
                .Index(t => t.Game_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Players", "Game_Id", "dbo.Games");
            DropIndex("dbo.Players", new[] { "Game_Id" });
            DropTable("dbo.Players");
        }
    }
}
