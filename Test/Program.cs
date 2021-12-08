using System.Data;
using System.Data.SqlClient;
using System.Xml;

string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LibraryDataBase;Integrated Security=True";
//using (SqlConnection connection = new SqlConnection(connectionString))
//{
//    // Connect to the database then retrieve the schema information.  
//    connection.Open();
//    DataTable table = connection.GetSchema("Tables");

//    // Display the contents of the table.  
//    DisplayData(table);
//    Console.WriteLine("Press any key to continue.");
//    Console.ReadKey();
//}
GetDatabaseList(connectionString);


static void GetDatabaseList(string conString)
{
    using (SqlConnection con = new SqlConnection(conString))
    {
        con.Open();
        using (SqlCommand cmd = new SqlCommand("SELECT TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME, ORDINAL_POSITION, COLUMN_DEFAULT, DATA_TYPE," +
                                               " CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, NUMERIC_PRECISION_RADIX, NUMERIC_SCALE, DATETIME_PRECISION " +
                                               "FROM INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA in ('dbo','meta') and table_name in " +
                                               "(select name from sys.tables) order by TABLE_SCHEMA, TABLE_NAME ,ORDINAL_POSITION", con))
        {
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Console.WriteLine(rdr.GetValue(0) + " " + rdr.GetValue(1) + " " + rdr.GetValue(2) + " " + rdr.GetValue(3) +
                                  rdr.GetValue(4) + " " + rdr.GetValue(5) + " " + rdr.GetValue(6) + " " + rdr.GetValue(7));
            }
        }
    }
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