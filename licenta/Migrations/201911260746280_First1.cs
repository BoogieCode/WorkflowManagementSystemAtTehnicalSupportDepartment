namespace licenta.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class First1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Department",
                c => new
                {
                    departmentId = c.Int(nullable: false, identity: true),
                    name = c.String(nullable: false, maxLength: 50),
                    description = c.String(maxLength: 500),
                })
                .PrimaryKey(t => t.departmentId);

            CreateTable(
                "dbo.User",
                c => new
                {
                    userId = c.Int(nullable: false, identity: true),
                    company = c.String(nullable: false, maxLength: 50),
                    type = c.Int(nullable: false),
                    username = c.String(nullable: false, maxLength: 50),
                    password = c.String(nullable: false, maxLength: 50),
                    email = c.String(nullable: false, maxLength: 50),
                    departmentId = c.Int(),
                    role = c.Int(),
                })
                .PrimaryKey(t => t.userId)
                .ForeignKey("dbo.Department", t => t.departmentId)
                .Index(t => t.departmentId);

            CreateTable(
                "dbo.Request",
                c => new
                {
                    requestId = c.Int(nullable: false, identity: true),
                    createdBy = c.Int(nullable: false),
                    title = c.String(nullable: false, maxLength: 50),
                    description = c.String(nullable: false, maxLength: 500),
                    type = c.Boolean(nullable: false),
                    departmentAssigned = c.String(maxLength: 50),
                    employeeAssigned = c.String(maxLength: 50),
                    image = c.String(unicode: false),
                    priority = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.requestId)
                .ForeignKey("dbo.User", t => t.createdBy)
                .Index(t => t.createdBy);

            CreateTable(
                "dbo.RequestHistory",
                c => new
                {
                    requestHistoryId = c.Int(nullable: false, identity: true),
                    requestId = c.Int(nullable: false),
                    from = c.String(nullable: false, maxLength: 50),
                    to = c.String(nullable: false, maxLength: 50),
                    data = c.DateTime(nullable: false),
                    approval = c.String(maxLength: 50),
                    message = c.String(maxLength: 50),
                    status = c.String(nullable: false, maxLength: 50),
                })
                .PrimaryKey(t => t.requestHistoryId);

            CreateTable(
                "dbo.sysdiagrams",
                c => new
                {
                    diagram_id = c.Int(nullable: false, identity: true),
                    name = c.String(nullable: false, maxLength: 128),
                    principal_id = c.Int(nullable: false),
                    version = c.Int(),
                    definition = c.Binary(),
                })
                .PrimaryKey(t => t.diagram_id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Request", "createdBy", "dbo.User");
            DropForeignKey("dbo.User", "departmentId", "dbo.Department");
            DropIndex("dbo.Request", new[] { "createdBy" });
            DropIndex("dbo.User", new[] { "departmentId" });
            DropTable("dbo.sysdiagrams");
            DropTable("dbo.RequestHistory");
            DropTable("dbo.Request");
            DropTable("dbo.User");
            DropTable("dbo.Department");
        }
    }
}
