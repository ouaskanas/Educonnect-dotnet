using Educonnect.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Educonnect.Domain.Entities;
public class User : IdentityUser<Guid>
{
    public string Name { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.None;
    public DateTime CreateAt { get; set; }
    public Guid ProfilId { get; set; }
    public virtual Profile Profile { get; set; }
    public DateTime? DeletedAt { get; set; }
    public bool IsDeleted { get; set; }
    public Guid? DeletedBy { get; set; }

    public void Delete(Guid userId)
    {
        DeletedAt = DateTime.Now;
        IsDeleted = true;
        DeletedBy = userId;
    }
}
