
namespace TaskHive.API.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string CreatedByUserId { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public string ModifiedByUserId { get; set; } = string.Empty;
    public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.UtcNow;

    public string Title { get; set; } = string.Empty;    
    public string Description { get; set; } = string.Empty;
    

}