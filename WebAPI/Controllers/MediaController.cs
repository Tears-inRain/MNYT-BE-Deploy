using Microsoft.AspNetCore.Mvc;
using Application.ViewModels.Media;
using Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Application.Services.IServices;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;

        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateMediaDTO mediaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<CreateMediaDTO>.FailureResponse(
                    "Invalid data.",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                ));
            }

            try
            {
                var createdDto = await _mediaService.CreateMediaAsync(mediaDto);
                return Ok(ApiResponse<ReadMediaDTO>.SuccessResponse(
                    createdDto,
                    "Media created successfully."
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<CreateMediaDTO>.FailureResponse(
                    "An error occurred while creating the media."
                ));
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var mediaDto = await _mediaService.GetMediaByIdAsync(id);
                if (mediaDto == null)
                {
                    return NotFound(ApiResponse<ReadMediaDTO>.FailureResponse(
                        "Media plan not found."
                    ));
                }

                return Ok(ApiResponse<ReadMediaDTO>.SuccessResponse(
                    mediaDto,
                    "Media retrieved successfully."
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReadMediaDTO>.FailureResponse(
                    "An error occurred while retrieving the media."
                ));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var media = await _mediaService.GetAllMediaAsync();
                return Ok(ApiResponse<IEnumerable<ReadMediaDTO>>.SuccessResponse(
                    media,
                    "List of media retrieved successfully."
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<ReadMediaDTO>>.FailureResponse(
                    "An error occurred while retrieving the list of media."
                ));
            }
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromBody] ReadMediaDTO mediaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<ReadMediaDTO>.FailureResponse(
                    "Invalid data.",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                ));
            }

            try
            {
                var updatedDto = await _mediaService.UpdateMediaAsync(mediaDto);
                if (updatedDto == null)
                {
                    return NotFound(ApiResponse<ReadMediaDTO>.FailureResponse(
                        "Media not found or missing ID."
                    ));
                }

                return Ok(ApiResponse<ReadMediaDTO>.SuccessResponse(
                    updatedDto,
                    "Media updated successfully."
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReadMediaDTO>.FailureResponse(
                    "An error occurred while updating the media."
                ));
            }
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var isDeleted = await _mediaService.DeleteMediaAsync(id);
                if (!isDeleted)
                {
                    return NotFound(ApiResponse<bool>.FailureResponse(
                        "Media not found."
                    ));
                }

                return Ok(ApiResponse<bool>.SuccessResponse(
                    true,
                    "Media deleted successfully."
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.FailureResponse(
                    "An error occurred while deleting the media."
                ));
            }
        }
    }
}
