using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Taste.DataAccess.Data.Repository.IRepository;

namespace Taste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public MenuItemController(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(new { data = _unitOfWork.MenuItem.GetAll(null,null,"Category,FoodType") });

        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            try
            {
                var objFromDb = _unitOfWork.MenuItem.GetFirstOrDefault(u => u.Id == id);
                if (objFromDb == null)
                {
                    return new JsonResult(new { success = false, message = "Error while Deleting" });
                }
                var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, objFromDb.Image.TrimStart('\\'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }


                _unitOfWork.MenuItem.Remove(objFromDb);
                _unitOfWork.Save();
            }
            catch (Exception)
            {

                return new JsonResult(new { success = false, message = "Error while Deleting" });

            }
            return new JsonResult(new { success = true, message = "Delete Successful" });
        
        
        }
    }
}
