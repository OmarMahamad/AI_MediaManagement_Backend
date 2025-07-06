using System.Security.Claims;
using System.Threading.Tasks;
using HelperLayer.Constants;
using HelperLayer.Constants.Services;
using HelperLayer.File.Interface;
using MailKit;
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
        private readonly IRepository<User> _repo;
        private readonly IRepository<EmailVerificationToken> _repoTokn;

        public UserController(IUsre usre, IFile file, MessageService message, IRepository<EmailVerificationToken> repoTokn, IRepository<User> repo)
        {
            _repo = repo;
            _repoTokn = repoTokn;
            _message = message;
            _user = usre;
            _file = file;
        }


        [HttpPost("RegisterUser")]
        public async Task<IActionResult> Register([FromForm] RegisterDto registerDto, IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var filepath = _file.ProcessFileUser(file, "User");

            var reqister = await _user.RegisterUserAsync(registerDto, filepath);
            string mes = _message.GetMessage(MessageKeys.FoundEmail, Language.English);
            if (reqister == mes)
            {
                _ = _file.DeleteImageAsync(filepath, "User");
                return BadRequest(mes);
            }
            mes = _message.GetMessage(MessageKeys.RegisterSuccess, Language.English);
            return Ok(mes);
        }


        [HttpPost("LoginUser")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _user.LoginAsync(loginDto);

            if (!response.IsSuccess)
                return BadRequest(response.Message);

            return Ok(response);
        }

        [HttpGet("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            var tokenEntity = await _repoTokn.FirstOrderAsync(t => t.Token == token);
            if (tokenEntity == null || tokenEntity.IsUsed || tokenEntity.ExpiryDate < DateTime.UtcNow)
                return BadRequest("Invalid or expired token");

            var user = await _repo.GetItemAsync(tokenEntity.UserId);
            if (user == null)
                return BadRequest("User not found");

            user.IsEmailVerified = true;
            await _repo.EditItemAsync(user.UserId, user);

            tokenEntity.IsUsed = true;
            await _repoTokn.EditItemAsync(tokenEntity.Id, tokenEntity);

            return Ok("Email verified successfully");
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized("Unauthorized access.");
            }

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return BadRequest("Invalid user id.");
            }

            var logoutResult = await _user.LogoutAsync(userId);
            if (!logoutResult.IsSuccess)
                return BadRequest(logoutResult.Message);

            return Ok(logoutResult.Message);
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

            var userrespon = await _user.GetUserByIDAsync(id);
            if (!userrespon.IsSuccess)
            {
                return NotFound();
            }
            return Ok(userrespon);
        }
        
        [HttpGet("GetCurrentUser")]
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

            var userrespon = await _user.GetUserByIDAsync(userid);
            if (!userrespon.IsSuccess)
            {
                return NotFound();
            }
            userrespon.ImagePath = await _file.GetImgAsync(userrespon.ImagePath, "User");
            return Ok(userrespon);
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassord(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email is required.");

            var respon =await _user.ForgotPasswordAsync(email);
            if (!respon.IsSuccess)
                return BadRequest(respon.Message);
            return Ok(respon.Message);
        }

        [HttpGet("CheckOTP")]
        public async Task<IActionResult> CheckOTP(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return NotFound(_message.GetMessage(MessageKeys.Invaliduserid,Language.English));
            }
            var respon = await _user.CheckOtpCode(code);
            if (!respon.IsSuccess)
            {
                return BadRequest(respon.Message);
            }
            return Ok(respon.Message);
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePassordDto changePassord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var respone=await _user.ChangePasswordAsync(changePassord);
            if (!respone.IsSuccess)
            {
                return BadRequest(respone.Message);
            }
            return Ok(respone.Message);
        }

        //[HttpGet("GetAllUser")]
        //public IActionResult GetAllUser()
        //{

        //}

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
