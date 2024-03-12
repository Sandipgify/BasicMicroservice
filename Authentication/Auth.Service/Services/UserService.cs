using Auth.Data.Entity;
using Auth.Data.Repository;
using Auth.Service.DTO;
using System.Security.Cryptography;

namespace Auth.Service.Services
{

    public interface IUserService
    {
        Task AddUserAsync(UserDTO user, string passWord);
        Task UpdateUserAsync(UserDTO userDTO);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task AddUserAsync(UserDTO user, string passWord)
        {
            ValidateUserInput(user, passWord);

            var existingUser = await _userRepository.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
            {
                throw new ArgumentException("User with the same email already exists.");
            }

            string salt = GenerateSalt();
            string hashedPassword = HashPassword(passWord, salt);

            await _userRepository.Add(new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PasswordHash = hashedPassword,
                Salt = salt,
                IsActive = true
            });


        }

        public async Task UpdateUserAsync(UserDTO userDTO)
        {
            ValidateUserInput(userDTO);

            var existingUser = await _userRepository.Get(userDTO.Id);
            if (existingUser == null)
            {
                throw new ArgumentException("User not found.");
            }
            existingUser.FirstName = userDTO.FirstName;
            existingUser.LastName = userDTO.LastName;
            existingUser.Email = userDTO.Email;
            _userRepository.Update(existingUser);
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.Get();

            IEnumerable<UserDTO> result = users.Select(x => new UserDTO { Id = x.Id, FirstName = x.FirstName, Email = x.Email, LastName = x.LastName });
            return result;
        }

        private string HashPassword(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password + salt);
        }

        private string GenerateSalt()
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();

            return salt;
        }

        private void ValidateUserInput(UserDTO user, string passWord = null)
        {
            if (user == null || string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName) || string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentException("User information is required.");
            }

            if (!string.IsNullOrEmpty(passWord) && passWord.Length < 8)
            {
                throw new ArgumentException("Password must be at least 8 characters long.");
            }
        }
    }
}
