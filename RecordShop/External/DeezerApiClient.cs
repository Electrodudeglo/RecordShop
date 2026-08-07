using System.Net;
using System.Web;

namespace RecordShop.External
{
    public interface IDeezerApiClient
    {
        Task<DeezerAlbumResult> FindAlbumAsync(string albumName, string artistName);
    }

    public class DeezerApiClient : IDeezerApiClient
    {
        private readonly HttpClient _http;
      

        public DeezerApiClient(HttpClient http)
        {
            _http = http;
            _http.BaseAddress = new Uri("https://api.deezer.com/");
        }

        public async Task<DeezerAlbumResult> FindAlbumAsync(string albumName,string artistName)
        {
            try 
            {
                var query = HttpUtility.UrlEncode($"{albumName} {artistName}");
                var searchUrl = $"search/album?q={query}&limit=1";

                //fuzzy search section
                var response = await _http.GetAsync(searchUrl);

                if(!response.IsSuccessStatusCode)
                {

                    return new DeezerAlbumResult
                    {
                        ResultStatus = MapStatus(response.StatusCode),
                        Album = null
                    };
                }

                var searchResults = await response.Content.ReadFromJsonAsync<DeezerSearchResult>();

                if (searchResults?.Data == null || searchResults.Data.Count == 0)
                {
                    return new DeezerAlbumResult
                    {
                        ResultStatus = DeezerResultStatusEnum.NotFound,
                        Album = null
                    };
                }

                //actual album search
                var albumId = searchResults.Data[0].Id;
                var albumResponse = await _http.GetAsync($"album/{albumId}");

                if(!albumResponse.IsSuccessStatusCode)
                {
                    return new DeezerAlbumResult
                    {
                        ResultStatus = MapStatus(albumResponse.StatusCode),
                        Album = null
                    };
                }

                var albumDetails = await albumResponse.Content.ReadFromJsonAsync<DeezerAlbumDetails>();

                return new DeezerAlbumResult
                {
                    ResultStatus = DeezerResultStatusEnum.Success,
                    Album = albumDetails
                };

            }
            catch(HttpRequestException)
            {
                return new DeezerAlbumResult
                {
                    ResultStatus = DeezerResultStatusEnum.NetworkError,
                    Album = null
                };
            }

            catch (Exception)
            {
                return new DeezerAlbumResult
                {
                    ResultStatus = DeezerResultStatusEnum.InvalidJson,
                    Album = null
                };
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

