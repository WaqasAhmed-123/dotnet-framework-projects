using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcelReadingAdvanced.Models;
using ExcelReadingAdvanced.BL;
using ExcelReadingAdvanced.Helping_Classes;
using System.Data.SqlClient;
using System.Data;

namespace ExcelReadingAdvanced.Controllers
{
    public class TypeController : Controller
    {
        DatabaseEntities de = new DatabaseEntities();

        public void CreateMultipleUser()
        {
            var cs = "Server=LocalDB;Database=Database;Trusted_Connection=True;";

            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("exec Add_Users @UserType", con);

                var pList = new SqlParameter("@UserType", SqlDbType.Structured);
                pList.TypeName = "dbo.UserTableType";
                pList.Value = GetAddUserList();
                cmd.Parameters.Add(pList);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public List<User> GetAddUserList()
        {
            List<User> ulist = new List<User>();

            for(int i = 1000; i<=1500; i++)
            {
                User obj = new User()
                {
                    Name = "User" + i,
                    Contact = "contact" + i,
                    IsActive = 1,
                    CreatedAt = DateTime.Now
                };

                ulist.Add(obj);
            }

            return ulist;
        }

        
    }
}