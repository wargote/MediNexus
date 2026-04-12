using System;
using MediNexus.Domain.Patients;

namespace MediNexus.Domain.Triages
{
    public class Triage
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime TriageDateTime { get; set; }
        public string Priority { get; set; } = null!;
        public string ConsultationReason { get; set; } = null!;

        // Vital Signs
        public string? BloodPressure { get; set; }
        public int? HeartRate { get; set; }
        public int? RespiratoryRate { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public decimal? Temperature { get; set; }
        public int? Glasgow { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public Patient Patient { get; set; } = null!;
    }
}
