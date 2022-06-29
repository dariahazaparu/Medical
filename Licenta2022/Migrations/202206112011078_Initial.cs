namespace Licenta2022.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Abonament",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Denumire = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ServiciuXAbonament",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdAbonament = c.Int(nullable: false),
                        IdServiciu = c.Int(nullable: false),
                        ProcentReducere = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Abonament", t => t.IdAbonament)
                .ForeignKey("dbo.Serviciu", t => t.IdServiciu)
                .Index(t => t.IdAbonament)
                .Index(t => t.IdServiciu);
            
            CreateTable(
                "dbo.Serviciu",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Denumire = c.String(nullable: false),
                        Pret = c.Single(nullable: false),
                        IdSpecializare = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Specializare", t => t.IdSpecializare)
                .Index(t => t.IdSpecializare);
            
            CreateTable(
                "dbo.Programare",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        Prezent = c.Boolean(nullable: false),
                        IdTrimitereParinte = c.Int(),
                        IdPacient = c.Int(nullable: false),
                        IdDoctor = c.Int(nullable: false),
                        IdServiciu = c.Int(),
                        TrimitereParinte_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctor", t => t.IdDoctor)
                .ForeignKey("dbo.Pacient", t => t.IdPacient)
                .ForeignKey("dbo.Serviciu", t => t.IdServiciu)
                .ForeignKey("dbo.Trimitere", t => t.IdTrimitereParinte)
                .ForeignKey("dbo.Trimitere", t => t.TrimitereParinte_Id)
                .Index(t => t.IdTrimitereParinte)
                .Index(t => t.IdPacient)
                .Index(t => t.IdDoctor)
                .Index(t => t.IdServiciu)
                .Index(t => t.TrimitereParinte_Id);
            
            CreateTable(
                "dbo.Doctor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nume = c.String(nullable: false),
                        Prenume = c.String(nullable: false),
                        DataAngajarii = c.DateTime(nullable: false),
                        IdSpecializare = c.Int(),
                        IdClinica = c.Int(nullable: false),
                        Specializare_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clinica", t => t.IdClinica)
                .ForeignKey("dbo.Specializare", t => t.Specializare_Id)
                .ForeignKey("dbo.Specializare", t => t.IdSpecializare)
                .Index(t => t.IdSpecializare)
                .Index(t => t.IdClinica)
                .Index(t => t.Specializare_Id);
            
            CreateTable(
                "dbo.Clinica",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Nume = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Adresa", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Adresa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Strada = c.String(nullable: false),
                        Numar = c.Int(nullable: false),
                        IdLocalitate = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Localitate", t => t.IdLocalitate)
                .Index(t => t.IdLocalitate);
            
            CreateTable(
                "dbo.Localitate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nume = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pacient",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nume = c.String(nullable: false),
                        Prenume = c.String(nullable: false),
                        CNP = c.String(nullable: false),
                        IdAdresa = c.Int(nullable: false),
                        IdAbonament = c.Int(),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Abonament", t => t.IdAbonament)
                .ForeignKey("dbo.Adresa", t => t.IdAdresa)
                .Index(t => t.IdAdresa)
                .Index(t => t.IdAbonament);
            
            CreateTable(
                "dbo.PacientXDiagnosticXProgramare",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdPacient = c.Int(nullable: false),
                        IdDiagnostic = c.Int(nullable: false),
                        IdProgramare = c.Int(nullable: false),
                        Data = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Diagnostic", t => t.IdDiagnostic)
                .ForeignKey("dbo.Pacient", t => t.IdPacient)
                .ForeignKey("dbo.Programare", t => t.IdProgramare)
                .Index(t => t.IdPacient)
                .Index(t => t.IdDiagnostic)
                .Index(t => t.IdProgramare);
            
            CreateTable(
                "dbo.Diagnostic",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Denumire = c.String(nullable: false),
                        Descriere = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DoctorXProgramTemplate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdDoctor = c.Int(nullable: false),
                        IdProgramTemplate = c.Int(nullable: false),
                        Config = c.String(),
                        Data = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctor", t => t.IdDoctor)
                .ForeignKey("dbo.ProgramTemplate", t => t.IdProgramTemplate)
                .Index(t => t.IdDoctor)
                .Index(t => t.IdProgramTemplate);
            
            CreateTable(
                "dbo.ProgramTemplate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Config = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Specializare",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Denumire = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Trimitere",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Observatii = c.String(),
                        IdProgramareParinte = c.Int(nullable: false),
                        IdSpecializare = c.Int(nullable: false),
                        ProgramareParinte_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Programare", t => t.IdProgramareParinte)
                .ForeignKey("dbo.Programare", t => t.ProgramareParinte_Id)
                .ForeignKey("dbo.Specializare", t => t.IdSpecializare)
                .Index(t => t.IdProgramareParinte)
                .Index(t => t.IdSpecializare)
                .Index(t => t.ProgramareParinte_Id);
            
            CreateTable(
                "dbo.Factura",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Total = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Programare", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Reteta",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        DataEmiterii = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Programare", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.RetetaXMedicament",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdReteta = c.Int(nullable: false),
                        IdMedicament = c.Int(nullable: false),
                        Doza = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Medicament", t => t.IdMedicament)
                .ForeignKey("dbo.Reteta", t => t.IdReteta)
                .Index(t => t.IdReteta)
                .Index(t => t.IdMedicament);
            
            CreateTable(
                "dbo.Medicament",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Denumire = c.String(nullable: false),
                        Observatii = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
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
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
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
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.TrimitereServiciu",
                c => new
                    {
                        Trimitere_Id = c.Int(nullable: false),
                        Serviciu_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Trimitere_Id, t.Serviciu_Id })
                .ForeignKey("dbo.Trimitere", t => t.Trimitere_Id, cascadeDelete: true)
                .ForeignKey("dbo.Serviciu", t => t.Serviciu_Id, cascadeDelete: true)
                .Index(t => t.Trimitere_Id)
                .Index(t => t.Serviciu_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ServiciuXAbonament", "IdServiciu", "dbo.Serviciu");
            DropForeignKey("dbo.Serviciu", "IdSpecializare", "dbo.Specializare");
            DropForeignKey("dbo.Programare", "TrimitereParinte_Id", "dbo.Trimitere");
            DropForeignKey("dbo.Programare", "IdTrimitereParinte", "dbo.Trimitere");
            DropForeignKey("dbo.Programare", "IdServiciu", "dbo.Serviciu");
            DropForeignKey("dbo.RetetaXMedicament", "IdReteta", "dbo.Reteta");
            DropForeignKey("dbo.RetetaXMedicament", "IdMedicament", "dbo.Medicament");
            DropForeignKey("dbo.Reteta", "Id", "dbo.Programare");
            DropForeignKey("dbo.Programare", "IdPacient", "dbo.Pacient");
            DropForeignKey("dbo.Factura", "Id", "dbo.Programare");
            DropForeignKey("dbo.Programare", "IdDoctor", "dbo.Doctor");
            DropForeignKey("dbo.Doctor", "IdSpecializare", "dbo.Specializare");
            DropForeignKey("dbo.Trimitere", "IdSpecializare", "dbo.Specializare");
            DropForeignKey("dbo.TrimitereServiciu", "Serviciu_Id", "dbo.Serviciu");
            DropForeignKey("dbo.TrimitereServiciu", "Trimitere_Id", "dbo.Trimitere");
            DropForeignKey("dbo.Trimitere", "ProgramareParinte_Id", "dbo.Programare");
            DropForeignKey("dbo.Trimitere", "IdProgramareParinte", "dbo.Programare");
            DropForeignKey("dbo.Doctor", "Specializare_Id", "dbo.Specializare");
            DropForeignKey("dbo.DoctorXProgramTemplate", "IdProgramTemplate", "dbo.ProgramTemplate");
            DropForeignKey("dbo.DoctorXProgramTemplate", "IdDoctor", "dbo.Doctor");
            DropForeignKey("dbo.Doctor", "IdClinica", "dbo.Clinica");
            DropForeignKey("dbo.Clinica", "Id", "dbo.Adresa");
            DropForeignKey("dbo.PacientXDiagnosticXProgramare", "IdProgramare", "dbo.Programare");
            DropForeignKey("dbo.PacientXDiagnosticXProgramare", "IdPacient", "dbo.Pacient");
            DropForeignKey("dbo.PacientXDiagnosticXProgramare", "IdDiagnostic", "dbo.Diagnostic");
            DropForeignKey("dbo.Pacient", "IdAdresa", "dbo.Adresa");
            DropForeignKey("dbo.Pacient", "IdAbonament", "dbo.Abonament");
            DropForeignKey("dbo.Adresa", "IdLocalitate", "dbo.Localitate");
            DropForeignKey("dbo.ServiciuXAbonament", "IdAbonament", "dbo.Abonament");
            DropIndex("dbo.TrimitereServiciu", new[] { "Serviciu_Id" });
            DropIndex("dbo.TrimitereServiciu", new[] { "Trimitere_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.RetetaXMedicament", new[] { "IdMedicament" });
            DropIndex("dbo.RetetaXMedicament", new[] { "IdReteta" });
            DropIndex("dbo.Reteta", new[] { "Id" });
            DropIndex("dbo.Factura", new[] { "Id" });
            DropIndex("dbo.Trimitere", new[] { "ProgramareParinte_Id" });
            DropIndex("dbo.Trimitere", new[] { "IdSpecializare" });
            DropIndex("dbo.Trimitere", new[] { "IdProgramareParinte" });
            DropIndex("dbo.DoctorXProgramTemplate", new[] { "IdProgramTemplate" });
            DropIndex("dbo.DoctorXProgramTemplate", new[] { "IdDoctor" });
            DropIndex("dbo.PacientXDiagnosticXProgramare", new[] { "IdProgramare" });
            DropIndex("dbo.PacientXDiagnosticXProgramare", new[] { "IdDiagnostic" });
            DropIndex("dbo.PacientXDiagnosticXProgramare", new[] { "IdPacient" });
            DropIndex("dbo.Pacient", new[] { "IdAbonament" });
            DropIndex("dbo.Pacient", new[] { "IdAdresa" });
            DropIndex("dbo.Adresa", new[] { "IdLocalitate" });
            DropIndex("dbo.Clinica", new[] { "Id" });
            DropIndex("dbo.Doctor", new[] { "Specializare_Id" });
            DropIndex("dbo.Doctor", new[] { "IdClinica" });
            DropIndex("dbo.Doctor", new[] { "IdSpecializare" });
            DropIndex("dbo.Programare", new[] { "TrimitereParinte_Id" });
            DropIndex("dbo.Programare", new[] { "IdServiciu" });
            DropIndex("dbo.Programare", new[] { "IdDoctor" });
            DropIndex("dbo.Programare", new[] { "IdPacient" });
            DropIndex("dbo.Programare", new[] { "IdTrimitereParinte" });
            DropIndex("dbo.Serviciu", new[] { "IdSpecializare" });
            DropIndex("dbo.ServiciuXAbonament", new[] { "IdServiciu" });
            DropIndex("dbo.ServiciuXAbonament", new[] { "IdAbonament" });
            DropTable("dbo.TrimitereServiciu");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Medicament");
            DropTable("dbo.RetetaXMedicament");
            DropTable("dbo.Reteta");
            DropTable("dbo.Factura");
            DropTable("dbo.Trimitere");
            DropTable("dbo.Specializare");
            DropTable("dbo.ProgramTemplate");
            DropTable("dbo.DoctorXProgramTemplate");
            DropTable("dbo.Diagnostic");
            DropTable("dbo.PacientXDiagnosticXProgramare");
            DropTable("dbo.Pacient");
            DropTable("dbo.Localitate");
            DropTable("dbo.Adresa");
            DropTable("dbo.Clinica");
            DropTable("dbo.Doctor");
            DropTable("dbo.Programare");
            DropTable("dbo.Serviciu");
            DropTable("dbo.ServiciuXAbonament");
            DropTable("dbo.Abonament");
        }
    }
}
