namespace EFConsole.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameProduct : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.DbInvestmentAgreements", name: "Product_Id", newName: "DbProduct_Id");
            RenameIndex(table: "dbo.DbInvestmentAgreements", name: "IX_Product_Id", newName: "IX_DbProduct_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.DbInvestmentAgreements", name: "IX_DbProduct_Id", newName: "IX_Product_Id");
            RenameColumn(table: "dbo.DbInvestmentAgreements", name: "DbProduct_Id", newName: "Product_Id");
        }
    }
}
