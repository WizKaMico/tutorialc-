using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace myWebAppTest1.Pages.Products
{
    public class IndexModel : PageModel
    {
        public List<productModelList> productInfo = new List<productModelList>();

        public productModelList insertProduct = new productModelList();
        public void OnGet()
        {
            String connectionString = "Data Source=thinkitsolution\\sqlexpress;Initial Catalog=thinktestnowdb;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string viewListOfProduct = "EXEC myListOfProduct";
                using (SqlCommand command = new SqlCommand(viewListOfProduct, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader()) 
                    {
                        while (reader.Read())
                        {
                            productModelList info = new productModelList();
                            info.productId = reader.GetInt32(0);
                            info.productName = reader.GetString(1);
                            info.productPrice = reader.GetDecimal(2);
                            info.brandName = reader.GetString(6).ToString();
                            info.dateCreated = reader.GetDateTime(4).ToString();
                            productInfo.Add(info);
                        }
                    }
                }
            }
        }

        public void OnPost() 
        {
            string productName = Request.Form["productName"];
            double productPrice = double.Parse(Request.Form["productPrice"]);
            int productBrand = int.Parse(Request.Form["productBrand"]); 

            try
            {

                String connectionString = "Data Source=thinkitsolution\\sqlexpress;Initial Catalog=thinktestnowdb;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string insertProduct = "EXEC myProductInserting @productName, @productPrice, @productBrand";
                    using (SqlCommand command = new SqlCommand(insertProduct, connection))
                    {
                        command.Parameters.AddWithValue("productName", productName);
                        command.Parameters.AddWithValue("productPrice", productPrice);
                        command.Parameters.AddWithValue("productBrand", productBrand);
                        command.ExecuteNonQuery();
                    }
                }
                Response.Redirect("/Products/Index/?message=success");
            }catch(Exception ex)
            {
                throw;
                Console.WriteLine(ex.Message);
            }
        }

    }

    public class productModelList
    {
        public int productId;
        public string productName;
        public int productBrand;
        public decimal productPrice;
        public string brandName;
        public string dateCreated;
    }

}
