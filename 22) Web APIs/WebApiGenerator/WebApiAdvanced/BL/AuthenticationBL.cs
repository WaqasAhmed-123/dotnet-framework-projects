using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiAdvanced.Models;
using WebApiAdvanced.DAL;

namespace WebApiAdvanced.BL
{
    public class AuthenticationBL
    {

        public List<Authentication> GetAuthentications()
        {
            return new AuthenticationDAL().GetAuthentications();
        }

        public Authentication GetAuthenticationByUserId(int _Id)
        {
            return new AuthenticationDAL().GetAuthenticationByUserId(_Id);
        }

        public Authentication GetAuthenticationById(int _Id)
        {
            return new AuthenticationDAL().GetAuthenticationById(_Id);
        }

        public bool AddAuthentication(Authentication _auth, DatabaseEntities de = null)
        {
            return new AuthenticationDAL().AddAuthentication(_auth, de);
        }

        public bool DeleteAuthentication(int _Id)
        {
            return new AuthenticationDAL().DeleteAuthentication(_Id);
        }
    }
}