using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using BiancasBikeShop.Models;
using System.Data.Common;

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
            
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT b.Id, b.Brand, b.Color,
                                               o.Id AS OwnerId, o.Name AS OwnerName, o.Address, o.Email, o.Telephone,
                                               bt.Id AS BikeTypeId, bt.Name AS BikeTypeName,
                                               wo.Id AS WorkOrderId, wo.DateInitiated, wo.Description, wo.DateCompleted
                                        FROM Bike b
                                        JOIN Owner o ON b.OwnerId = o.Id
                                        JOIN BikeType bt ON b.BikeTypeId = bt.Id
                                        LEFT JOIN WorkOrder wo ON wo.BikeId = b.Id
                                        WHERE b.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (bike == null)
                            {
                                bike = new Bike
                                {
                                    Id = id,
                                    Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                    Color = reader.GetString(reader.GetOrdinal("Color")),
                                    Owner = new Owner
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                        Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                                        Address = reader.GetString(reader.GetOrdinal("Address")),
                                        Email = reader.GetString(reader.GetOrdinal("Email")),
                                        Telephone = reader.GetString(reader.GetOrdinal("Telephone"))
                                    },
                                    BikeType = new BikeType
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("BikeTypeId")),
                                        Name = reader.GetString(reader.GetOrdinal("BikeTypeName"))
                                    },
                                    WorkOrders = new List<WorkOrder>()
                                };                              
                            }
                            if (!reader.IsDBNull(reader.GetOrdinal("WorkOrderId")))
                            {
                                WorkOrder workOrder = new WorkOrder
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("WorkOrderId")),
                                    DateInitiated = reader.GetDateTime(reader.GetOrdinal("DateInitiated")),
                                    Description = reader.GetString(reader.GetOrdinal("Description"))
                                };
                                if (!reader.IsDBNull(reader.GetOrdinal("DateCompleted")))
                                {
                                    workOrder.DateCompleted = reader.GetDateTime(reader.GetOrdinal("DateCompleted"));
                                }
                                bike.WorkOrders.Add(workOrder);
                            }
                        }
                    }
                }
            }
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
