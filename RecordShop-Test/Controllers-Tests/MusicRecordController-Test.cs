using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RecordShop.Controllers;
using RecordShop.Model;
using RecordShop.Services;
using System.Collections.Generic;

namespace RecordShop_Test
{
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

        
        [Test]
        public void GetOneRecord_Returns_Ok_With_Record()
        {
            var record = new MusicRecordModel("AlbumX", "ArtistX", "1999", "Rock");

            _serviceMock.Setup(s => s.ServiceGetOneRecord(1)).Returns(record);

            var result = _controller.getOneRecord(1) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(record, result.Value);
        }

        [Test]
        public void GetOneRecord_Returns_Ok_With_Null_When_NotFound()
        {
            _serviceMock.Setup(s => s.ServiceGetOneRecord(999))
                        .Returns((MusicRecordModel)null);

            var result = _controller.getOneRecord(999) as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsNull(result.Value);
        }

     
        [Test]
        public void AddOneRecord_Returns_CreatedAtAction_With_Record()
        {
            var newRecord = new MusicRecordModel("New Album", "ArtistZ", "2024", "Pop")
            {
                Id = 10
            };

            _serviceMock.Setup(s => s.ServiceAddOneRecord(newRecord))
                        .Returns(newRecord);

            var result = _controller.AddOneRecord(newRecord) as CreatedAtActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(201, result.StatusCode);
            Assert.AreEqual("getOneRecord", result.ActionName);
            Assert.AreEqual(newRecord, result.Value);
        }

       
        [Test]
        public void UpdateOneRecord_Returns_CreatedAtAction_With_Updated_Record()
        {
            var updated = new MusicRecordModel("Updated Album", "ArtistY", "2020", "Jazz")
            {
                Id = 5
            };

            _serviceMock.Setup(s => s.ServiceUpdateOneRecord(updated, 5))
                        .Returns(updated);

            var result = _controller.UpdateOneRecord(updated, 5) as CreatedAtActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(201, result.StatusCode);
            Assert.AreEqual("getOneRecord", result.ActionName);
            Assert.AreEqual(updated, result.Value);
        }

        
        [Test]
        public void DeleteOneRecord_Returns_NoContent_When_Deleted()
        {
            _serviceMock.Setup(s => s.ServiceDeleteOneRecord(1)).Returns(true);

            var result = _controller.DeleteOneRecord(1) as NoContentResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(204, result.StatusCode);
        }

        [Test]
        public void DeleteOneRecord_Returns_NotFound_When_Not_Deleted()
        {
            _serviceMock.Setup(s => s.ServiceDeleteOneRecord(999)).Returns(false);

            var result = _controller.DeleteOneRecord(999) as NotFoundObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }
    }
}
