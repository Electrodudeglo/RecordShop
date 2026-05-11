using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using RecordShop.Model;
using RecordShop.Services;

namespace RecordShop.Controllers
{

    [ApiController]
    [Route("api/v1")]
    public class MusicRecordController : Controller
    {
        private readonly IMusicRecordService _musicRecordService;

        public MusicRecordController(IMusicRecordService musicRecordService)
        {
            _musicRecordService = musicRecordService;
        }

        [HttpGet("records")]
        public IActionResult Index()
        {
            var getData = _musicRecordService.ServiceGetAllRecords();
            return Ok(getData);
        }

        [Route("records/{id}")]
        public IActionResult getOneRecord(int id)
        {
            var OneRecord = _musicRecordService.ServiceGetOneRecord(id);
            return Ok(OneRecord);
        }

        [HttpPost("records")]
        public IActionResult AddOneRecord(MusicRecordModel musicRecordModel)
        {
            var postRecord = _musicRecordService.ServiceAddOneRecord(musicRecordModel);
            return CreatedAtAction(nameof(getOneRecord), new {id = postRecord.Id}, postRecord);
        }

        [HttpPut("records/{id}")]

        public IActionResult UpdateOneRecord(MusicRecordModel musicRecord, ,int id)
        {

            var updateRecord = _musicRecordService.ServiceUpdateOneRecord(musicRecord,id);

            return Ok(updateRecord);

        }

        [HttpDelete("records/{id}")]
        public IActionResult DeleteOneRecord(int id)
        {
            var deleted = _musicRecordService.ServiceDeleteOneRecord(id);

            if (!deleted)
                return NotFound(new { message = $"Record with id {id} not found" });

            return NoContent(); // 204
        }

    }
}
