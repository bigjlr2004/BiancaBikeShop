using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using BiancasBikeShop.Models;
using BiancasBikeShop.Utils;
using System.Linq;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

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

            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT bk.Id as BikeId, bk.Brand, bk.Color,
                            own.Id as OwnerId, own.Name, own.Address, own.Email, own.Telephone,
                            bt.Id as btId, bt.Name,
                            wo.Id as WorkOrderId, wo.Description, wo.DateInitiated, wo.DateCompleted
                            FROM Bike bk 
                                JOIN Owner own on bk.OwnerId = own.Id
                                JOIN BikeType bt on bk.BikeTypeId = bt.Id
                                JOIN WorkOrder wo on wo.BikeId = bk.Id
                                ";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        var bikes = new List<Bike>();

                        while (reader.Read())
                        {
                            var bikeId = DbUtils.GetInt(reader, "BikeId");
                            var existingBike = bikes.FirstOrDefault(b => b.Id == bikeId);

                            if (existingBike == null)
                            {
                                existingBike = new Bike()
                                {
                                    Id = DbUtils.GetInt(reader, "BikeId"),
                                    Color = DbUtils.GetString(reader, "Color"),
                                    Brand = DbUtils.GetString(reader, "Brand"),

                                    Owner = new Owner()
                                    {
                                        Id = DbUtils.GetInt(reader, "OwnerId"),
                                        Name = DbUtils.GetString(reader, "Name"),
                                        Email = DbUtils.GetString(reader, "Email"),
                                        Address = DbUtils.GetString(reader, "Address"),
                                        Telephone = DbUtils.GetString(reader, "Telephone")
                                    },

                                    BikeType = new BikeType()
                                    {
                                        Id = DbUtils.GetInt(reader, "btId"),
                                        Name = DbUtils.GetString(reader, "Name"),

                                    },
                                    WorkOrders = new List<WorkOrder>()
                                };
                                bikes.Add(existingBike);
                            }
                            if (DbUtils.IsNotDbNull(reader, "WorkOrderId"))
                            {
                                existingBike.WorkOrders.Add(new WorkOrder()
                                {
                                    Id = DbUtils.GetInt(reader, "WorkOrderId"),
                                    Description = DbUtils.GetString(reader, "Description"),
                                    DateInitiated = DbUtils.GetDateTime(reader, "DateInitiated"),
                                    DateCompleted = DbUtils.GetNullableDateTime(reader, "DateCompleted"),

                                });
                            }
                        }

                        return bikes;
                    }
                }
            }
        }

        public Bike GetBikeById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT bk.Id as BikeId, bk.Brand, bk.Color,
                            own.Id as OwnerId, own.Name, own.Address, own.Email, own.Telephone,
                            bt.Id as btId, bt.Name,
                            wo.Id as WorkOrderId, wo.Description, wo.DateInitiated, wo.DateCompleted
                            FROM Bike bk 
                                JOIN Owner own on bk.OwnerId = own.Id
                                JOIN BikeType bt on bk.BikeTypeId = bt.Id
                                JOIN WorkOrder wo on wo.BikeId = bk.Id
                            WHERE bk.Id = @Id 
                                ";
                    DbUtils.AddParameter(cmd, "@Id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Bike bike = null;
                        while (reader.Read())
                        {
                            if (bike == null)
                            {
                                bike = new Bike()
                                {
                                    Id = DbUtils.GetInt(reader, "BikeId"),
                                    Color = DbUtils.GetString(reader, "Color"),
                                    Brand = DbUtils.GetString(reader, "Brand"),

                                    Owner = new Owner()
                                    {
                                        Id = DbUtils.GetInt(reader, "OwnerId"),
                                        Name = DbUtils.GetString(reader, "Name"),
                                        Email = DbUtils.GetString(reader, "Email"),
                                        Address = DbUtils.GetString(reader, "Address"),
                                        Telephone = DbUtils.GetString(reader, "Telephone")
                                    },

                                    BikeType = new BikeType()
                                    {
                                        Id = DbUtils.GetInt(reader, "btId"),
                                        Name = DbUtils.GetString(reader, "Name"),

                                    },
                                    WorkOrders = new List<WorkOrder>()
                                };

                            }
                            if (DbUtils.IsNotDbNull(reader, "WorkOrderId"))
                            {
                                bike.WorkOrders.Add(new WorkOrder()
                                {
                                    Id = DbUtils.GetInt(reader, "WorkOrderId"),
                                    Description = DbUtils.GetString(reader, "Description"),
                                    DateInitiated = DbUtils.GetDateTime(reader, "DateInitiated"),
                                    DateCompleted = DbUtils.GetNullableDateTime(reader, "DateCompleted"),

                                });
                            }
                        }

                        return bike;
                    }

                }
            }
        }

        public int GetBikesInShopCount()
        {

            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                      SELECT COUNT(DISTINCT wo.BikeId) as BikeCount
                        FROM WORKORDER wo
                        WHERE wo.DateCompleted IS NULL";
                    

                    int count = 0;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        
                        while (reader.Read())
                        {

                            count = DbUtils.GetInt(reader, "BikeCount");
                            
                            
                        }
                        
                        return count;
                    }
                }
            }
        }
    }
}

