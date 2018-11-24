using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace ConsumerMaster
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.BindGrid();
            }
        }

        private void BindGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnStringDb1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Consumers_CRUD"))
                {
                    cmd.Parameters.AddWithValue("@Action", "SELECT");
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            GridView1.DataSource = dt;
                            GridView1.DataBind();
                        }
                    }
                }
            }
        }

        protected void Insert(object sender, EventArgs e)
        {
            string Username = txtUsername.Text;
            string Provincename = txtProvinceName.Text;
            string Cityname = txtCityname.Text;
            string Number = txtNumber.Text;
            string Name = txtName.Text;
            string ContentType = txtContentType.Text;
            string Data = txtData.Text;

            string consumer_first = txtUserName.Text;
            string consumer_last = txtUserName") as TextBox).Text;
            string consumer_middle = (row.FindControl("txtUserName") as TextBox).Text;
            string date_of_birth = (row.FindControl("txtUserName") as TextBox).Text;
            string address_line_1 = (row.FindControl("txtUserName") as TextBox).Text;
            string address_line_2 = (row.FindControl("txtUserName") as TextBox).Text;
            string city = (row.FindControl("txtProvincename") as TextBox).Text;
            string state = (row.FindControl("txtCityname") as TextBox).Text;
            string zip_code = (row.FindControl("txtNumber") as TextBox).Text;
            string identifier = (row.FindControl("txtName") as TextBox).Text;
            string gender = (row.FindControl("txtContentType") as TextBox).Text;
            string diagnosis = (row.FindControl("txtContentType") as TextBox).Text;
            string nickname_first = (row.FindControl("txtContentType") as TextBox).Text;
            string nickname_last = (row.FindControl("txtData") as TextBox).Text;


            string constr = ConfigurationManager.ConnectionStrings["ConnStringDb1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Consumers_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "INSERT");
                    cmd.Parameters.AddWithValue("@consumer_internal_number", consumer_internal_number);
                    cmd.Parameters.AddWithValue("@consumer_first", consumer_first);
                    cmd.Parameters.AddWithValue("@consumer_last", consumer_last);
                    cmd.Parameters.AddWithValue("@consumer_middle", consumer_middle);
                    cmd.Parameters.AddWithValue("@date_of_birth", date_of_birth);
                    cmd.Parameters.AddWithValue("@address_line_1", address_line_1);
                    cmd.Parameters.AddWithValue("@address_line_2", address_line_2);
                    cmd.Parameters.AddWithValue("@city", city);
                    cmd.Parameters.AddWithValue("@state", state);
                    cmd.Parameters.AddWithValue("@zip_code", zip_code);
                    cmd.Parameters.AddWithValue("@identifier", identifier);
                    cmd.Parameters.AddWithValue("@gender", gender);
                    cmd.Parameters.AddWithValue("@diagnosis", diagnosis);
                    cmd.Parameters.AddWithValue("@nickname_first", nickname_first);
                    cmd.Parameters.AddWithValue("@nickname_last", nickname_last);

                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            this.BindGrid();
        }

        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            this.BindGrid();
        }

        protected void OnRowCancelingEdit(object sender, EventArgs e)
        {
            GridView1.EditIndex = -1;
            this.BindGrid();
        }

        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView1.Rows[e.RowIndex];
            int consumer_internal_number = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            string consumer_first = (row.FindControl("txtUserName") as TextBox).Text;
            string consumer_last = (row.FindControl("txtUserName") as TextBox).Text;
            string consumer_middle = (row.FindControl("txtUserName") as TextBox).Text;
            string date_of_birth = (row.FindControl("txtUserName") as TextBox).Text;
            string address_line_1 = (row.FindControl("txtUserName") as TextBox).Text;
            string address_line_2 = (row.FindControl("txtUserName") as TextBox).Text;
            string city = (row.FindControl("txtProvincename") as TextBox).Text;
            string state = (row.FindControl("txtCityname") as TextBox).Text;
            string zip_code = (row.FindControl("txtNumber") as TextBox).Text;
            string identifier = (row.FindControl("txtName") as TextBox).Text;
            string gender = (row.FindControl("txtContentType") as TextBox).Text;
            string diagnosis = (row.FindControl("txtContentType") as TextBox).Text;
            string nickname_first = (row.FindControl("txtContentType") as TextBox).Text;
            string nickname_last = (row.FindControl("txtData") as TextBox).Text;

            string constr = ConfigurationManager.ConnectionStrings["ConnStringDb1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Consumers_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "UPDATE");
                    cmd.Parameters.AddWithValue("@consumer_internal_number", consumer_internal_number);
                    cmd.Parameters.AddWithValue("@consumer_first", consumer_first);
                    cmd.Parameters.AddWithValue("@consumer_last", consumer_last);
                    cmd.Parameters.AddWithValue("@consumer_middle", consumer_middle);
                    cmd.Parameters.AddWithValue("@date_of_birth", date_of_birth);
                    cmd.Parameters.AddWithValue("@address_line_1", address_line_1);
                    cmd.Parameters.AddWithValue("@address_line_2", address_line_2);
                    cmd.Parameters.AddWithValue("@city", city);
                    cmd.Parameters.AddWithValue("@state", state);
                    cmd.Parameters.AddWithValue("@zip_code", zip_code);
                    cmd.Parameters.AddWithValue("@identifier", identifier);
                    cmd.Parameters.AddWithValue("@gender", gender);
                    cmd.Parameters.AddWithValue("@diagnosis", diagnosis);
                    cmd.Parameters.AddWithValue("@nickname_first", nickname_first);
                    cmd.Parameters.AddWithValue("@nickname_last", nickname_last);

                    //cmd.Parameters.AddWithValue("@ContentType", SqlDbType.VarBinary, -1);
                    //cmd.Parameters.AddWithValue("@Data", SqlDbType.VarBinary, -1);

                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            GridView1.EditIndex = -1;
            this.BindGrid();
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int consumer_internal_number = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values[0]);
            string constr = ConfigurationManager.ConnectionStrings["ConnStringDb1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Consumers_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "DELETE");
                    cmd.Parameters.AddWithValue("@consumer_internal_number",consumer_internal_number);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            this.BindGrid();
        }

        protected void OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            this.BindGrid();
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView1.EditIndex)
            //{
            //    (e.Row.Cells[2].Controls[2] as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            //}
        }

        protected void DownloadFile(object sender, EventArgs e)
        {
            int id = int.Parse((sender as LinkButton).CommandArgument);
            byte[] bytes;
            string fileName, contentType;
            string constr = ConfigurationManager.ConnectionStrings["ConnStringDb1"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "select Name, Data, ContentType from tblbooking where BId=@BId";
                    cmd.Parameters.AddWithValue("@BId", id);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["Data"];
                        contentType = sdr["ContentType"].ToString();
                        fileName = sdr["Name"].ToString();
                    }
                    con.Close();
                }
            }
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = contentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }
    }
}