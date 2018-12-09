//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Common;
//using System.Linq;
//using System.Threading.Tasks;
//using Kinabalu.Models;
//using MySql.Data.MySqlClient;

//namespace Kinabalu.DAL
//{
//    public class CustomerContext
//    {
//        public readonly ApplicationDb Db;
//        public CustomerContext(ApplicationDb db)
//        {
//            Db = db;
//        }

//        public async Task InsertAsync(Customer customer)
//        {
//            var cmd = Db.Connection.CreateCommand() as MySqlCommand;
//            cmd.CommandText = $"INSERT INTO `customer` (`first_name`, `last_name`, `email_address`, `last_login`) VALUES (@first_name, @last_name, @email_address, @last_login);";
//            BindParams(cmd, customer);
//            await cmd.ExecuteNonQueryAsync();
//            customer.CustomerId = (int)cmd.LastInsertedId;
//        }

//        public async Task UpdateAsync(Customer customer)
//        {
//            var cmd = Db.Connection.CreateCommand() as MySqlCommand;
//            cmd.CommandText = @"UPDATE `customer` SET `first_name` = @first_name, `last_name` = @last_name, `email_address` = @email_address, `last_login` = @last_login WHERE `customer_id` = @customer_id;";
//            BindParams(cmd, customer);
//            BindId(cmd, customer.CustomerId);
//            await cmd.ExecuteNonQueryAsync();
//        }

//        public async Task DeleteAsync(Customer customer)
//        {
//            var cmd = Db.Connection.CreateCommand() as MySqlCommand;
//            cmd.CommandText = @"DELETE FROM `customer` WHERE `customer_id` = @customer_id;";
//            BindId(cmd, customer.CustomerId);
//            await cmd.ExecuteNonQueryAsync();
//        }

//        private void BindId(MySqlCommand cmd, int customerId)
//        {
//            cmd.Parameters.Add(new MySqlParameter
//            {
//                ParameterName = "@customer_id",
//                DbType = DbType.Int32,
//                Value = customerId,
//            });
//        }

//        private void BindParams(MySqlCommand cmd, Customer customer)
//        {
//            cmd.Parameters.Add(new MySqlParameter
//            {
//                ParameterName = "@first_name",
//                DbType = DbType.String,
//                Value = customer.FirstName,
//            });
//            cmd.Parameters.Add(new MySqlParameter
//            {
//                ParameterName = "@last_name",
//                DbType = DbType.String,
//                Value = customer.LastName,
//            });

//            cmd.Parameters.Add(new MySqlParameter
//            {
//                ParameterName = "@email_address",
//                DbType = DbType.String,
//                Value = customer.EmailAddress,
//            });

//            cmd.Parameters.Add(new MySqlParameter
//            {
//                ParameterName = "@last_login",
//                MySqlDbType = MySqlDbType.Timestamp,
//                Value = customer.LastLogin
//            });
//        }

//        private async Task<List<Customer>> ReadAllAsync(DbDataReader reader)
//        {
//            var posts = new List<Customer>();
//            using (reader)
//            {
//                while (await reader.ReadAsync())
//                {
//                    var post = new Customer()
//                    {
//                        CustomerId = await reader.GetFieldValueAsync<int>(0),
//                        FirstName = await reader.GetFieldValueAsync<string>(1),
//                        LastName = await reader.GetFieldValueAsync<string>(2),
//                        EmailAddress = await reader.GetFieldValueAsync<string>(3),
//                        LastLogin = await reader.GetFieldValueAsync<DateTime>(4),
//                        LastUpdate = await reader.GetFieldValueAsync<DateTime>(5)
//                    };
//                    posts.Add(post);
//                }
//            }
//            return posts;
//        }

//        public async Task<Customer> FindOneAsync(int id)
//        {
//            var cmd = Db.Connection.CreateCommand() as MySqlCommand;
//            cmd.CommandText = @"SELECT `customer_id`, `first_name`, `last_name`, `email_address`, `last_login`, `last_update` FROM `customer` WHERE `customer_id` = @customer_id;";
//            BindId(cmd, id);
//            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
//            return result.Count > 0 ? result[0] : null;
//        }

//        public async Task<List<Customer>> LatestPostsAsync()
//        {
//            var cmd = Db.Connection.CreateCommand();
//            cmd.CommandText = @"SELECT `customer_id`, `first_name`, `last_name`, `email_address`, `last_login`, `last_update` FROM `customer` ORDER BY `customer_id` DESC LIMIT 10;";
//            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
//        }

//        public async Task DeleteAllAsync()
//        {
//            var txn = await Db.Connection.BeginTransactionAsync();
//            try
//            {
//                var cmd = Db.Connection.CreateCommand();
//                cmd.CommandText = @"DELETE FROM `customer`";
//                await cmd.ExecuteNonQueryAsync();
//                txn.Commit();
//            }
//            catch
//            {
//                txn.Rollback();
//                throw;
//            }
//        }
//    }
//}
