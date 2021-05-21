using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RookieOnlineAssetManagement.Models;
using RookieOnlineAssetManagement.Repositories;
using RookieOnlineAssetManagement.Utils;

namespace RookieOnlineAssetManagement.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<bool> DisableUserAsync(string id)
        {
            return await _userRepo.DisableUserAsync(id);
        }

        public async Task<(ICollection<UserModel> Datas, int TotalPage)> GetListUserAsync(UserRequestParmas userRequestParmas)
        {
            return await _userRepo.GetListUserAsync(userRequestParmas);
        }

        public async Task<UserModel> UpdateUserAsync(string id, UserRequestModel userRequest)
        {
            return await _userRepo.UpdateUserAsync(id, userRequest);
        }

        public async Task<string> GetDefaultPassword(string id)
        {
            var user = await GetUserByIdAsync(id);
            return AccountHelper.GenerateAccountPass(user.UserName, user.DateOfBirth);
        }

        public Task<UserModel> CreateUserAsync(UserRequestModel userRequest)
        {
            DayOfWeek dayofweek;
            var checkage = CheckDateAgeGreaterThan(18, userRequest.DateOfBirth.Value);
            var checkjoineddate = CheckIsSaturdayOrSunday(userRequest.JoinedDate.Value, out dayofweek);
            var checkjoineddategreaterthanbirthofdate = CheckDateGreaterThan(userRequest.DateOfBirth.Value, userRequest.JoinedDate.Value);
            if (checkage == false)
            {
                throw new Exception("Age is not valid");
            }
            if (checkjoineddate == true)
            {
                throw new Exception("Joined Date is : " + dayofweek.ToString());
            }
            if (checkjoineddategreaterthanbirthofdate == false)
            {
                throw new Exception("Joined Date is smaller tham Birth Of Date");
            }
            return _userRepo.CreateUserAsync(userRequest);
        }
        public Task<UserDetailModel> GetUserByIdAsync(string id)
        {
            return _userRepo.GetUserByIdAsync(id);
        }
        public bool CheckDateGreaterThan(DateTime SmallDate, DateTime BigDate)
        {
            // if (SmallDate > BigDate)
            // {
            //     return false;
            // }
            // else
            // {
            //     return true;
            // }
            return !(SmallDate > BigDate);
        }
        public bool CheckDateAgeGreaterThan(int age, DateTime BirthOfDate)
        {
            // if (DateTime.Now.Year - BirthOfDate.Year >= 18)
            // {
            //     return true;
            // }
            // else
            // {
            //     return false;
            // }
            return (DateTime.Now.Year - BirthOfDate.Year >= 18);
        }
        public bool CheckIsSaturdayOrSunday(DateTime JoinedDate, out DayOfWeek dayofweek)
        {
            dayofweek = JoinedDate.DayOfWeek;
            // if (dayofweek == DayOfWeek.Sunday || dayofweek == DayOfWeek.Sunday)
            // {
            //     return true;
            // }
            // else
            // {
            //     return false;
            // }
            return (dayofweek == DayOfWeek.Sunday || dayofweek == DayOfWeek.Sunday);
        }


    }
}