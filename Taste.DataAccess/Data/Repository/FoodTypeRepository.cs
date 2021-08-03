﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Taste.DataAccess.Data.Repository.IRepository;
using Taste.Models;

namespace Taste.DataAccess.Data.Repository
{
    public class FoodTypeRepository : Repository<FoodType>, IFoodTypeRepository
    {

        private readonly ApplicationDbContext _db;
        public FoodTypeRepository(ApplicationDbContext db)
            : base(db)
        {
            _db = db;
        }
        public IEnumerable<SelectListItem> GetFoodTypeListForDropDown()
        {
            return _db.FoodType.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
        }

        public void Update(FoodType foodType)
        {
            var foodTypeFromDb = _db.FoodType.FirstOrDefault(s => s.Id == foodType.Id);
            foodTypeFromDb.Name = foodType.Name;

            _db.SaveChanges();
        }
    }
}