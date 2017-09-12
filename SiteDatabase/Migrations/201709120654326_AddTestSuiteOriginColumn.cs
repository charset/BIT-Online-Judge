namespace BITOJ.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTestSuiteOriginColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProblemEntities", "TestSuiteOrigin", builder => builder.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProblemEntities", "TestSuiteOrigin");
        }
    }
}
