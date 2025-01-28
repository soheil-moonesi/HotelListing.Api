using HotelListing.Api.Data;
using HotelListing.Api.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.Api.Contracts
{
    public interface IAuthManager
    {  
        Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto);

        Task<AuthResponseDto> Login(LoginDto loginDto);

        Task<string> CreateRefreshToken(ApiUser user);

        Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request);

    }
}
