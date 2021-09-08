using BLL.DTO.Account;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Traiting2.Model;

namespace Traiting2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Client> _userManager;
        private readonly SignInManager<Client> _signInManager;
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailService;
        readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountController> _logger;
        private readonly GoogleSecret _googleSecret;
        public AccountController(UserManager<Client> userManager, SignInManager<Client> signInManager,
            RoleManager<IdentityRole> roleManager, AppSettings appSettings, IEmailService emailService,
            ILogger<AccountController> logger, GoogleSecret googleSecret)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _appSettings = appSettings;
            _emailService = emailService;
            _logger = logger;
            _googleSecret = googleSecret;
        }

        //[HttpGet]
        //[ProducesResponseType(typeof(string), 200)]
        //[ProducesResponseType(typeof(string), 400)]
        //[ProducesResponseType(typeof(string), 500)]
        //public async Task<IActionResult> ForgotPassword(string email)
        //{
        //    Employee employee = await _userManager.FindByEmailAsync(email);
        //    if (employee == null)
        //        return BadRequest("There is no user with this email");
        //    string jwt = CreateJWT(GetIdentity(new List<Claim>() {
        //        new Claim(ClaimsIdentity.DefaultNameClaimType, email)
        //        //new Claim(ClaimsIdentity.DefaultIssuer, catOwner.Email)
        //    }, "ConfirmMail"));
        //    _emailService.SendForgotPasswordEmail(email, "Continue to recover password", jwt);
        //    return StatusCode(200, "A message has been sent to you to proceed with password recovery ");
        //}

        /// <summary>
        /// Reset password
        /// </summary>
        //[HttpPost]
        //[ProducesResponseType(typeof(string), 200)]
        //[ProducesResponseType(typeof(IEnumerable<IdentityError>), 400)]
        ////[ProducesResponseType(typeof(IEnumerable<IdentityError>), 400)]
        //[ProducesResponseType(typeof(string), 500)]
        //public async Task<IActionResult> ForggotPassword(ChangeForgottenPasswordDTO changeForgottenPasswordDTO)
        //{
        //    ClaimsPrincipal claimsPrincipal;
        //    if (ValidationJWT(changeForgottenPasswordDTO.Token, out claimsPrincipal))
        //    {
        //        Employee employee = await _userManager.FindByEmailAsync(claimsPrincipal.Identity.Name);
        //        var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(employee);

        //        var result = await _userManager.ResetPasswordAsync(employee, passwordResetToken,
        //            changeForgottenPasswordDTO.NewPassword);
        //        if (result.Succeeded)
        //            return StatusCode(200, "Password change succsess");
        //        else
        //            return BadRequest(result.Errors);
        //    }
        //    return BadRequest("Invalid token");
        //}

        [HttpPost("GoogleLogin")]
        public IActionResult GoogleLogin(string code)
        {
            //this.ControllerContext.HttpContext.Request.;
            return Ok();
        }

        [HttpGet("RedirectToGoogleLogin")]
        public IActionResult RedirectToGoogleLogin()
        {
            string url = $"https://accounts.google.com/o/oauth2/v2/auth?client_id={_googleSecret.client_id}" +
                $"&redirect_uri={_googleSecret.redirect_uris.First()}" +
                $"&response_type=code&scope=https://www.googleapis.com/auth/drive.metadata.readonly&include_granted_scopes=true&access_type=offline" +
                $"&include_granted_scopes=true";
            return RedirectToRoute(url);
        }

        [HttpPost("ChangePassword")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(IEnumerable<IdentityError>), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            Client client = await _userManager.FindByNameAsync(changePasswordDTO.UserName);
            var result = await _userManager.ChangePasswordAsync(client, changePasswordDTO.CurrentPassword, changePasswordDTO.NewPassword);
            if (result.Succeeded)
                return StatusCode(200);
            else
                return BadRequest(result.Errors);
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(LoginResponseModel), 200)]
        //[ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 500)]
        public async Task<IActionResult> Login(LoginModelDTO model)
        {
            Client user = await _userManager.FindByNameAsync(model.UserName);
            if (await _userManager.CheckPasswordAsync(
                user, model.Password))
            {
                if (!user.EmailConfirmed)
                    return BadRequest("Please, confirm e-mail");
                IList<string> userRole = await _userManager.GetRolesAsync(user);
                LoginResponseModel loginResponseModel = new LoginResponseModel()
                {
                    AccessToken = CreateJWT(GetIdentity(new List<Claim>() {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole.First())
                    })),
                    UserName = model.UserName
                };
                return Ok(loginResponseModel);
            }
            else
            {
                return this.BadRequest("Wrong password or userName ");
            }
        }

        //[HttpPost]
        //[ProducesResponseType(typeof(LoginResponseModel), 200)]
        ////[ProducesResponseType(typeof(ValidationProblemDetails), 400)]
        //[ProducesResponseType(typeof(string), 400)]
        //[ProducesResponseType(typeof(string), 500)]
        //public async Task<IActionResult> LoginWithGoogle(string googleToken)
        //{
        //    GoogleUserInfoModel googleUserInfoModel = await GetUserInfoFromGoogleTokenAsync(googleToken);
        //    Employee employee = await _userManager.FindByEmailAsync(googleUserInfoModel.Email);
        //    if (employee != null)
        //    {
        //        IList<string> employeeRole = await _userManager.GetRolesAsync(employee);
        //        if (employeeRole.Any(i => i.ToLower() == AccountRole.Trainee.ToLower()))
        //            return BadRequest("While you are a trainee you cannot log in");
        //        LoginResponseModel loginResponseModel = new LoginResponseModel()
        //        {
        //            AccessToken = CreateJWT(GetIdentity(new List<Claim>() {
        //                new Claim(ClaimsIdentity.DefaultNameClaimType, employee.Id),
        //                new Claim(ClaimsIdentity.DefaultRoleClaimType, employeeRole.First())
        //            })),
        //            UserName = employee.UserName
        //        };
        //        return Json(loginResponseModel);
        //    }
        //    else
        //    {
        //        return this.BadRequest("Wrong googleToken");
        //    }
        //}

        //private async Task<GoogleUserInfoModel> GetUserInfoFromGoogleTokenAsync(string googleToken)
        //{
        //    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(
        //        "https://www.googleapis.com/oauth2/v3/tokeninfo?id_token=" + googleToken);
        //    HttpWebResponse resp = (HttpWebResponse)await req.GetResponseAsync();
        //    return await JsonSerializer.DeserializeAsync<GoogleUserInfoModel>(resp.GetResponseStream());
        //}

        private bool ValidationJWT(string token, out ClaimsPrincipal claimsPrincipal)
        {
            claimsPrincipal = null;
            var mySecurityKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(_appSettings.Secret));
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _appSettings.Issuer,
                    ValidAudience = _appSettings.Audience,
                    IssuerSigningKey = mySecurityKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private string CreateJWT(ClaimsIdentity claimsIdentity)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: _appSettings.Issuer,
                audience: _appSettings.Audience,
                notBefore: now,
                claims: claimsIdentity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(_appSettings.Lifetime)),
                signingCredentials: new SigningCredentials(new
                SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(_appSettings.Secret)),
                SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private ClaimsIdentity GetIdentity(string username)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, username)
                };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
        private ClaimsIdentity GetIdentity(List<Claim> claims, string authenticationType = "Token")
        {
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, authenticationType, ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

    }
}
