using System.Net;
using System.Web;

namespace RecordShop.External
{
    public class DeezerApiClient
    {
        private readonly HttpClient _http;

        public DeezerApiClient(HttpClient http)
        {
            _http = http;
        }


        public async Task<DeezerApiResponse> FindAlbumAsync(string title, string artist)
        {
            try 
            {
                var query = HttpUtility.UrlDecode($"{title} {artist}");
                var searchUrl = $"search/album?q={query}&limit=1";

                var response = await _http.GetAsync(searchUrl);

                if(!response.IsSuccessStatusCode)
                {

                    return new DeezerApiResponse
                    {
                        Status = MapStatus(response.StatusCode),
                        Album = null
                    };
                }

                var searchResults = await response.Content.ReadFromJsonAsync<DeezerSearchResult>();

                if (searchResults?.Data == null || searchResults.Data.Count == 0)
                {
                    return new DeezerApiResponse
                    {
                        Status = DeezerResultStatusEnum.NotFound,
                        Album = null
                    };
                }


            }
            catch
            {
            }           
        }

        private DeezerResultStatusEnum MapStatus(HttpStatusCode code)
        {
            return code switch
            {
                HttpStatusCode.NotFound => DeezerResultStatusEnum.NotFound,
                HttpStatusCode.InternalServerError => DeezerResultStatusEnum.ServerError,
                _ => DeezerResultStatusEnum.ServerError
            };
        }


    }
   
    }

