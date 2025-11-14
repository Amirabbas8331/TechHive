namespace TechHive.Domain.Enums;

public sealed class ProductStatus : Enumeration<ProductStatus>
{
    public static readonly ProductStatus Draft = new(1, "DRAFT", nameof(Draft));
    public static readonly ProductStatus Active = new(2, "ACTIVE", nameof(Active));
    public static readonly ProductStatus Inactive = new(3, "INACTIVE", nameof(Inactive));
    public static readonly ProductStatus Archived = new(4, "ARCHIVED", nameof(Archived));
    public static readonly ProductStatus Deleted = new(4, "DELETED", nameof(Deleted));

    public ProductStatus(int id, string code, string name)
        : base(id, code, name) { }

    //? Behaviors
    public bool CanBeActivated => this == Draft || this == Inactive;
    public bool CanBeInactivated => this == Draft || this == Active;
    public bool CanBeArchived => this != Deleted;
    public bool CanBeDeleted => this != Deleted && this != Archived;

    //? Policy Selection Logic
}
