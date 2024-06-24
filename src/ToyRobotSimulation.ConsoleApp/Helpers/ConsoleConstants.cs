namespace ToyRobotSimulation.ConsoleApp.Helpers
{
    internal static class ConsoleConstants
    {
        internal const string ExitCommand = "EXIT";

        internal const string WelcomeMessage = "Welcome to the Toy Robot Simulation!";

        internal const string Instructions = @"In this simulation, you can control a toy robot on a rectangular surface. Update appsettings.json to control the dimensions of the surface.
Commands (case insensitive):
  PLACE X,Y,F - Place the robot on the surface at position (X,Y) facing direction F (NORTH, SOUTH, EAST, WEST).
  MOVE - Move the robot one unit forward in the direction it is currently facing.
  LEFT - Rotate the robot 90 degrees to the left.
  RIGHT - Rotate the robot 90 degrees to the right.
  REPORT - Display the current position and direction of the robot.
  ERROR - Additional command to test the error logging to the '{0}logs/toyrobot-simulator.json' file.
  EXIT - Exit the simulation.
  Example: PLACE 2,3,EAST

Enter your commands below:

";
    }
}
