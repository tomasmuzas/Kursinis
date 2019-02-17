namespace MigrationDemo.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddProducts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DbProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Isin = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.DbInvestmentAgreements", "Product_Id", c => c.Int());
            
            CreateIndex("dbo.DbInvestmentAgreements", "Product_Id");

            Sql("UPDATE dbo.DbInvestmentAgreements SET Product_Id = 1 WHERE Product_Id IS NULL");
        }

        public override void Down()
        {
            DropIndex("dbo.DbInvestmentAgreements", new[] { "Product_Id" });
            DropColumn("dbo.DbInvestmentAgreements", "Product_Id");
            DropTable("dbo.DbProducts");
        }
    }
}
