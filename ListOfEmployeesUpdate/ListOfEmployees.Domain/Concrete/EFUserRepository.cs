using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListOfEmployees.Domain.Entities;
using ListOfEmployees.Domain.Interface;

namespace ListOfEmployees.Domain.Concrete
{
    public class EFUserRepository: IUserRepository
    {
        private EFDbContext context = new EFDbContext();
        public bool ValidateUser(string login, string password)
        {
            bool isValid = false;

            try
            {
                User user = context.Users.
                    FirstOrDefault(x => x.Login == login && x.Password == password);
                     
                    if (user != null)
                    {
                        isValid = true;
                    }
                }
                catch
                {
                    isValid = false;
                }
            
            return isValid;
        }
    }
}
