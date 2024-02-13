using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace myWebAppTest1.Pages.Products
{
    public class EditModel : PageModel
    {
        public productModelList infoProduct = new productModelList();
        public void OnGet()
        {
            int productId = int.Parse(Request.Query["productId"]);

            try
            {
                String connectionString = "Data Source=thinkitsolution\\sqlexpress;Initial Catalog=thinktestnowdb;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string checkExistence = "EXEC checkProductExistence @productId";
                    using (SqlCommand command = new SqlCommand(checkExistence, connection))
                    { 
                        command.Parameters.AddWithValue("productId", productId);
                        command.ExecuteNonQuery();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                infoProduct.productId = reader.GetInt32(0);
                                infoProduct.productName = reader.GetString(1);
                                infoProduct.productPrice = reader.GetDecimal(2);
                                infoProduct.productBrand = reader.GetInt32(3);
                            }
                        }
                    }
                }
            
            }
            catch (Exception ex) 
            {
                throw;
                Console.WriteLine(ex.Message);
            }
        }

        public void OnPost()
        {
            int productId = int.Parse(Request.Query["ProductId"]);
            string productName = Request.Form["productName"];
            decimal productPrice = decimal.Parse(Request.Form["productPrice"]);
            int productBrand = int.Parse(Request.Form["productBrand"]);

            try
            {
                String connectionString = "Data Source=thinkitsolution\\sqlexpress;Initial Catalog=thinktestnowdb;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string updateSpecificProduct = "EXEC updateProductInformation @productId, @productName, @productPrice, @productBrand";
                    using (SqlCommand command = new SqlCommand(updateSpecificProduct, connection))
                    {
                        command.Parameters.AddWithValue("productId", productId);
                        command.Parameters.AddWithValue("productName", productName);
                        command.Parameters.AddWithValue("productPrice", productPrice);
                        command.Parameters.AddWithValue("productBrand", productBrand);
                        command.ExecuteNonQuery();
                    }
                }
                Response.Redirect("/Products/Index/?response=success");
            }
            catch (Exception ex)
            {
                throw; 
                Console.WriteLine(ex.Message + "\n");   
            }
        }
    }
}
