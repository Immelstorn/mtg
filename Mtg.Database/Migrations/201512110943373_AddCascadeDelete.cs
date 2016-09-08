namespace Mtg.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCascadeDelete : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bottoms", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Exiles", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Graves", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Tops", "Game_Id", "dbo.Games");
            AddForeignKey("dbo.Bottoms", "Game_Id", "dbo.Games", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Exiles", "Game_Id", "dbo.Games", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Graves", "Game_Id", "dbo.Games", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Tops", "Game_Id", "dbo.Games", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tops", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Graves", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Exiles", "Game_Id", "dbo.Games");
            DropForeignKey("dbo.Bottoms", "Game_Id", "dbo.Games");
            AddForeignKey("dbo.Tops", "Game_Id", "dbo.Games", "Id");
            AddForeignKey("dbo.Graves", "Game_Id", "dbo.Games", "Id");
            AddForeignKey("dbo.Exiles", "Game_Id", "dbo.Games", "Id");
            AddForeignKey("dbo.Bottoms", "Game_Id", "dbo.Games", "Id");
        }
    }
}
