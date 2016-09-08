namespace Mtg.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFinishedField : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Games", "Finished");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Games", "Finished", c => c.Boolean(nullable: false));
        }
    }
}