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
                return Ok(ApiResponse<ReadMediaDetailDTO>.SuccessResponse(
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
                    return NotFound(ApiResponse<ReadMediaDetailDTO>.FailureResponse(
                        "Media plan not found."
                    ));
                }

                return Ok(ApiResponse<ReadMediaDetailDTO>.SuccessResponse(
                    mediaDto,
                    "Media retrieved successfully."
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReadMediaDetailDTO>.FailureResponse(
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
                return Ok(ApiResponse<IEnumerable<ReadMediaDetailDTO>>.SuccessResponse(
                    media,
                    "List of media retrieved successfully."
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<ReadMediaDetailDTO>>.FailureResponse(
                    "An error occurred while retrieving the list of media."
                ));
            }
        }

        [HttpPut("{mediaId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Update(int mediaId, [FromBody] UpdateMediaDTO updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<ReadMediaDetailDTO>.FailureResponse(
                    "Invalid data.",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                ));
            }

            try
            {
                var updatedDto = await _mediaService.UpdateMediaAsync(mediaId, updateDto);
                if (updatedDto == null)
                {
                    return NotFound(ApiResponse<ReadMediaDetailDTO>.FailureResponse(
                        "Media not found or missing ID."
                    ));
                }

                return Ok(ApiResponse<ReadMediaDetailDTO>.SuccessResponse(
                    updatedDto,
                    "Media updated successfully."
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ReadMediaDetailDTO>.FailureResponse(
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
