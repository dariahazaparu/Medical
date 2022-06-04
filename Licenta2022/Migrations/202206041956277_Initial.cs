namespace Licenta2022.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Adresa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Strada = c.String(nullable: false),
                        Numar = c.Int(nullable: false),
                        Localitate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Localitate", t => t.Localitate_Id)
                .Index(t => t.Localitate_Id);
            
            CreateTable(
                "dbo.Clinica",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nume = c.String(nullable: false),
                        Adresa_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Adresa", t => t.Adresa_Id)
                .Index(t => t.Adresa_Id);
            
            CreateTable(
                "dbo.Doctor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nume = c.String(nullable: false),
                        Prenume = c.String(nullable: false),
                        DataAngajarii = c.DateTime(nullable: false),
                        Clinica_Id = c.Int(),
                        ProgramTemplate_Id = c.Int(),
                        Specialitate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clinica", t => t.Clinica_Id)
                .ForeignKey("dbo.ProgramTemplate", t => t.ProgramTemplate_Id)
                .ForeignKey("dbo.Specialitate", t => t.Specialitate_Id)
                .Index(t => t.Clinica_Id)
                .Index(t => t.ProgramTemplate_Id)
                .Index(t => t.Specialitate_Id);
            
            CreateTable(
                "dbo.DoctorXProgramTemplate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdDoctor = c.Int(nullable: false),
                        IdProgramTemplate = c.Int(nullable: false),
                        Config = c.String(),
                        Data = c.DateTime(nullable: false),
                        Doctor_Id = c.Int(),
                        ProgramTemplate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctor", t => t.Doctor_Id)
                .ForeignKey("dbo.ProgramTemplate", t => t.ProgramTemplate_Id)
                .Index(t => t.Doctor_Id)
                .Index(t => t.ProgramTemplate_Id);
            
            CreateTable(
                "dbo.ProgramTemplate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Config = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Specialitate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Denumire = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Serviciu",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Denumire = c.String(nullable: false),
                        Pret = c.Single(nullable: false),
                        Specialitate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Specialitate", t => t.Specialitate_Id)
                .Index(t => t.Specialitate_Id);
            
            CreateTable(
                "dbo.ServiciuXAsigurare",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdAsigurare = c.Int(nullable: false),
                        IdServiciu = c.Int(nullable: false),
                        ProcentReducere = c.Int(nullable: false),
                        Asigurare_Id = c.Int(),
                        Serviciu_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Asigurare", t => t.Asigurare_Id)
                .ForeignKey("dbo.Serviciu", t => t.Serviciu_Id)
                .Index(t => t.Asigurare_Id)
                .Index(t => t.Serviciu_Id);
            
            CreateTable(
                "dbo.Asigurare",
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
                        Pacient_Id = c.Int(),
                        Programare_Id = c.Int(),
                        ProgramareT_Id = c.Int(),
                        Specialitate_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pacient", t => t.Pacient_Id)
                .ForeignKey("dbo.Programare", t => t.Programare_Id)
                .ForeignKey("dbo.Programare", t => t.ProgramareT_Id)
                .ForeignKey("dbo.Specialitate", t => t.Specialitate_Id)
                .Index(t => t.Pacient_Id)
                .Index(t => t.Programare_Id)
                .Index(t => t.ProgramareT_Id)
                .Index(t => t.Specialitate_Id);
            
            CreateTable(
                "dbo.Pacient",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nume = c.String(nullable: false),
                        Prenume = c.String(nullable: false),
                        CNP = c.String(nullable: false),
                        Adresa_Id = c.Int(),
                        Asigurare_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Adresa", t => t.Adresa_Id)
                .ForeignKey("dbo.Asigurare", t => t.Asigurare_Id)
                .Index(t => t.Adresa_Id)
                .Index(t => t.Asigurare_Id);
            
            CreateTable(
                "dbo.PacientXDiagnostic",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdPacient = c.Int(nullable: false),
                        IdDiagnostic = c.Int(nullable: false),
                        IdProgramare = c.Int(nullable: false),
                        Data = c.DateTime(nullable: false),
                        Diagnostic_Id = c.Int(),
                        Pacient_Id = c.Int(),
                        Programare_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Diagnostic", t => t.Diagnostic_Id)
                .ForeignKey("dbo.Pacient", t => t.Pacient_Id)
                .ForeignKey("dbo.Programare", t => t.Programare_Id)
                .Index(t => t.Diagnostic_Id)
                .Index(t => t.Pacient_Id)
                .Index(t => t.Programare_Id);
            
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
                "dbo.Programare",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        Prezent = c.Boolean(nullable: false),
                        Doctor_Id = c.Int(),
                        Pacient_Id = c.Int(),
                        Trimitere_Id = c.Int(),
                        TrimitereT_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctor", t => t.Doctor_Id)
                .ForeignKey("dbo.Pacient", t => t.Pacient_Id)
                .ForeignKey("dbo.Trimitere", t => t.Trimitere_Id)
                .ForeignKey("dbo.Trimitere", t => t.TrimitereT_Id)
                .Index(t => t.Doctor_Id)
                .Index(t => t.Pacient_Id)
                .Index(t => t.Trimitere_Id)
                .Index(t => t.TrimitereT_Id);
            
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
                        Medicament_Id = c.Int(),
                        Reteta_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Medicament", t => t.Medicament_Id)
                .ForeignKey("dbo.Reteta", t => t.Reteta_Id)
                .Index(t => t.Medicament_Id)
                .Index(t => t.Reteta_Id);
            
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
                "dbo.Localitate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nume = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Factura",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Total = c.Single(nullable: false),
                        Programare_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Programare", t => t.Programare_Id)
                .Index(t => t.Programare_Id);
            
            CreateTable(
                "dbo.ProgramareFromForm",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdTrimitere = c.Int(nullable: false),
                        Data = c.DateTime(nullable: false),
                        IdPacient = c.Int(nullable: false),
                        IdDoctor = c.Int(nullable: false),
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
            DropForeignKey("dbo.Factura", "Programare_Id", "dbo.Programare");
            DropForeignKey("dbo.Adresa", "Localitate_Id", "dbo.Localitate");
            DropForeignKey("dbo.Trimitere", "Specialitate_Id", "dbo.Specialitate");
            DropForeignKey("dbo.TrimitereServiciu", "Serviciu_Id", "dbo.Serviciu");
            DropForeignKey("dbo.TrimitereServiciu", "Trimitere_Id", "dbo.Trimitere");
            DropForeignKey("dbo.Trimitere", "ProgramareT_Id", "dbo.Programare");
            DropForeignKey("dbo.Trimitere", "Programare_Id", "dbo.Programare");
            DropForeignKey("dbo.Trimitere", "Pacient_Id", "dbo.Pacient");
            DropForeignKey("dbo.PacientXDiagnostic", "Programare_Id", "dbo.Programare");
            DropForeignKey("dbo.Programare", "TrimitereT_Id", "dbo.Trimitere");
            DropForeignKey("dbo.Programare", "Trimitere_Id", "dbo.Trimitere");
            DropForeignKey("dbo.RetetaXMedicament", "Reteta_Id", "dbo.Reteta");
            DropForeignKey("dbo.RetetaXMedicament", "Medicament_Id", "dbo.Medicament");
            DropForeignKey("dbo.Reteta", "Id", "dbo.Programare");
            DropForeignKey("dbo.Programare", "Pacient_Id", "dbo.Pacient");
            DropForeignKey("dbo.Programare", "Doctor_Id", "dbo.Doctor");
            DropForeignKey("dbo.PacientXDiagnostic", "Pacient_Id", "dbo.Pacient");
            DropForeignKey("dbo.PacientXDiagnostic", "Diagnostic_Id", "dbo.Diagnostic");
            DropForeignKey("dbo.Pacient", "Asigurare_Id", "dbo.Asigurare");
            DropForeignKey("dbo.Pacient", "Adresa_Id", "dbo.Adresa");
            DropForeignKey("dbo.Serviciu", "Specialitate_Id", "dbo.Specialitate");
            DropForeignKey("dbo.ServiciuXAsigurare", "Serviciu_Id", "dbo.Serviciu");
            DropForeignKey("dbo.ServiciuXAsigurare", "Asigurare_Id", "dbo.Asigurare");
            DropForeignKey("dbo.Doctor", "Specialitate_Id", "dbo.Specialitate");
            DropForeignKey("dbo.DoctorXProgramTemplate", "ProgramTemplate_Id", "dbo.ProgramTemplate");
            DropForeignKey("dbo.Doctor", "ProgramTemplate_Id", "dbo.ProgramTemplate");
            DropForeignKey("dbo.DoctorXProgramTemplate", "Doctor_Id", "dbo.Doctor");
            DropForeignKey("dbo.Doctor", "Clinica_Id", "dbo.Clinica");
            DropForeignKey("dbo.Clinica", "Adresa_Id", "dbo.Adresa");
            DropIndex("dbo.TrimitereServiciu", new[] { "Serviciu_Id" });
            DropIndex("dbo.TrimitereServiciu", new[] { "Trimitere_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Factura", new[] { "Programare_Id" });
            DropIndex("dbo.RetetaXMedicament", new[] { "Reteta_Id" });
            DropIndex("dbo.RetetaXMedicament", new[] { "Medicament_Id" });
            DropIndex("dbo.Reteta", new[] { "Id" });
            DropIndex("dbo.Programare", new[] { "TrimitereT_Id" });
            DropIndex("dbo.Programare", new[] { "Trimitere_Id" });
            DropIndex("dbo.Programare", new[] { "Pacient_Id" });
            DropIndex("dbo.Programare", new[] { "Doctor_Id" });
            DropIndex("dbo.PacientXDiagnostic", new[] { "Programare_Id" });
            DropIndex("dbo.PacientXDiagnostic", new[] { "Pacient_Id" });
            DropIndex("dbo.PacientXDiagnostic", new[] { "Diagnostic_Id" });
            DropIndex("dbo.Pacient", new[] { "Asigurare_Id" });
            DropIndex("dbo.Pacient", new[] { "Adresa_Id" });
            DropIndex("dbo.Trimitere", new[] { "Specialitate_Id" });
            DropIndex("dbo.Trimitere", new[] { "ProgramareT_Id" });
            DropIndex("dbo.Trimitere", new[] { "Programare_Id" });
            DropIndex("dbo.Trimitere", new[] { "Pacient_Id" });
            DropIndex("dbo.ServiciuXAsigurare", new[] { "Serviciu_Id" });
            DropIndex("dbo.ServiciuXAsigurare", new[] { "Asigurare_Id" });
            DropIndex("dbo.Serviciu", new[] { "Specialitate_Id" });
            DropIndex("dbo.DoctorXProgramTemplate", new[] { "ProgramTemplate_Id" });
            DropIndex("dbo.DoctorXProgramTemplate", new[] { "Doctor_Id" });
            DropIndex("dbo.Doctor", new[] { "Specialitate_Id" });
            DropIndex("dbo.Doctor", new[] { "ProgramTemplate_Id" });
            DropIndex("dbo.Doctor", new[] { "Clinica_Id" });
            DropIndex("dbo.Clinica", new[] { "Adresa_Id" });
            DropIndex("dbo.Adresa", new[] { "Localitate_Id" });
            DropTable("dbo.TrimitereServiciu");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ProgramareFromForm");
            DropTable("dbo.Factura");
            DropTable("dbo.Localitate");
            DropTable("dbo.Medicament");
            DropTable("dbo.RetetaXMedicament");
            DropTable("dbo.Reteta");
            DropTable("dbo.Programare");
            DropTable("dbo.Diagnostic");
            DropTable("dbo.PacientXDiagnostic");
            DropTable("dbo.Pacient");
            DropTable("dbo.Trimitere");
            DropTable("dbo.Asigurare");
            DropTable("dbo.ServiciuXAsigurare");
            DropTable("dbo.Serviciu");
            DropTable("dbo.Specialitate");
            DropTable("dbo.ProgramTemplate");
            DropTable("dbo.DoctorXProgramTemplate");
            DropTable("dbo.Doctor");
            DropTable("dbo.Clinica");
            DropTable("dbo.Adresa");
        }
    }
}
