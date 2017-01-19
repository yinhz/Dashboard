using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace yhz.Dashboard.Controllers
{
    public class ResourceController : Controller
    {
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImageUpload()
        {
            foreach (string upload in Request.Files)
            {
                if (Request.Files[upload].ContentLength == 0) continue;
                string pathToSave = Server.MapPath("~/" + GlobalResources.CONST_USER_IMAGE_PATH);
                string filename = Path.GetFileName(Request.Files[upload].FileName);
                Request.Files[upload].SaveAs(Path.Combine(pathToSave, filename));
            }

            return View("Upload");
        }

        [HttpPost]
        public ActionResult AudioUpload()
        {
            foreach (string upload in Request.Files)
            {
                if (Request.Files[upload].ContentLength == 0) continue;
                string pathToSave = Server.MapPath("~/" + GlobalResources.CONST_USER_AUDIO_PATH);
                string filename = Path.GetFileName(Request.Files[upload].FileName);
                Request.Files[upload].SaveAs(Path.Combine(pathToSave, filename));
            }

            return View("Upload");
        }

        [HttpPost]
        public ActionResult VideoUpload()
        {
            foreach (string upload in Request.Files)
            {
                if (Request.Files[upload].ContentLength == 0) continue;
                string pathToSave = Server.MapPath("~/" + GlobalResources.CONST_USER_VIDEO_PATH);
                string filename = Path.GetFileName(Request.Files[upload].FileName);
                Request.Files[upload].SaveAs(Path.Combine(pathToSave, filename));
            }

            return View("Upload");
        }

        #region delete file action

        [HttpPost]
        public JsonResult DeleteImage(string fileName)
        {
            string pathToDelete = Server.MapPath("~/" + GlobalResources.CONST_USER_IMAGE_PATH);
            var filepath = Path.Combine(pathToDelete, fileName);

            try
            {
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }

                return Json(new { Success = true });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Info = e.ToString() });
            }
        }

        [HttpPost]
        public JsonResult DeleteAudio(string fileName)
        {
            string pathToDelete = Server.MapPath("~/" + GlobalResources.CONST_USER_AUDIO_PATH);
            var filepath = Path.Combine(pathToDelete, fileName);

            try
            {
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }

                return Json(new { Success = true });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Info = e.ToString() });
            }
        }

        [HttpPost]
        public JsonResult DeleteVideo(string fileName)
        {
            string pathToDelete = Server.MapPath("~/" + GlobalResources.CONST_USER_VIDEO_PATH);
            var filepath = Path.Combine(pathToDelete, fileName);

            try
            {
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }

                return Json(new { Success = true });
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Info = e.ToString() });
            }
        }

        #endregion
    }
}