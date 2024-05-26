using Center.Models;
using Center.ModelsDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Center.Services
{
    public static class DBCall
    {
        private static readonly Ispr2336AvetisyanSkCenterContext _dbContext = new();

        public static Worker Authorize(string login, string password)
        {
            return _dbContext.Workers
                .Include(w => w.JobTitle)
                .Include(j => j.Department)
                .Include(w => w.User)
                .ThenInclude(u => u.Role)
                .FirstOrDefault(u => u.User.Login == login && u.User.Password == password);
        }

        public static List<MagazinDTO> GetMagazines()
        {
            return _dbContext.Magazins
                .Include(m => m.Creator)
                .Select(mag => new MagazinDTO
                {
                    Id = mag.Id,
                    Title = mag.Title,
                    CreationDate = mag.CreatinDate,
                    Count = mag.Count,
                    Photo = mag.Photo,
                    Creator = mag.Creator,
                    Price = mag.Price,
                }).ToList();
        }

        public static List<Worker> GetCreators()
        {
            return _dbContext.Workers.ToList();
        }

        public static void Add(MagazinDTO magazinDTO)
        {
            var magazin = new Magazin
            {
                Title = magazinDTO.Title,
                Creator = magazinDTO.Creator,
                CreatinDate = DateOnly.FromDateTime(DateTime.Now),
                Count = magazinDTO.Count,
                Photo = magazinDTO.Photo,
                Price = magazinDTO.Price,
            };
            _dbContext.Magazins.Add(magazin);
            _dbContext.SaveChanges(); 
        }

        public static void DeleteMagazin(MagazinDTO MagazinDTO)
        {
            var drug = _dbContext.Magazins.First(d => d.Id == MagazinDTO.Id);
            _dbContext.Magazins.Remove(drug);
            _dbContext.SaveChanges();
        }
    }
}
