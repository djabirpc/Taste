using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Taste.DataAccess;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;

namespace Taste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private ApplicationDbContext _db;

        public UserController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }

        [HttpGet]
        public JsonResult Get()
        {
            //return new JsonResult(new { data = _unitOfWork.ApplicationUser.GetAll() });
            return new JsonResult(new { data = _db.ApplicationUser
                .Select(s => new { 
                    s.Id, 
                    s.PhoneNumber,
                    s.LockoutEnd,
                    s.FullName,
                    s.Email
                }) });
        }

        [HttpPost]
        public JsonResult LockUnlock([FromBody]string id)
        {
            var objFromDb = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == id);
            if(objFromDb == null)
            {
                return new JsonResult(new { success = false, message = "Error while Locking/Unlocking" });
            }

            if(objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(10);
            }

            _unitOfWork.Save();
            return new JsonResult(new { success = true, message = "Operation Successful" });
        }
    }
}
