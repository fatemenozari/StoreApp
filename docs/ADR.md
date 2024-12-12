# Architecture Decision Record (ADR)

### Context
The **OnlineStore** project aims to provide a maintainable, scalable, and testable platform for managing products and their categories. The project is implemented using **ASP.NET Minimal API**, leveraging its lightweight and performance-efficient capabilities for modern web development.

Key requirements influencing the architectural and design decisions:
- Support dynamic product discounting and categorization features.
- Ensure modularity for ease of future extension and testing.
- Avoid over-engineering features while staying aligned with the project's objectives.

Two primary models are at the core of the system:
- **Product**: Represents individual items sold in the store.
- **Category**: Groups products into logical categories for easier management.

### Decision
#### 1. Use of Clean Architecture
The project adopts **Clean Architecture** principles to ensure:
- Clear separation of concerns between the business logic, application logic, and infrastructure.
- Flexibility to switch or modify frameworks, databases, or external dependencies without impacting core business logic.
- Enhanced testability by isolating the application’s core logic from implementation details.

#### 2. Use of Design Patterns
- **Strategy Pattern**:
  - Implemented for the discounting system to allow different discount strategies for products and categories.
  - Simplifies the application of varying discount rules without modifying the core logic.
  - Example: A specific discount strategy for mobile products can be dynamically applied at runtime.

- **Decorator Pattern**:
  - Used to enhance product descriptions dynamically. When a discount is applied, additional details (e.g., "discounted price") are appended to the product’s description.
  - Keeps the core product model clean while allowing flexible and reusable extensions.

### Alternatives Considered
#### 1. **Modeling Discounts as a Separate Entity**
- Pros:
  - Would allow managing discounts as independent entities.
  - Could provide more detailed tracking of discount rules.
- Cons:
  - Adds unnecessary complexity for this project’s scope since discounts are applied primarily on categories or individual products.
  - Clean Architecture principles already facilitate modular discounting via strategies.

#### 2. **Adding Product Update and Deletion Features**
- Pros:
  - Increases the application’s completeness and aligns with common e-commerce use cases.
- Cons:
  - Adds functionality not required for the project’s scope or current use case.
  - Adopting a "YAGNI" (You Aren’t Gonna Need It) approach ensures simplicity and focus.

### Consequences
#### Benefits:
1. **Maintainability**:
   - Modular and testable code ensures ease of future enhancements.
   - Clean Architecture prevents coupling between the business logic and infrastructure.

2. **Scalability**:
   - The adoption of design patterns facilitates easy addition of new features like alternative discount strategies.
   - Clear separation of layers supports potential scaling efforts.

3. **Clarity and Flexibility**:
   - Developers can quickly grasp the purpose and implementation of features.
   - Clean Architecture’s abstractions make integration with other services or frameworks straightforward.

#### Trade-offs:
1. **Increased Initial Complexity**:
   - New developers unfamiliar with Clean Architecture may face a learning curve.

2. **Delayed Implementation of Features**:
   - Not including product update and deletion capabilities limits functionality, but this aligns with the project’s defined scope.

### Technical Debt
While the project adheres to modern best practices and includes essential features such as unit tests, integration tests, logging, and documentation, there are areas for potential improvement:

1. **Testing Coverage**:
   - While unit tests and integration tests exist, further expanding test coverage to include additional edge cases can enhance reliability.

2. **Logging**:
   - Logging is implemented but could be improved by adopting structured logging formats (e.g., JSON) for better observability and debugging.

3. **Documentation**:
   - Swagger is implemented and functional in development mode. Expanding it for production environments or incorporating automated API documentation tools can improve developer onboarding and user experience.

### Future Considerations
1. **Feature Expansion**:
   - Introduce product updates and deletions when the project’s scope demands.

2. **Security Enhancements**:
   - Add robust authentication and authorization mechanisms as the application grows.

3. **Improved User Experience**:
   - Develop a front-end interface or API client for easier interaction with the backend.

4. **Performance Optimization**:
   - Introduce caching mechanisms for frequently accessed product or category data.
