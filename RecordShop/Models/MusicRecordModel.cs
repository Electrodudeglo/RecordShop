using System.Text.Json.Serialization;

namespace RecordShop.Model
{
    public class MusicRecordModel
    {
        public int Id { get; set; }
        [JsonPropertyName("record_title")]
        public string RecordTitle { get; set; }
        public string Artists { get; set; }
        [JsonPropertyName("release_year")]
        public string ReleaseYear { get; set; }
        public string Genre { get; set; }
        public int Stock { get; set; }

        public MusicRecordModel(){ }

        public MusicRecordModel(string recordTitle, string artists, string releaseYear, string genre)
        {
            RecordTitle = recordTitle;
            Artists = artists;
            ReleaseYear = releaseYear;
            Genre = genre;
        }

    }
}
