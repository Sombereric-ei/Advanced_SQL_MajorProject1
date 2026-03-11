using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace WorkerSimulation
{
    internal class DatabaseWriter
    {
        private string connectionString = "";

        public DatabaseWriter()
        {
            connectionString = ConfigurationManager.ConnectionStrings["FogLampDB"].ConnectionString;
        }
        /// <summary>
        /// writes the workstations status to the databaase
        /// </summary>
        /// <param name="WorkstationID">workstation id who created the log</param>
        /// <param name="WorkerID">the workers id</param>
        /// <param name="LogType">the type of log made in the system</param>
        /// <param name="logMessage">the message attached to the log</param>
        public void workstationStatusWriter(int WorkstationID, int WorkerID, string LogType, string logMessage)
        {
            //creates the connection to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            //creates the procedure command
            using (SqlCommand command = new SqlCommand("LogWorkStationStatus", connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@WorkStationID", WorkstationID);
                command.Parameters.AddWithValue("@WorkerID", WorkerID);
                command.Parameters.AddWithValue("@LogType", LogType);
                command.Parameters.AddWithValue("@LogMessage", logMessage);

                connection.Open();
                command.ExecuteNonQuery();

                Console.WriteLine("Log Status Written.");
            }
        }
        public void partConsumptionWriter()
        {

        }
        public void lampCreationLog()
        {

        }
    }
}
