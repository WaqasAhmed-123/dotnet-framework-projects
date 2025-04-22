using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CodeCreator.Models;

namespace CodeCreator.DAL
{
    public class CodeDAL
    {
        public List<Code> GetAllCodesList(DatabaseEntities de)
        {
            return de.Codes.ToList();
        }

        public List<Code> GetActiveCodesList(DatabaseEntities de)
        {
            return de.Codes.Where(x => x.IsActive == 1).ToList();
        }

        public Code GetCodeById(int id, DatabaseEntities de)
        {
            return de.Codes.Where(x => x.Id == id).FirstOrDefault();
        }

        public Code GetActiveCodeById(int id, DatabaseEntities de)
        {
            return de.Codes.Where(x => x.Id == id).FirstOrDefault(x => x.IsActive == 1);
        }

        public bool AddCode(Code Code, DatabaseEntities de)
        {
            try
            {
                de.Codes.Add(Code);
                de.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }


        public int AddCode2(Code Code, DatabaseEntities de)
        {
            try
            {
                de.Codes.Add(Code);
                de.SaveChanges();

                return Code.Id;
            }
            catch
            {
                return -1;
            }
        }

        public bool UpdateCode(Code Code, DatabaseEntities de)
        {
            try
            {
                de.Entry(Code).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCode(int id, DatabaseEntities de)
        {
            try
            {
                de.Codes.Remove(de.Codes.Where(x => x.Id == id).FirstOrDefault());
                de.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}