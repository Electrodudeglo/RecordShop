using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using RecordShop.External;
using RecordShop.Model;
using RecordShop.Services;

namespace RecordShop.Controllers
{

    [ApiController]
    [Route("api/v1")]
    public class MusicRecordController : ControllerBase
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

        [HttpGet("records/{id}")]
        public IActionResult getOneRecord(int id)
        {
            var OneRecord = _musicRecordService.ServiceGetOneRecord(id);
            return Ok(OneRecord);
        }

        [HttpPost("records/check-deezer")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CheckDeezerApi(DeezerCheckRequest request)
        {
            var postToDeezer = await _musicRecordService.CheckDeezer(request);
            return Ok(postToDeezer);    
        }

        [HttpPost("records")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddOneRecord(MusicRecordModel musicRecordModel)
        {
            var postRecord = _musicRecordService.ServiceAddOneRecord(musicRecordModel);
            return CreatedAtAction(nameof(getOneRecord), new {id = postRecord.Id}, postRecord);
        }

        [HttpPut("records/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateOneRecord(MusicRecordModel musicRecord, int id)
        {
            var updateRecord = _musicRecordService.ServiceUpdateOneRecord(musicRecord,id);
            return CreatedAtAction(nameof(getOneRecord), new {id = updateRecord.Id}, updateRecord);
        }

        [HttpDelete("records/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteOneRecord(int id)
        {
            var deleted = _musicRecordService.ServiceDeleteOneRecord(id);
            if (!deleted)
            return NotFound(new { message = $"Record with id {id} not found" });
            return NoContent(); // 204
        }
    }
}
