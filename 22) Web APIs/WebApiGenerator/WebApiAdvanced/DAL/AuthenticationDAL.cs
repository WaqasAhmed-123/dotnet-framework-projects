using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiAdvanced.Models;

namespace WebApiAdvanced.DAL
{
    public class AuthenticationDAL
    {

        public List<Authentication> GetAuthentications()
        {
            DatabaseEntities db = new DatabaseEntities();

            return db.Authentications.Where(x => x.IsActive == 1).ToList();
        }

        public Authentication GetAuthenticationByUserId(int _Id)
        {
            DatabaseEntities db = new DatabaseEntities();
            return db.Authentications.Where(x => x.UserId == _Id).FirstOrDefault();
        }

        public Authentication GetAuthenticationById(int _Id)
        {
            DatabaseEntities db = new DatabaseEntities();
            return db.Authentications.Where(x => x.Id == _Id).FirstOrDefault();
        }

        public bool AddAuthentication(Authentication _auth, DatabaseEntities de = null)
        {
            try
            {
                if (de != null)
                {
                    de.Authentications.Add(_auth);
                    de.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteAuthentication(int _Id)
        {
            try
            {
                DatabaseEntities db = new DatabaseEntities();
                db.Authentications.Remove(db.Authentications.FirstOrDefault(x => x.Id == _Id));
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}