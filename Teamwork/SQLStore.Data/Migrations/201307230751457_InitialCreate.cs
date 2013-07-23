namespace SQLStore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BasePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MeasureId = c.Int(nullable: false),
                        ProductName = c.String(),
                        VendorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Measures", t => t.MeasureId, cascadeDelete: true)
                .ForeignKey("dbo.Vendors", t => t.VendorId, cascadeDelete: true)
                .Index(t => t.MeasureId)
                .Index(t => t.VendorId);
            
            CreateTable(
                "dbo.Measures",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        MeasureName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vendors",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        VendorName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationID = c.Int(nullable: false, identity: true),
                        LocationName = c.String(),
                    })
                .PrimaryKey(t => t.LocationID);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        SaleID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LocationID = c.Int(nullable: false),
                        SaleOnDateID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SaleID)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.LocationID, cascadeDelete: true)
                .ForeignKey("dbo.SaleOnDates", t => t.SaleOnDateID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.LocationID)
                .Index(t => t.SaleOnDateID);
            
            CreateTable(
                "dbo.SaleOnDates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Sales", new[] { "SaleOnDateID" });
            DropIndex("dbo.Sales", new[] { "LocationID" });
            DropIndex("dbo.Sales", new[] { "ProductID" });
            DropIndex("dbo.Products", new[] { "VendorId" });
            DropIndex("dbo.Products", new[] { "MeasureId" });
            DropForeignKey("dbo.Sales", "SaleOnDateID", "dbo.SaleOnDates");
            DropForeignKey("dbo.Sales", "LocationID", "dbo.Locations");
            DropForeignKey("dbo.Sales", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Products", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.Products", "MeasureId", "dbo.Measures");
            DropTable("dbo.SaleOnDates");
            DropTable("dbo.Sales");
            DropTable("dbo.Locations");
            DropTable("dbo.Vendors");
            DropTable("dbo.Measures");
            DropTable("dbo.Products");
        }
    }
}
