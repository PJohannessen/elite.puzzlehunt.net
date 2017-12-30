using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using MvcMiniProfiler;
using MvcMiniProfiler.Data;

namespace PuzzleHunt.Web.Models
{
    public partial class PuzzleHuntContext
    {
        public static PuzzleHuntContext GetContext()
        {
            return new PuzzleHuntContext(GetConnection());
        }

        private static DbConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["PuzzleHuntConnectionString"].ConnectionString;
            var connection = new SqlConnection(connectionString);
            var profiledConnection = new ProfiledDbConnection(connection, MiniProfiler.Current);
            return profiledConnection;
        }
    }
}