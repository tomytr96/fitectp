namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Person", "Email", c => c.String());
            AddColumn("dbo.Person", "Username", c => c.String(nullable: false));
            AddColumn("dbo.Person", "Password", c => c.String(nullable: false));
            AddColumn("dbo.Person", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Person", "ImagePath");
            DropColumn("dbo.Person", "Password");
            DropColumn("dbo.Person", "Username");
            DropColumn("dbo.Person", "Email");
        }
    }
}
