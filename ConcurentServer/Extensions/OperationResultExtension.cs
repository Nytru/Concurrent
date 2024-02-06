using System.Text.Json;
using ConcurentModels.Wrapper;

namespace ConcurentServer.Extensions;

public static class OperationResultExtension
{
    private static readonly JsonSerializerOptions SnakeCaseOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public static IResult AsResult(this OperationResult result, bool internalFlag = false)
    {
        if (internalFlag)
        {
            return Results.Json(result, statusCode: (int)result.Status, options: SnakeCaseOptions);
        }

        if (result.Success)
            return Results.Ok();

        return Results.Json(result.PublicExceptionMessage, statusCode: (int)result.Status, options: SnakeCaseOptions);
    }

    public static IResult AsResult<T>(this OperationResult<T> result, bool internalFlag = false)
    {
        if (internalFlag)
        {
            return Results.Json(result, statusCode: (int)result.Status, options: SnakeCaseOptions);
        }

        if (result.Success)
            return Results.Json(result.Value, options: SnakeCaseOptions);

        return Results.Json(result.PublicExceptionMessage, statusCode: (int)result.Status, options: SnakeCaseOptions);
    }
}