using Lesson3A.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.Xml;

namespace Lesson3A.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
                return Redirect("Login");
            else
            {
                var model = new
                {
                    username = HttpContext.Session.GetString("Username"),
                    fullname = HttpContext.Session.GetString("Fullname")
                };
                return View(model);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult get_course(int Page, int Size, string Group)
        {
            var data = getCourse(Page, Size, Group);
            if (data != null)
            {
                var res = new
                {
                    Success = true,
                    Message = "",
                    Data = data
                };
                return Json(res);
            }
            else
            {
                var res = new
                {
                    Success = false,
                    Message = "Loi xay ra!!!"
                };
                return Json(res);
            }
        }
        ///
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult update_course(Course course)
        {
            var c = updateCourse(course);
            if (c != null)
            {
                var res = new
                {
                    Success = true,
                    Message = "",
                    Data = c
                };
                return Json(res);
            }
            else
            {
                var res = new
                {
                    Success = false,
                    Message = "ERROR",
                };
                return Json(res);
            }
        }

        public IActionResult delete_course(int id)
        {
            var c =deleteCourse(id);
            if (c != null)
            {
                var res = new
                {
                    Success = c,
                    Message = "",
                };
                return Json(res);
            }
            else
            {
                var res = new
                {
                    Success = false,
                    Message = "ERROR",
                };
                return Json(res);
            }
        }


        private object? getCourse(int page, int size, string grp)
        {

            try
            {
                var db = new LtwebContext();
                var ls = db.Courses.Where(x => x.Group == grp);
                var offset = (page - 1) * size;
                var totalRecord = ls.Count();
                var totalPage = (totalRecord % size) == 0 ?
                    (int)(totalRecord / size) :
                    (int)(totalRecord / size + 1);
                var lst = ls.Skip(offset).Take(size).ToList();
                return new
                {
                    Data = lst,
                    TotalRecord = totalRecord,
                    TotalPage = totalPage,
                    Page = page,
                    Size = size,
                };

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IActionResult insert_course(Course course)
        {
            var c = insertCourse(course);
            if (c != null)
            {
                var res = new
                {
                    Success = true,
                    Message = "",
                    Data = c
                };
                return Json(res);
            }
            else
            {
                var res = new
                {
                    Success = false,
                    Message = "ERROR",
                };
                return Json(res);
            }
        }

        private object? updateCourse(Course c)
        {
            try
            {
                if (c == null)
                    return null;
                var db = new LtwebContext();
                var c1 = db.Courses.Where(x => x.Id == c.Id).FirstOrDefault();
                if (c1.Group != c.Group)
                    c1.Group = c.Group;
                if (c1.Major != c.Major)
                    c1.Major = c.Major;
                if (c1.Note != c.Note)
                    c1.Note = c.Note;
                if (c1.CourseName != c.CourseName)
                    c1.CourseName = c.CourseName;
                if (c1.SubCode != c.SubCode)
                    c1.SubCode = c.SubCode;
                if (c1.Credit != c.Credit)
                    c1.Credit = c.Credit;
                db.Courses.Update(c1);
                db.SaveChanges();
                return c1;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private object? insertCourse(Course c)
        {
            try
            {
                if (c == null)
                    return null;
                var db = new LtwebContext();
                var c1 = new Course();
                if (c1.Group != c.Group)
                    c1.Group = c.Group;
                if (c1.Major != c.Major)
                    c1.Major = c.Major;
                if (c1.Note != c.Note)
                    c1.Note = c.Note;
                if (c1.CourseName != c.CourseName)
                    c1.CourseName = c.CourseName;
                if (c1.SubCode != c.SubCode)
                    c1.SubCode = c.SubCode;
                if (c1.Credit != c.Credit)
                    c1.Credit = c.Credit;
                db.Courses.Add(c1);
                db.SaveChanges();
                return c1;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private Boolean? deleteCourse(int? id)
        {
            try
            {
                if (id == null)
                    return null;
                var db = new LtwebContext();
                var c1 = db.Courses.Where(x => x.Id == id).FirstOrDefault();
                if (c1 != null)
                {
                    db.Courses.Remove(c1);
                    db.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
       
    }
}

