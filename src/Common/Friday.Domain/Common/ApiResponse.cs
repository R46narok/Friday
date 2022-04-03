namespace Friday.Domain.Common;

/// <summary>
/// An empty response model used in the business layer.
/// </summary>
public class ApiResponse
{
    public List<string>? Errors { get; init; }

    public ApiResponse(IEnumerable<string>? errors = null)
    {
        Errors = errors?.ToList();
    }

    public bool IsValid => Errors == null || !Errors.Any();
}

/// <see cref="ApiResponse"/>
/// <typeparam name="TModel">Type of result.</typeparam>
public class ApiResponse<TModel> : ApiResponse
{
    public TModel Result { get; init; }
    
    public ApiResponse(TModel model, IEnumerable<string>? errors = null) 
        : base(errors)
    {
        Result = model;
    }
}