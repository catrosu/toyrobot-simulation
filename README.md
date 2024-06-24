# Toy Robot Simulation 

## Overview

The Toy Robot Simulation is a console application that simulates a toy robot moving on a 5x5 square tabletop. The robot can be controlled using various commands, such as `PLACE`, `MOVE`, `LEFT`, `RIGHT`, and `REPORT`, to move, turn, and report its position. The application ensures that the robot does not fall off the table.

For full requirements, read the section: [Toy Robot Coding Challenge Requirements](#toy-robot-coding-challenge-requirements)

## Dependencies
- .NET 8

## Setup
1. **Clone the Repository**
   ```bash
   git clone https://github.com/catrosu/toyrobot-simulation
   cd toyrobot-simulation
   ```

2. **Restore Dependencies**
   Run the following command to restore the required NuGet packages:
   ```bash
   dotnet restore
   ```

3. **Configure the Application**
   Update the `appsettings.json` file to set the dimensions of the surface:
   ```json
   {
     "SurfaceConfiguration": {
       "Width": 5,
       "Height": 5
     }
   }
   ```

4. **Logging**
   The output location for the application logs, including exceptions and errors, is '{Environment.GetLogicalDrives()[0]}logs/toyrobot-simulator.json'

## Running the Application
1. **Build the Application**
   Run the following command to build the project:
   ```bash
   dotnet build --configuration Release
   ```

2. **Navigate to the Releases artifacts**
   ```bash
   cd src\ToyRobotSimulation.ConsoleApp\bin\Release\net8.0
   ```

3. **Run the Application**
   You can run the application with or without a command file:

   **From a Command File:**
   ```bash
   ToyRobotSimulation.ConsoleApp.exe robot-commands.txt
   ```
   (Ensure `robot-commands.txt` contains the commands you want to execute.)

   **Interactively:**
   ```bash
   ToyRobotSimulation.ConsoleApp.exe
   ```
   Enter commands interactively in the console.

## Commands
- `PLACE X,Y,F`: Places the robot on the surface at position `(X,Y)` facing direction `F` (NORTH, SOUTH, EAST, WEST).
- `MOVE`: Moves the robot one unit forward in the direction it is currently facing.
- `LEFT`: Rotates the robot 90 degrees to the left.
- `RIGHT`: Rotates the robot 90 degrees to the right.
- `REPORT`: Displays the current position and direction of the robot.
- `ERROR`:  Additional command that throws `ArgumentException` to allow testing the logging to the '{Environment.GetLogicalDrives()[0]}logs/toyrobot-simulator.json' file, i.e. 'C:\logs\toyrobot-simulator.json'
- `EXIT`: Exits the simulation.

### Example Commands
```
PLACE 0,0,NORTH
MOVE
REPORT
# Output: 0,1,NORTH
```

## Running Tests
To run the `Unit Tests`, navigate to the Unit Tests project, from the main root:
```bash
cd tests/ToyRobotSimulation.UnitTests
```

Execute the following command to run the unit tests:
```bash
dotnet test
```

To run the `Integration Tests`, navigate to the Integration Tests project, from the main root:
```bash
cd tests/ToyRobotSimulation.IntegrationTests
```

Execute the following command to run the integration tests:
```bash
dotnet test
```


## Design Decisions
1. **Clean Architecture**: The project is structured following Clean Architecture principles to ensure separation of concerns. This allows for easier maintenance, testing, and scalability. The architecture promotes:
  - Independence of frameworks
  - Clear separation between business logic and implementation details
  - Testability
  - Dependency inversion

2. **Command and Query Pattern**: The application uses the Command and Query pattern to separate command execution and queries. This enhances the maintainability and testability of the code.

3. **Simulation Mediator/Orchestrator**: The `Simulation` class acts as a mediator between the robot and the table, orchestrating the interactions without tightly coupling them. This design allows the `Robot` to operate independently of the `Table` and vice versa, promoting reusability and flexibility, making it easier to adapt or extend each part without affecting the other.

4. **Avoiding libraries like MediatR**: Using libraries like MediatR was avoided to keep the solution simple and focused. While MediatR can be beneficial for managing complex command and query dispatching, it introduces additional dependencies and complexity that may not be necessary for this project.

5. **Ardalis.Result**: The decision to use the `Ardalis.Result` library rather than throwing exceptions was made to facilitate communication between components. This approach ensures that errors are handled gracefully and meaningful feedback is provided without relying on exception handling for flow control.
The library was used in the Core project for convenience, to avoid the need to write a custom Result type.

6. **Synchronous Processing**: Given the context of the toy robot simulation, the command processing is primarily about updating the robot's position and does not involve I/O-bound operations. Therefore, using synchronous processing is appropriate. However, if future requirements might include I/O-bound operations, the methods can be updated to be asynchronous.

7. **Serilog**: Serilog was chosen for its flexibility and powerful capabilities in structured logging. Serilog allows the capture of detailed, structured log information that can be easily queried and analyzed, enhancing our ability to diagnose issues and understand application behavior.

8. **File Logging Sink**: While logging to the console is useful for real-time monitoring and debugging, logging to files provides a persistent record of application activity. File logging is essential for post-mortem analysis and long-term monitoring, enabling us to review historical data, analyze trends, and troubleshoot issues that may not be immediately visible in console output.

## Assumptions and Simplifications
- **Single Robot**: The simulation assumes only one robot can be on the table at any given time.
- **Small Command File**: The application assumes the command test file is small, containing a limited set of commands. This avoids the need for streaming large files.
- **Sequential Command Execution**: Commands in the test file are executed as a continuation, with no option to run them as separate examples or scenarios.
- **No Simulation Restart**: There is no option to restart the simulation apart from restarting the entire application.
- **Console Interaction**: The interaction with the console could be more user-friendly, supporting one-key to simplify the commands like `EXIT`, `MOVE`, `LEFT`, and `RIGHT`.
- **Simple Query Responses**: The responses to queries are kept simple for clarity and ease of use.

&nbsp;
---

# Toy Robot Coding Challenge Requirements
## Description and Requirements

The application is a simulation of a toy robot moving on a square table top, of dimensions 5 units x 5 units. There are no other obstructions on the table surface. The robot is free to roam around the surface of the table, but must be prevented from falling to destruction. Any movement that would result in the robot falling from the table must be prevented, however further valid movement commands must still be allowed.

Create a console application that can read in commands of the following form:
- `PLACE X,Y,F`
- `MOVE`
- `LEFT`
- `RIGHT`
- `REPORT`

`PLACE` will put the toy robot on the table in position `X,Y` and facing `NORTH`, `SOUTH`, `EAST` or `WEST`. The origin `(0,0)` can be considered to be the SOUTH WEST most corner. It is required that the first command to the robot is a `PLACE` command, after that, any sequence of commands may be issued, in any order, including another `PLACE` command. The application should discard all commands in the sequence until a valid `PLACE` command has been executed. `MOVE` will move the toy robot one unit forward in the direction it is currently facing. `LEFT` and `RIGHT` will rotate the robot 90 degrees in the specified direction without changing the position of the robot. `REPORT` will announce the X,Y and F of the robot. This can be in any form, but standard output is sufficient. A robot that is not on the table can choose to ignore the `MOVE`, `LEFT`, `RIGHT` and `REPORT` commands. Input can be from a file, or from standard input, as the developer chooses. Provide test data to exercise the application. It is not required to provide any graphical output showing the movement of the toy robot. The application should handle error states appropriately and be robust to user input.

### Constraints

The toy robot must not fall off the table during movement. This also includes the initial placement of the toy robot. Any move that would cause the robot to fall must be ignored.

### Example Input and Output

#### Example 1

```
PLACE 0,0,NORTH
MOVE
REPORT
```

**Output:**

```
0,1,NORTH
```

#### Example 2

```
PLACE 0,0,NORTH
LEFT
REPORT
```

**Output:**

```
0,0,WEST
```

#### Example 3

```
PLACE 1,2,EAST
MOVE
MOVE
LEFT
MOVE
REPORT
```

**Output:**

```
3,3,NORTH
```