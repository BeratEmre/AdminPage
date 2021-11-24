using Core.Entities;

namespace Entity.Dtos
{
    public class UserForRegisterDto : IDto
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
