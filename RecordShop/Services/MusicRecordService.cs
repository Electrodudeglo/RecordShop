using RecordShop.Model;
using RecordShop.Repository;

namespace RecordShop.Services
{

    public interface IMusicRecordService
    {
        public IEnumerable<MusicRecordModel> ServiceGetAllRecords();
        public MusicRecordModel ServiceGetOneRecord(int id);
        public MusicRecordModel ServiceAddOneRecord(MusicRecordModel musicRecordModel);
    }

    public class MusicRecordService : IMusicRecordService
    {

        private readonly IMusicRecordRepo _musicRecordRepo;

        public MusicRecordService(IMusicRecordRepo musicRecordRepo)
        {
            _musicRecordRepo = musicRecordRepo;
        }

        public IEnumerable<MusicRecordModel> ServiceGetAllRecords()
        {
            return _musicRecordRepo.GetAllRecords();
        }

        public MusicRecordModel ServiceGetOneRecord(int id)
        {
            return _musicRecordRepo.GetOneRecord(id);
        }

        public MusicRecordModel ServiceAddOneRecord(MusicRecordModel musicRecordModel)
        {
            return _musicRecordRepo.AddOneRecord(musicRecordModel);
        }

    }
}
