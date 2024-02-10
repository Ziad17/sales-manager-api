using System.Collections.Generic;

namespace Vizage.Infrastructure.Exceptions.Models
{
    /// <summary>
    /// validation error
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationError"/> class.
        /// initial new validation error object
        /// </summary>
        public ValidationError()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationError"/> class.
        /// initial new validation error object
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="validations"></param>
        public ValidationError(string propertyName, List<ErrorProperty> validations)
        {
            PropertyName = propertyName;
            Validations = validations;
        }

        /// <summary>
        /// Gets or sets the name of property which has a validation exception
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets list of validation errors
        /// </summary>
        public List<ErrorProperty> Validations { get; set; } = new List<ErrorProperty>();
    }
}
