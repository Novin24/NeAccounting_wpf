﻿using Domain.BaseDomain.User;
using NeApplication.Common;

namespace NeApplication.IBaseRepositories
{
    public interface IIdentityUserManager : IBaseRepository<IdentityUser>
    {
        Task<bool> LogInUser(string userName, string password);
        Task<IdentityUser> GetUser(string userName);
    }
}