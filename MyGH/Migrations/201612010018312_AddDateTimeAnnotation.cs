namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateTimeAnnotation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gigs", "Time", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Gigs", "Time");
        }
    }
}
