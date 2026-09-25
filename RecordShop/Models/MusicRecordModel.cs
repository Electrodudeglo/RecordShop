using System.Text.Json.Serialization;

namespace RecordShop.Model
{
    public class MusicRecordModel
    {
        public int Id { get; set; }
        [JsonPropertyName("record_title")]
        public string RecordTitle { get; set; } = string.Empty;
        public string Artists { get; set; } = string.Empty;
        [JsonPropertyName("release_year")]
        public string ReleaseYear { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int Stock { get; set; }
        public string Cover { get; set; } = string.Empty;

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
