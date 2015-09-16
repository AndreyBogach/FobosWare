using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListOfEmployees.Domain.Interface
{
    public interface IUserRepository
    {
        bool ValidateUser(string login, string password);
    }
}
