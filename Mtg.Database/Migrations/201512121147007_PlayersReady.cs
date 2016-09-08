namespace Mtg.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlayersReady : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "FirstPlayerReady", c => c.Boolean(nullable: false));
            AddColumn("dbo.Games", "SecondPlayerReady", c => c.Boolean(nullable: false));
            DropColumn("dbo.Games", "Started");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Games", "Started", c => c.Boolean(nullable: false));
            DropColumn("dbo.Games", "SecondPlayerReady");
            DropColumn("dbo.Games", "FirstPlayerReady");
        }
    }
}
