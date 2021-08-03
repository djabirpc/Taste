using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taste.DataAccess.Data.Repository.IRepository;

namespace Taste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodTypeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public FoodTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(new { data = _unitOfWork.FoodType.GetAll() });

        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            var objFromDb = _unitOfWork.FoodType.GetFirstOrDefault(u => u.Id == id);
            if (objFromDb == null)
            {
                return new JsonResult(new { success = false, message = "Error while Deleting" });
            }
            _unitOfWork.FoodType.Remove(objFromDb);
            _unitOfWork.Save();
            return new JsonResult(new { success = true, message = "Delete Successful" });
        }
    }
}
