using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using PuzzleHunt.Web.Models;

namespace PuzzleHunt.Web
{
    public static class Current
    {
        private const string DisposeConnectionKey = "dispose_connections";

        public static void DisposeRegisteredConnections()
        {
            List<SqlConnection> connections = HttpContext.Current.Items[DisposeConnectionKey] as List<SqlConnection>;
            if (connections != null)
            {
                HttpContext.Current.Items[DisposeConnectionKey] = null;

                foreach (var connection in connections)
                {
                    try
                    {
                        if (connection.State != ConnectionState.Closed)
                        {
                            //GlobalApplication.LogException("Connection was not in a closed state.");
                        }

                        connection.Dispose();
                    }
                    catch
                    {
                        /* don't care, nothing we can do */
                    }
                }
            }
        }

        public static PuzzleHuntContext DB
        {
            get
            {
                PuzzleHuntContext result = null;

                if (HttpContext.Current != null)
                {
                    result = HttpContext.Current.Items["DB"] as PuzzleHuntContext;
                }
                else
                {
                    // unit tests
                    result = CallContext.GetData("DB") as PuzzleHuntContext;
                }

                if (result == null)
                {
                    result = PuzzleHuntContext.GetContext();
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Items["DB"] = result;
                    }
                    else
                    {
                        CallContext.SetData("DB", result);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Allows end of reqeust code to clean up this request's DB.
        /// </summary>
        public static void DisposeDB()
        {
            PuzzleHuntContext db = null;
            if (HttpContext.Current != null)
            {
                db = HttpContext.Current.Items["DB"] as PuzzleHuntContext;
            }
            else
            {
                db = CallContext.GetData("DB") as PuzzleHuntContext;
            }
            if (db != null)
            {
                db.Dispose();
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items["DB"] = null;
                }
                else
                {
                    CallContext.SetData("DB", null);
                }
            }
        }

        public static string Title(this HtmlHelper helper, string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return "Elite Puzzle Hunt 2011";
            }
            else
            {
                return string.Format("{0} - Elite Puzzle Hunt 2011", title);
            }
        }

        public static string DisplayTimespan(this HtmlHelper helper, TimeSpan time)
        {
            if (time.TotalDays < 1)
            {
                return string.Format("{0:hh\\:mm\\:ss}", time);
            }
            else
            {
                return string.Format("{0} day(s), {1:hh\\:mm\\:ss}", time.Days, time);
            }
            
        }
    }
}