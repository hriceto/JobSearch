using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HristoEvtimov.Websites.Work.WorkDal
{
    public class OrderDataAccess : DataAccess
    {
        /// <summary>
        /// create a new order and return the order id
        /// </summary>
        /// <param name="newOrder"></param>
        /// <returns></returns>
        public int AddOrder(Order newOrder)
        {
            int result = -1;
            using (WorkEntities context = GetContext())
            {
                context.Orders.AddObject(newOrder);
                if (context.SaveChanges() >= 1)
                {
                    result = newOrder.OrderId;
                }
            }
            return result;
        }

        /// <summary>
        /// Update an order
        /// </summary>
        /// <param name="updateOrder"></param>
        /// <returns></returns>
        public bool UpdateOrder(Order updateOrder)
        {
            bool result = false;
            using (WorkEntities context = GetContext())
            {
                context.Orders.Attach(updateOrder);
                context.ObjectStateManager.ChangeObjectState(updateOrder, System.Data.EntityState.Modified);
                result = (context.SaveChanges() == 1);
            }

            return result;
        }

        /// <summary>
        /// Get a specific order for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public Order GetOrder(int userId, int orderId)
        {
            using (WorkEntities context = GetContext())
            {
                var order = from o in context.Orders 
                            where o.UserId == userId &&
                            o.OrderId == orderId
                            select o;
                return order.FirstOrDefault(); 
            }
        }

        /// <summary>
        /// Get a specific order for a user. Include Order items
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public Order GetOrderJobData(int userId, int orderId)
        {
            using (WorkEntities context = GetContext())
            {
                var order = from o in context.Orders.Include("JobPosts").Include("Coupon")
                            where o.UserId == userId &&
                            o.OrderId == orderId
                            select o;
                return order.FirstOrDefault();
            }
        }

        public int GetNumberOfInvalidCheckoutAttempts(int userId, int invalidAttemptWindow)
        {
            DateTime invalidStartFrom = DateTime.Now.AddHours(-invalidAttemptWindow);

            using (WorkEntities context = GetContext())
            {
                var orders = from o in context.Orders
                            where o.UserId == userId &&
                            o.OrderComplete == false &&
                            o.CreatedDate > invalidStartFrom
                            select o;
                return orders.Count();
            }
        }
    }
}
