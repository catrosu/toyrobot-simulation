using Ardalis.Result;
using ToyRobotSimulation.Core.Queries;

namespace ToyRobotSimulation.UnitTests.QueryTests
{
    public class QueryParserTests
    {
        [Theory]
        [InlineData("REPORT")]
        public void Parse_ValidQueries_ShouldReturnCorrectQuery(string input)
        {
            // Act
            var result = QueryParser.ParseQuery(input);

            // Assert
            Assert.True(result.IsSuccess);
            var query = result.Value;
            Assert.NotNull(query);

            switch (input)
            {
                case "REPORT":
                    Assert.IsType<ReportQuery>(query);
                    break;
            }
        }

        [Theory]
        [InlineData("REPORT NOW")]
        [InlineData("SHOW STATUS")]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("UNKNOWN QUERY")]
        public void Parse_InvalidQueries_ShouldReturnInvalidResult(string input)
        {
            // Act
            var result = QueryParser.ParseQuery(input);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ResultStatus.Invalid, result.Status);
        }
    }
}