using RecordShop.External;
using RecordShop.Model;
using RecordShop.Repository;

namespace RecordShop.Services
{

    public interface IMusicRecordService
    {
        public IEnumerable<MusicRecordModel> ServiceGetAllRecords();
        public MusicRecordModel ServiceGetOneRecord(int id);
        public Task<DeezerAlbumResult> CheckDeezer(DeezerCheckRequest request);
        public MusicRecordModel ServiceAddOneRecord(MusicRecordModel musicRecordModel);
        public MusicRecordModel ServiceUpdateOneRecord(MusicRecordModel musicRecord, int id);
        public bool ServiceDeleteOneRecord(int id);
    }

    public class MusicRecordService : IMusicRecordService
    {

        private readonly IMusicRecordRepo _musicRecordRepo;
        private readonly IDeezerApiClient _deezer;

        public MusicRecordService(IMusicRecordRepo musicRecordRepo,IDeezerApiClient deezerApiClient)
        {
            _musicRecordRepo = musicRecordRepo;
            _deezer = deezerApiClient;
        }

        public IEnumerable<MusicRecordModel> ServiceGetAllRecords()
        {
            return _musicRecordRepo.GetAllRecords();
        }

        public MusicRecordModel ServiceGetOneRecord(int id)
        {
            return _musicRecordRepo.GetOneRecord(id);
        }

        public Task<DeezerAlbumResult> CheckDeezer(DeezerCheckRequest request)
        {
            return _deezer.FindAlbumAsync(request.AlbumName, request.ArtistName);
        }

        public MusicRecordModel ServiceAddOneRecord(MusicRecordModel musicRecordModel)
        {
            return _musicRecordRepo.AddOneRecord(musicRecordModel);
        }

        public MusicRecordModel ServiceUpdateOneRecord(MusicRecordModel musicRecord, int id)
        {
            return _musicRecordRepo.UpdateOneRecord(musicRecord,id);
        }

        public bool ServiceDeleteOneRecord(int id)
        {
            return _musicRecordRepo.DeleteOneRecord(id);
        }

    }
}
