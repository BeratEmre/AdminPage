using Core.DataAccess;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IUserDal: IEntityRepository<UserForRegisterDto>
    {
        List<OperationClaim> GetClaims(Entity.Dtos.UserForRegisterDto user);
        List<OperationClaim> GetOperationClaims();
    }
}
