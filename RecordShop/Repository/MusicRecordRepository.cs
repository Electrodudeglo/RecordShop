using Azure.Core.Serialization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using RecordShop.Model;
using System.Text.Json;

namespace RecordShop.Repository
{

    public interface IMusicRecordRepo
    {
        public IEnumerable<MusicRecordModel> GetAllRecords();
        public MusicRecordModel GetOneRecord(int id);
        public MusicRecordModel AddOneRecord(MusicRecordModel musicRecordModel);
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
            return _dbContext.MusicRecords.FirstOrDefault(m => m.Id == id);
        }

        public MusicRecordModel AddOneRecord(MusicRecordModel musicRecordModel)
        {
            _dbContext.MusicRecords.Add(musicRecordModel);
            _dbContext.SaveChanges();
            return musicRecordModel;
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
