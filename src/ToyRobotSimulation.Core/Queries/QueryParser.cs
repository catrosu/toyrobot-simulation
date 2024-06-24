using Ardalis.Result;

namespace ToyRobotSimulation.Core.Queries
{
    /// <summary>
    /// Parses query objects.
    /// </summary>
    public static class QueryParser
    {      
        /// <summary>
        /// Parses a query string and creates a corresponding <see cref="IQuery{TResult}"/> object.
        /// </summary>
        /// <param name="commandString">The query string to be parsed.</param>
        /// <returns>A <see cref="Result{IQuery{TResult}}"/> containing the query object or validation errors.</returns>
        public static Result<IQuery<string>> ParseQuery(string commandString)
        {
            var parts = commandString.Trim().ToUpper().Split(' ');
            var command = parts[0];

            return command switch
            {
                CommandConstants.ReportCommand when parts.Length == 1 => Result<IQuery<string>>.Success(new ReportQuery()),
                _ => Result<IQuery<string>>.Invalid(new ValidationError("Invalid query"))
            };
        }
    }
}