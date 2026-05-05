namespace RecordShop.Model
{
    public class MusicRecordModel
    {
        public int Id;
        public string RecordTitle;
        public string Artists;
        public string ReleaseYear;
        public string Genre;
        public int Stock = 5;

        public MusicRecordModel(){ }

        public MusicRecordModel(string recordTitle, string artists, string releaseYear)
        {
            RecordTitle = recordTitle;
            Artists = artists;
            ReleaseYear = releaseYear;
        }

    }
}
