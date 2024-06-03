using KInspector.Core.Constants;
using KInspector.Reports.OnlineMarketingTableAnalysis;
using KInspector.Reports.OnlineMarketingTableAnalysis.Models;

using NUnit.Framework;

namespace KInspector.Tests.Common.Reports
{
    [TestFixture(10)]
    [TestFixture(11)]
    [TestFixture(12)]
    [TestFixture(13)]
    public class OnlineMarketingTableAnalysisTests : AbstractModuleTest<Report, Terms>
    {
        private readonly Report _mockReport;

        public OnlineMarketingTableAnalysisTests(int majorVersion) : base(majorVersion)
        {
            _mockReport = new Report(_mockDatabaseService.Object, _mockModuleMetadataService.Object);
        }

        [Test]
        public async Task Should_ReturnGoodResult_When_TablesBelowLimit()
        {
            // Arrange
            _mockDatabaseService
                .Setup(p => p.ExecuteSqlFromFileScalar<int>(Scripts.GetActivityCount))
                .Returns(Task.FromResult(1));
            _mockDatabaseService
                .Setup(p => p.ExecuteSqlFromFileScalar<int>(Scripts.GetContactCount))
                .Returns(Task.FromResult(1));
            _mockDatabaseService
                .Setup(p => p.ExecuteSqlFromFileScalar<int>(Scripts.GetContactGroupCount))
                .Returns(Task.FromResult(1));
            _mockDatabaseService
                .Setup(p => p.ExecuteSqlFromFileScalar<int>(Scripts.GetScoringRulesCount))
                .Returns(Task.FromResult(1));

            // Act
            var results = await _mockReport.GetResults();

            // Assert
            Assert.That(results.StringResults.Count == 4);
            Assert.That(results.Status == ResultsStatus.Good);
        }

        [TestCase(1000000001, 0, 0, 0)]
        [TestCase(0, 100000001, 0, 0)]
        [TestCase(0, 0, 51, 0)]
        [TestCase(0, 0, 0, 51)]
        public async Task Should_ReturnWarningResult_When_AnyTableExceedsLimit(
            int numActivities,
            int numContacts,
            int numContactGroups,
            int numScoringRules)
        {
            // Arrange
            _mockDatabaseService
                .Setup(p => p.ExecuteSqlFromFileScalar<int>(Scripts.GetActivityCount))
                .Returns(Task.FromResult(numActivities));
            _mockDatabaseService
                .Setup(p => p.ExecuteSqlFromFileScalar<int>(Scripts.GetContactCount))
                .Returns(Task.FromResult(numContacts));
            _mockDatabaseService
                .Setup(p => p.ExecuteSqlFromFileScalar<int>(Scripts.GetContactGroupCount))
                .Returns(Task.FromResult(numContactGroups));
            _mockDatabaseService
                .Setup(p => p.ExecuteSqlFromFileScalar<int>(Scripts.GetScoringRulesCount))
                .Returns(Task.FromResult(numScoringRules));

            // Act
            var results = await _mockReport.GetResults();

            // Assert
            Assert.That(results.StringResults.Count == 4);
            Assert.That(results.Status == ResultsStatus.Warning);
        }
    }
}
