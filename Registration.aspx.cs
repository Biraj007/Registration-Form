using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Registration_Form
{
    public partial class Registration : System.Web.UI.Page
    {
        // Connection string to your local database
        string conString = "Data Source=(localdb)\\mylocodb;Initial Catalog=Registration;Integrated Security=True";

        // Runs on first page load only (avoids reloading grid on every button click)
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadGrid(); // Load existing user data into grid
        }

        // Method to hash passwords using SHA256 encryption for security
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString(); // Returns hashed string
            }
        }

        // Register new user
        protected void Register_Click(object sender, EventArgs e)
        {
            // Check if Password and Confirm Password match
            if (Password.Text.Trim() != ConPas.Text.Trim())
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Password and Confirm Password do not match.');", true);
                return; // Stop registration if mismatch
            }

            // Insert user details into Reg_Table and Login_Details via Stored Procedure
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand("ManageUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "INSERT");
                    cmd.Parameters.AddWithValue("@Name", Name.Text.Trim());
                    cmd.Parameters.AddWithValue("@Surname", Surname.Text.Trim());
                    cmd.Parameters.AddWithValue("@DOB", DOB.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", Email.Text.Trim());
                    cmd.Parameters.AddWithValue("@Password", HashPassword(Password.Text.Trim()));

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            Response.Redirect(Request.RawUrl); // Refresh page to reload grid and clear fields
        }

        // Clear all input fields and reset form
        protected void Cancel_Click(object sender, EventArgs e)
        {
            ClearFields();
            Register.Visible = true;
            UpdateBtn.Visible = false;
            confirmRow.Visible = true;
        }

        // Load data from Reg_Table into GridView
        private void LoadGrid()
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand("ManageUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "SELECT");

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }
                }
            }
        }

        // Clears form fields
        private void ClearFields()
        {
            Name.Text = string.Empty;
            Surname.Text = string.Empty;
            DOB.Text = string.Empty;
            Email.Text = string.Empty;
            Password.Text = string.Empty;
            ConPas.Text = string.Empty;
            HiddenUserID.Value = string.Empty;
        }

        // Handles Edit and Delete button actions from GridView
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument); // Get selected record's ID

            // When Edit button clicked
            if (e.CommandName == "EditRow")
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand("ManageUser", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GETONE");
                        cmd.Parameters.AddWithValue("@id", id);

                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            // Populate form fields with selected record
                            Name.Text = dr["Name"].ToString();
                            Surname.Text = dr["Surname"].ToString();
                            DOB.Text = Convert.ToDateTime(dr["DOB"]).ToString("yyyy-MM-dd");
                            Email.Text = dr["Email"].ToString();
                            HiddenUserID.Value = id.ToString();
                        }
                    }
                }

                // Show Update button, hide Register button and Confirm Password
                Register.Visible = false;
                UpdateBtn.Visible = true;
                Password.Text = "";
                ConPas.Text = "";
                confirmRow.Visible = false;
            }

            // When Delete button clicked
            if (e.CommandName == "DeleteRow")
            {
                using (SqlConnection con = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand("ManageUser", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "DELETE");
                        cmd.Parameters.AddWithValue("@id", id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                Response.Redirect(Request.RawUrl); // Refresh grid after deletion
            }
        }

        // Update record after verifying password
        protected void UpdateBtn_Click(object sender, EventArgs e)
        {
            string enteredPassword = Password.Text.Trim();
            string hashedEnteredPassword = HashPassword(enteredPassword);
            string storedPasswordHash = "";

            // Fetch stored password from Login_Details to verify
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand("ManageUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "GETPASS");
                    cmd.Parameters.AddWithValue("@Email", Email.Text.Trim());

                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        storedPasswordHash = result.ToString();
                }
            }

            // If entered password doesn't match stored hash, deny update
            if (hashedEnteredPassword != storedPasswordHash)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Incorrect Password. Update Denied.');", true);
                return;
            }

            // If password correct, update record
            using (SqlConnection con = new SqlConnection(conString))
            {
                using (SqlCommand cmd = new SqlCommand("ManageUser", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "UPDATE");
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(HiddenUserID.Value));
                    cmd.Parameters.AddWithValue("@Name", Name.Text.Trim());
                    cmd.Parameters.AddWithValue("@Surname", Surname.Text.Trim());
                    cmd.Parameters.AddWithValue("@DOB", DOB.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", Email.Text.Trim());

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            Response.Redirect(Request.RawUrl); // Refresh grid after update
        }
    }
}
