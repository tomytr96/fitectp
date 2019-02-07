namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModificationControlleur : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Person", "Role");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Person", "Role", c => c.String());
        }
    }
}
