namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModificationControlleur : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Person", "Role");
            DropColumn("dbo.Person", "ConfirmPassword");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Person", "ConfirmPassword", c => c.String());
            AddColumn("dbo.Person", "Role", c => c.String());
        }
    }
}
