using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CkEditor.Models;

namespace CkEditor.DAL
{
    public class PageDAL
    {

        public Page GetPage(string title, DatabaseEntities de)
        {
            return de.Pages.Where(x=> x.Title == title).FirstOrDefault();
        }
        
        public Page GetPageById(int id, DatabaseEntities de)
        {
            return de.Pages.Where(x=> x.Id == id).FirstOrDefault();
        }

        public bool AddPage(Page page, DatabaseEntities de)
        {
            try
            {
                de.Pages.Add(page);
                de.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdatePage(Page page, DatabaseEntities de)
        {
            try
            {
                de.Entry(page).State = System.Data.Entity.EntityState.Modified;
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