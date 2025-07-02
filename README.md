# Martian Robots Solution

## 📋 Problem Description

The surface of Mars is modeled as a rectangular grid where robots move according to instructions from Earth. Robots can turn left (L), turn right (R), or move forward (F). When a robot moves off the grid, it becomes "lost" but leaves a "scent" that prevents future robots from falling off at the same position.

**Input Format:**
```
5 3
1 1 E
RFRFRFRF
3 2 N
FRRFLLFFRRFLL
0 3 W
LLFFFLFLFL
```

**Expected Output:**
```
1 1 E
3 3 N LOST
2 3 S
```

## The straight forward way
The solution contains a project 'StraightForwardWay' which is the AI generated approach to the problem. It exhibits the most simplest solution to the problem without taking much design principles into account or scalability.
The StraightForwardWay approach however definitely ticks the box for 'KISS' (Keep It Simple Stupid).

## 🏗️ Architecture & Design Decisions

### Domain-Driven Design (DDD)
The solution follows DDD principles with clear separation between:
- **Domain Models**: Core business entities (`Rover`, `Direction`, `Movement`)
- **Domain Services**: Business logic encapsulation (`MovementService`)
- **Application Services**: Orchestration and infrastructure concerns (`CommandLineService`)
- **Application Layer**: Entry points and configuration

### Layered Architecture
```
┌─────────────────────────────────────────────┐
│ Application.CommandLine.Pilot               │  ← Entry Point
├─────────────────────────────────────────────┤
│ Application.IoC                             │  ← Dependency Injection
├─────────────────────────────────────────────┤
│ Domain.Services                             │  ← Business Logic
├─────────────────────────────────────────────┤
│ Domain.Models                               │  ← Core Entities
└─────────────────────────────────────────────┘
```

### Multi-Project Structure Benefits
The solution is split into multiple projects to enable:
- **📦 NuGet Package Creation**: Each layer can be packaged independently
- **🔄 Code Reusability**: Domain models and services can be shared across applications
- **🏢 Enterprise Scalability**: Different teams can work on different layers
- **🔧 Future Extensibility**: Easy to add web interfaces, APIs, or other applications

### Key Design Patterns

#### Interface-Based Models
```csharp
public interface IRobot
{
    Direction CurrentDirection { get; set; }
    int CurrentXPosition { get; set; }
    int CurrentYPosition { get; set; }
    bool IsLost { get; set; }
    List<Movement> InstructionsProcessed { get; set; }
}

public interface IRover : IRobot
{
    int Odometer { get; set; }
}
```

**Benefits over inheritance:**
- ✅ **Multiple Interface Implementation**: Classes can implement multiple interfaces (vs. single inheritance limit)
- ✅ **Generic Method Support**: Methods can accept generic interface types
- ✅ **Future Extensibility**: Easy to add `IDrone`, `ISubmarine` etc. without inheritance conflicts
- ✅ **Composition over Inheritance**: More flexible object relationships

#### Dependency Injection
```csharp
builder.Services.AddScoped<ICommandLineService, CommandLineService>();
builder.Services.AddScoped<IMovementService, MovementService>();
```

**Advantages:**
- 🧪 **Testability**: Easy to inject mocks for unit testing
- 🔄 **Flexibility**: Swap implementations without changing dependent code
- 📦 **Modularity**: Services can be developed and tested independently
- 🎯 **Single Responsibility**: Each service has a focused purpose

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 or higher
- Visual Studio 2022 or VS Code

### Running the Application
```bash
# Clone the repository
git clone <repository-url>
cd MartianRobots

# Build the solution
dotnet build

# Run the application
dotnet run --project Solubard.CodingChallenge.MartianRobots.Application.CommandLine.Pilot

# Run tests
dotnet test
```

### Sample Input
Enter the following when prompted:
```
5 3
1 1 E
RFRFRFRF
3 2 N
FRRFLLFFRRFLL
0 3 W
LLFFFLFLFL

```
*(Press Enter twice to finish input)*

## 🧪 Testing Strategy

### Comprehensive Test Coverage
- **Unit Tests**: Domain models and services with mocked dependencies
- **Integration Tests**: End-to-end scenario validation
- **Mocking Framework**: FakeItEasy for clean, readable test setup

### Test Structure
```
Tests/
├── Domain.Models/          # Entity behavior tests
├── Domain.Services/        # Business logic tests
├── Application.Services/   # Service integration tests
└── Application.Pilot/      # Program logic tests
```

### Key Testing Features
- ✅ **Boundary Testing**: Robot movement edge cases
- ✅ **Scent System**: Lost robot prevention mechanism
- ✅ **Movement Logic**: All directional operations
- ✅ **Input Parsing**: Various instruction formats
- ✅ **Error Handling**: Invalid commands and directions

## 🔮 Future Extensibility

The architecture supports easy expansion:

### Additional Application Interfaces
```csharp
// Web API Controller
[ApiController]
public class MartianRobotsController : ControllerBase
{
    private readonly IMovementService _movementService;
    // Same domain services, different interface
}

// Blazor Component
public partial class RobotSimulator : ComponentBase
{
    [Inject] private IMovementService MovementService { get; set; }
    // Same business logic, web UI
}
```

### New Robot Types
```csharp
public interface IDrone : IRobot
{
    int Altitude { get; set; }
    int HobbsMeter { get; set; }
}

public class Drone : IDrone, IRobot
{
    // Implements both interfaces - no inheritance conflicts
}
```

### Additional Movement Types
```csharp
public enum Movement
{
    Forward,
    TurnLeft,
    TurnRight,
    Hover,      // New for drones
    Dive,       // New for submarines
    Surface     // New for submarines
}
```

## 📁 Project Structure

```
Solubard.CodingChallenge.MartianRobots/
├── Solubard.CodingChallenge.MartianRobots.Domain.Models/
│   └── Rover.cs
├── Solubard.CodingChallenge.MartianRobots.Domain.Models.Enums/
│   ├── Direction.cs
│   └── Movement.cs
├── Solubard.CodingChallenge.MartianRobots.Domain.Models.Interfaces/
│   ├── IRobot.cs
│   ├── IRover.cs
│   └── IDrone.cs
├── Solubard.CodingChallenge.MartianRobots.Domain.Services/
│   ├── CommandLineService.cs
│   └── MovementService.cs
├── Solubard.CodingChallenge.MartianRobots.Domain.Services.Interfaces/
│   ├── ICommandLineService.cs
│   └── IMovementService.cs
├── Solubard.CodingChallenge.MartianRobots.Application.Ioc/
│   └── PilotConfig.cs
├── Solubard.CodingChallenge.MartianRobots.Application.CommandLine.Pilot/
│   └── Program.cs
├── Solubard.CodingChallenge.MartianRobots.Tests.Domain.Models/
│   └── Rover.cs
├── Solubard.CodingChallenge.MartianRobots.Tests.Domain.Services/
│   ├── CommandLineService.cs
│   └── MovementService.cs
├── Solubard.CodingChallenge.MartianRobots.Tests.Application.CommandLine.Pilot/
│   └── ProgramLogic.cs
├── StraightForwardWay/
│   ├── Direction.cs
│   ├── MartianRobotSimulator.cs
│   ├── Program.cs
│   └── Robot.cs
└── StraightForwardWay.Tests
    └── UnitTest1.cs
```

## 🎯 Design Principles Applied

### SOLID Principles
- **Single Responsibility**: Each service has one focused purpose
- **Open/Closed**: Easy to extend with new robot types without modifying existing code
- **Liskov Substitution**: All implementations properly fulfill their interface contracts
- **Interface Segregation**: Focused interfaces (`IRobot`, `IRover`, `IDrone`)
- **Dependency Inversion**: High-level modules depend on abstractions, not concretions

### Domain-Driven Design
- **Ubiquitous Language**: Terms like "Robot", "Movement", "Scent" match problem domain
- **Bounded Contexts**: Clear separation between movement logic and I/O concerns
- **Domain Services**: Complex business rules encapsulated in services
- **Value Objects**: Enums for `Direction` and `Movement` represent domain concepts

## 🔧 Configuration & Dependencies

### NuGet Packages
```xml
<PackageReference Include="Microsoft.Extensions.Hosting" Version="8.0.0" />
<PackageReference Include="xunit" Version="2.5.3" />
<PackageReference Include="FakeItEasy" Version="8.3.0" />
```

### Dependency Injection Container
```csharp
// Singleton shared state
builder.Services.AddSingleton(new HashSet<(int x, int y)>());

// Scoped services for request lifecycle
builder.Services.AddScoped<ICommandLineService, CommandLineService>();
builder.Services.AddScoped<IMovementService, MovementService>();
```

## 📚 Additional Resources

- [Domain-Driven Design Reference](https://domainlanguage.com/ddd/)
- [Clean Architecture Principles](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [.NET Dependency Injection](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
- [xUnit Testing Framework](https://xunit.net/)
- [FakeItEasy Mocking Framework](https://fakeiteasy.github.io/)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Follow the existing architecture patterns
4. Add comprehensive tests
5. Commit changes (`git commit -m 'Add amazing feature'`)
6. Push to branch (`git push origin feature/amazing-feature`)
7. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.