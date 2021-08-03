using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;

namespace Taste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(new { data = _unitOfWork.Category.GetAll() });
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            if(objFromDb == null)
            {
                return new JsonResult(new { success = false, message = "Error while Deleting" });
            }
            _unitOfWork.Category.Remove(objFromDb);
            _unitOfWork.Save();
            return new JsonResult(new { success = true, message = "Delete Successful" });
        }
    }
}
