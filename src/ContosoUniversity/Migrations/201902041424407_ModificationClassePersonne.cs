namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModificationClassePersonne : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Person", "Login", c => c.String(nullable: false));
            AddColumn("dbo.Person", "Password", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Person", "Password");
            DropColumn("dbo.Person", "Login");
        }
    }
}
