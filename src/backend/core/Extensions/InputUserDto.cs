using core.Models.Dtos;
using core.Models;

namespace core.Extensions
{
    public static class InputUserDtoExtensions
    {
        public static UserModel ConvertToUserModel(this InputUserDto inputLoginUserDto)
            => new()
            {
                Email = inputLoginUserDto.Email,
                //Username = inputLoginUserDto.Username,
                Password = inputLoginUserDto.Password,
                //PhoneNumber = inputLoginUserDto.PhoneNumber
            };
    }
}
