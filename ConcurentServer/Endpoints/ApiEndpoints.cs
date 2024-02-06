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
        app.MapDelete("remove/{id:int}", RemoveTask);
    }

    private static IResult AddCringeTask(CounterService service)
    {
        var random = Random.Shared.Next();
        var result = service.AddTask(new CringeTask(random));
        return result.AsResult();
    }

    private static IResult RemoveTask([FromRoute] int id, CounterService service)
    {
        var result = service.RemoveTask(id);
        return result.AsResult();
    }
}
