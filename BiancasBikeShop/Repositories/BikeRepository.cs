using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using BiancasBikeShop.Models;

namespace BiancasBikeShop.Repositories
{
    public class BikeRepository : IBikeRepository
    {
        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection("server=localhost\\SQLExpress;database=BiancasBikeShop;integrated security=true;TrustServerCertificate=true");
            }
        }

        public List<Bike> GetAllBikes()
        {
            var bikes = new List<Bike>();
            
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT b.Id, b.Brand, b.Color,
                                               o.Id AS OwnerId, o.Name                                              
                                        FROM Bike b
                                        JOIN Owner o ON b.OwnerId = o.Id";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var bike = new Bike
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                Color = reader.GetString(reader.GetOrdinal("Color")),
                                Owner = new Owner
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name"))
                                }
                            };
                            bikes.Add(bike);
                        }
                    }
                }
            }
            return bikes;
        }

        public Bike GetBikeById(int id)
        {
            Bike bike = null;
            //implement code here...
            return bike;
        }

        public int GetBikesInShopCount()
        {
            int count = 0;
            // implement code here... 
            return count;
        }
    }
}
