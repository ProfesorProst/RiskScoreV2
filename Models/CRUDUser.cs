using DependencyCheck.Data;
using DependencyCheck.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RiskScore.Models
{
    class CRUDUser
    {
        Context context;

        public CRUDUser()
        {
            context = new Context();
        }

        public void Create(UserDB user)
        {
            context = new Context();
            context.userDBs.Add(user);
            try
            {
                context.SaveChanges();
            }
            catch (Exception e) { }
        }

        public void Update(UserDB obj) { }

        public void Delet(UserDB obj) { }

        public UserDB Read(long id)
        {
            return context.userDBs.Find(id);
        }

        public List<UserDB> GetObjects() 
        {
            return context.userDBs.ToList();
        }
    }
}
