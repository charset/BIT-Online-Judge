namespace BITOJ.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddContestId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProblemEntities", "ContestId", builder => builder.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProblemEntities", "ContestId");
        }
    }
}
