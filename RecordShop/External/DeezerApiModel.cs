using System.Web;
namespace RecordShop.External
{
    public class DeezerApiModel
    {

    }

    public enum DeezerResultStatusEnum
{
    Success,
    NotFound,
    ServerError,
    NetworkError,
    InvalidJson
}

public class DeezerApiResponse
{
    public DeezerResultStatusEnum Status { get; set; }
    public DeezerAlbumDetails? Album { get; set; }
}

public class DeezerSearchResult
{
    public List<DeezerAlbumSummary> Data { get; set; } = new();
}

public class DeezerAlbumSummary
{
    public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
}

public class DeezerAlbumDetails
{
    public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string Cover { get; set; } = String.Empty;
        public string Artist { get; set; } = String.Empty;
}


}
