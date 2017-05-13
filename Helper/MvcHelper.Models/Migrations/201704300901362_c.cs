namespace Models
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class c : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Labels", "Remark", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Labels", "Remark");
        }
    }
}
