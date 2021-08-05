using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using Taste.Models;

namespace Taste.DataAccess.Data.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        int IncreamentCount(ShoppingCart shoppingCart, int count);
        int DecreamentCount(ShoppingCart shoppingCart, int count);
    }
}
