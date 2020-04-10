using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace IndividualProject
{
    class Event
    {
        int eventID = 0;
        String title;
        String start;
        String end;
        String Location;
        String Description;
        String reminder;

        public void readEvent(String t, DateTime s, 
            DateTime e, String l, String d, DateTime r)
        {
            title = t;
            start = s.ToString("u").Substring(0,19);
            end = e.ToString("u").Substring(0, 19);
            Location = l;
            Description = d;
            reminder = r.ToString("u").Substring(0, 19);
            Console.WriteLine(start);
        }

        public void saveToDB()
        {
            string connStr = "server=csdatabase.eku.edu;user=stu_csc340;database=csc340_db;port=3306;password=Colonels18;";
            MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);


            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                if(eventID == 0) //if the current event is a new one, the eventID will be zero
                {
                    string sql1 = "INSERT INTO ingramEvent VALUES ()"; //this creates a blank event in the DB to be updated with new info
                    MySql.Data.MySqlClient.MySqlCommand newEID = new MySql.Data.MySqlClient.MySqlCommand(sql1, conn);
                    newEID.ExecuteNonQuery();
                    sql1 = "select max(EventID) as 'top' from ingramEvent"; //this makes the new event show as an old event by recalling the new eventID to be used later
                    newEID = new MySql.Data.MySqlClient.MySqlCommand(sql1, conn);
                    DataTable myTable = new DataTable();
                    MySqlDataAdapter myAdapter = new MySqlDataAdapter(newEID);
                    myAdapter.Fill(myTable);
                    foreach (DataRow row in myTable.Rows)
                    {
                        eventID = (int)row["top"];
                    }
                }

                string sql = "update ingramEvent " +
                    "set Title = @title , startD = @startD , endD = @endD , Location = @Location , Description = @Description , reminder = @reminder  " +
                    "where eventID = @eventID ;";
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@title", title);
                cmd.Parameters.AddWithValue("@startD", start);
                cmd.Parameters.AddWithValue("@endD", end);
                cmd.Parameters.AddWithValue("@Location", Location);
                cmd.Parameters.AddWithValue("@Description", Description);
                cmd.Parameters.AddWithValue("@reminder", reminder);
                cmd.Parameters.AddWithValue("@eventID", eventID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();

        }

        public void remFromDB()
        {
            if (eventID != 0)
            {
                string connStr = "server=csdatabase.eku.edu;user=stu_csc340;database=csc340_db;port=3306;password=Colonels18;";
                MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection(connStr);
                try
                {
                    Console.WriteLine("Connecting to MySQL...");
                    conn.Open();

                    string sql1 = "DELETE FROM ingramevent WHERE EventID = @id"; 
                    MySql.Data.MySqlClient.MySqlCommand del = new MySql.Data.MySqlClient.MySqlCommand(sql1, conn);
                    del.Parameters.AddWithValue("@id", eventID);
                    del.ExecuteNonQuery();

                } catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                string messageText = "No event is currently saved.";
                string caption = "Please Select an event";
                MessageBoxButtons button = MessageBoxButtons.OK;
                MessageBox.Show(messageText, caption, button);
            }
        }

        public DateTime getStart()
        {
            return DateTime.Parse(start);
        }
        public DateTime getEnd()
        {
            return DateTime.Parse(end);
        }

        public String getTitle()
        {
            return title;
        }
        public String getLocation()
        {
            return Location;
        }
        public DateTime getReminder()
        {
            return DateTime.Parse(reminder);
        }
        public String getDescription()
        {
            return Description;
        }
        public static ArrayList getEventList(string dateString, Boolean m) 
        {
            ArrayList eventList = new ArrayList();  //a list to save the events
            //prepare an SQL query to retrieve all the events on the same, specified date
            DataTable myTable = new DataTable();
            string connStr = "server=csdatabase.eku.edu;user=stu_csc340;database=csc340_db;port=3306;password=Colonels18;";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT * FROM ingramEvent WHERE DATE(startD)>=@mySDate and Date(endD)<=@mySDate ORDER BY startD ASC";
                String year = dateString.Substring(0, 4);

                if (m) //m is true when getting all events for a month
                { 
                    dateString = dateString.Substring(5,2);
                    if(dateString[0] == '0')
                    {
                        dateString = dateString[1]+"";
                    }
                    Console.WriteLine(dateString);
                    sql = "SELECT * FROM ingramEvent WHERE ( month(startD)>=@mySDate and year(startD)>=@myYear ) and " +
                        "( month(endD)<=@mySDate and year(endD)<=@myYear ) ORDER BY startD ASC";
                }
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@mySDate", dateString);
                cmd.Parameters.AddWithValue("@myYear", year);

                MySqlDataAdapter myAdapter = new MySqlDataAdapter(cmd);
                myAdapter.Fill(myTable);
                Console.WriteLine("Table is ready.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            conn.Close();
            //convert the retrieved data to events and save them to the list
            foreach (DataRow row in myTable.Rows)
            {
                Event newEvent = new Event();
                newEvent.eventID = (int)row["EventID"];
                newEvent.title = row["title"].ToString();
                newEvent.start = row["startD"].ToString();
                newEvent.end = row["endD"].ToString();
                newEvent.Description = row["description"].ToString();
                newEvent.Location = row["Location"].ToString();
                newEvent.reminder = row["Reminder"].ToString();


                eventList.Add(newEvent);
            }
            return eventList;  //return the event list
        }

        public static void writeTo()
        {

        }

        public static String SQLDTformat(DateTime a) //yyyy-MM-dd hh:mm:ss
        {
            String ret;

            ret = a.Year.ToString() + "-";
            if (a.Month.ToString().Length == 1)
                ret = ret + "0";
            ret = ret + a.Month.ToString() + "-";
            if (a.Day.ToString().Length == 1)
                ret = ret + "0";
            ret = ret + a.Day.ToString() + " ";

            if (a.Hour.ToString().Length == 1)
                ret = ret + "0";
            ret = ret + a.Hour.ToString() + ":";
            if (a.Minute.ToString().Length == 1)
                ret = ret + "0";
            ret = ret + a.Minute.ToString() + ":";
            if (a.Second.ToString().Length == 1)
                ret = ret + "0";
            ret = ret + a.Second.ToString();

            System.Windows.Forms.MessageBox.Show(ret);

            return ret;
        }

    }


}
