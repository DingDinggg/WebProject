using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using Lesson3A.Models;
using System;

namespace Lesson3A.Controllers
{
    public class LoginController : Controller
    {
        private string _key;

        public LoginController()
        {
            _key = "E546C8DF278CD5931069B522E695D4F2";
        }
        public IActionResult Index()
        {
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            {
                var model = GetProvinces();
              //  var p = new Province();
             //   p.Id = 0;
                return View(model);
            }
            else
            {
                return Redirect("Home");
            }
        }

        [HttpPost]
        public IActionResult Register(IFormCollection f)
        {
            User u = new User();
            u.Fullname = f["txtFullName"].ToString();
            u.Username = f["txtUserName1"].ToString();
            u.Password = f["txtPassword1"].ToString();
            u.City = f["txtCity"].ToString();
            u.State = f["selState"].ToString();
            u.Zip = f["txtZip"].ToString();
            var obj = CreateUser(u);

            return View(obj);
        }

        [HttpPost]
        public IActionResult doLogin(LoginData login)
        {
            LoginResponnse res =new LoginResponnse();
            if (login != null)
            {
               Users? usr = checkLogin(login);
                if(usr != null)
                {
                 //   var passHash = EncryptString(login.Password, _key);
                    var decryptedPass = DecryptString(usr.Password, _key);
                    if(decryptedPass == login.Password)
                    {
                        res.Message = "Dang nhap thanh cong!";
                        res.Success = true;
                        res.User = usr;
                        HttpContext.Session.SetString("Username", usr.Username);
                        HttpContext.Session.SetString("Fullname", usr.Fullname);
                    }
                }
                else
                {
                    res.Message = "Sai Mat Khau!";
                    res.Success = false;
                    res.User=null;
                }
            }
            else
            {
                res.Message = "Khong co du lieu!";
                res.Success = false;
            }
            return Json(res);
        }
        public IActionResult signout()
        {
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("Fullname");
            LoginResponnse res = new LoginResponnse();
            res.Message = "";
            res.Success = true;
            return Json(res);
        }

        public Users? checkLogin(LoginData login)
        {
            Users? usr = new Users(); 
            if(login != null)
            {
                string cnStr = "Server =LAPTOP-ARHAGM1K;Database = LTWeb;User id=vinh;password =123; ";
                SqlConnection cnn = new SqlConnection(cnStr);
                try
                {
                    cnn.Open();
                    SqlCommand cmd = cnn.CreateCommand();
                    cmd.Connection = cnn;

                    string sql = "select * from Users";
                    sql += " where Username ='" + login.Username + "'";
                  //  sql += " and Password ='" + login.Password + "'";
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read()) { 
                        usr.id = int.Parse(reader["id"].ToString());
                        usr.Username=reader["username"].ToString();
                        usr.Password=reader["password"].ToString();
                        if (reader["LastLogin"] != null && reader["LastLogin"].ToString() != "")
                            usr.LastLogin = DateTime.Parse(reader["LastLogin"].ToString());
                        usr.Fullname=reader["fullname"].ToString();
                }
                    reader.Close();
                    if (!(usr.id > 0))
                        usr = null;
                }
                catch (Exception ex)
                {
                    usr = null;
                }
                if (cnn.State == System.Data.ConnectionState.Open)
                {
                    cnn.Close();
                }
            }
            return usr;
        }
        public object? change_pass(string username, string oldPass, string newPass)
        {
            var obj = ChangePass(username, oldPass, newPass);
            return Json(obj);
        }
        private object? ChangePass(string uid, string oPass, string nPass)
        {
            try
            {
                var db = new LtwebContext();
                var usr = db.Users.Where(x => x.Username == uid).FirstOrDefault();
                if(usr == null)
                {
                    return new
                    {
                        success = false,
                        message = "User Khong Ton Tai!!",
                    };
                }
                else
                {
                    var cPass = DecryptString(usr.Password, _key);
                    if (cPass != oPass)
                    {
                        return new
                        {
                            success = false,
                            message = "Mat Khau Cu Khong Chinh Xac!!",
                        };
                    }
                    else
                    {
                        var hashPass = EncryptString(nPass, _key);
                        usr.Password = hashPass;
                        db.Users.Update(usr);
                        db.SaveChanges();
                        HttpContext.Session.Remove("Username");
                        HttpContext.Session.Remove("Fullname");
                        return new
                        {
                            success = true,
                            message = "Cap Nhat Mat Khau Thanh Cong!!",
                        };
                    }
                }
            }catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = ex.Message,
                };
            }
        }

  
        private string EncryptString(string text, string keyString)
        {
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        private string DecryptString(string cipherText, string keyString)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }

        private object? GetProvinces()
        {
            var db = new LtwebContext();
            var res = db.Provinces.ToList();
            return res;
        }

        private object? CreateUser(User u)
        {
            try
            {
                var db = new LtwebContext();
                var usr = db.Users.Where(x => x.Username == u.Username).FirstOrDefault();
                if (usr != null)
                {
                    //Username bi trung
                    return new
                    {
                        success = false,
                        message = "Trung Username!! Khong the Tao!!",
                        data = ""
                    };
                }
                else
                {
                    var hashPass = EncryptString(u.Password, _key);
                    u.Password=hashPass;
                    db.Users.Add(u);
                    db.SaveChanges();
                    return new
                    {
                        success = true,
                        message = "Tao User Thanh Cong!!",
                        data = u
                    };
                }
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = ex.Message,
                };
            }
        }
    }

    public class LoginData
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponnse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public Users? User { get; set; }
    }
    public class Users
    {
        public int? id { get; set; }

        public String Username { get; set; }

        public String Password { get; set; }

        public String Fullname { get; set; }

        public DateTime? LastLogin { get; set; }

    }


    
    }
