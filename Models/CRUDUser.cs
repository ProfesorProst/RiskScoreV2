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
            context.users.Add(user);
            context.SaveChanges();
        }

        public void Update(UserDB obj) { }

        public void Delet(UserDB obj) { }

        public UserDB Read(long id)
        {
            return context.users.Find(id);
        }

        public List<UserDB> GetObjects() 
        {
            return context.users.ToList();
        }
    }
}
