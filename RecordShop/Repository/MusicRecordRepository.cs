using Azure.Core.Serialization;
using RecordShop.Model;
using System.Text.Json;

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

            string filePath = Path.Combine("DummyData", "MusicRecordData.json");

            string getAllData = File.ReadAllText(filePath);

            var serialize = JsonSerializer.Deserialize<List<MusicRecordModel>>(getAllData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});

            return serialize;
        }

        public MusicRecordModel GetOneRecord(int id)
        {
            string filePath = Path.Combine("DummyData", "MusicRecordData.json");

            string getAllData = File.ReadAllText(filePath);

            var serialize = JsonSerializer.Deserialize<List<MusicRecordModel>>(getAllData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return serialize.FirstOrDefault(s => s.Id == id);
        }

    }
}
