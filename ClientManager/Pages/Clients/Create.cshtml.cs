using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Net.Security;

namespace ClientManager.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public Client clientData = new Client();

        public String errorMsg = "";
        public String successMsg = "";
        public void OnGet()
        {

        }

        public void OnPost() {
            clientData.name = Request.Form["name"];
            clientData.email = Request.Form["email"];
            clientData.phone = Request.Form["phone"];
            clientData.address = Request.Form["address"];

            if (clientData.name.Length == 0 || clientData.email.Length == 0 || clientData.phone.Length == 0 || clientData.address.Length == 0)
            {
                errorMsg = "All the fields are required";
                return;
            }

            // save new client into database
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=clients;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO clients " + "(name, email, phone, address) VALUES" + "(@name, @email, @phone, @address);";
                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", clientData.name);
                        command.Parameters.AddWithValue("@email", clientData.email);
                        command.Parameters.AddWithValue("@phone", clientData.phone);
                        command.Parameters.AddWithValue("@address", clientData.address);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception e)
            {
                errorMsg = e.Message;
                return;
            }

            clientData.name = "";
            clientData.email = "";
            clientData.phone = "";
            clientData.address = "";
            successMsg = "Successfully added new Client!";

            Response.Redirect("/Clients/Index");
        }
    }
}
