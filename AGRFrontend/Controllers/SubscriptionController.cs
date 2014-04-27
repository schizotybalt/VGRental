using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

using VGRental.Model;
using System.Threading.Tasks;

namespace AGRFrontend.Controllers
{
    [Authorize]
    public class SubscriptionController : Controller
    {
        //
        // GET: /Subscription/
        public ActionResult Index()
        {
            string connection_string
                = WebConfigurationManager.ConnectionStrings["EC2Connection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                using (SqlCommand command = new SqlCommand("Command String", connection))
                {
                    command.CommandText = "SELECT customerID, SubscriptionID, Price, "
                    + "ExpirationDate, Description FROM Subscriptions";
                    try
                    {
                        connection.Open();
                        List<Subscription> subscriptions = new List<Subscription>();
                        using (SqlDataReader reader = command.ExecuteReader())
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
                                subscriptions.Add(new Subscription
                                {
                                    customerID = Convert.ToInt32(reader["customerID"]),
                                    SubscriptionID = Convert.ToInt32(reader["SubscriptionID"]),
                                    Price = Convert.ToSingle(reader["Price"]),
                                    ExpirationDate = Convert.ToDateTime(reader["ExpirationDate"]),
                                    Description = reader["Description"].ToString(),
                                    Customer = new Customer
                                    {
                                        UserName = username,
                                    },
                                });
                            }
                        }

                        connection.Close();
                        return View(subscriptions);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        return View(new List<Subscription>());
                    }
                }
            }
        }

        //
        // GET: /Subscription/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Subscription/Create
        public ActionResult Create()
        {
            string connection_string
                = WebConfigurationManager.ConnectionStrings["EC2Connection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                SqlCommand command = new SqlCommand("Command String", connection);
                command.CommandText = "SELECT customerID, UserName FROM Customers "
                    + "where UserName = @UserName";
                SqlParameter UserName = new SqlParameter("@UserName", User.Identity.GetUserName());
                command.Parameters.Add(UserName);
                try
                {
                    connection.Open();
                    Subscription subscription = new Subscription();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        subscription.customerID = Convert.ToInt32(reader["customerID"]);
                        subscription.ExpirationDate = DateTime.UtcNow.AddMonths(1);
                        subscription.Price = 7F;
                        subscription.Description
                            = string.Format("Unlimited rentals for ${0}/month",
                            subscription.Price);
                        subscription.Customer = new Customer
                        {
                            UserName = reader["UserName"].ToString(),
                        };
                    }

                    connection.Close();
                    return View(subscription);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return View();
                }
            }
        }

        //
        // POST: /Subscription/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Subscription new_subscription)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string connection_string
                        = WebConfigurationManager.ConnectionStrings["EC2Connection"].ConnectionString;

                    using (SqlConnection connection = new SqlConnection(connection_string))
                    {
                        using (SqlCommand command
                            = new SqlCommand("INSERT INTO Subscriptions (customerID, "
                            + "SubscriptionID, Price, ExpirationDate, Description) "
                            + "Values (@customerID, @SubscriptionID, @Price, "
                            + "@ExpirationDate, @Description)", connection))
                        {
                            command.Parameters.AddWithValue("@customerID",
                                new_subscription.customerID);
                            command.Parameters.AddWithValue("@SubscriptionID",
                                new_subscription.SubscriptionID);
                            command.Parameters.AddWithValue("@Price",
                                new_subscription.Price);
                            command.Parameters.AddWithValue("@ExpirationDate",
                                new_subscription.ExpirationDate);
                            command.Parameters.AddWithValue("@Description",
                                new_subscription.Description);

                            try
                            {
                                connection.Open();
                                if (command.ExecuteNonQuery() != 1)
                                    throw new Exception("The number of new users created isn't equal to 1");

                                connection.Close();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }
                        }
                    }

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }

            // If we got this far, something failed, redisplay form
            return View(new_subscription);
        }

        //
        // GET: /Subscription/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Subscription/Edit/5
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
        // GET: /Subscription/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Subscription/Delete/5
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
