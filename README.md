# Project README

## Overview

This project is a microservices architecture developed using .NET 7. It consists of two main modules: Product and Order. The Product module is responsible for CRUD operations related to products, while the Order module handles CRUD operations for orders. Additionally, there is an authentication service responsible for user authentication, and an Ocelot gateway for routing requests to the appropriate microservice.

## Technologies Used

- **.NET 7**: The core framework used for development.
- **Fluent Validation**: Utilized for input validation and business logic validation.
- **JWT**: Employed for authentication purposes.
- **Entity Framework Core**: Used as the ORM (Object-Relational Mapping) tool.
- **NUnit Test Framework**: Utilized for unit testing.
- **PostgreSQL (PgSQL)**: Chosen as the database solution.

## Project Structure

Each microservice (Product and Order) follows a clean architecture pattern with the following components:

1. **API**: Contains API endpoints and database migration scripts.
2. **Application**: Holds services responsible for business logic, validation, and mapping.
3. **Domain**: Houses entities (core business objects) of the respective microservice.
4. **Infrastructure**: Responsible for accessing database objects.
5. **Test**: Consists of NUnit tests for each service.

## Dependency Structure

- **Domain**: Acts as the core project for each microservice.
- **Application**: References the Domain project.
- **Infrastructure**: References the Application project.
- **API**: References the Infrastructure project.

## Running the Project

To run the project successfully, follow these steps:

1. Define connection strings for the databases used by each microservice.
2. Update the database migrations located in the API project for each microservice.
3. Ensure all dependencies are installed and configured correctly.
4. Build and run each microservice individually or orchestrate them using a container orchestration tool if desired.

## Notes

- This project follows clean architecture principles, ensuring separation of concerns and maintainability.
- Unit tests are provided for each service, allowing for easy validation of functionality and regression testing.
