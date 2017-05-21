namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGenreNameLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Genres", "Name", c => c.String(nullable: false, maxLength: 255));
            Sql("INSERT INTO Genres(Id, Name) VALUES (1,'JAZZ')");
            Sql("INSERT INTO Genres(Id, Name) VALUES (2,'CLASSIC')");
            Sql("INSERT INTO Genres(Id, Name) VALUES (3,'ROCK')");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Genres", "Name", c => c.String(nullable: false));
        }
    }
}
