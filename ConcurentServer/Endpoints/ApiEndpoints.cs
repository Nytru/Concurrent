using ConcurentModels.Models;
using ConcurentServer.Extensions;
using ConcurentServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConcurentServer.Endpoints;

public static class ApiEndpoints
{
    public static void RegisterApiEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("add", AddCringeTask);
        app.MapDelete("remove/{id:guid}", RemoveTask);
        app.MapPost("switch/{state:bool}", Switch);
        app.MapGet("", GetCounter);
    }

    private static IResult GetCounter()
    {
        return Results.Ok(TasksService.Counter);
    }

    private static IResult AddCringeTask([FromBody]AddRequest? request, TasksService service)
    {
        var task = new CringeTask(request?.Id ?? Random.Shared.Next());
        var result = service.AddTask(task);
        return result.AsResult();
    }

    private static IResult RemoveTask([FromRoute] Guid id, TasksService service)
    {
        var result = service.RemoveTask(id);
        return result.AsResult();
    }

    private static IResult Switch([FromRoute] bool state, TasksService service)
    {
        service.Pause(state);
        return Results.Ok();
    }
}
