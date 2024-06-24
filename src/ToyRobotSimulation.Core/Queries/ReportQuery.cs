using ToyRobotSimulation.Domain.Interfaces;

namespace ToyRobotSimulation.Core.Queries
{
    /// <summary>
    /// Query to report on the toy robot's position.
    /// </summary>
    public class ReportQuery : IQuery<string>
    {
        public string Execute(IToy toy)
        {
            return toy.Report();
        }
    }
}