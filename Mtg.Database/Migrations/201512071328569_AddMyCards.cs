namespace Mtg.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMyCards : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "MyCards", c => c.Int(nullable: false, defaultValue: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "MyCards");
        }
    }
}
