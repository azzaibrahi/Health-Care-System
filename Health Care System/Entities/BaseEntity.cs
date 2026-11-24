namespace Health_Care_System.Entities;

/// <summary>
/// Shared fields for every entity that supports auditing and soft delete.
/// </summary>
public class BaseEntity
{
    public int Id { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public Guid? ModifiedBy { get; set; }
}
