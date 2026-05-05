using Azure.Core.Serialization;
using RecordShop.Model;
using System.Text.Json;

namespace RecordShop.Repository
{

    public interface IMusicRecordRepo
    {
        public IEnumerable<MusicRecordModel> GetAllRecords();
        public MusicRecordModel GetOneRecord(int id);
        public MusicRecordModel AddOneRecord(MusicRecordModel musicRecordModel);

        public string DeleteOneRecord(int id);

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

        public MusicRecordModel AddOneRecord(MusicRecordModel musicRecordModel)
        {

            string filePath = Path.Combine("DummyData", "MusicRecordData.json");

            string rawJson = File.ReadAllText(filePath);

            var getAllRecord = JsonSerializer.Deserialize<List<MusicRecordModel>>(rawJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            musicRecordModel.Id = getAllRecord.Max(g => g.Id) + 1;

            getAllRecord.Add(musicRecordModel);

            var updateRecords = JsonSerializer.Serialize(getAllRecord, new JsonSerializerOptions { WriteIndented = true});

            return musicRecordModel;

        }

        public string DeleteOneRecord(int id)
        {


            return "";

        }

    }
}
