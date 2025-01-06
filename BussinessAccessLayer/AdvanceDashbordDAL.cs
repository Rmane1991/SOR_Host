using MaxiSwitch.EncryptionDecryption;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessAccessLayer
{
    public class AdvanceDashbordDAL
    {
        static string ConnectionString = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEncryption"]) ? ConnectionStringEncryptDecrypt.DecryptConnectionString(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString()) : ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public static int PageRequestTimeoutInMLS = Convert.ToInt32(ConfigurationManager.AppSettings["PageRequestTimeoutInMLS"]);

        public DataSet GetRuleWiseData(string dateFilter = null)
        {
            DataSet ds = new DataSet();

            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT * FROM Get_Top5_Rule_Data(@dateFilter)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@dateFilter", (object)dateFilter ?? DBNull.Value);
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, "RuleData");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

            return ds;
        }

        public DataSet GetChannelWiseData(string dateFilter = null)
        {
            DataSet ds = new DataSet();

            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT * FROM get_channelwisechartdata(@dateFilter)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@dateFilter", (object)dateFilter ?? DBNull.Value);
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, "ChannelData");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

            return ds;
        }

        public DataSet GetAgentOnboardingData(string dateFilter = null)
        {
            DataSet ds = new DataSet();

            try
            {
                using (var conn = new NpgsqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand("SELECT * FROM get_AgentOnboardedDataCount(@filtertype)", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@filtertype", (object)dateFilter ?? DBNull.Value);
                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds, "AgentOnbordingData");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }

            return ds;
        }
    }
}
