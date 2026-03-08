using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MediNexus.Domain.Users
{
    public class User
    {
        public int Id { get; set; }

        // NUEVOS CAMPOS
        public int DocumentTypeId { get; set; }
        public string DocumentNumber { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Username { get; set; } = null!;
        public int UserProfileId { get; set; }
        public int UserRoleId { get; set; }
        public int UserStatusId { get; set; }
        public string Name => $"{FirstName} {LastName}";
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Telephone { get; set; } = null!;
        public string? Signature { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // NAVIGATION PROPERTIES
        public DocumentType DocumentType { get; set; } = null!;
        public UserProfile UserProfile { get; set; } = null!;
        public UserRole UserRole { get; set; } = null!;
        public UserStatus UserStatus { get; set; } = null!;
    }
}
