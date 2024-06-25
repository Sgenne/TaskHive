using TaskHive.API.Models;
using TaskHive.API.Repositories;

namespace TaskHive.API.Services;

public interface ITaskItemService
{
    List<TaskItem> GetAllTaskItems();
}

public class TaskItemService : ITaskItemService
{

    private readonly ITaskItemRepository _taskItemRepository;

    public TaskItemService(ITaskItemRepository taskItemRepository)
    {
        _taskItemRepository = taskItemRepository;
    }

    public List<TaskItem> GetAllTaskItems()
    {
        var taskItems = _taskItemRepository.GetAllTaskItems();

        return taskItems;
    }
}

