using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainShared.ViewModels.Users
{
    public struct UsersListDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public bool IsActive { get; set; }

    }
}
