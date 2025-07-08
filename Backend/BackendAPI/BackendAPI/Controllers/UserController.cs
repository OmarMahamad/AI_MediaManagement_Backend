using System.Security.Claims;
using System.Threading.Tasks;
using AuthorizationLayer.Interface;
using HelperLayer.Constants;
using HelperLayer.Constants.Services;
using HelperLayer.File.Interface;
using MailKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RepositoryLayer.Entitys.AuthorizationEntity;
using RepositoryLayer.Entitys.UserEntity;
using RepositoryLayer.Interface;
using ServiceLayer.Dtos.userDtos;
using ServiceLayer.UserStatus.Interface;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUsre _user;
        private readonly IFile _file;
        private readonly MessageService _message;


        public UserController(IUsre usre, IFile file, MessageService message)
        {

            _message = message;
            _user = usre;
            _file = file;
        }

        [HttpDelete("RemoveUser")]
        public async Task<IActionResult> Delete()
        {
            var userIdClam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClam == null)
                return Unauthorized("Unauthorized access");
            if (!int.TryParse(userIdClam, out int userId))
                return BadRequest("Invalid user id");
            await _user.DeleteUserAsync(userId);
            return Ok("Removed user");
        }

        [HttpPut("EditUser")]
        public async Task<IActionResult> EditUser([FromBody]EditUserDto editUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var UserIdClam = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (UserIdClam == null)
            {
                return Unauthorized("Unauthorized access.");
            }

            if (!int.TryParse(UserIdClam, out int userId))
            {
                return BadRequest("Invalid user id.");
            }

            var resultEdit = await _user.EditUserAsync(userId, editUserDto);
            if (!resultEdit.IsSuccess)
                return BadRequest(resultEdit.Message);

            return Ok(resultEdit.Message);
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            if (id <= 0) return BadRequest("Invalid user id.");

            var userrespon = await _user.GetCurantUserByIDAsync(id);
            if (!userrespon.IsSuccess)
            {
                return NotFound();
            }
            return Ok(userrespon);
        }
        
        [HttpGet("GetCurrentUser")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
           var userClimId=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userClimId==null)
            {
                return Unauthorized("Unauthorized access .");
            }
            if (!int.TryParse(userClimId, out int userid))
            {
                return BadRequest("Invalid user id.");
            }

            var userrespon = await _user.GetCurantUserByIDAsync(userid);
            if (!userrespon.IsSuccess)
            {
                return NotFound();
            }
            userrespon.ImagePath = await _file.GetImgAsync(userrespon.ImagePath, "User");
            return Ok(userrespon);
        }


        [HttpPut("UpdateImageUser")]
        public async Task<IActionResult> UpdateImageUser(IFormFile file)
        {
            var UserClimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (UserClimId == null)
            {
                return Unauthorized("Unauthorized access.");
            }
            if (!int.TryParse(UserClimId,out int userid))
            {
                return BadRequest(_message.GetMessage(MessageKeys.Invaliduserid,Language.English));
            }
            if (file==null)
            {
                return BadRequest("Invalid input");
            }
            var userentity= await _user.GetUserByIDAsync(userid);
            await _file.DeleteImageAsync(userentity.ImagePath,"User");
            var newfilepath = _file.ProcessFileUser(file, "User");

            var respon=await _user.EditUserAsync(userid, newfilepath);
            if (!respon.IsSuccess)
            {
                return BadRequest(respon.Message);
            }
            return Ok(respon.Message);
            
        }

    }
}
