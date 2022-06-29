using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Licenta2022.Models
{
    public class ProgramareIndexView
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public IdDictionaryItem Doctor { get; set; }
        public string NumeClinica { get; set; }
        public PacientUserView Pacient { get; set; }
        public bool Prezent { get; set; }
    }
    public class PacientUserView : IdDictionaryItem
    {
        public string UserId { get; set; }
    }
    public class ProgramareDetailsView
    {
        public int Id { get; set; }
        public bool Prezent { get; set; }
        public IdDictionaryItem Pacient { get; set; }   
        public DateTime Data { get; set; }
        public int TrimitereId { get; set; }
        public IdDictionaryItem Doctor { get; set; }
        public ClinicaProgramareView Clinica { get; set; }
        public DiagnosticView Diagnostic { get; set; }
        public IEnumerable<ServiciuProgramareView> Servicii { get; set; }
        public int TrimitereTId { get; set; }
        public int RetetaId { get; set; }
        public int FacturaId { get; set; }
    }
    public class DiagnosticView
    {
        public int Id { get; set; }
        public string Denumire { get; set; }
    }

    public class ServiciuProgramareView
    {
        public float Pret { get;set; }  
        public string Denumire { get; set; }
    }
    public class ClinicaProgramareView
    {
        public string Nume { get; set; }
        public string Adresa { get; set; }
    }
}