using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Infrastructure.Users;
using API.Infrastructure.Classes;
using API.Infrastructure.Helpers;
using API.Infrastructure.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace API.Infrastructure.Auth {

    [Route("api/[controller]")]
    public class AuthController : ControllerBase {

        #region variables

        private readonly AppDbContext context;
        private readonly TokenSettings settings;
        private readonly UserManager<UserExtended> userManager;

        #endregion

        public AuthController(AppDbContext context, IOptions<TokenSettings> settings, UserManager<UserExtended> userManager) {
            this.context = context;
            this.settings = settings.Value;
            this.userManager = userManager;
        }

        [HttpPost("[action]")]
        public async Task<Login> Auth([FromBody] TokenRequest model) {
            return model.GrantType == "password"
                ? await Login(model)
                : await RefreshToken(model);
        }

        [HttpPost("[action]")]
        public IActionResult Logout([FromBody] JObject z) {
            var tokens = context.Tokens.Where(x => x.UserId == z.First.FirstOrDefault().ToString()).ToList();
            // context.Tokens.RemoveRange(tokens);
            // context.SaveChanges();
            return StatusCode(200, new {
                response = ApiMessages.OK()
            });
        }

        private async Task<Login> Login(TokenRequest model) {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user?.IsActive == true && await userManager.IsEmailConfirmedAsync(user) && await userManager.CheckPasswordAsync(user, model.Password)) {
                var newRefreshToken = CreateRefreshToken(settings.ClientId, user.Id);
                context.Tokens.Add(newRefreshToken);
                var response = await CreateToken(user, newRefreshToken.Value);
                await context.SaveChangesAsync();
                return new Login {
                    UserId = user.Id,
                    IsAdmin = user.IsAdmin,
                    Displayname = user.Displayname,
                    IsFirstFieldFocused = user.IsFirstFieldFocused,
                    Token = response.Token,
                    RefreshToken = response.RefreshToken,
                    Expiration = response.Expiration,
                    DotNetVersion = GetNetVersion()
                };
            } else {
                throw new CustomException() {
                    ResponseCode = 401
                };
            }
        }

        private async Task<Login> RefreshToken(TokenRequest model) {
            var existingToken = context.Tokens.FirstOrDefault(t => t.ClientId == settings.ClientId && t.Value == model.RefreshToken);
            if (existingToken == null) AuthenticationFailed();
            var user = await userManager.FindByIdAsync(existingToken.UserId);
            if (user == null) AuthenticationFailed();
            var newToken = CreateRefreshToken(existingToken.ClientId, existingToken.UserId);
            context.Tokens.Add(newToken);
            context.Tokens.Remove(existingToken);
            context.SaveChanges();
            var token = await CreateToken(user, newToken.Value);
            return new Login {
                UserId = user.Id,
                IsAdmin = user.IsAdmin,
                Displayname = user.Displayname,
                IsFirstFieldFocused = user.IsFirstFieldFocused,
                Token = token.Token,
                RefreshToken = token.RefreshToken,
                Expiration = token.Expiration,
                DotNetVersion = GetNetVersion()
            };
        }

        private Token CreateRefreshToken(string clientId, string userId) {
            return new Token() {
                ClientId = clientId,
                UserId = userId,
                Value = Guid.NewGuid().ToString("N"),
                CreatedDate = DateTime.UtcNow,
                ExpiryTime = DateTime.UtcNow.AddMinutes(Convert.ToDouble(settings.ExpireTime))
            };
        }

        private async Task<TokenResponse> CreateToken(UserExtended user, string refreshToken) {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.Secret));
            var roles = await userManager.GetRolesAsync(user);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new Claim[] {
                    new(ClaimTypes.NameIdentifier, user.Id),
                    new(ClaimTypes.Role, roles.FirstOrDefault()),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(JwtRegisteredClaimNames.Sub, user.UserName),
                    new("LoggedOn", DateTime.UtcNow.ToString())
                }),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Issuer = settings.Site,
                Audience = settings.Audience,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(settings.ExpireTime))
            };
            var newtoken = tokenHandler.CreateToken(tokenDescriptor);
            var encodedToken = tokenHandler.WriteToken(newtoken);
            var response = new TokenResponse() {
                Token = encodedToken,
                RefreshToken = refreshToken,
                Expiration = newtoken.ValidTo,
            };
            return response;
        }

        private static void AuthenticationFailed() {
            throw new CustomException() {
                ResponseCode = 401
            };
        }

        private static string GetNetVersion() {
            return System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
        }

    }

}