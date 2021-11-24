using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;

namespace DataAccess.Concrete
{
    public class UserDal : EntityRepositoryBase<UserForRegisterDto, AdminSql>, IUserDal
    {
        public List<OperationClaim> GetClaims(Entity.Dtos.UserForRegisterDto user)
        {
            try
            {
                using (var context = new AdminSql())
                {
                    var result = from operationClaim in context.OperationClaims
                                 join userOperationClaim in context.UserOperationClaims
                                 on operationClaim.Id equals userOperationClaim.OperationClaimId
                                 where userOperationClaim.UserId == user.Id
                                 select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };
                    return result.ToList();
                }
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        public List<OperationClaim> GetOperationClaims()
        {
            using (AdminSql context = new AdminSql())
            {
                return context.Set<OperationClaim>().ToList();
            }
        }
    }
}
