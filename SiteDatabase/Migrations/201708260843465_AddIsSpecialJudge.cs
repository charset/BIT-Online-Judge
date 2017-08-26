namespace BITOJ.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsSpecialJudge : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProblemEntities", "IsSpecialJudge", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProblemEntities", "IsSpecialJudge");
        }
    }
}
