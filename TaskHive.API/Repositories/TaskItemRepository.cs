using TaskHive.API.Models;

namespace TaskHive.API.Repositories;

public interface ITaskItemRepository
{
    List<TaskItem> GetAllTaskItems();
}

public class TaskItemRepository : ITaskItemRepository
{
    public List<TaskItem> GetAllTaskItems()
    {
        List<TaskItem> taskItems = [
            new TaskItem
            {
                Id = 1,
                Title = "Task A",
                Description = "Task A Description"
            },
            new TaskItem
            {
                Id = 2,
                Title = "Task B",
                Description = "Task B Description"
            },
            new TaskItem
            {
                Id = 3,
                Title = "Task C",
                Description = "Task C Description"
            }
        ];

        return taskItems;
    }

}
