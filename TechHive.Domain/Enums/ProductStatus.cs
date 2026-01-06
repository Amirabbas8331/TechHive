namespace TechHive.Domain.Enums;

public sealed class ProductStatus : Enumeration<ProductStatus>
{
    public static readonly ProductStatus Draft = new(1, "DRAFT", nameof(Draft));
    public static readonly ProductStatus Active = new(2, "ACTIVE", nameof(Active));
    public static readonly ProductStatus Inactive = new(3, "INACTIVE", nameof(Inactive));
    public static readonly ProductStatus Archived = new(4, "ARCHIVED", nameof(Archived));
    public static readonly ProductStatus Deleted = new(5, "DELETED", nameof(Deleted));
    private ProductStatus(int id, string code, string name) : base(id, code, name) { }


    public bool CanBeActivated => this is { Code: "DRAFT" or "INACTIVE" };
    public bool CanBeInactivated => this is { Code: "DRAFT" or "ACTIVE" };
    public bool CanBeArchived => this != Deleted;
    public bool CanBeDeleted => this != Deleted && this != Archived;
}