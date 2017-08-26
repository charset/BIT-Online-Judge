namespace BITOJ.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTimeMemoryLimit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProblemEntities", "TimeLimit", c => c.Int(nullable: false));
            AddColumn("dbo.ProblemEntities", "MemoryLimit", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProblemEntities", "MemoryLimit");
            DropColumn("dbo.ProblemEntities", "TimeLimit");
        }
    }
}
