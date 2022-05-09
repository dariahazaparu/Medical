namespace Licenta2022.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Adresas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Strada = c.String(nullable: false),
                        Numar = c.Int(nullable: false),
                        IdLocalitate = c.Int(nullable: false),
                        Localitate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Localitates", t => t.Localitate_Id)
                .Index(t => t.Localitate_Id);
            
            CreateTable(
                "dbo.Clinicas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nume = c.String(nullable: false),
                        IdAdresa = c.Int(nullable: false),
                        Adresa_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Adresas", t => t.Adresa_Id)
                .Index(t => t.Adresa_Id);
            
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nume = c.String(nullable: false),
                        Prenume = c.String(nullable: false),
                        DataAngajarii = c.DateTime(nullable: false),
                        IdSpecialitate = c.Int(nullable: false),
                        IdClinica = c.Int(nullable: false),
                        Clinica_Id = c.Int(),
                        Specialitate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clinicas", t => t.Clinica_Id)
                .ForeignKey("dbo.Specialitates", t => t.Specialitate_Id)
                .Index(t => t.Clinica_Id)
                .Index(t => t.Specialitate_Id);
            
            CreateTable(
                "dbo.Specialitates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Denumire = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Servicius",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Denumire = c.String(nullable: false),
                        IdSpecialitate = c.Int(nullable: false),
                        Specialitate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Specialitates", t => t.Specialitate_Id)
                .Index(t => t.Specialitate_Id);
            
            CreateTable(
                "dbo.Localitates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nume = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Asigurares",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Denumire = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Medicaments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Denumire = c.String(nullable: false),
                        Observatii = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RetetaXMedicaments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdReteta = c.Int(nullable: false),
                        IdMedicament = c.Int(nullable: false),
                        Doza = c.String(),
                        Medicament_Id = c.Int(),
                        Reteta_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Medicaments", t => t.Medicament_Id)
                .ForeignKey("dbo.Retetas", t => t.Reteta_Id)
                .Index(t => t.Medicament_Id)
                .Index(t => t.Reteta_Id);
            
            CreateTable(
                "dbo.Retetas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DataEmiterii = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pacients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nume = c.String(nullable: false),
                        Prenume = c.String(nullable: false),
                        CNP = c.String(nullable: false),
                        IdAdresa = c.Int(nullable: false),
                        IdAsigurare = c.Int(nullable: false),
                        Adresa_Id = c.Int(),
                        Asigurare_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Adresas", t => t.Adresa_Id)
                .ForeignKey("dbo.Asigurares", t => t.Asigurare_Id)
                .Index(t => t.Adresa_Id)
                .Index(t => t.Asigurare_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Pacients", "Asigurare_Id", "dbo.Asigurares");
            DropForeignKey("dbo.Pacients", "Adresa_Id", "dbo.Adresas");
            DropForeignKey("dbo.RetetaXMedicaments", "Reteta_Id", "dbo.Retetas");
            DropForeignKey("dbo.RetetaXMedicaments", "Medicament_Id", "dbo.Medicaments");
            DropForeignKey("dbo.Adresas", "Localitate_Id", "dbo.Localitates");
            DropForeignKey("dbo.Servicius", "Specialitate_Id", "dbo.Specialitates");
            DropForeignKey("dbo.Doctors", "Specialitate_Id", "dbo.Specialitates");
            DropForeignKey("dbo.Doctors", "Clinica_Id", "dbo.Clinicas");
            DropForeignKey("dbo.Clinicas", "Adresa_Id", "dbo.Adresas");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Pacients", new[] { "Asigurare_Id" });
            DropIndex("dbo.Pacients", new[] { "Adresa_Id" });
            DropIndex("dbo.RetetaXMedicaments", new[] { "Reteta_Id" });
            DropIndex("dbo.RetetaXMedicaments", new[] { "Medicament_Id" });
            DropIndex("dbo.Servicius", new[] { "Specialitate_Id" });
            DropIndex("dbo.Doctors", new[] { "Specialitate_Id" });
            DropIndex("dbo.Doctors", new[] { "Clinica_Id" });
            DropIndex("dbo.Clinicas", new[] { "Adresa_Id" });
            DropIndex("dbo.Adresas", new[] { "Localitate_Id" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Pacients");
            DropTable("dbo.Retetas");
            DropTable("dbo.RetetaXMedicaments");
            DropTable("dbo.Medicaments");
            DropTable("dbo.Asigurares");
            DropTable("dbo.Localitates");
            DropTable("dbo.Servicius");
            DropTable("dbo.Specialitates");
            DropTable("dbo.Doctors");
            DropTable("dbo.Clinicas");
            DropTable("dbo.Adresas");
        }
    }
}
