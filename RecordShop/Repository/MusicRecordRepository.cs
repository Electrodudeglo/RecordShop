using RecordShop.Model;

namespace RecordShop.Repository
{

    public interface IMusicRecordRepo
    {
        public IEnumerable<MusicRecordModel> GetAllRecords();
        public MusicRecordModel GetOneRecord(int id);
    }
    public class MusicRecordRepository : IMusicRecordRepo
    {
        public IEnumerable<MusicRecordModel> GetAllRecords()
        {
            return new List<MusicRecordModel>();
        }

        public MusicRecordModel GetOneRecord(int id)
        {
            return new MusicRecordModel();
        }

    }
}
