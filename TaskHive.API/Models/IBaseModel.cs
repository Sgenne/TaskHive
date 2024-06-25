namespace TaskHive.API.Models;

public interface IBaseModel
{
    public int Id { get; set; }

    public string CreatedByUserId { get; set; }
    public string ModifiedByUserId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset ModifiedAt { get; set; }
}