using System.Data;
using System.Data.SqlClient;

string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LibraryDataBase;Integrated Security=True";
Console.WriteLine(DateTime.Parse("01.01.2021").ToString("yyyy-MM-dd"));


static void GetDatabaseList(string conString)
{
    var list = new List<string>();
    using SqlConnection con = new SqlConnection(conString);
    con.Open();
    using SqlCommand cmd = new SqlCommand("SELECT CONCAT('DELETE FROM ', TABLE_NAME) FROM INFORMATION_SCHEMA.TABLES " +
                                          "WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_NAME<>'sysdiagrams'", con);
    var s = cmd.ExecuteNonQuery();
    Console.WriteLine(s);
}


static void DisplayData(DataTable table)
{

    //foreach (DataRow row in table.Rows)
    //{

    //    foreach (DataColumn col in table.Columns)
    //    {

    //        Console.WriteLine("{0} = {1}", col.ColumnName, row[col]);
    //    }
    //    Console.WriteLine("============================");
    //}
}