using AutoMapper;
using HotelListing.Api.Core.Contracts;
using HotelListing.Api.Data;
using HotelListing.Api.Core.Models.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListing.Api.Core.Repository
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;

        private const string _loginProvider = "HotelListingApi";
        private const string _tokenPurpose = "RefreshToken";

        private ApiUser _user;

        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            this._mapper = mapper;
            this._userManager = userManager;
            this._configuration = configuration;
        }

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {

            _user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (_user == null)
            {
                return null;
            }
            bool isValidPassword = await _userManager.CheckPasswordAsync(_user, loginDto.Password);
            if (!isValidPassword )
            {
                return null;
            }
            return new AuthResponseDto()
            {
                UserId = _user.Id,
                Token = await GenerateToken(),
                RefreshToken = await CreateRefreshToken()
            };
        }

        async Task<IEnumerable<IdentityError>> IAuthManager.Register(ApiUserDto userDto)
        {
            _user = _mapper.Map<ApiUser>(userDto);
            _user.UserName = _user.Email;
            var result = await _userManager.CreateAsync(_user, userDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, "User");
            }
            return result.Errors;
        }
        public async Task<string> GenerateToken( )
        {
            string key = _configuration["JwtSettings:Key"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roles = await _userManager.GetRolesAsync(_user);
            var roleClaims = roles.Select(c => new Claim(ClaimTypes.Role, c)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(_user);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,_user.Email ),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString() ),
            new Claim(JwtRegisteredClaimNames.Email, _user.Email),
            new Claim("uid", _user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public async Task<string> CreateRefreshToken()
        {
            await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _tokenPurpose);
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user,_loginProvider, _tokenPurpose);
            var result = await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _tokenPurpose, newRefreshToken);
            return newRefreshToken;
         }

        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
            var userName = tokenContent.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)?.Value;
            _user = await _userManager.FindByEmailAsync(userName);
            if (_user == null || _user.Id != request.UserId)
            {
                return null;
            }
            var isValidaRefreshToken = await _userManager.VerifyUserTokenAsync(_user, _loginProvider, _tokenPurpose, request.RefreshToken);
            if (isValidaRefreshToken)
            {
                var token = await GenerateToken();
                return new AuthResponseDto
                {
                    Token = token,
                    UserId = _user.Id,
                    RefreshToken = await CreateRefreshToken()
                };
            }
            await _userManager.UpdateSecurityStampAsync(_user);
            return null;
         }
    }
}
