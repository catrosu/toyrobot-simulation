using ToyRobotSimulation.Domain.Interfaces;

namespace ToyRobotSimulation.Core.Queries
{
    public interface IQuery<out TResult>
    {
        TResult Execute(IToy toy);
    }
}