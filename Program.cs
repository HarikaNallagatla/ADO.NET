using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Speech.Synthesis;
using System.Data;


namespace ADO.net1
{
    class Program
    {
        /*Please add the system.speech assembly to the project to use the speak method*/

        //  static string conString = @"Server=INCHCMPC11363;Database=assignmentDatabase;Trusted_Connection=True;";
        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder(@"Server=");
            Console.WriteLine("Please enter the server name");
            sb.Append(Console.ReadLine());
            sb.Append(";Database=");
            Console.WriteLine("Please enter the database name");
            sb.Append(Console.ReadLine());
            sb.Append(";Trusted_Connection=True;");
            Console.WriteLine(sb);
            try
            {
                string confirm;
                do
                {
                    Console.WriteLine("1.Insert\n2.Update\n3.Delete\n4.Display");
                    int choice = GetInt("Please choose the valid option");
                    SpeechSynthesizer speaker = new SpeechSynthesizer();
                    switch (choice)
                    {
                        case 1:
                            InsertData(sb);
                            speaker.Speak("Data is inserted successfully");
                            break;
                        case 2:
                            Update(sb);
                            speaker.Speak("Data is updated successfully");
                            break;
                        case 3:
                            DeleteData(sb);
                            speaker.Speak("Data is deleted successfully");
                            break;
                        case 4:
                            Display(sb);
                            break;
                        default:
                            Console.WriteLine("Please choose the valid option");
                            break;
                    }
                    Console.WriteLine("Press y to continue...");
                    confirm = Console.ReadLine().ToUpper();
                } while (confirm == "Y");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }

        private static int GetInt(string v)
        {
            int val = 0;
            while (true)
            {
                Console.WriteLine(v);
                if (int.TryParse(Console.ReadLine(),out val))
                {
                    break;
                }
                Console.WriteLine("The entered number is not in the correct format");
                Console.ReadLine();
            }
            return val;
        }

        private static void Update(StringBuilder sb)
        {
            string commandtext = @"update  sectors set sector_name = @sector_name where sector_id = @sector_id";
            using (SqlConnection con = new SqlConnection(sb.ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(commandtext,con))
                {
                    Console.WriteLine("Please enter the sector name");
                    cmd.Parameters.AddWithValue("@sector_name", Console.ReadLine());
                    Console.WriteLine("Please enter the sector_id");
                    cmd.Parameters.Add("@sector_id", SqlDbType.Int);
                    cmd.Parameters["@sector_id"].Value = int.Parse(Console.ReadLine());
                    cmd.ExecuteNonQuery();
                }
            }

        }

        private static void DeleteData(StringBuilder sb)
        {
            string stringg = @"Delete from sectors where sector_id = @sector_id";
            using (SqlConnection con = new SqlConnection(sb.ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(stringg, con))
                {
                    cmd.Parameters.Add("@sector_id", SqlDbType.Int);
                    Console.WriteLine("Please enter the sector_id");
                    cmd.Parameters["@sector_id"].Value = int.Parse(Console.ReadLine());
                    cmd.ExecuteNonQuery();
                }

            }
        }

        private static void InsertData(StringBuilder sb)
        {
            Console.WriteLine("Please enter the data to be inserted");
            string insertdatstring = @"Insert into sectors values('" + Console.ReadLine() + "')";
            Console.WriteLine(insertdatstring);
            using (SqlConnection con = new SqlConnection(sb.ToString()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(insertdatstring, con))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        static void Display(StringBuilder sb)
        {
            Console.WriteLine("Enter the Table name to display the results");
            string selectALLString = @"Select * from " + Console.ReadLine();
            using (SqlConnection con = new SqlConnection(sb.ToString()))
            {
                con.Open();

                /*Error Execute reader : connection property is not intialised*/
                // using (SqlCommand cmd = new SqlCommand(selectALLString))
                using (SqlCommand cmd = new SqlCommand(selectALLString, con))
                {
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Console.WriteLine($"The sectors id:{rdr[0]}| sectorsName:{rdr[1]}");
                    }
                    Console.ReadLine();
                }
            }
        }
    }
}
