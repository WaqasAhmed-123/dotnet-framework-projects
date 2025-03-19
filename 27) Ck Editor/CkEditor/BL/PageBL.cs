using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CkEditor.Models;
using CkEditor.DAL;

namespace CkEditor.BL
{
    public class PageBL
    {
        public Page GetPageById(int id, DatabaseEntities de)
        {
            return new PageDAL().GetPageById(id, de);
        }

        public Page GetPage(string title, DatabaseEntities de)
        {
            return new PageDAL().GetPage(title, de);
        }

        public bool AddPage(Page page, DatabaseEntities de)
        {
            return new PageDAL().AddPage(page, de);
        }

        public bool UpdatePage(Page page, DatabaseEntities de)
        {
            return new PageDAL().UpdatePage(page, de);
        }

    }
}