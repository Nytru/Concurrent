using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace ConcurentModels.Wrapper;

public record OperationResult
{
    public static OperationResult Ok => new OperationResult();

    public OperationResult(HttpStatusCode status = HttpStatusCode.OK)
    {
        Status = status;
    }

    public OperationResult(Exception e, HttpStatusCode status = HttpStatusCode.InternalServerError)
    {
        Exception = e;
        Status = status;
        PublicExceptionMessage = e.Message;
    }

    public OperationResult(Exception e, string message, HttpStatusCode status = HttpStatusCode.InternalServerError)
    {
        Exception = e;
        Status = status;
        PublicExceptionMessage = message;
    }

    public HttpStatusCode Status { get; }

    public Exception? Exception { get; }

    public string? PublicExceptionMessage { get; }

    [MemberNotNullWhen(returnValue: false, nameof(Exception))]
    [MemberNotNullWhen(returnValue: false, nameof(PublicExceptionMessage))]
    public virtual bool Success => Status is HttpStatusCode.OK;

    public static implicit operator OperationResult(HttpStatusCode code) => new(code);

    public static implicit operator OperationResult(Exception exception) => new(exception);
}

public record OperationResult<T> : OperationResult
{
    public OperationResult(T value, HttpStatusCode status = HttpStatusCode.OK) : base(status)
    {
        Value = value;
    }

    public OperationResult(Exception e, HttpStatusCode status = HttpStatusCode.InternalServerError) : base(e, status)
    {
    }

    public OperationResult(Exception e, string message, HttpStatusCode status = HttpStatusCode.InternalServerError) : base(e, message, status)
    {
    }

    public T? Value { get; }

    [MemberNotNullWhen(returnValue: true, nameof(Value))]
    public override bool Success => Status is HttpStatusCode.OK && Value is not null;

    public static implicit operator T(OperationResult<T> op)
    {
        if (op.Success)
            return op.Value;

        throw new InvalidOperationException("No value");
    }

    public static implicit operator OperationResult<T>(T op) => new(op);

    public static implicit operator OperationResult<T>(Exception exception) => new(exception);
}