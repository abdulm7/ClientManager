using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace ClientManager.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<Client> clientList = new List<Client>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=clients;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM clients";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                       using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Client clientData = new Client();
                                clientData.id = reader.GetInt32(0).ToString();
                                clientData.name = reader.GetString(1);
                                clientData.email = reader.GetString(2);
                                clientData.phone = reader.GetString(3);
                                clientData.address = reader.GetString(4);
                                clientData.created_at = reader.GetDateTime(5).ToString();

                                clientList.Add(clientData);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: " + e.ToString());
            }
        }
    }

    public class Client {
        public String id;
        public String name;
        public String email;
        public String phone;
        public String address;
        public String created_at;
    }
}
