﻿using AdminDyanamoEnterprises.DTOs;
using AdminDyanamoEnterprises.DTOs.Master;
using AdminDyanamoEnterprises.Repository.IRepository;
using Microsoft.Data.SqlClient; 
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdminDyanamoEnterprises.Repository.Repository
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly IConfiguration _config;

        public MaterialRepository(IConfiguration config)
        {
            this._config = config;
        }
        public string sqlConnection()
        {
            return _config.GetConnectionString("DyanamoEnterprises_DB").ToString();
        }


        public void Sp_InsertOrUpdateOrDeleteMaterialType(MaterialTypePageViewModel materialType)
        {
            using (SqlConnection con = new SqlConnection(sqlConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("Sp_InsertOrUpdateOrDeleteMaterialType", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    int id = materialType.AddMaterial.MaterialID;
                    string action = id <= 0 ? "insert" : "update";

                    cmd.Parameters.AddWithValue("@MaterialID", id);
                    cmd.Parameters.AddWithValue("@MaterialName", materialType.AddMaterial.MaterialName);
                    cmd.Parameters.AddWithValue("@Action", action);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public List<MaterialType> GetAllListType()
        {
            List<MaterialType> materialnames = new List<MaterialType>();
            using (SqlConnection con = new SqlConnection(sqlConnection()))
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertOrUpdateOrDeleteMaterialType", con);

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@MaterialID", 0); // Dummy value
                cmd.Parameters.AddWithValue("@MaterialName", DBNull.Value); // Dummy value
                cmd.Parameters.AddWithValue("@Action", "select");
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    // Add each row's value to your list
                    MaterialType obj = new MaterialType()
                    {
                        MaterialName = dr["MaterialName"].ToString(),
                        MaterialID = Convert.ToInt32(dr["MaterialID"])
                    };

                    materialnames.Add(obj);
                }
            }
            return materialnames;
        }

        public void DeleteMaterial(int id)
        {
            using (SqlConnection con = new SqlConnection(sqlConnection()))
            {
                SqlCommand cmd = new SqlCommand("Sp_InsertOrUpdateOrDeleteMaterialType", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "delete");
                cmd.Parameters.AddWithValue("@MaterialID", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

