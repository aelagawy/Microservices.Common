# Microservices.Common

Microservices.Common is an open-source .NET package built using C# that aims to simplify the development of microservices by addressing various cross-cutting concerns.
This package provides a collection of helpful utilities and components to handle common challenges encountered in the microservices world, including but not limited to authentication-helpers, validation, localization, mapping, code documentation, and exception handling.

## Features

- **Authentication**: Microservices.Common offers set of helpers under the `Microservices.Common.Authentication` namespace, that will help your service capture the `ICurrentUser` whenever needed with respective roles as well,
also authentication delegation handlers for the ClientSecret and Authorization grants.

- **Validation**: 
The package depend on [FluentValidation](https://fluentvalidation.net)

The package includes a robust validation framework that allows developers to define validation rules for incoming requests and easily validate input data. 
It helps ensure that the data received by microservices adheres to the specified rules and constraints.

- **Localization**: Microservices.Common provides localization support to enable multi-language applications. 
It offers convenient methods and utilities for managing resource files, retrieving localized strings, and handling language-specific content.

- **Mapping**: The package includes a powerful object mapping library that simplifies the mapping of data between different models and formats. 
It allows developers to define mappings using a fluent API and handles the conversion of complex objects automatically.

- **Code Documentation**: Microservices.Common promotes good documentation practices by offering tools to generate code documentation automatically. 
It helps developers produce comprehensive and up-to-date documentation for their microservices APIs, making it easier for other developers to understand and use the services.

- **Exception Handling**: The package provides a centralized exception handling mechanism that allows developers to handle exceptions consistently across microservices. 
It simplifies error logging, exception wrapping, and response generation, ensuring a uniform approach to handling errors throughout the system.

## Getting Started

To use Microservices.Common in your project, follow these steps:

1. Install the package via [NuGet](https://www.nuget.org/packages/Microservices.Common).

2. in your program.cs 
```
//ConfigureServices
...
var ssoOAuth2Options = builder.Configuration.GetSection(SsoOAuth2Options.SSO_OAUTH2).Get<SsoOAuth2Options>();
builder.Services.AddCommon(Assembly.GetExecutingAssembly(), ssoOAuth2Options);
...


//Configure
...
app.UseCommon(Assembly.GetExecutingAssembly());
...

```

2. Configure the package according to your microservice's requirements. This may involve setting up authentication providers, defining validation rules, configuring localization resources, and mapping definitions.

3. Integrate the package into your microservices codebase by utilizing its various modules and utilities. For example, you can use the authentication module to secure your endpoints, apply validation rules to incoming requests, localize user-facing messages, map data between models, and handle exceptions consistently.

4. Explore the documentation and examples provided to gain a deeper understanding of the package's features and how to leverage them effectively. The package documentation will guide you through the available APIs, best practices, and common usage patterns.

## Contributing

Microservices.Common is an open-source project, and contributions are welcome. 
If you would like to contribute to the project, please follow the guidelines outlined in the CONTRIBUTING.md file. 
Contributions may include bug fixes, feature enhancements, documentation improvements, or other relevant updates.

## License

Microservices.Common is released under the [MIT License](https://opensource.org/licenses/MIT). You are free to use, modify, and distribute the package in accordance with the terms of the license.

## Support

If you encounter any issues or have questions regarding the usage or implementation of Microservices.Common, please consult the project's issue tracker on GitHub. You may also find answers or seek assistance from the community on relevant forums or discussion platforms.


## About the Author

Microservices.Common is developed and maintained by Ahmed El-Agawy. 
I'm a senior software developer and I'm passionate about creating tools and frameworks that simplify microservices development and promote best practices in the industry. 
Feel free to contact me with any inquiries or feedback.


Thank you for choosing Microservices.Common! We hope it proves to be a valuable asset in your microservices projects. Happy coding!