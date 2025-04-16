using ForumBE.DTOs.Attachments;
using ForumBE.Response;
using ForumBE.Services.Implementations;
using ForumBE.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForumBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttachmentController : ControllerBase
    {
        private readonly IAttachmentService _attachmenService;
        public AttachmentController(IAttachmentService attachmenService)
        {
            _attachmenService = attachmenService;
        }

        [HttpGet]
        public async Task<ResponseBase> GetAllAttachment()
        {
            var attachment = await _attachmenService.GetAllAttachmentAsync();
            return ResponseBase.Success(attachment);
        }

        [HttpGet("{id}")]
        public async Task<ResponseBase> GetAttachmentById(int id)
        {
            var attachment = await _attachmenService.GetAttachmentByIdAsync(id);
            return ResponseBase.Success(attachment);
        }
        [HttpPost]
        public async Task<ResponseBase> CreateAttachment([FromForm] AttachmentCreateRequestDto input)
        {
            var isCreated = await _attachmenService.CreateAttachmentsAsync(input);

            if (!isCreated)
            {
                return ResponseBase.Fail("Create attachment failed");
            }

            return ResponseBase.Success("Create attachment failed");
        }
        [HttpDelete("{id}")]
        public async Task<ResponseBase> DeleteAttachment(int id)
        {
            var isDeleted = await _attachmenService.DeleteAttachmentAsync(id);
            if (!isDeleted)
            {
                return ResponseBase.Fail("Delete attachment failed");
            }
            return ResponseBase.Success("Delete attachment success");
        }
    }
}
