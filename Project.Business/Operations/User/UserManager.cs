using Project.Business.DataProtection;
using Project.Business.Operations.User.Dtos;
using Project.Business.Types;
using Project.Data.Entities;
using Project.Data.Enums;
using Project.Data.Repositories;
using Project.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Business.Operations.User
{
    public class UserManager : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IDataProtection _dataProtector;
        public UserManager(IUnitOfWork unitOfWork, IRepository<UserEntity> userRepository,IDataProtection protector)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _dataProtector = protector;
        }

        public async Task<ServiceMessage> AddAdmin(AddAdminDto admin)
        {
            var hasMail = _userRepository.GetAll(x => x.Email.ToLower() == admin.Email.ToLower());

            if (hasMail.Any())
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Email Mevcut, Farklı bir email deneyiniz"
                };
            }

            var userEntity = new UserEntity
            {
                Email = admin.Email,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Password = _dataProtector.Protect(admin.Password),
                PhoneNumber = admin.PhoneNumber,
                UserType = UserType.Admin
            };

            await _userRepository.Add(userEntity);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Admin kaydı sırasında bir hata oluştu. Lütfen Tekrar Deneyiniz");
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Admin başarıyla oluşturuldu"
            };
        }

        public async Task<ServiceMessage> AddUser(AddUserDto user)
        {
            var hasMail = _userRepository.GetAll(x => x.Email.ToLower() == user.Email.ToLower());

            if (hasMail.Any())
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Email Mevcut, Farklı bir email deneyiniz"
                };
            }

            var userEntity = new UserEntity
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = _dataProtector.Protect(user.Password),
                PhoneNumber = user.PhoneNumber,
                UserType = UserType.Customer
            };

           await _userRepository.Add(userEntity);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Kullanıcı kaydı sırasında bir hata oluştu. Lütfen Tekrar Deneyiniz.");
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Kullanıcı başarıyla oluşturuldu."
            };
        }

        public ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user)
        {
            var userEntity = _userRepository.Get(x => x.Email.ToLower() == user.Email.ToLower());
           

            if (userEntity is null)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "Kullanıcı adı veya şifre hatalı."
                };   
            }

            var unProtected = _dataProtector.UnProtect(userEntity.Password);

            if ((user.Password == unProtected))
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = true,
                    Data = new UserInfoDto
                    {
                        Email = userEntity.Email,
                        FirstName = userEntity.FirstName,
                        LastName = userEntity.LastName,
                        UserType = userEntity.UserType
                       
                    }
                };
            }
            else
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "Kullanıcı adı veya şifre hatalı."
                };
            }


        }
    }
}
