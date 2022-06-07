using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Licenta2022.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {

            Database.SetInitializer(new
            MigrateDatabaseToLatestVersion<ApplicationDbContext,
            Licenta2022.Migrations.Configuration>("DefaultConnection"));
        }

        public DbSet<Specializare> Specialitati { get; set; }
        public DbSet<Doctor> Doctori { get; set; }
        public DbSet<Clinica> Clinici { get; set; }
        public DbSet<Adresa> Adrese { get; set; }
        public DbSet<Localitate> Localitati { get; set; }
        public DbSet<Asigurare> Asigurari { get; set; }
        public DbSet<Pacient> Pacienti { get; set; }
        public DbSet<Serviciu> Servicii { get; set; }
        public DbSet<Reteta> Retete { get; set; }
        public DbSet<Medicament> Medicamente { get; set; }
        public DbSet<RetetaXMedicament> RetetaXMedicaments { get; set; }
        public DbSet<Diagnostic> Diagnostics { get; set; }
        public DbSet<PacientXDiagnosticXProgramare> PacientXDiagnosticXProgramares { get; set; }
        public DbSet<ServiciuXAsigurare> ServiciuXAsigurari { get; set; }
        public DbSet<Programare> Programari { get; set; }
        public DbSet<Trimitere> Trimiteri { get; set; }
        public DbSet<Factura> Facturi { get; set; }
        public DbSet<ProgramTemplate> ProgramTemplates { get; set; }
        public DbSet<DoctorXProgramTemplate> DoctorXProgramTemplates { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        public System.Data.Entity.DbSet<Licenta2022.Models.ProgramareFromForm> ProgramareFromForms { get; set; }
    }
}