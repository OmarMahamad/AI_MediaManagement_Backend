using System.Threading.Tasks;
using HelperLayer.Constants;
using HelperLayer.Constants.Services;
using HelperLayer.File.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public UserController(IUsre usre,IFile file, MessageService message, IRepository<EmailVerificationToken> repoTokn, IRepository<User> repo)
        {
            _repo = repo;
            _repoTokn= repoTokn;
            _message= message;
            _user = usre;
            _file = file;
        }


        [HttpPost("RegisterUser")]
        public async Task<IActionResult> Register([FromForm] RegisterDto registerDto ,IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var filepath = _file.ProcessFileUser(file, "User");

            var reqister = await _user.RegisterUserAsync(registerDto,filepath);
            string mes = _message.GetMessage(MessageKeys.FoundEmail, Language.English);
            if (reqister ==mes)
            {
                _ = _file.DeleteImageAsync(filepath,"User");
                return BadRequest(mes);
            }
            mes = _message.GetMessage(MessageKeys.RegisterSuccess, Language.English);
            return Ok(mes);
        }


        [HttpPost("LoginUser")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token=await _user.LoginAsync(loginDto);



            return Ok(token);
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



    }
}
