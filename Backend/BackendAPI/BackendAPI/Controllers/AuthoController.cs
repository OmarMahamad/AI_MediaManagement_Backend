using System.Security.Claims;
using HelperLayer.Constants;
using HelperLayer.Constants.Services;
using HelperLayer.File.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entitys.AuthorizationEntity;
using RepositoryLayer.Entitys.UserEntity;
using RepositoryLayer.Interface;
using ServiceLayer.AuthorizationStatus.Interface;
using ServiceLayer.Dtos.userDtos;
using ServiceLayer.UserStatus.Interface;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthoController : ControllerBase
    {
        private readonly IUsre _user;
        private readonly IFile _file;
        private readonly MessageService _message;
        private readonly IRepository<User> _repo;
        private readonly IRepository<EmailVerificationToken> _repoTokn;
        private readonly IAuthStatus _authStatus;
        public AuthoController(IAuthStatus authStatus,IUsre usre, IFile file, MessageService message, IRepository<EmailVerificationToken> repoTokn, IRepository<User> repo)
        {
            _authStatus= authStatus;
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

            var reqister = await _authStatus.RegisterUserAsync(registerDto, filepath);
            if (reqister.IsSuccess)
            {
                _ = _file.DeleteImageAsync(filepath, "User");
                return BadRequest(reqister.Message);
            }
            return Ok(reqister.Message);
        }


        [HttpPost("LoginUser")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authStatus.LoginAsync(loginDto);

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
            var authorizationHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                return Unauthorized("Token is missing or invalid");

            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            var logoutResult = await _authStatus.LogoutAsync(token);
            if (!logoutResult.IsSuccess)
                return BadRequest(logoutResult.Message);

            return Ok(logoutResult.Message);
        }


        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassord(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email is required.");

            var respon = await _authStatus.ForgotPasswordAsync(email);
            if (!respon.IsSuccess)
                return BadRequest(respon.Message);
            return Ok(respon.Message);
        }

        [HttpGet("CheckOTP")]
        public async Task<IActionResult> CheckOTP(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return NotFound(_message.GetMessage(MessageKeys.Invaliduserid, Language.English));
            }
            var respon = await _authStatus.CheckOtpCode(code);
            if (!respon.IsSuccess)
            {
                return BadRequest(respon.Message);
            }
            return Ok(respon.Message);
        }

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassordDto changePassord)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var respone = await _authStatus.ChangePasswordAsync(changePassord);
            if (!respone.IsSuccess)
            {
                return BadRequest(respone.Message);
            }
            return Ok(respone.Message);
        }
    }
}
