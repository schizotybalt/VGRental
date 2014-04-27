using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.Mvc;

using VGRental.Model;

namespace AGRFrontend.Controllers
{
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/
        public ActionResult Index()
        {
            string connection_string
                = WebConfigurationManager.ConnectionStrings["EC2Connection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                SqlCommand command = new SqlCommand("Command String", connection);
                command.CommandText = "SELECT customerID, DOB, Email, Name, "
                + "UserName, Password, ShippingAddress, City, State, ZIP "
                + "FROM Customers";
                try
                {
                    connection.Open();
                    List<Customer> customers = new List<Customer>();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Customer
                            {
                                DOB = Convert.ToDateTime(reader["DOB"]),
                                Email = reader["Email"].ToString(),
                                Name = reader["Name"].ToString(),
                                UserName = reader["UserName"].ToString(),
                                ShippingAddress = reader["ShippingAddress"].ToString(),
                                City = reader["City"].ToString(),
                                State = reader["State"].ToString(),
                                ZIP = Convert.ToInt32(reader["ZIP"]),
                            });
                        }
                    }

                    connection.Close();
                    return View(customers);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return View(new List<Customer>());
                }
            }
        }

        //
        // GET: /Customer/Orders/Open
        public ActionResult Orders(string status)
        {
            return View();
        }

        //
        // GET: /Customer/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Customer/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Customer/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Customer/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Customer/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Customer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
