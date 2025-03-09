using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexus.Data;
using Nexus.Interfaces;
using Nexus.Models;
using Nexus.Services;

namespace Nexus.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ISystemUser _systemUser;
        private readonly HuggingFaceService _chatService;


        public ChatController(AppDbContext context, ISystemUser systemUser,HuggingFaceService  chat)
        {
            _dbContext = context;
            _systemUser = systemUser;
            _chatService = chat;
        }

        [HttpGet("start/{friendId}")]
        public async Task<IActionResult> StartChat(string friendId)
        {
            var currentUserId = _systemUser.Id.ToString();

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized("User not authenticated.");
            }

            var chatRoomId = GenerateChatRoomId(currentUserId, friendId);

            var existingChatRoom = await _dbContext.ChatRooms
                .FirstOrDefaultAsync(cr => cr.ChatRoomId == chatRoomId);

            if (existingChatRoom != null)
            {
                return Ok(new { ChatRoomId = chatRoomId });
            }

            var newChatRoom = new ChatRoomModel
            {
                ChatRoomId = chatRoomId,
                User1Id = currentUserId,
                User2Id = friendId,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.ChatRooms.Add(newChatRoom);
            await _dbContext.SaveChangesAsync();

            return Ok(new { ChatRoomId = chatRoomId });
        }

        private string GenerateChatRoomId(string userId1, string userId2)
        {
            var sortedIds = new[] { userId1, userId2 }.OrderBy(id => id).ToArray();
            return $"chat_{sortedIds[0]}_{sortedIds[1]}";
        }


        [HttpPost]
        public async Task<IActionResult> Chat([FromBody] AiRequestModel request)
        {
            var reply = await _chatService.GetAiResponse(request.Message);
            return Ok(new { reply });
        }
    }



}


