using KInspector.Core.Constants;
using KInspector.Reports.ContentTreeChildrenAnalysis;
using KInspector.Reports.ContentTreeChildrenAnalysis.Models;

using NUnit.Framework;

namespace KInspector.Tests.Common.Reports
{
    [TestFixture(10)]
    [TestFixture(11)]
    [TestFixture(12)]
    [TestFixture(13)]
    public class ContentTreeChildrenAnalysisTests : AbstractModuleTest<Report, Terms>
    {
        private readonly Report _mockReport;

        private IEnumerable<CmsTreeNode> CleanResults => Enumerable.Empty<CmsTreeNode>();

        private IEnumerable<CmsTreeNode> BadResults => new List<CmsTreeNode>()
        {
            new()
            {
                NodeID = 1,
                NodeAliasPath = "/A"
            },
            new()
            {
                NodeID = 2,
                NodeAliasPath = "/B"
            }
        };

        public ContentTreeChildrenAnalysisTests(int majorVersion) : base(majorVersion)
        {
            _mockReport = new Report(_mockDatabaseService.Object, _mockModuleMetadataService.Object);
        }

        [Test]
        public async Task Should_ReturnGoodResult_When_TableClean()
        {
            // Arrange
            _mockDatabaseService
                .Setup(p => p.ExecuteSqlFromFile<CmsTreeNode>(Scripts.GetNodesWithManyChildren))
                .Returns(Task.FromResult(CleanResults));

            // Act
            var results = await _mockReport.GetResults();

            // Assert
            Assert.That(results.Status == ResultsStatus.Good);
            Assert.That(results.Type == ResultsType.NoResults);
        }

        [Test]
        public async Task Should_ReturnErrorResult_When_NodesReturned()
        {
            // Arrange
            _mockDatabaseService
                .Setup(p => p.ExecuteSqlFromFile<CmsTreeNode>(Scripts.GetNodesWithManyChildren))
                .Returns(Task.FromResult(BadResults));

            // Act
            var results = await _mockReport.GetResults();

            // Assert
            Assert.That(results.Status == ResultsStatus.Error);
            Assert.That(results.Type == ResultsType.TableList);
            Assert.That(results.TableResults.FirstOrDefault()?.Rows.Count() == 2);
        }
    }
}
