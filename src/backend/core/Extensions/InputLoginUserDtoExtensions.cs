using core.Models;
using core.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Extensions
{
    public static class InputLoginUserDtoExtensions
    {
        public static UserModel ConvertToUserModel(this InputLoginUserDto inputLoginUserDto)
            => new UserModel
            {
                Email = inputLoginUserDto.Email,
                Username = inputLoginUserDto.Username,
                Password = inputLoginUserDto.Password,
                //PhoneNumber = inputLoginUserDto.PhoneNumber
            };

    }
}
