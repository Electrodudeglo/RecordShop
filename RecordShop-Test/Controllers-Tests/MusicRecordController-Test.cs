using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using RecordShop.Controllers;
using RecordShop.Model;
using RecordShop.External;
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

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(records));
        }

        
        [Test]
        public void GetOneRecord_Returns_Ok_With_Record()
        {
            var record = new MusicRecordModel("AlbumX", "ArtistX", "1999", "Rock");

            _serviceMock.Setup(s => s.ServiceGetOneRecord(1)).Returns(record);

            var result = _controller.getOneRecord(1) as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(record));
        }

        [Test]
        public void GetOneRecord_Returns_Ok_With_Empty_Record_When_NotFound()
        {
            // The repository returns an empty MusicRecordModel (not null) when no record matches.
            var emptyRecord = new MusicRecordModel();

            _serviceMock.Setup(s => s.ServiceGetOneRecord(999))
                        .Returns(emptyRecord);

            var result = _controller.getOneRecord(999) as OkObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(emptyRecord));
        }

        [Test]
        public async Task CheckDeezerApi_Returns_Ok_With_Result()
        {

            //Arrange
            var albumDetails = new DeezerAlbumDetails{
                Id = 1,
                Title = "In Times New Roman",
                Artist = new DeezerArtist
                {
                    Name = "Queens of the Stone Age"
                },
                Release_Date = "2023-06-16",
                Genres = new DeezerGenreContainer
                {
                    Data = new List<DeezerGenre>
                    {
                        new DeezerGenre {Name = "Rock" }
                    }
                },
                Cover = "https://e-cdns-images.dzcdn.net/images/cover/abc123/1000x1000.jpg",
                Cover_small = "https://e-cdns-images.dzcdn.net/images/cover/abc123/56x56.jpg",
                Cover_medium = "https://e-cdns-images.dzcdn.net/images/cover/abc123/250x250.jpg",
                Cover_big = "https://e-cdns-images.dzcdn.net/images/cover/abc123/500x500.jpg",
                Cover_xl = "https://e-cdns-images.dzcdn.net/images/cover/abc123/1000x1000.jpg",
                Fans = 500000
            };
            var deezerRequest = new DeezerCheckRequest("albumName","artistName");
            var deezerResult = new DeezerAlbumResult{
                Album = albumDetails,
                ResultStatus = DeezerResultStatusEnum.Success
            };          
            _serviceMock.Setup(s=>s.CheckDeezer(deezerRequest)).ReturnsAsync(deezerResult);

            //Act
            var result = await _controller.CheckDeezerApi(deezerRequest) as OkObjectResult;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(deezerResult));

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

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(201));
            Assert.That(result.ActionName, Is.EqualTo("getOneRecord"));
            Assert.That(result.Value, Is.EqualTo(newRecord));
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

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(201));
            Assert.That(result.ActionName, Is.EqualTo("getOneRecord"));
            Assert.That(result.Value, Is.EqualTo(updated));
        }

        
        [Test]
        public void DeleteOneRecord_Returns_NoContent_When_Deleted()
        {
            _serviceMock.Setup(s => s.ServiceDeleteOneRecord(1)).Returns(true);

            var result = _controller.DeleteOneRecord(1) as NoContentResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(204));
        }

        [Test]
        public void DeleteOneRecord_Returns_NotFound_When_Not_Deleted()
        {
            _serviceMock.Setup(s => s.ServiceDeleteOneRecord(999)).Returns(false);

            var result = _controller.DeleteOneRecord(999) as NotFoundObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(404));
        }
    }
}
