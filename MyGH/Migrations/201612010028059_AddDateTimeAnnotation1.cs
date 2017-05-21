namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateTimeAnnotation1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Gigs", "Time");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Gigs", "Time", c => c.DateTime(nullable: false));
        }
    }
}
