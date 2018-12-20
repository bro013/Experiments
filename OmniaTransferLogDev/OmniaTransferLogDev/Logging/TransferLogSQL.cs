using System;
using System.Data;
using System.Data.SqlClient;
using OmniaTransferLogDev.Models;

namespace OmniaTransferLogDev.Logging
{
    public class TransferLogSQL
    {
        public LogItem Item { get; set; }
        public string ConnectionString { get; set; }

        public TransferLogSQL(LogItem item,string username, string password)
        {
            Item = item;
            ConnectionString = $"Server=tcp:dataplatformsqldev.database.windows.net,1433;Initial Catalog=dataplatform-common;Persist Security Info=False;User ID={username};Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        }

        /// <summary>
        /// Upserting log data to the table [Common].[DataTransferLog] in the database dataplatform-common by executing
        /// the stored procedure [Common].[UpsertDataTransferLog]
        /// </summary>
        public void UpsertTransferLog()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string transferType = Item.FullUpload ? "full" : "partial";
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("[Common].[UpsertDataTransferLog]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@SourceSystem", Item.System));
                    cmd.Parameters.Add(new SqlParameter("@SourceDataset", Item.Source));
                    cmd.Parameters.Add(new SqlParameter("@DestinationDataset", Item.Sink));
                    cmd.Parameters.Add(new SqlParameter("@PipelineRunId", Item.RunId));
                    cmd.Parameters.Add(new SqlParameter("@Status", Item.Status));
                    cmd.Parameters.Add(new SqlParameter("@ErrorMessage", Item.ErrorMessage));
                    cmd.Parameters.Add(new SqlParameter("@TransferType", transferType));

                    if (Item.RowCountSource != null)
                        cmd.Parameters.Add(new SqlParameter("@RowCountSource", Item.RowCountSource));
                    if (Item.RowCountSink != null)
                        cmd.Parameters.Add(new SqlParameter("@RowCountDestination", Item.RowCountSink));
                    if (Item.StartTime != null)
                        cmd.Parameters.Add(new SqlParameter("@ExtractionTimeUTC", Item.StartTime));
                    if (Item.CompletionTime != null)
                        cmd.Parameters.Add(new SqlParameter("@CompletedTimeUTC", Item.CompletionTime));

                    cmd.ExecuteNonQuery();
                }
            }

        }
    }
}
