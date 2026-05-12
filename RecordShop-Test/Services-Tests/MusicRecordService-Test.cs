using Microsoft.IdentityModel.Tokens;
using Moq;
using RecordShop.Model;
using RecordShop.Repository;
using RecordShop.Services;
namespace RecordShop_Test;

public class MusicRecordService_Test
{
    private MusicRecordService _musicRecordService;
    private Mock<IMusicRecordRepo> _musicRecordRepoMoq;

    [SetUp]
    public void Setup()
    {
        _musicRecordRepoMoq = new Mock<IMusicRecordRepo>();
        _musicRecordService = new MusicRecordService(_musicRecordRepoMoq.Object);
    }

    [Test]
    public void ServiceGetAllRecords_Returns_ListOfRecords()
    {
        List<MusicRecordModel> musicRecord = new List<MusicRecordModel>();
        _musicRecordRepoMoq.Setup(repo => repo.GetAllRecords()).Returns(musicRecord);
        IEnumerable<MusicRecordModel> actual = _musicRecordService.ServiceGetAllRecords();
        Assert.That(actual, Is.EqualTo(musicRecord));        
    }

    [Test]
    public void ServicerGetOneRecord_Returns_One_Record_From_Id()
    {
        MusicRecordModel album = new MusicRecordModel("In Your Honor", "Foo Fighers", "2005", "Rock");
        _musicRecordRepoMoq.Setup(repo => repo.GetOneRecord(1)).Returns(album);
        MusicRecordModel actual = _musicRecordService.ServiceGetOneRecord(1);
        Assert.That(actual, Is.EqualTo(album));
    }

    [Test]
    public void ServiceAddOneRecord_Add_Album_Returns_Created_Data()
    {
        MusicRecordModel album = new MusicRecordModel("In Your Honor", "Foo Fighers", "2005", "Rock");
        _musicRecordRepoMoq.Setup(repo => repo.AddOneRecord(album)).Returns(album);
        MusicRecordModel actual = _musicRecordService.ServiceAddOneRecord(album);
        Assert.That(actual, Is.EqualTo(album));
    }

    [Test]
    public void ServiceUpdateOneRecord_Updates_Record_Returns_Updated_Data()
    {
        MusicRecordModel existingAlbum = new MusicRecordModel("In Your Honor", "Foo Fighters", "2005", "Rock");
        MusicRecordModel updatedAlbum = new MusicRecordModel("Echoes, Silence, Patience & Grace", "Foo Fighters", "2007", "Rock");

        _musicRecordRepoMoq.Setup(repo => repo.GetOneRecord(1)).Returns(existingAlbum);
        _musicRecordRepoMoq.Setup(repo => repo.UpdateOneRecord(updatedAlbum, 1)).Returns(updatedAlbum);

        MusicRecordModel actual = _musicRecordService.ServiceUpdateOneRecord(updatedAlbum, 1);

        Assert.That(actual, Is.EqualTo(updatedAlbum));
    }

    [Test]
    public void ServiceDeleteOneRecord_Deletes_Record_Returns_True()
    {
        _musicRecordRepoMoq.Setup(repo => repo.DeleteOneRecord(1)).Returns(true);

        bool actual = _musicRecordService.ServiceDeleteOneRecord(1);

        Assert.That(actual, Is.True);
    }

    [Test]
    public void ServiceDeleteOneRecord_Record_Not_Found_Returns_False()
    {
        _musicRecordRepoMoq.Setup(repo => repo.DeleteOneRecord(99)).Returns(false);

        bool actual = _musicRecordService.ServiceDeleteOneRecord(99);

        Assert.That(actual, Is.False);
    }
}
