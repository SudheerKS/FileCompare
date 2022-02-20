using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Read_CSV_MVC.Models;

namespace Read_CSV_MVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View(new List<Values>());
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase[] files)
        {
            List<Values> values = new List<Values>();

            string filePath = string.Empty;

            try
            {
                if (files != null)
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    Dictionary<int, string>[] dict = new Dictionary<int, string>[]
                    {
                    new Dictionary<int, string>(),
                    new Dictionary<int, string>()
                    };

                    int i = 0;
                    foreach (var item in files)
                    {

                        filePath = path + Path.GetFileName(item.FileName);
                        string extension = Path.GetExtension(item.FileName);
                        item.SaveAs(filePath);
                        string csvData = System.IO.File.ReadAllText(filePath);

                        foreach (string row in csvData.Split('\n'))
                        {
                            if (!string.IsNullOrEmpty(row))
                            {
                                dict[i].Add(Convert.ToInt32(row.Split(',')[0]), row.Split(',')[1].Trim());
                            }
                        }
                        i++;
                    }
                    var diff = dict[0].Except(dict[1]).Concat(dict[1].Except(dict[0]));

                    foreach (var item in diff)
                    {
                        values.Add(new Values()
                        {
                            Key = item.Key,
                            Value = item.Value,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(values);
        }
    }
}