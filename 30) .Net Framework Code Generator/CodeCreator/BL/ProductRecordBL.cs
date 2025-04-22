using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeCreator.Models;
using CodeCreator.DAL;

namespace CodeCreator.BL
{
    public class CodeBL
    {
        public List<Code> GetAllCodesList(DatabaseEntities de)
        {
            return new CodeDAL().GetAllCodesList(de);
        }

        public List<Code> GetActiveCodesList(DatabaseEntities de)
        {
            return new CodeDAL().GetActiveCodesList(de);
        }

        public Code GetCodeById(int id, DatabaseEntities de)
        {
            return new CodeDAL().GetCodeById(id, de);
        }

        public Code GetActiveCodeById(int id, DatabaseEntities de)
        {
            return new CodeDAL().GetActiveCodeById(id, de);
        }

        public bool AddCode(Code Code, DatabaseEntities de)
        {
            return new CodeDAL().AddCode(Code, de);
        }


        public int AddCode2(Code Code, DatabaseEntities de)
        {
            return new CodeDAL().AddCode2(Code, de);
        }

        public bool UpdateCode(Code Code, DatabaseEntities de)
        {
            return new CodeDAL().UpdateCode(Code, de);
        }

        public bool DeleteCode(int id, DatabaseEntities de)
        {
            return new CodeDAL().DeleteCode(id, de);
        }

    }
}