#  Employee Management Console App

This is a **simple C# console application**

- Add Employee (ID, Name)
- View All Employees
- Exit Application

---

##  SOLID Principles Applied

### 1. **S — Single Responsibility Principle**

> Every class should have only one reason to change.

- `Employee.cs`: Stores employee properties.
- `EmployeeService.cs`: Handles storage/retrieval logic.
- `EmployeeHandler.cs`: Handles user interface interaction.

 **Each class does one job and one job only.**

---

### 2. **O — Open/Closed Principle**

> Software should be open for extension but closed for modification.

- You can add new operations (e.g., search, delete) by **extending** `EmployeeHandler` or `EmployeeService` without changing their existing code.

 **Easily extend behavior without modifying working code.**

---

### 3. **L — Liskov Substitution Principle**

> Derived classes must be substitutable for their base classes.

- `IEmployeeService` is implemented by `EmployeeService`.
- The app uses the **interface**, so you could substitute `MockEmployeeService`, `FileEmployeeService`, or a database-backed version easily.

 **You can replace implementation without breaking the app.**

---

### 4. **I — Interface Segregation Principle**

> No client should be forced to depend on methods it doesn't use.

- `IEmployeeService` defines only methods needed for employee operations (`AddEmployee`, `GetAllEmployees`).

**Interfaces are small and relevant.**

---

### 5. **D — Dependency Inversion Principle**

> High-level modules should depend on abstractions, not on concrete classes.

- `Program.cs` and `EmployeeHandler.cs` depend on `IEmployeeService` instead of `EmployeeService`.

 **Promotes loose coupling and easier testing.**