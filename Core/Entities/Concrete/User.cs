using System;
using Core.Entities;

namespace Core.Entities.Concrete
{
    public class UserForRegisterDto: IEntity
    {
        public int Id { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string Password { get; set; }
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }
        public bool Status { get; set; }
    }
}
