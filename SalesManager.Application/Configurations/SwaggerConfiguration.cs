using System.Reflection;

namespace SalesManager.Application.Configurations
{
    public class SwaggerConfiguration
    {
        /// <summary>
        /// Is Swagger Ui Enabled
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// The Name Of Swagger Document
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// The Name Of The Build Version
        /// </summary>
        public string Build { get; set; } = Assembly.GetExecutingAssembly().GetName().Version?.ToString();

        /// <summary>
        /// Swagger Ui Title
        /// </summary>
        public string Title { get; set; } = default!;

        /// <summary>
        /// Swagger Ui Version
        /// </summary>
        public string Version { get; set; } = default!;

        /// <summary>
        /// Swagger Ui Description
        /// </summary>
        public string Description { get; set; } = default!;

        /// <summary>
        /// Swagger Ui Terms Of Service Url
        /// </summary>
        public string TermsOfService { get; set; } = default!;

        /// <summary>
        /// Swagger Ui License
        /// </summary>
        public SwaggerLicense License { get; set; } = new SwaggerLicense();

        /// <summary>
        /// Swagger Ui Contact
        /// </summary>
        public SwaggerContact Contact { get; set; } = new SwaggerContact();
    }

    public class SwaggerLicense
    {
        /// <summary>
        /// The Name Of License
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// The Url Of License
        /// </summary>
        public string Url { get; set; } = default!;
    }

    public class SwaggerContact
    {
        /// <summary>
        /// The Name Of Contact
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// The Email Of Contact
        /// </summary>
        public string Email { get; set; } = default!;

        /// <summary>
        /// The Url Of Contact
        /// </summary>
        public string Url { get; set; } = default!;
    }
}
