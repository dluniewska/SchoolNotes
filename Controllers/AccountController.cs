//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using School.DTO;
//using School.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace School.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AccountController : ControllerBase
//    {
//        private readonly IAccountService _accountService;
//        private readonly ILogger<FilesController> _logger;

//        public AccountController(IAccountService accountService, ILogger<FilesController> logger)
//        {
//            _accountService = accountService;
//            _logger = logger;
//        }

//        [HttpPost("register")]
//        public ActionResult RegisterUser([FromForm] RegisterUserDto dto)
//        {
//            _accountService.RegisterUser(dto);
//            return Ok();
//        }

//        [HttpPost("login")]
//        public ActionResult Login([FromForm] LoginDto dto)
//        {
//            try
//            {
//                string token = _accountService.GenerateJwt(dto);
//                return Ok(token);
//            } catch (Exception e)
//            {
//                _logger.LogError(e.Message);
//                return BadRequest(e.ToString());
//            }
//        }
        
//        //[HttpPost("logout")]
//        //[ValidateAntiForgeryToken]
//        //public async Task<IActionResult> Logout()
//        //{
//        //}
//    }
//}
