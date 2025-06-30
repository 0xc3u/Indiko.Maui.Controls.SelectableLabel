The `SelectableLabel` control is a custom label for MAUI applications that provides selectable text capabilities with extensive customization options. This control supports various text properties and formatted text, making it versatile for creating rich, interactive labels in cross-platform apps.


Please follow these guidelines when contributing:

## Code Standards

### General Principles
- **Target Framework:** All projects target .NET 9.0. Ensure compatibility with this version.
- **Language:** Use C# for all code unless otherwise specified.
- **Readability:** Write clear, self-explanatory code. Use meaningful variable, method, and class names.
- **Consistency:** Follow consistent naming conventions and code formatting throughout the solution.
- **Comments:** Add inline comments only where necessary to clarify complex logic.
- **Error Handling:** Use structured exception handling. Avoid swallowing exceptions; log or rethrow as appropriate.
- **SOLID Principles:** Adhere to SOLID design principles for maintainable and extensible code.
- **Dependency Injection:** Prefer constructor injection for dependencies.
- **Async/Await:** Use asynchronous programming patterns where appropriate, especially for I/O-bound operations.
- **Magic Numbers:** Avoid magic numbers; use named constants or enums.
- **Follow existing Code principles:** Search for similar implementation of other domain parts and adopt the style as blueprint.

- ### Naming Conventions
- **Classes & Interfaces:** PascalCase (e.g., `UserService`, `IUserRepository`)
- **Methods & Properties:** PascalCase (e.g., `GetUserById`)
- **Variables & Parameters:** camelCase (e.g., `userId`)
- **Constants:** PascalCase (e.g., `DefaultTimeout`)
- **Unit Test Methods:** Use descriptive names indicating the scenario and expected outcome (e.g., `GetUserById_ReturnsUser_WhenUserExists`)

### Code Style
- **Braces:** Use Allman style (braces on new lines).
- **Indentation:** Use 4 spaces per indentation level.
- **Line Length:** Limit lines to 120 characters.
- **Usings:** Place `using` statements outside the namespace and remove unused usings.


### Required Before Each Commit
- Use Semantic Release Prefixes for Commits (e.g., `feat:`, `fix:`, `docs:`, `chore:`) with the following meanings:
  - `feat:` for new features
  - `fix:` for bug fixes
  - `docs:` for documentation changes
  - `chore:` for maintenance tasks (e.g., updating dependencies, refactoring)

## Repository Structure
- `Indiko.Maui.Controls.SelectableLabel/`: contains the selectable Label library
- `Indiko.Maui.Controls.SelectableLabel.Sample/`: contains a sample projects showing the usage of the selectable Label library.


## Key Guidelines
1. Follow c# best practices
2. Maintain existing code structure and organization
3. Use dependency injection patterns where appropriate
4. Write unit tests for new functionality and place it in the tests projects
5. **Adhere to these standards** when generating or modifying code.
6. **Prefer existing patterns** and conventions found in the solution.
7. **Generate code that is ready to use** and fits seamlessly into the current structure.
8. **Document any deviations** from these standards in pull requests or code reviews.

