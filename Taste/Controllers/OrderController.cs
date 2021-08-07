using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;
using Taste.Models.ViewModels;
using Taste.Utility;

namespace Taste.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        [HttpGet]
        [Authorize]
        public IActionResult Get(string status = null)
        {
            List<OrderDetailsViewModel> orderListVm = new List<OrderDetailsViewModel>();

            IEnumerable<OrderHeader> orderHeaderList;
            if (User.IsInRole(SD.CustomerRole))
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                orderHeaderList = _unitOfWork.OrderHeader.GetAll(u => u.UserId == claim.Value, null, "ApplicationUser");


            }
            else
            {
                orderHeaderList = _unitOfWork.OrderHeader.GetAll(null, null, "ApplicationUser");

            }


            //switch (status)
            //{
            //    case "cancelled":
            //        orderHeaderList = orderHeaderList.Where(o => o.Status == SD.StatusCancelled || o.Status == SD.StatusRefunded || o.Status == SD.StatusRefunded || o.Status == SD.PaymentStatusRejected);
            //        break;
            //    case "completed":
            //        orderHeaderList = orderHeaderList.Where(o => o.Status == SD.StatusCompleted);
            //        break;
            //    default:
            //        orderHeaderList = orderHeaderList.Where(o => o.Status == SD.StatusReady || o.Status == SD.StatusSubmitted || o.Status == SD.StatusInProcess || o.Status == SD.PaymentStatusPending);
            //        break;
            //}

            if (status == "cancelled")
            {
                orderHeaderList = orderHeaderList.Where(o => o.Status == SD.StatusCancelled || o.Status == SD.StatusRefunded || o.Status == SD.PaymentStatusRejected);
            }
            else
            {
                if (status == "completed")
                {
                    orderHeaderList = orderHeaderList.Where(o => o.Status == SD.StatusCompleted);
                }
                else
                {
                    orderHeaderList = orderHeaderList.Where(o => o.Status == SD.StatusReady || o.Status == SD.StatusInProcess || o.Status == SD.StatusSubmitted || o.Status == SD.PaymentStatusPending);
                }
            }



            //if (status == "cancelled")
            //{
            //    orderHeaderList = orderHeaderList.Where(o => o.Status == SD.StatusCancelled || o.Status == SD.StatusRefunded);
            //}


            foreach (OrderHeader item in orderHeaderList)
            {
                OrderDetailsViewModel orderVM = new OrderDetailsViewModel
                {
                    OrderHeader = item,
                    OrderDetails = _unitOfWork.OrderDetails.GetAll(o => o.OrderId == item.Id).ToList()
                };
                orderListVm.Add(orderVM);
            }

            return Json(new { data = orderListVm });
        
        }
    }
}
