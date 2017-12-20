using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models;
using System.IO;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        ImageLibrariesEntities entity = new ImageLibrariesEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ImageLibraries model)
        {
            Debug.WriteLine(model);
            ImageLibrary img = new ImageLibrary();
            img.id = model.id;
            entity.ImageLibraries.Add(img);
            
            if (model.listImage != null)
            {
                foreach (var image in model.listImage)
                {
                    uploadImage(image.image, img.id);
                }
            }
            entity.SaveChanges();
            return View();
        }

        [NonAction]
        public bool uploadImage(string imgBase64,int id)
        {
            try
            {
                string pathFile = "/Upload";
                string[] listImage = imgBase64.Split(';');
                string extension = listImage[0].Replace("data:image/", "");
                string fileName = id.ToString() + Guid.NewGuid() + "." + extension;
                string filePath = Server.MapPath(pathFile) + "/" + fileName;
                string base64Image = listImage[1].Split(',')[1];

                System.IO.File.WriteAllBytes(filePath, Convert.FromBase64String(base64Image));

                Image ig = new Image();
                ig.ImageLibraryId = id;
                ig.image1 = fileName;
                entity.Images.Add(ig);
                return true;
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
                return false;
            }
        }

        public ActionResult Edit(int id)
        {
            var imgLibs = entity.ImageLibraries.FirstOrDefault(n => n.id == id);
            var images = entity.Images.Where(n => n.ImageLibraryId == id).Select(item => new Web.Models.Image {
                image = item.image1
            }).ToList();
            if (imgLibs == null)
                return Redirect("Index");
            var model = new ImageLibraries();
            model.id = imgLibs.id;
            model.listImage = images;
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(ImageLibraries model)
        {
            foreach (var image in model.listImage)
            {
                if (image.image.IndexOf("data:image/") > -1)
                {
                    uploadImage(image.image, model.id);
                }
            }
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }


}