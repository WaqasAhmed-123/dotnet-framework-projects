using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using System;
using System.Drawing;
using System.IO;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.BL;
using WebApplication1.Helping_Classes;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseEntities de = new DatabaseEntities();
        private readonly GeneralPurpose gp = new GeneralPurpose();

        public ActionResult Index(string msg="", string color="")
        {
            ViewBag.Message = msg;
            ViewBag.Color = color;
            
            return View();
        }


        #region Manage User
        public ActionResult AddUser(string msg = "", string color = "black")
        {
            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }

        [HttpPost]
        public ActionResult PostAddUser(User _user)
        {

            bool checkEmail = gp.ValidateEmail(_user.Email);

            if (!checkEmail)
            {
                return RedirectToAction("AddUser", "Home", new { msg = "Email already exist, Please try another", color = "red" });
            }

            User obj = new User()
            {
                FirstName = _user.FirstName.Trim(),
                LastName = _user.LastName.Trim(),
                Dob = _user.Dob,
                Contact = _user.Contact,
                Address = _user.Address,
                Email = _user.Email.Trim(),
                Gender = _user.Gender,
                Role = _user.Role,
                IsActive = 1,
                CreatedAt = GeneralPurpose.DateTimeNow()
            };


            if (!new UserBL().AddUser(obj, de))
            {
                return RedirectToAction("AddUser", "Home", new { msg = "Somethings' wrong", color = "red"});
            }
                
            return RedirectToAction("AddUser", "Home", new { msg = "User inserted successfully", color = "green" });
        }


        public ActionResult ViewUser(string msg = "", string color = "black")
        {
            ViewBag.Message = msg;
            ViewBag.Color = color;

            return View();
        }

        [HttpPost]
        public ActionResult PostUpdateUser(User _user)
        {
            bool checkEmail = gp.ValidateEmail(_user.Email, _user.Id);
            if (!checkEmail)
            {
                return RedirectToAction("ViewUser", "Admin", new { msg = "Email already exist, Please try another", color = "red"});
            }

            User u = new UserBL().GetActiveUserById(_user.Id, de);
            u.FirstName = _user.FirstName.Trim();
            u.LastName = _user.LastName.Trim();
            u.Dob = _user.Dob;
            u.Contact = _user.Contact;
            u.Address = _user.Address;
            u.Email = _user.Email.Trim();
            u.Gender = _user.Gender;
            u.Role = _user.Role;
                
            if (!new UserBL().UpdateUser(u, de))
            {
                return RedirectToAction("ViewUser", "Home", new { msg = "Somethings' wrong", color = "red"});
            }
                
            return RedirectToAction("ViewUser", "Home", new { msg = "User updated successfully", color = "green"});
        }


        public ActionResult DeleteUser(int id)
        {
            if (!new UserBL().ArchiveUser(id, de))
            {
                return RedirectToAction("ViewUser", "Home", new { msg = "Somethings' wrong", color = "red"});
            }

            return RedirectToAction("ViewUser", "Home", new { msg = "Record deleted successfully", color = "green" });
        }

        #endregion


        public ActionResult GeneratePdf(int id)
        {
            User u = new UserBL().GetActiveUserById(id, de);

            if (u == null)
            {
                return RedirectToAction("ViewUser", "Admin", new { msg = "Record not Found", color = "red" });
            }

            PdfDocument document = new PdfDocument();

            //Add a new page to the PDF document.
            PdfPage page = document.Pages.Add();

            //Create PDF graphics for the page.
            PdfGraphics graphics = page.Graphics;

            //Set the standard font.
            PdfFont headingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 20, PdfFontStyle.Bold | PdfFontStyle.Underline);
            PdfFont textFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 10, PdfFontStyle.Bold);
            PdfFont textFont2 = new PdfStandardFont(PdfFontFamily.TimesRoman, 8);

            //Draw the Heading.
            graphics.DrawString("User Details", headingFont, PdfBrushes.Black, new PointF(200, 0));


            //First Row
            graphics.DrawString("First Name;", textFont, PdfBrushes.Black, new PointF(10, 50));
            PdfTextBoxField textBoxField = new PdfTextBoxField(page, "FirstName");
            textBoxField.Bounds = new RectangleF(10, (float)61.5, 200, 20);
            textBoxField.Text = u.FirstName;
            textBoxField.Required = true;
            textBoxField.ToolTip = "First Name";

            document.Form.Fields.Add(textBoxField);


            graphics.DrawString("Last Name;", textFont, PdfBrushes.Black, new PointF(240, 50));
            textBoxField = new PdfTextBoxField(page, "LastName");
            textBoxField.Bounds = new RectangleF(240, (float)61.5, 200, 20);
            textBoxField.Text = u.LastName;
            textBoxField.Required = true;
            textBoxField.ToolTip = "Last Name";

            document.Form.Fields.Add(textBoxField);


            //Second Row
            graphics.DrawString("D.O.B;", textFont, PdfBrushes.Black, new PointF(10, 100));
            textBoxField = new PdfTextBoxField(page, "DOB");
            textBoxField.Bounds = new RectangleF(10, (float)111.5, 200, 20);
            textBoxField.Text = u.Dob.Value.ToString("MM/dd/yyyy");
            textBoxField.Required = true;
            textBoxField.ToolTip = "Date of Birth";
            textBoxField.Actions.KeyPressed = new PdfJavaScriptAction("AFDate_KeystrokeEx(\"m/d/yyyy\")");
            textBoxField.Actions.Format = new PdfJavaScriptAction("AFDate_FormatEx(\"m/d/yyyy\")");
            document.Form.Fields.Add(textBoxField);


            graphics.DrawString("Contact;", textFont, PdfBrushes.Black, new PointF(240, 100));
            textBoxField = new PdfTextBoxField(page, "Contact");
            textBoxField.Bounds = new RectangleF(240, (float)111.5, 200, 20);
            textBoxField.Text = u.Contact;
            textBoxField.Required = true;
            textBoxField.ToolTip = "Contact Number";

            document.Form.Fields.Add(textBoxField);


            //third row
            graphics.DrawString("Address;", textFont, PdfBrushes.Black, new PointF(10, 150));
            textBoxField = new PdfTextBoxField(page, "Address");
            textBoxField.Bounds = new RectangleF(10, (float)161.5, 430, 40);
            textBoxField.Text = u.Address;
            textBoxField.Required = true;
            textBoxField.ToolTip = "Address";

            document.Form.Fields.Add(textBoxField);


            //Fourth row
            graphics.DrawString("Email;", textFont, PdfBrushes.Black, new PointF(10, 220));
            textBoxField = new PdfTextBoxField(page, "Address");
            textBoxField.Bounds = new RectangleF(10, (float)231.5, 430, 20);
            textBoxField.Text = u.Email;
            textBoxField.Required = true;
            textBoxField.ToolTip = "Email Address";

            document.Form.Fields.Add(textBoxField);


            //Fifth row
            graphics.DrawString("Role;", textFont, PdfBrushes.Black, new PointF(10, 270));
            PdfComboBoxField comboBoxField = new PdfComboBoxField(page, "JobTitle");
            comboBoxField.Bounds = new RectangleF(10, (float)281.5, 430, 20);
            comboBoxField.Required = true;
            comboBoxField.ToolTip = "User Role";
            comboBoxField.Items.Add(new PdfListFieldItem("Admin", "1"));
            comboBoxField.Items.Add(new PdfListFieldItem("Manager", "2"));
            comboBoxField.Items.Add(new PdfListFieldItem("Employee", "3"));
            comboBoxField.SelectedIndex = (int)u.Role-1;
            
            document.Form.Fields.Add(comboBoxField);


            //Sixth row
            graphics.DrawString("Gender;", textFont, PdfBrushes.Black, new PointF(10, 320));
            PdfRadioButtonListField genderField = new PdfRadioButtonListField(page, "Gender");
            
            document.Form.Fields.Add(genderField);

            graphics.DrawString("Male", textFont2, PdfBrushes.Black, new PointF(10, 335));
            PdfRadioButtonListItem radioButtonItem1 = new PdfRadioButtonListItem("Male");
            radioButtonItem1.Bounds = new RectangleF(32, 335, 10, 10);
            radioButtonItem1.ToolTip = "Male";
            genderField.Items.Add(radioButtonItem1);

            graphics.DrawString("Female", textFont2, PdfBrushes.Black, new PointF(55, 335));
            PdfRadioButtonListItem radioButtonItem2 = new PdfRadioButtonListItem("Female");
            radioButtonItem2.Bounds = new RectangleF(82, 335, 10, 10);
            radioButtonItem2.ToolTip = "Female";
            genderField.Items.Add(radioButtonItem2);

            if(u.Gender == "Male")
            {
                genderField.SelectedIndex = 0;
            }
            else
            {
                genderField.SelectedIndex = 1;
            }


            MemoryStream stream = new MemoryStream();

            //Save the document as stream
            document.Save(stream);
            stream.Position = 0;

            document.Close(true);

            string contentType = "application/pdf";

            string fileName = u.FirstName + GeneralPurpose.DateTimeNow().Ticks + ".pdf";

            return File(stream, contentType, fileName);
        }


        public ActionResult Test()
        {
            PdfDocument document = new PdfDocument();

            //Add a new page to the PDF document.
            PdfPage page = document.Pages.Add();

            //Create PDF graphics for the page.
            PdfGraphics graphics = page.Graphics;

            //Set the standard font.
            PdfFont fontBold = new PdfStandardFont(PdfFontFamily.Helvetica, 20, PdfFontStyle.Bold | PdfFontStyle.Underline);

            //Draw the text.
            graphics.DrawString("User Details", fontBold, PdfBrushes.Black, new PointF(200, 0));



            #region Input Date
            PdfTextBoxField field = new PdfTextBoxField(page, "datePick");
            //Set the text box properties
            field.Bounds = new RectangleF(10, 10, 100, 20);
            field.Text = "10/14/22";
            field.Actions.KeyPressed = new PdfJavaScriptAction("AFDate_KeystrokeEx(\"m/d/yyyy\")");
            field.Actions.Format = new PdfJavaScriptAction("AFDate_FormatEx(\"m/d/yyyy\")");
            //Add field to the form
            document.Form.Fields.Add(field);
            #endregion

            #region Input Text
            //Create a Input Text field and add the properties.
            PdfTextBoxField textBoxField = new PdfTextBoxField(page, "FirstName");

            textBoxField.Bounds = new RectangleF(0, 0, 100, 20);//position and dimensions of input field
            textBoxField.Text = "First Name";
            textBoxField.Required = true;
            textBoxField.ToolTip = "First Name";

            document.Form.Fields.Add(textBoxField);
            #endregion


            #region Dropdown Input Field
            //Creating the dropdown input field
            PdfComboBoxField comboBoxField = new PdfComboBoxField(page, "JobTitle");
            comboBoxField.Bounds = new RectangleF(0, 40, 100, 20);

            //Set tooltip.
            comboBoxField.ToolTip = "Job Title";

            //Add list items.
            comboBoxField.Items.Add(new PdfListFieldItem("Development", "accounts"));
            comboBoxField.Items.Add(new PdfListFieldItem("Support", "advertise"));
            comboBoxField.Items.Add(new PdfListFieldItem("Documentation", "content"));
            comboBoxField.SelectedIndex = 1;
            //Add combo box to the form.
            document.Form.Fields.Add(comboBoxField);
            #endregion


            #region radio Input Field
            //Create a Radio button.
            PdfRadioButtonListField employeesRadioList = new PdfRadioButtonListField(page, "employeesRadioList");

            //Add the radio button into form
            document.Form.Fields.Add(employeesRadioList);

            //Create radio button items.

            PdfRadioButtonListItem radioButtonItem1 = new PdfRadioButtonListItem("Male");
            radioButtonItem1.Bounds = new RectangleF(0, 80, 20, 20);
            PdfRadioButtonListItem radioButtonItem2 = new PdfRadioButtonListItem("Female");

            radioButtonItem2.Bounds = new RectangleF(0, 120, 20, 20);

            //Add the items to radio button group.
            employeesRadioList.Items.Add(radioButtonItem1);
            employeesRadioList.Items.Add(radioButtonItem2);
            #endregion


            #region Multi select Nav list
            //Create list box.
            PdfListBoxField listBoxField = new PdfListBoxField(page, "list1");

            //Set the properties.
            listBoxField.Bounds = new RectangleF(0, 160, 100, 40);

            //Add the items to the list box.
            listBoxField.Items.Add(new PdfListFieldItem("English", "English"));

            listBoxField.Items.Add(new PdfListFieldItem("French", "French"));

            listBoxField.Items.Add(new PdfListFieldItem("German", "German"));

            //Select the multiple item.
            listBoxField.SelectedIndex = 1;

            //Set the multi select option.
            listBoxField.MultiSelect = true;

            //Add the list box into PDF document
            document.Form.Fields.Add(listBoxField);
            #endregion


            #region Input Checkbox
            //Create Check Box field.
            PdfCheckBoxField checkBoxField = new PdfCheckBoxField(page, "CheckBox");

            //Set check box properties.
            checkBoxField.ToolTip = "Check Box";
            checkBoxField.Bounds = new RectangleF(0, 220, 10, 10);
            checkBoxField.Checked = true;

            //Add the form field to the document.
            document.Form.Fields.Add(checkBoxField);
            #endregion


            #region Action Button with Js Alter
            //Create a new PdfButtonField
            PdfButtonField submitButton = new PdfButtonField(page, "submitButton");

            //setting button properties
            submitButton.Bounds = new RectangleF(0, 290, 90, 20);
            submitButton.Text = "Apply";
            submitButton.BackColor = new PdfColor(181, 191, 203);

            //Create a new PdfJavaScriptAction
            PdfJavaScriptAction scriptAction = new PdfJavaScriptAction("app.alert(\"You are looking at Form field action of PDF \")");

            //Set the scriptAction to submitButton
            submitButton.Actions.MouseDown = scriptAction;

            //Add the submit button to the new document.
            document.Form.Fields.Add(submitButton);
            #endregion


            string pdfName = "Form" + DateTime.Now.Ticks.ToString() + ".pdf";

            string path = Server.MapPath("~/Content/PdfFiles/");


            document.Save(path + pdfName);

            //close the document
            document.Close(true);

            return View();


            #region Direct download pdf

            //MemoryStream stream = new MemoryStream();

            ////Save the document as stream

            //document.Save(stream);

            ////If the position is not set to '0', then the PDF will be empty

            //stream.Position = 0;

            ////Close the document

            //document.Close(true);

            ////Defining the ContentType for PDF file.

            //string contentType = "application/pdf";

            ////Define the file name

            //string fileName = "Output.pdf";

            ////Creates a FileContentResult object by using the file contents, content type, and file name

            //return File(stream, contentType, fileName);
            #endregion
        }
    }
}