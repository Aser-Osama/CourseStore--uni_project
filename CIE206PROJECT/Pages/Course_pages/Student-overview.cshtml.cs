using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Xml.Linq;
using CIE206PROJECT.Controllers;
using CIE206PROJECT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;


namespace CIE206PROJECT.Pages.Course_pages
{
    public class Student_overviewModel : PageModel
    {
        [BindProperty]
        public Request request { get; set; }
		[BindProperty(SupportsGet =true)]
		public int id_in {set; get; }
		public DataTable? Stats { get; set; }
		public DataTable? UserInfo{ get; set; }
		public DataTable? AdditionalUserInfo{ get; set; }
		public DataTable? PhoneNumbers{ get; set; }
		public DataTable? StudentAttendance{ get; set; }
		public CoursePage _DB { get; set; }
		private readonly DB_Container _DBC;
        private readonly LoginController _LC;
		private readonly ILogger<Student_ProfileModel> _logger;

        public Student_overviewModel(ILogger<Student_ProfileModel> logger, DB_Container container,LoginController Controller_LG){
            _logger = logger;
			_DBC = container;
            _LC=Controller_LG;
        }

        public void OnGet(int id)
        {	
            _DB = _DBC.coursePage_DB;
            if (id > 0) { 
            TempData["ID"] = id;
            }
            else
            {
				TempData["ID"] = id_in;

			}
			UserInfo =_DB.getUserInfo(id);
            StudentAttendance=_DB.getStudentAttendance(id);
            AdditionalUserInfo=_DB.getStudentInfo(id);
            Stats=_DB.getStudentEvaluations(id);
            PhoneNumbers=_DB.getUserPhonenumbers(id);
        }
        public IActionResult OnPost()
        {
			if (ModelState.IsValid)
			{
				_DB.CreateNote(request,_LC.GetLoggedInUserId(),(int)TempData["ID"]);
                return RedirectToPage("/Course_pages/Student-overview", new { id = (int)TempData["ID"] });

			}
			else
			{
				return RedirectToPage("/Course_pages/Student-overview", new { id= (int)TempData["ID"] });
			}

		}

    }
}
