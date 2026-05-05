using Microsoft.AspNetCore.Mvc;
using RecordShop.Services;

namespace RecordShop.Controllers
{

    [ApiController]
    [Route("[Controller]")]
    public class MusicRecordController : Controller
    {
        private readonly IMusicRecordService _musicRecordService;

        public MusicRecordController(IMusicRecordService musicRecordService)
        {
            _musicRecordService = musicRecordService;
        }

        [Route("/records")]
        public IActionResult Index()
        {
            var getData = _musicRecordService.ServiceGetAllRecords():
            return Ok(getData);
        }

        [Route("/records/{id}")]

        public IActionResult getOneRecord(int id)
        {
            var OneRecord = _musicRecordService.ServiceGetOneRecord(id);
            return Ok(OneRecord);
        }
    }
}
