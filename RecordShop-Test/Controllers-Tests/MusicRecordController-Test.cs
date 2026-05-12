using Microsoft.AspNetCore.Mvc;
using Moq;
using RecordShop.Controllers;
using RecordShop.Model;
using RecordShop.Services;
namespace RecordShop_Test;

    public class MusicRecordController_Test
    {
        private MusicRecordController _controller;
        private Mock<IMusicRecordService> _serviceMock;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<IMusicRecordService>();
            _controller = new MusicRecordController(_serviceMock.Object);
        }

        [Test]
        public void Index_Returns_Ok_With_ListOfRecords()
        {
            var records = new List<MusicRecordModel>
            {
                new MusicRecordModel("Album1", "Artist1", "2000", "Genre1"),
                new MusicRecordModel("Album2", "Artist2", "2005", "Genre2")
            };

            _serviceMock.Setup(s => s.ServiceGetAllRecords()).Returns(records);

            var result = _controller.Index() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(records, result.Value);
        }

   }
