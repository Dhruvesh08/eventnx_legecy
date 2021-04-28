using DataLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public static class PackageService
    {
        private static EventRegistrationEntities db = new EventRegistrationEntities();

        public static Package GetPackageById(int Id)
        {
            return db.Packages.Where(x => x.PackageId == Id).FirstOrDefault();
        }

        public static IEnumerable<Package> GetAllPackages()
        {
            return db.Packages.AsEnumerable();
        }


        public static void InsertPackage(Package package)
        {
            db.Packages.Add(package);
            db.SaveChanges();
        }

        public static void UpdatePackage(Package package)
        {
            db.Entry(package).State = EntityState.Modified;
            db.SaveChanges();
        }

        public static void DeletePackage(int Id)
        {
            Package package = db.Packages.Find(Id);
            db.Packages.Remove(package);
            db.SaveChanges();
        }
        public static IEnumerable<Packagedetails1_Result> PackageDetails(string SearchTitle, string Alpha)
        {
            SearchTitle = SearchTitle.Trim().ToLower();
            Alpha = Alpha.Trim().ToLower();
            int id = 0;
            var RtnData = (from data2 in db.Packagedetails1(id, Alpha)
                           select data2);

            return RtnData;
        }
    }
}
