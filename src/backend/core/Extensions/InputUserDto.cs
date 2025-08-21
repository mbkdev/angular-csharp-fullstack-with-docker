using core.Models.Dtos;
using core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Extensions
{
    public static class InputUserDtoExtensions
    {
        public static UserModel ConvertToUserModel(this InputUserDto inputLoginUserDto)
    => new UserModel
    {
        Email = inputLoginUserDto.Email,
        //Username = inputLoginUserDto.Username,
        Password = inputLoginUserDto.Password,
        //PhoneNumber = inputLoginUserDto.PhoneNumber
    };
    }
}
