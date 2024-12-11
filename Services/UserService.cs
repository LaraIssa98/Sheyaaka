using Models;
using Services;
using Services.Repository.Interfaces;
using Sheyaaka.HelperClasses;
using Sheyaaka.Repositories;
using System.Threading.Tasks;

namespace Sheyaaka.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TokenHelper _tokenHelper;
        private readonly EmailService _emailService;
        private readonly CacheService _cacheService;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, TokenHelper tokenHelper, EmailService emailService, CacheService cacheService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _tokenHelper = tokenHelper;
            _emailService = emailService;
            _cacheService = cacheService;
        }

        public async Task<string> RegisterUser(User user)
        {
            if (user == null) return "User cannot be null.";

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            user.IsEmailConfirmed = false;
            _userRepository.Add(user);
            _unitOfWork.Save();
            _cacheService.Remove("AllUsers");
            var token = _tokenHelper.GenerateEmailConfirmationToken(user.UserID);
            var confirmationLink = $"https://localhost:7296/api/user/confirm-email?token={token}";

            var emailBody = $"Click <a href='{confirmationLink}'>here</a> to confirm your email.";

            await _emailService.SendEmailAsync(user.Email, "Email Confirmation", emailBody);

            return "Registration successful! Please check your email for confirmation.";
        }

        public string ConfirmEmail(string token)
        {
            var userId = _tokenHelper.ValidateToken(token);
            if (userId == null) return "Invalid or expired token.";

            var user = _userRepository.Get(u => u.UserID == userId);
            if (user == null) return "User not found.";

            user.IsEmailConfirmed = true;
            _unitOfWork.Save();
            _cacheService.Remove($"User_{userId}");
            return "Email confirmed successfully!";
        }

        public string ChangePassword(ChangePasswordRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.CurrentPassword) ||
                string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return "Invalid input.";
            }

            var user = _userRepository.Get(u => u.Email == request.Email);
            if (user == null || !PasswordHelper.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            {
                return "Invalid email or password.";
            }

            user.PasswordHash = PasswordHelper.HashPassword(request.NewPassword);
            _userRepository.update(user);
            _unitOfWork.Save();
            _cacheService.Remove($"User_{user.UserID}");
            return "Password changed successfully.";
        }

        public string UpdateProfile(int id, User updatedUser)
        {
            if (updatedUser == null || id != updatedUser.UserID)
            {
                return "Invalid user data.";
            }

            var user = _userRepository.Get(u => u.UserID == id);
            if (user == null)
            {
                return "User not found.";
            }

            _userRepository.update(user);
            _unitOfWork.Save();
            _cacheService.Remove($"User_{user.UserID}");
            return "Profile updated successfully.";
        }

        public User GetUser(int id)
        {
            // Cache user data by ID
            string cacheKey = $"User_{id}";
            return _cacheService.GetOrAdd(cacheKey, () =>
            {
                return _userRepository.Get(u => u.UserID == id);
            }, TimeSpan.FromMinutes(10));
        }

        public IEnumerable<User> GetAllUsers()
        {
            // Cache all users
            return _cacheService.GetOrAdd("AllUsers", () =>
            {
                return _userRepository.GetAll();
            }, TimeSpan.FromMinutes(10));
        }
    }
}
