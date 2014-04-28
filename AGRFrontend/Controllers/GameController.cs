using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

using VGRental.Model;

namespace AGRFrontend.Controllers
{
    public class GameController : Controller
    {
        //
        // GET: /Game/
        public ActionResult Index()
        {
            string connection_string
                = WebConfigurationManager.ConnectionStrings["EC2Connection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connection_string))
            {
                SqlCommand command = new SqlCommand("Command String", connection);
                command.CommandText = "SELECT [SystemName], [GameName], [Multiplayer], "
                    + "[NumberOfPlayers], [Rating], [Genre], [Developer], "
                    + "[Publisher], [ReleaseDate], [QuantityAvailable], [TotalQuantity] "
                + "FROM Games";
                //try
                //{
                connection.Open();
                List<Game> games = new List<Game>();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Game game = new Game();

                        game.SystemName = reader["SystemName"].ToString();
                        game.GameName = reader["GameName"].ToString();

                        if (reader["Multiplayer"] != DBNull.Value)
                            game.Multiplayer = Convert.ToBoolean(reader["Multiplayer"]);

                        if (reader["NumberOfPlayers"] != DBNull.Value)
                            game.NumberOfPlayers = Convert.ToInt32(reader["NumberOfPlayers"]);

                        game.Rating = reader["Rating"].ToString();
                        game.Genre = reader["Genre"].ToString();
                        game.Developer = reader["Developer"].ToString();
                        game.Publisher = reader["Publisher"].ToString();

                        if (reader["ReleaseDate"] != DBNull.Value)
                            game.ReleaseDate = Convert.ToDateTime(reader["ReleaseDate"]);

                        game.QuantityAvailable = Convert.ToInt32(reader["QuantityAvailable"]);
                        game.TotalQuantity = Convert.ToInt32(reader["TotalQuantity"]);

                        games.Add(game);
                    }
                }

                connection.Close();
                return View(games);
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e.ToString());
                //    return View(new List<Game>());
                //}
            }
        }

        public ActionResult Order(string GameName, string SystemName)
        {
            try
            {
                string connection_string
                    = WebConfigurationManager.ConnectionStrings["EC2Connection"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connection_string))
                {
                    using (SqlCommand command = new SqlCommand("Command String", connection))
                    {
                        command.CommandText = "SELECT customerID from customers where"
                            + " UserName = @UserName";
                        command.Parameters.AddWithValue("@UserName", User.Identity.GetUserName());

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            int customerID = Convert.ToInt32(reader["customerID"]);
                            connection.Close();

                            using (SqlCommand order_command
                                = new SqlCommand(
                                    "begin transaction OrderGame" +
                                    " with mark N'Ordering a Game';" +

                                    " insert INTO orders (OrderID, CustomerID, GameName, SystemName)" +
                                    " Values (@OrderID, @CustomerID, @GameName, @SystemName);" +

                                    " insert INTO orderdetails (OrderID, Timestamp, Status)" +
                                    " Values (@OrderID, @Timestamp, @Status);" +

                                    " update games set QuantityAvailable = QuantityAvailable - 1" +
                                    " where Gamename = @GameName and SystemName = @SystemName;" +
                                    "  COMMIT TRANSACTION", connection))
                            {
                                order_command.Parameters.AddWithValue("@GameName", GameName);
                                order_command.Parameters.AddWithValue("@SystemName", SystemName);
                                order_command.Parameters.AddWithValue("@OrderID", Guid.NewGuid());
                                order_command.Parameters.AddWithValue("@Timestamp", DateTime.UtcNow);
                                order_command.Parameters.AddWithValue("@Status", "Submitted");
                                order_command.Parameters.AddWithValue("@CustomerID", customerID);

                                connection.Open();
                                order_command.ExecuteNonQuery();

                                connection.Close();
                            }
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

        //
        // GET: /Game/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Game/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Game/Create
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
        // GET: /Game/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Game/Edit/5
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
        // GET: /Game/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Game/Delete/5
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
