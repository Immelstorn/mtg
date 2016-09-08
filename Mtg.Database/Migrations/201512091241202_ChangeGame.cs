namespace Mtg.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeGame : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "Started", c => c.Boolean(nullable: false));
            AddColumn("dbo.Games", "Finished", c => c.Boolean(nullable: false));
            DropColumn("dbo.Games", "SessionId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Games", "SessionId", c => c.String());
            DropColumn("dbo.Games", "Finished");
            DropColumn("dbo.Games", "Started");
        }
    }
}
