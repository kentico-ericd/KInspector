using KInspector.Core.Constants;
using KInspector.Reports.PageNotFoundSummary;
using KInspector.Reports.PageNotFoundSummary.Models;

using NUnit.Framework;

namespace KInspector.Tests.Common.Reports
{
    [TestFixture(10)]
    [TestFixture(11)]
    [TestFixture(12)]
    [TestFixture(13)]
    public class PageNotFoundSummaryTests : AbstractModuleTest<Report, Terms>
    {
        private readonly Report _mockReport;

        private IEnumerable<CmsNotFoundEvent> CleanResults => Enumerable.Empty<CmsNotFoundEvent>();

        private IEnumerable<CmsNotFoundEvent> BadResults => new List<CmsNotFoundEvent>()
        {
            new()
            {
                Count = 2,
                EventUrl = "https://test.com/a",
                Referrer = "https://test.com/b",
                FirstOccurrence = DateTime.Now
            },
            new()
            {
                Count = 3,
                EventUrl = "https://test.com/c",
                Referrer = "https://test.com/d",
                FirstOccurrence = DateTime.Now
            }
        };

        public PageNotFoundSummaryTests(int majorVersion) : base(majorVersion)
        {
            _mockReport = new Report(_mockDatabaseService.Object, _mockModuleMetadataService.Object);
        }

        [Test]
        public async Task Should_ReturnGoodResult_When_NoEvents()
        {
            // Arrange
            _mockDatabaseService
                .Setup(p => p.ExecuteSqlFromFile<CmsNotFoundEvent>(Scripts.GetPageNotFoundEventLogEntries))
                .Returns(Task.FromResult(CleanResults));

            // Act
            var results = await _mockReport.GetResults();

            // Assert
            Assert.That(results.Status == ResultsStatus.Good);
            Assert.That(results.Type == ResultsType.NoResults);
            Assert.That(results.TableResults.Count == 0);
        }

        [Test]
        public async Task Should_ReturnWarningResult_When_EventsExist()
        {
            // Arrange
            _mockDatabaseService
                .Setup(p => p.ExecuteSqlFromFile<CmsNotFoundEvent>(Scripts.GetPageNotFoundEventLogEntries))
                .Returns(Task.FromResult(BadResults));

            // Act
            var results = await _mockReport.GetResults();

            // Assert
            Assert.That(results.Status == ResultsStatus.Warning);
            Assert.That(results.Type == ResultsType.TableList);
            Assert.That(results.TableResults.Count == 1);
        }
    }
}
