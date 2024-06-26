﻿using FluentValidation;
using MediatR.Pipeline;

namespace SalesManager.Application.Base
{
    public class ValidationProcessor<TRequest> : IRequestPreProcessor<TRequest>
        where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationProcessor(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults =
                    await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                var failures =
                    validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                    throw new ValidationException(failures);
            }
        }
    }
}
