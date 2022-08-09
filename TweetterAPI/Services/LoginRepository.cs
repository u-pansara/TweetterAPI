using TweetsAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using TweetsAPI.Data;

namespace TweetsAPI.Services
{
    public class LoginRepository : ILoginRepository
    {
        private readonly IMapper _mapper;
        private readonly TweetsDataContext _dataContext;
        private readonly IConfiguration _configuration;
        public LoginRepository(TweetsDataContext dataContext, IConfiguration configuration, IMapper mapper)
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<string>> Login(string userName, string password)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName.ToLower().Equals(userName.ToLower()));
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password";
            }
            else
            {
                response.Data = CreateToken(user);
            }
            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            if (await UserExists(user.UserName))
            {
                response.Success = false;
                response.Message = "User already exist";
                return response;
            }

            _dataContext.Users.Add(user);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _dataContext.SaveChangesAsync();

            response.Success = true;
            response.Message = "User created successfully";
            return response;
        }

        public async Task<ServiceResponse<string>> Register(string userName, string password)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                response.Success = false;
                response.Message = "Invalid user";
                return response;
            }

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _dataContext.Users.Add(user);

            await _dataContext.SaveChangesAsync();

            response.Success = true;
            return response;
        }

        public async Task<bool> UserExists(string userName)
        {
            if (await _dataContext.Users.AnyAsync(u => u.UserName.ToLower() == userName.ToLower()))
                return true;
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<ServiceResponse<List<UserCredential>>> GetAllUsers()
        {
            ServiceResponse<List<UserCredential>> response = new ServiceResponse<List<UserCredential>>();
            var users = await _dataContext.Users.ToListAsync();
            if (users == null)
            {
                response.Success = false;
                response.Message = "User is emapty.";
                return response;
            }
            else
            {
                response.Success = true;
                response.Data = _mapper.Map<List<UserCredential>>(users);
                return response;
            }

            response.Success = true;
            return response;
        }

        public async Task<ServiceResponse<UserCredential>> GetByUserName(string userName)
        {
            ServiceResponse<UserCredential> response = new ServiceResponse<UserCredential>();
            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }
            else
            {
                response.Success = true;
                response.Data = new UserCredential() { UserName = user.UserName, Email = user.Email };
                return response;
            }

            response.Success = true;
            return response;
        }

        public async Task<ServiceResponse<List<UserCredential>>> SearchByUserName(string userName)
        {
            ServiceResponse<List<UserCredential>> response = new ServiceResponse<List<UserCredential>>();
            var users = await _dataContext.Users.Where(u => u.UserName.Contains(userName)).ToListAsync();
            if (users.Any())
            {
                response.Success = true;
                response.Data = _mapper.Map<List<UserCredential>>(users);
                return response;
            }
            else
            {
                response.Success = false;
                response.Message = "User not found";
                return response;
            }

            response.Success = true;
            return response;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
