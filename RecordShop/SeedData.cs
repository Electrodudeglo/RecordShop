using RecordShop.Model;
using System.Text.Json;

namespace RecordShop
{
    public class SeedData
    {
        public static void Initialize(MyDbContext myDbContext)
        {
            if (myDbContext.MusicRecords.Any()) return;
            var filePath = Path.Combine("Resources", "MusicRecordData.json");
            var json = File.ReadAllText(filePath);
            var records = JsonSerializer.Deserialize<List<MusicRecordModel>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
            myDbContext.MusicRecords.AddRange(records);
            myDbContext.SaveChanges();
        }
    }
}
