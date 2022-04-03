﻿using FluentValidation;
using Friday.Domain.Common;
using MediatR;

namespace Friday.Application.Common.Behaviors;

/// <summary>
/// Behavior in the MediatR ecosystem. Performs FluentValidation logic before
/// proceeding to command/query execution.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse> 
    where TResponse : class
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var context = new ValidationContext<TRequest>(request);
        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            var responseType = typeof(TResponse);
 
            if (responseType.IsGenericType)
            {
                var resultType = responseType.GetGenericArguments()[0];
                var invalidResponseType = typeof(ApiResponse<>).MakeGenericType(resultType);
 
                var invalidResponse =
                    Activator.CreateInstance(invalidResponseType, null, failures.Select(s => s.ErrorMessage).ToList()) as TResponse;
 
                return invalidResponse;
            }
        }

        return await next();
    }
}