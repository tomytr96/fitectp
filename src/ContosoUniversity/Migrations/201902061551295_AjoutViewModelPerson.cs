namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutViewModelPerson : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Person", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.Person", "ImagePath");
        }
    }
}
