namespace Mtg.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovePlayersToGames_AddFirstAndSecondPlayers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PlayerGames", "Player_Id", "dbo.Players");
            DropForeignKey("dbo.PlayerGames", "Game_Id", "dbo.Games");
            DropIndex("dbo.PlayerGames", new[] { "Player_Id" });
            DropIndex("dbo.PlayerGames", new[] { "Game_Id" });
            AddColumn("dbo.Games", "FirstPlayerHp", c => c.Int(nullable: false));
            AddColumn("dbo.Games", "SecondPlayerHp", c => c.Int(nullable: false));
            AddColumn("dbo.Games", "FirstPlayer_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Games", "SecondPlayer_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Games", "FirstPlayer_Id");
            CreateIndex("dbo.Games", "SecondPlayer_Id");
            AddForeignKey("dbo.Games", "FirstPlayer_Id", "dbo.Players", "Id");
            AddForeignKey("dbo.Games", "SecondPlayer_Id", "dbo.Players", "Id");
            DropColumn("dbo.Games", "MyHp");
            DropColumn("dbo.Games", "EnemyHp");
            DropColumn("dbo.Games", "MyCards");
            DropColumn("dbo.Games", "EnemyCards");
            DropTable("dbo.PlayerGames");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PlayerGames",
                c => new
                    {
                        Player_Id = c.String(nullable: false, maxLength: 128),
                        Game_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Player_Id, t.Game_Id });
            
            AddColumn("dbo.Games", "EnemyCards", c => c.Int(nullable: false));
            AddColumn("dbo.Games", "MyCards", c => c.Int(nullable: false));
            AddColumn("dbo.Games", "EnemyHp", c => c.Int(nullable: false));
            AddColumn("dbo.Games", "MyHp", c => c.Int(nullable: false));
            DropForeignKey("dbo.Games", "SecondPlayer_Id", "dbo.Players");
            DropForeignKey("dbo.Games", "FirstPlayer_Id", "dbo.Players");
            DropIndex("dbo.Games", new[] { "SecondPlayer_Id" });
            DropIndex("dbo.Games", new[] { "FirstPlayer_Id" });
            DropColumn("dbo.Games", "SecondPlayer_Id");
            DropColumn("dbo.Games", "FirstPlayer_Id");
            DropColumn("dbo.Games", "SecondPlayerHp");
            DropColumn("dbo.Games", "FirstPlayerHp");
            CreateIndex("dbo.PlayerGames", "Game_Id");
            CreateIndex("dbo.PlayerGames", "Player_Id");
            AddForeignKey("dbo.PlayerGames", "Game_Id", "dbo.Games", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PlayerGames", "Player_Id", "dbo.Players", "Id", cascadeDelete: true);
        }
    }
}
