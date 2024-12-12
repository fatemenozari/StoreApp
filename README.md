# OnlineStore

## Project Overview

The **OnlineStore** project is a web application developed using the **ASP.NET Minimal API** framework. It adheres to the **Clean Architecture** principles to ensure a modular, maintainable, and testable codebase. The application has been **dockerized** to facilitate easy deployment and scalability. The Docker image is tagged as `onlinestore:dev`.

Two design patterns have been implemented to enhance the application's flexibility and extendability:

- **Decorator Pattern**: Dynamically appends product descriptions with additional details, such as indicating discounted products.
- **Strategy Pattern**: Enables flexible discounting logic for different product categories or types, allowing dynamic selection of discount algorithms.

The project includes **unit tests written with xUnit** and **integration tests**, ensuring comprehensive testing of critical components.

Additionally, a **Docker Compose** file is provided for easy multi-container setup, including database configuration.

## Running the Application

To run the OnlineStore application locally, follow these steps:

1. **Clone the Repository**:

   ```bash
   git clone https://github.com/fatemenozari/StoreApp
   cd StoreApp
   ```

2. **Use Docker Compose**: Ensure Docker and Docker Compose are installed and running on your machine. Then start the application using:

   ```bash
   docker-compose up
   ```

3. **Access the Application**: Open your browser and navigate to `http://localhost:8080` to interact with the OnlineStore API.

## Features

1. **Discounting System**:

   - Flexible discount logic using Strategy Pattern.
   - Dynamic description updates via Decorator Pattern.

2. **Database Seeding**:

   - Initial data is seeded into the database automatically during startup.

3. **Swagger Documentation**:

   - Accessible at `/swagger` for testing and exploring the API endpoints.

## Areas for Improvement

1. **Performance Optimization**:

   - Introduce caching mechanisms for frequently accessed data.
   - Profile API performance and optimize database queries to reduce latency.

2. **Security Enhancements**:

   - Implement JWT-based authentication and authorization.
   - Conduct periodic security audits.

3. **Enhanced CI/CD**:

   - Integrate automated testing, linting, and deployment workflows.

4. **Extensibility**:

   - Expand the discounting system to support more complex rules or conditions.

5. **User Experience**:

   - Develop a client-side interface to complement the API.
   - Provide more detailed API error messages and codes for better usability.

---


