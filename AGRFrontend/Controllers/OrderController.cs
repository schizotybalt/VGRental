using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using System.Web.Configuration;
using System.Data.SqlClient;

using VGRental.Model;
using VGRental;
using AGRFrontend.Models;

namespace AGRFrontend.Controllers
{
    public class OrderController : Controller
    {
        private VGRentalEntities db = new VGRentalEntities();

        // GET: /Order/
        public ActionResult Index()
        {
            string connection_string
                = WebConfigurationManager.ConnectionStrings["EC2Connection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                SqlCommand order_command = new SqlCommand("Order Command", connection);
                order_command.CommandText = "SELECT customerID, GameName, SystemName, "
                + "Timestamp, ShipDate, Status FROM Orders, OrderDetails "
                + "where OrderDetails.OrderID = Orders.OrderID";
                //try
                //{
                connection.Open();
                List<OrderViewModel> orders = new List<OrderViewModel>();
                using (SqlDataReader reader = order_command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string username = "";

                        using (SqlConnection customer_connection = new SqlConnection(connection_string))
                        {
                            SqlCommand customer_command = new SqlCommand("Command String",
                                customer_connection);
                            customer_command.CommandText = "SELECT UserName FROM Customers where "
                            + "customerID = @customerID";
                            customer_command.Parameters.AddWithValue("@customerID", reader["customerID"]);

                            customer_connection.Open();
                            using (SqlDataReader customer_reader
                                = customer_command.ExecuteReader())
                            {
                                customer_reader.Read();
                                username = customer_reader["UserName"].ToString();
                            }
                            customer_connection.Close();
                        }
                        OrderViewModel order_view_model = new OrderViewModel();
                        Order order = new Order();
                        OrderDetails order_details = new OrderDetails();

                        order.CustomerID = Convert.ToInt32(reader["customerID"]);
                        order.GameName = reader["GameName"].ToString();
                        order.SystemName = reader["SystemName"].ToString();
                        order.Customer = new Customer
                        {
                            UserName = username,
                        };

                        order_details.Timestamp = Convert.ToDateTime(reader["Timestamp"]);

                        if (reader["ShipDate"] != DBNull.Value)
                            order_details.ShipDate = Convert.ToDateTime(reader["ShipDate"]);

                        order_details.Status = reader["Status"].ToString();

                        order_view_model.Order = order;
                        order_view_model.OrderDetails = order_details;

                        orders.Add(order_view_model);
                    }
                }

                connection.Close();
                order_command.Dispose();
                return View(orders);
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e.ToString());
                //    return View(new List<OrderViewModel>());
                //}
            }
        }

        // GET: /Order/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: /Order/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "customerID", "Email");
            ViewBag.SystemName = new SelectList(db.Games, "SystemName", "Rating");
            ViewBag.OrderID = new SelectList(db.Promotions, "OrderID", "OrderID");
            return View();
        }

        // POST: /Order/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,GameName,SystemName,CustomerID")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.OrderID = Guid.NewGuid();
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "customerID", "Email", order.CustomerID);
            ViewBag.SystemName = new SelectList(db.Games, "SystemName", "Rating", order.SystemName);
            ViewBag.OrderID = new SelectList(db.Promotions, "OrderID", "OrderID", order.OrderID);
            return View(order);
        }

        // GET: /Order/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "customerID", "Email", order.CustomerID);
            ViewBag.SystemName = new SelectList(db.Games, "SystemName", "Rating", order.SystemName);
            ViewBag.OrderID = new SelectList(db.Promotions, "OrderID", "OrderID", order.OrderID);
            return View(order);
        }

        // POST: /Order/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,GameName,SystemName,CustomerID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "customerID", "Email", order.CustomerID);
            ViewBag.SystemName = new SelectList(db.Games, "SystemName", "Rating", order.SystemName);
            ViewBag.OrderID = new SelectList(db.Promotions, "OrderID", "OrderID", order.OrderID);
            return View(order);
        }

        // GET: /Order/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: /Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
