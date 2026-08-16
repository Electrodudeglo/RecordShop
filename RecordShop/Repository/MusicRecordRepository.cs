using Azure.Core.Serialization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using RecordShop.Model;
using System.Runtime.CompilerServices;

namespace RecordShop.Repository
{

    public interface IMusicRecordRepo
    {
        public IEnumerable<MusicRecordModel> GetAllRecords();
        public MusicRecordModel GetOneRecord(int id);
        public Task<MusicRecordModel> AlbumExists(string artistName, string albumName);
        public MusicRecordModel AddOneRecord(MusicRecordModel musicRecordModel);
        public MusicRecordModel UpdateOneRecord(MusicRecordModel musicRecord, int id);
        public bool DeleteOneRecord(int id);

    }
    public class MusicRecordRepository : IMusicRecordRepo
    {
        public readonly MyDbContext _dbContext;

        public MusicRecordRepository(MyDbContext myDbContext)
        {
            _dbContext = myDbContext;
        }

        public IEnumerable<MusicRecordModel> GetAllRecords()
        {
            return _dbContext.MusicRecords.ToList();
        }

        public MusicRecordModel GetOneRecord(int id)
        {
            return _dbContext.MusicRecords.FirstOrDefault(m => m.Id == id) ?? new MusicRecordModel();
        }
 
        public async Task<MusicRecordModel> AlbumExists(string artistName, string albumName)
        {

            MusicRecordModel result = _dbContext.MusicRecords.FirstOrDefault(d=>d.Artists == artistName && d.RecordTitle == albumName) ?? new MusicRecordModel();

            if(result.Artists != null)
            {
                return result;
            }
            return new MusicRecordModel();
        }

        public MusicRecordModel AddOneRecord(MusicRecordModel musicRecordModel)
        {
            _dbContext.MusicRecords.Add(musicRecordModel);
            _dbContext.SaveChanges();
            return musicRecordModel;
        }

        public MusicRecordModel UpdateOneRecord(MusicRecordModel musicRecord, int id)
        {

            var record = _dbContext.MusicRecords.FirstOrDefault(r => r.Id == id);

            record.RecordTitle = musicRecord.RecordTitle;
            record.Artists = musicRecord.Artists;
            record.ReleaseYear = musicRecord.ReleaseYear;
            record.Stock = musicRecord.Stock;

            _dbContext.SaveChanges();

            return record;
        }

        public bool DeleteOneRecord(int id)
        {
            var record = _dbContext.MusicRecords.FirstOrDefault(r => r.Id == id);
            if (record == null)
                return false;

            _dbContext.MusicRecords.Remove(record);
            _dbContext.SaveChanges();
            return true;
        }

    }
}
