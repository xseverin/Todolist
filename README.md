# Todolist

Todolist is a full-stack application built using ASP.NET Core for the backend and React for the frontend. It enables users to manage their tasks (Todos) and collaborate by sharing tasks with other users.


Welcome to Todolist, a comprehensive full-stack application designed to help users efficiently manage their tasks and collaborate with others. Built with a robust backend powered by ASP.NET Core and a dynamic frontend utilizing React, TodoApp provides a seamless experience for task management, user interaction, and collaboration.

This documentation provides an in-depth look at the structure, features, and practical knowledge gained throughout the development of Todolist. Whether you're a developer looking to contribute or a user interested in exploring its capabilities, this guide will walk you through everything from the core functionalities to the technical details behind the scenes.

Dive into the sections below to learn more about how Todolist is structured, how to set it up, and the practical applications of the technologies used.


## Table of Contents

- [Features](#features)
- [Coming features](#coming-features)
- [Project Structure](#project-structure)
    - [Backend (API)](#backend-api)
    - [Domain](#domain)
    - [Repository](#repository)
    - [UseCases](#usecases)
    - [Testing](#testing)
    - [Frontend (Web)](#frontend-web)
- [Configuration and Settings](#configuration-and-settings)
- [Installation](#installation)
- [License](#license)
- [Practical Knowledge Gained](#practical-knowledge-gained)

## Features

- **User Management**: Registration, login, profile updates, and password management.
- **Task Management**: Create, update, delete, and organize tasks.
- **Task Sharing**: Share tasks with other users for collaboration.
- **Authentication & Authorization**: Secure endpoints with JWT tokens.
- **API Documentation**: Integrated Swagger for easy API testing and documentation.

## Coming features
- **Responsive UI**: Clean and responsive web interface built with React and Tailwind CSS.
- **Shared Content Visibility**: Allows users to see with whom they've shared specific tasks, providing greater control and transparency over shared items.

## Project Structure

### Backend (API)

The `src/Api` directory contains the backend of the application, developed using ASP.NET Core. Key components include:

1. **Controllers**:
    - `TodoController.cs`: Handles task-related requests such as adding, updating, deleting, and retrieving tasks.
    - `UserController.cs`: Manages user-related requests like registration, login, profile updates, and password management.

2. **Configuration Files**:
    - `appsettings.json`: Main configuration file containing database connection strings, token settings, SMTP configurations, and logging levels.
    - `appsettings.Development.json`: Development-specific configuration, overriding some settings from `appsettings.json`.
    - `appsettings.Test.json`: Test environment configuration, also overriding settings from `appsettings.json`.
    - `launchSettings.json`: Configuration for application launch settings, including IIS Express profiles for HTTP and HTTPS.

3. **Program.cs**:
    - Main file for setting up and running the application, including service configurations like Entity Framework, Identity, JWT authentication, CORS, Swagger, and more.

4. **Dependency Injection**:
    - Dependencies such as `TodoUseCases`, `UserService`, repositories, and email services are configured in `Program.cs`.

5. **Authentication and Authorization**:
    - JWT token-based authentication and authorization configured to secure API endpoints.

6. **Swagger**:
    - Enabled for API documentation and testing in development and testing environments.

### Domain

The `src/Domain` directory includes classes that represent the core entities and data models used in the application. Key components include:

- **AddTodoModel**: Model for adding a new task (Todo).
- **ResetPasswordRequest**: Model for password reset requests.
- **Result<T>**: Generic class representing the result of an operation, with properties for success status, messages, and data.
- **SharedTodo**: Class representing a task shared with another user.
- **SharedTodoDTO**: Data Transfer Object for shared task information.
- **ShareTodoModel**: Model for sharing a task with another user.
- **SmtpSettings**: Class for SMTP settings.
- **Todo**: Class representing a task with properties like `Id`, `Name`, `Done`, `ApplicationUserId`, etc.
- **UserDetail**: Class representing user details.
- **UserProfile**: Class representing a user profile.
- **UserRegisterRequest**: Model for user registration requests.

### Repository

The `src/Repository` directory contains classes and interfaces that provide data access and interaction with the database. Key components include:

- **ApplicationUserRepository**: Interface and implementation for finding users by email.
- **UserDetailRepository**: Interface and implementation for managing user details.
- **UserRepository**: Interface and implementation for managing users.
- **SharedTodoRepository**: Interface and implementation for managing shared tasks.
- **TodoRepository**: Interface and implementation for managing tasks.
- **TokenSettings**: Class containing token-related settings like issuer, audience, and expiration times.

### UseCases

The `src/UseCases` directory includes classes and methods that implement the business logic of the application. Key components include:

- **UserService**: Manages user-related operations such as registration, login, password reset, and profile updates.
- **TokenUtil**: Utility class for handling token generation and extraction of token data.
- **EmailService**: Handles sending of emails, such as password reset and confirmation emails.
- **TodoUseCases**: Manages task-related operations such as adding, updating, deleting, and sharing tasks.

### Testing

The `src/Tests` directory contains unit tests for various components of the application. Key test files include:

- `UserServiceRefreshTokenTest.cs`: Tests for refreshing user tokens.
- `UserServiceRegisterTest.cs`: Tests for user registration.
- `UserServiceResetPasswordTest.cs`: Tests for password reset functionality.
- `TodoAddTest.cs`: Tests for adding new tasks.
- `TodoMakeUnDoneTest.cs`: Tests for marking tasks as undone.
- `TodoShareTest.cs`: Tests for sharing tasks with other users.

### Frontend (Web)

The `src/web` directory contains the frontend code for the application, built with React and Tailwind CSS. Key components include:

- **Features**:
    - `todo`: Handles task-related functionality, including task API interactions and UI components like `TodoForm`, `TodoTable`, and `TodoRow`.
    - `user`: Manages user-related functionality, including authentication (`authAPI.ts`, `authSlice.ts`) and UI components for forms and navigation.
- **Layout**: Components for different page layouts, including `DefaultLayout` and `UserLayout`.
- **Pages**: Components for application pages like `HomePage`, `LoginPage`, `RegisterPage`, `UserProfile`, and more.
- **App Configuration**:
    - `store.ts`: Redux store configuration.
    - `AxiosApiInterceptor.ts`: Axios interceptor for API requests.
- **Styles**: Tailwind CSS configuration in `tailwind.config.js` and global styles in `index.css`.
- **Main Entry Point**: `main.tsx` is the entry point for the React application.

## Configuration and Settings

- **PostCSS**: Configured via `postcss.config.cjs` to include Tailwind CSS and Autoprefixer.
- **Tailwind CSS**: Configured via `tailwind.config.js`, extending the theme with custom properties.
- **Vite**: Configured via `vite.config.ts` for building and serving the frontend.
- **TypeScript**: TypeScript configuration files are located in `tsconfig.json` and `tsconfig.node.json`.

## Installation

1. **Clone the repository**:
   ```sh
   git clone https://github.com/yourusername/TodoApp.git
   cd TodoApp
    ```
2. **Backend**:

- **Set up .NET Secrets for Development**:
    - Initialize the User Secrets:
      ```sh
      dotnet user-secrets init
      ```
    - Set the SMTP password and Token SecretKey using the Secret Manager tool:
      ```sh
      dotnet user-secrets set "SmtpSettings:Password" "<Your-SMTP-Password>"
      dotnet user-secrets set "TokenSettings:SecretKey" "<Your-Secret-Key>"
      ```
      Replace `<Your-SMTP-Password>` and `<Your-Secret-Key>` with your actual values.
  

- Update the database with migrations:
    ```
    dotnet ef database update
    ```
- Run the API:
    ```
    dotnet run
    ```
3. **Frontend**

- Navigate to the src/web directory and install dependencies:
     ```
     npm install
     ```
 - Start the frontend development server:
    ```
    npm start
    ```
- Navigate to the `src/Api` directory and restore dependencies:

  ```
  dotnet restore
    ```
  
# License

This project is licensed under the GNU/GPL License - see the [LICENSE](LICENSE) file for details.

## Practical Knowledge Gained

We've used the following technologies:

- **C#**: Writing server-side logic and business logic.
- **TypeScript**: Developing typed code for the frontend.
- **JavaScript**: Creating interactive elements on web pages.
- **React**: Building user interfaces.
- **ASP.NET Core**: Handling API requests and building web applications.
- **Entity Framework Core**: Managing the database and executing queries.
- **JWT (JSON Web Tokens)**: Authentication and authorization of users.
- **Moq**: Mocking dependencies in unit tests.
- **MSTest**: Writing and executing unit tests.
- **Redux**: Managing application state on the frontend.
- **Axios**: Performing HTTP requests from the frontend.
- **Tailwind CSS**: Styling user interface components.
- **Vite**: Building and developing frontend applications.
- **PostCSS**: Processing CSS files.
- **Autoprefixer**: Automatically adding vendor prefixes to CSS.
- **Swagger**: Documenting and testing APIs.
- **Microsoft Identity**: Managing users and their authentication.
- **Microsoft SQL Server**: Storing data in the database.

## src/Api
  In `src/Api`, we have used the following practical applications:

1. **Handling API Requests**: Processing requests for adding, updating, deleting, and retrieving tasks in `TodoController`.
2. **Database Operations**: Executing database queries through `Entity Framework Core` in `TodoRepository`.
3. **Authentication and Authorization**: Using JWT tokens to secure endpoints in `UserController`.
4. **API Documentation**: Automatically generating API documentation with Swagger.
5. **User Management**: Registering, logging in, and managing user profiles in `UserService`.
6. **Data Storage**: Storing and retrieving task and user data in `Microsoft SQL Server`.
7. **Dependency Injection Configuration**: Injecting dependencies via `Program.cs` for `TodoUseCases` and `UserService`.
8. **CORS Configuration**: Setting up CORS policies to allow frontend requests in `Program.cs`.
9. **Email Sending**: Sending password reset emails through `EmailService`.
10. **Mocking Dependencies in Tests**: Using Moq to create mocks of repositories in tests like `TodoAddTests`.
11. **Writing Unit Tests**: Testing the functionality of adding tasks in `TodoAddTests`.

## src/Domain

 Practical Knowledge Gained in `src/Domain`

1. **Creating Models for Database**: Learned to create models like `AddTodoModel`, `ApplicationUser`, `Todo`, `UserDetail`, etc., with appropriate properties and validation attributes.
2. **Entity Framework Core**: Gained experience in using Entity Framework Core for defining `DbContext` (`ApplicationDbContext`) and configuring entity relationships, constraints, and properties.
3. **Data Annotations**: Applied data annotations for validation and configuration of model properties, such as `[Required]`, `[StringLength]`, and `[EmailAddress]`.
4. **Navigation Properties**: Implemented navigation properties to define relationships between entities, such as `ApplicationUser` and `UserDetail`, `Todo` and `ApplicationUser`, `SharedTodo` and `ApplicationUser`.
5. **DTOs (Data Transfer Objects)**: Created DTOs like `SharedTodoDTO` to transfer data between layers.
6. **Handling Requests**: Developed models for handling various requests, such as `ChangePasswordRequest`, `ForgotPasswordRequest`, `ResetPasswordRequest`, and `UserRegisterRequest`.
7. **Result Wrapping**: Implemented a generic `Result<T>` class to standardize the response format for operations, including success status, messages, and data.
8. **Configuration Settings**: Defined configuration classes like `SmtpSettings` to manage application settings.
9. **Complex Relationships**: Managed complex relationships and constraints in the database, such as parent-child relationships in `Todo` and shared tasks in `SharedTodo`.
10. **Fluent API**: Used Fluent API in `OnModelCreating` method to configure entity properties, relationships, and constraints programmatically.

## src/Repository

 Practical Knowledge Gained in `src/Repository`

1. **Repository Pattern**: Implemented the repository pattern to abstract data access logic and promote separation of concerns.
2. **Entity Framework Core**: Used Entity Framework Core for database operations, including querying, adding, updating, and deleting entities.
3. **Asynchronous Programming**: Developed asynchronous methods for data access operations to improve application performance and responsiveness.
4. **LINQ Queries**: Utilized LINQ queries to filter, sort, and project data from the database.
5. **Navigation Properties**: Managed navigation properties to handle relationships between entities, such as `ApplicationUser` and `UserDetail`, `Todo` and `ApplicationUser`, `SharedTodo` and `ApplicationUser`.
6. **DTO Mapping**: Created methods to map entities to Data Transfer Objects (DTOs) for data transfer between layers.
7. **Handling Complex Relationships**: Managed complex relationships and constraints in the database, such as parent-child relationships in `Todo` and shared tasks in `SharedTodo`.
8. **Dependency Injection**: Configured repositories for dependency injection to promote loose coupling and testability.
9. **Unit Testing**: Wrote unit tests for repository methods using mocking frameworks like Moq to isolate and test data access logic.
10. **Error Handling**: Implemented error handling in repository methods to manage exceptions and provide meaningful error messages

## src/UseCases

 Practical Knowledge Gained in `src/UseCases`

1. **User Management**: Implemented user-related operations such as registration, login, password reset, and profile updates in `UserService`.
2. **Token Management**: Utilized `TokenUtil` for handling token generation, validation, and extraction of token data.
3. **Email Sending**: Developed `EmailService` to handle sending emails, such as password reset and confirmation emails.
4. **Task Management**: Managed task-related operations such as adding, updating, deleting, and sharing tasks in `TodoUseCases`.
5. **Dependency Injection**: Configured services for dependency injection to promote loose coupling and testability.
6. **Asynchronous Programming**: Developed asynchronous methods for business logic operations to improve application performance and responsiveness.
7. **Error Handling**: Implemented error handling in use case methods to manage exceptions and provide meaningful error messages.
8. **Unit Testing**: Wrote unit tests for use case methods using mocking frameworks like Moq to isolate and test business logic.
9. **Controller Integration**: Integrated use cases with ASP.NET Core controllers to handle API requests and responses.
10. **Security**: Implemented security measures such as JWT authentication and authorization in user-related use cases.

## Test

1. **Mocking Dependencies**: Using Moq to mock dependencies like `UserManager`, `SignInManager`, and repositories helps isolate the unit tests and focus on the logic being tested.
2. **Setup and Teardown**: Proper setup and teardown methods ensure a clean state for each test, preventing side effects from previous tests.
3. **Testing Asynchronous Methods**: Writing tests for asynchronous methods requires using `async` and `await` keywords to ensure the tests run correctly.
4. **Handling Different Scenarios**: Writing tests for various scenarios (success, failure, edge cases) ensures comprehensive coverage and robustness of the code.
5. **Using In-Memory Database**: Using an in-memory database for tests provides a lightweight and fast way to test database interactions without affecting the actual database.
6. **Assertions**: Using assertions to verify the expected outcomes of the tests helps ensure the code behaves as expected.
7. **Mocking Configuration**: Mocking configuration settings using `IConfiguration` and `IOptions` allows testing how the code interacts with configuration values.
8. **Testing Security**: Ensuring that security-related methods (like password changes and token generation) are tested to verify they handle different scenarios correctly.
9. **Error Handling**: Writing tests to ensure proper error messages and handling when operations fail, providing better user feedback and debugging information.
10. **Dependency Injection**: Understanding the importance of dependency injection for creating testable and maintainable code.

## src/web: React Development and Related Technologies

### 1. React Development
- Creating and using functional and class components.
- Managing state with `useState`, `useEffect`, and other hooks.
- Working with forms and data validation.
- Using routing libraries such as `react-router`.

### 2. Redux
- Setting up and using `Redux` for application state management.
- Creating `slices` with `@reduxjs/toolkit`.
- Handling asynchronous actions with `createAsyncThunk`.

### 3. API Integration
- Using `axios` for making HTTP requests.
- Handling API responses and managing errors.

### 4. CSS and Tailwind
- Using `Tailwind CSS` for styling components.
- Creating responsive interfaces with `Tailwind` utility classes.

### 5. TypeScript
- Using TypeScript for type-checking components and functions.
- Creating and using interfaces and types.

### 6. Ant Design
- Using components from the `Ant Design` library.
- Customizing and styling `Ant Design` components.

### 7. Vite
- Setting up and using `Vite` for application bundling and development.

### 8. Form Handling
- Using the `react-hook-form` library for form management.
- Validating form data and handling errors.

### 9. Authentication and Authorization
- Implementing user authentication.
- Managing access tokens and refresh tokens.

### 10. State Management
- Managing component state using hooks.
- Using context for passing data between components.

### 11. Error Handling
- Handling errors during HTTP requests.
- Displaying error messages to users.

### 12. Performance Optimization
- Using memoization and hooks to optimize performance.
- Code splitting and dynamic component loading.
