using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLayer
{
    public static class OrderService
    {
        private static EventRegistrationEntities db = new EventRegistrationEntities();

        public static Order GetOrderById(int Id)
        {
            return db.Orders.Where(x => x.OrderId== Id).FirstOrDefault();
        }
        public static void InsertOrder(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
        }

        public static IEnumerable<GetOrderDetails_Result> getOrderDetails(int CustomerId, int OrderId)
        {
           
            var RtnData = (from data2 in db.GetOrderDetails( CustomerId, OrderId)
                           select data2);

            return RtnData;
        }
       

    }
}
