using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ClientManager.Pages.Clients
{
    public class EditModel : PageModel
    {
        public Client clientData = new Client();
        public String errorMsg = "";
        public String successMsg = "";
        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=clients;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM clients WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader()) {

                            if (reader.Read())
                            {
                                clientData.id = "" + reader.GetInt32(0);
                                clientData.name = reader.GetString(1);
                                clientData.email = reader.GetString(2);
                                clientData.phone= reader.GetString(3);
                                clientData.address  = reader.GetString(4); 
                            } 
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                errorMsg = ex.Message;
            }
        }

        public void OnPost()
        {
            clientData.id = Request.Form["id"];
            clientData.name = Request.Form["name"];
            clientData.email = Request.Form["email"];
            clientData.phone = Request.Form["phone"];
            clientData.address = Request.Form["address"];


            if (clientData.name.Length == 0 || clientData.email.Length == 0 || 
                clientData.phone.Length == 0 || clientData.address.Length == 0)
            {
                errorMsg = "All the fields are required";
                return;
            }

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=clients;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE clients " + 
                        "SET name=@name, email=@email, phone=@phone, address=@address " + 
                        "WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", clientData.id);
                        command.Parameters.AddWithValue("@name", clientData.name);
                        command.Parameters.AddWithValue("@email", clientData.email);
                        command.Parameters.AddWithValue("@phone", clientData.phone);
                        command.Parameters.AddWithValue("@address", clientData.address);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                errorMsg = ex.Message;
                return;
            }

            Response.Redirect("/Clients/Index");
        }
    }
}
