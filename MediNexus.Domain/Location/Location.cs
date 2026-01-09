using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediNexus.Domain.Location
{
    public class Country
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public ICollection<State> States { get; set; } = new List<State>();
    }

    public class State
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? DaneCode { get; set; }
        public int CountryId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public Country Country { get; set; } = null!;
        public ICollection<City> Cities { get; set; } = new List<City>();
    }

    public class City
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string? FuripsCode { get; set; }
        public string Name { get; set; } = null!;
        public int StateId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation
        public State State { get; set; } = null!;
        public ICollection<Insurer.Insurer> Insurers { get; set; } = new List<Insurer.Insurer>();
    }
}
