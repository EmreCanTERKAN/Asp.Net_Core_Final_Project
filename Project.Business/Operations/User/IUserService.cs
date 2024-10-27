using Project.Business.Operations.User.Dtos;
using Project.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Business.Operations.User
{
    public interface IUserService
    {
        Task<ServiceMessage> AddUser(AddUserDto user);
        Task<ServiceMessage> AddAdmin(AddAdminDto admin);
        ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user);
    }
}
