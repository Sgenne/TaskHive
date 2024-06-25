using Microsoft.AspNetCore.Mvc;
using TaskHive.API.Services;

namespace TaskHive.API.Controllers;

[ApiController]
[Route("api/taskItems")]
public class TaskItemController : ControllerBase
{
    private readonly ITaskItemService _taskItemService;

    public TaskItemController(ITaskItemService taskItemService)
    {
        _taskItemService = taskItemService;
    }

    [HttpGet]
    public IActionResult GetAllTaskItems()
    {
        var taskItems = _taskItemService.GetAllTaskItems();

        return Ok(taskItems);
    }
}