namespace VGRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        customerID = c.Int(nullable: false, identity: true),
                        DOB = c.DateTime(),
                        Email = c.String(nullable: false, maxLength: 254),
                        Name = c.String(nullable: false, maxLength: 70),
                        UserName = c.String(nullable: false, maxLength: 254),
                        Password = c.String(nullable: false),
                        ShippingAddress = c.String(nullable: false, maxLength: 95),
                        City = c.String(nullable: false, maxLength: 35),
                        State = c.String(nullable: false, maxLength: 20),
                        ZIP = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.customerID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Guid(nullable: false),
                        GameName = c.String(nullable: false, maxLength: 128),
                        SystemName = c.String(nullable: false, maxLength: 128),
                        CustomerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .ForeignKey("dbo.Games", t => new { t.SystemName, t.GameName }, cascadeDelete: true)
                .Index(t => new { t.SystemName, t.GameName })
                .Index(t => t.CustomerID);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        SystemName = c.String(nullable: false, maxLength: 128),
                        GameName = c.String(nullable: false, maxLength: 128),
                        Multiplayer = c.Boolean(),
                        NumberOfPlayers = c.Int(),
                        Rating = c.String(),
                        Genre = c.String(),
                        Developer = c.String(),
                        Publisher = c.String(),
                        ReleaseDate = c.DateTime(),
                        QuantityAvailable = c.Int(nullable: false),
                        TotalQuantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SystemName, t.GameName })
                .ForeignKey("dbo.Systems", t => t.SystemName, cascadeDelete: true)
                .Index(t => t.SystemName);
            
            CreateTable(
                "dbo.Systems",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Manf = c.String(),
                        Version = c.String(),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.Promotions",
                c => new
                    {
                        OrderID = c.Int(nullable: false),
                        PromotionID = c.Int(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                        Amount = c.Int(nullable: false),
                        Order_OrderID = c.Guid(nullable: false),
                        OrderDetails_OrderID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.OrderID, t.PromotionID })
                .ForeignKey("dbo.Orders", t => t.Order_OrderID)
                .ForeignKey("dbo.OrderDetails", t => t.OrderDetails_OrderID)
                .Index(t => t.Order_OrderID)
                .Index(t => t.OrderDetails_OrderID);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        OrderID = c.Guid(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        ShipDate = c.DateTime(),
                        Status = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID);
            
            CreateTable(
                "dbo.Subscriptions",
                c => new
                    {
                        SubscriptionID = c.Int(nullable: false, identity: true),
                        customerID = c.Int(nullable: false),
                        Price = c.Single(nullable: false),
                        ExpirationDate = c.DateTime(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.SubscriptionID)
                .ForeignKey("dbo.Customers", t => t.customerID, cascadeDelete: true)
                .Index(t => t.customerID);
            
            CreateTable(
                "dbo.OrderStatus",
                c => new
                    {
                        Status = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Status);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subscriptions", "customerID", "dbo.Customers");
            DropForeignKey("dbo.Promotions", "OrderDetails_OrderID", "dbo.OrderDetails");
            DropForeignKey("dbo.Promotions", "Order_OrderID", "dbo.Orders");
            DropForeignKey("dbo.Orders", new[] { "SystemName", "GameName" }, "dbo.Games");
            DropForeignKey("dbo.Games", "SystemName", "dbo.Systems");
            DropForeignKey("dbo.Orders", "CustomerID", "dbo.Customers");
            DropIndex("dbo.Subscriptions", new[] { "customerID" });
            DropIndex("dbo.Promotions", new[] { "OrderDetails_OrderID" });
            DropIndex("dbo.Promotions", new[] { "Order_OrderID" });
            DropIndex("dbo.Games", new[] { "SystemName" });
            DropIndex("dbo.Orders", new[] { "CustomerID" });
            DropIndex("dbo.Orders", new[] { "SystemName", "GameName" });
            DropTable("dbo.OrderStatus");
            DropTable("dbo.Subscriptions");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.Promotions");
            DropTable("dbo.Systems");
            DropTable("dbo.Games");
            DropTable("dbo.Orders");
            DropTable("dbo.Customers");
        }
    }
}
