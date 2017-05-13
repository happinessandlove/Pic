namespace Models
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class b : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Labels", "Effective", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Labels", "Effective", c => c.Boolean(nullable: false));
        }
    }
}
