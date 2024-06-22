using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SkillsInternational
{
    public partial class frmStudent_Registration : Form
    {
        // Use a constant for the connection string
        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=Student;Integrated Security=True";

        DataTable dt;
        SqlDataAdapter adapter;
        SqlCommand cmd;
        DataSet ds = new DataSet();
        int empid;
        string g;

        public frmStudent_Registration()
        {
            InitializeComponent();
            LoadRegistrationNumbers();

        }

        void dataCode()
        {
            using (SqlConnection cnct = new SqlConnection(ConnectionString))
            {
                cnct.Open();
                dt = new DataTable();
                adapter = new SqlDataAdapter("SELECT * FROM Registration", cnct);
                adapter.Fill(dt);
            }
        }

        void clear()
        {
            cmbregno.Items.Clear();  
            txtfullname.Clear();
            txtlastname.Clear();
            txtaddress.Clear();
            txtemail.Clear();
            txtmobilephone.Clear();
            txthomeohone.Clear();
            txtparentname.Clear();
            txtnic.Clear();
            txtcntactno.Clear();
        }

        private void btnregister_Click(object sender, EventArgs e)
        {
            if (rdbmale.Checked)
            {
                g = "Male";
            }
            else
            {
                g = "Female";
            }

            using (SqlConnection cnct = new SqlConnection(ConnectionString))
            {
                cnct.Open();
                if (txtfullname.Text != string.Empty && txtlastname.Text != string.Empty && txtnic.Text != string.Empty)
                {
                    cmd = new SqlCommand("SELECT * FROM Registration WHERE firstName=@fname AND lastName=@lname AND nic=@nic", cnct);
                    cmd.Parameters.AddWithValue("@fname", txtfullname.Text);
                    cmd.Parameters.AddWithValue("@lname", txtlastname.Text);
                    cmd.Parameters.AddWithValue("@nic", txtnic.Text);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            MessageBox.Show("This Data Already Exists!");
                            ds.Clear();
                            return;
                        }
                    }
                }

                cmd = new SqlCommand("INSERT INTO Registration(regNo,firstName,lastName,dateOfBirth,gender,address,email,mobilePhone,homePhone,parentName,nic,contactNo) " +
                    "VALUES(@regNo,@fname,@lname,@dob,@gen,@add,@mail,@mPhn,@hPhn,@pName,@nic,@cNo)", cnct);
                cmd.Parameters.AddWithValue("@regNo", cmbregno.Text);
                cmd.Parameters.AddWithValue("@fname", txtfullname.Text);
                cmd.Parameters.AddWithValue("@lname", txtlastname.Text);
                cmd.Parameters.AddWithValue("@dob", dtpdob.Text);
                cmd.Parameters.AddWithValue("@gen", g);
                cmd.Parameters.AddWithValue("@add", txtaddress.Text);
                cmd.Parameters.AddWithValue("@mail", txtemail.Text);
                cmd.Parameters.AddWithValue("@mPhn", txtmobilephone.Text);
                cmd.Parameters.AddWithValue("@hPhn", txthomeohone.Text);
                cmd.Parameters.AddWithValue("@pName", txtparentname.Text);
                cmd.Parameters.AddWithValue("@nic", txtnic.Text);
                cmd.Parameters.AddWithValue("@cNo", txtcntactno.Text);

                cmd.ExecuteNonQuery();
                clear();
                MessageBox.Show("Record added successfully!", "Register Student", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            LoadRegistrationNumbers();
        }

        private void cmbregno_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection cnct = new SqlConnection(ConnectionString))
            {
                cnct.Open();
                string registrationNumber = cmbregno.SelectedItem.ToString();
                string query = "SELECT firstName, lastName, dateOfBirth, gender, address, email, mobilePhone, homePhone, parentName, nic, contactNo FROM Registration WHERE regNo = @RegistrationNumber";

                using (SqlCommand cmd = new SqlCommand(query, cnct))
                {
                    cmd.Parameters.AddWithValue("@RegistrationNumber", registrationNumber);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtfullname.Text = reader["firstName"].ToString();
                            txtlastname.Text = reader["lastName"].ToString();

                            // Assuming dateOfBirth is a DateTime field in the database
                            if (reader["dateOfBirth"] != DBNull.Value)
                            {
                                dtpdob.Value = (DateTime)reader["dateOfBirth"];
                            }

                            // Assuming gender is a string field (e.g., "Male" or "Female") in the database
                            string gender = reader["gender"].ToString();
                            if (gender == "Male")
                            {
                                rdbmale.Checked = true;
                            }
                            else if (gender == "Female")
                            {
                                rdbfemale.Checked = true;
                            }

                            txtaddress.Text = reader["address"].ToString();
                            txtemail.Text = reader["email"].ToString();
                            txtmobilephone.Text = reader["mobilePhone"].ToString();
                            txthomeohone.Text = reader["homePhone"].ToString();
                            txtparentname.Text = reader["parentName"].ToString();
                            txtnic.Text = reader["nic"].ToString();
                            txtcntactno.Text = reader["contactNo"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No data found for the selected registration number.");
                        }
                    }
                }
            }
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            clear();
            LoadRegistrationNumbers();

        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
           
            using (SqlConnection cnct = new SqlConnection(ConnectionString))
            {
                if (rdbmale.Checked)
                {
                    g = "Male";
                }
                else
                {
                    g = "Female";
                }
                cnct.Open();
                cmd = new SqlCommand("UPDATE Registration SET firstName=@fname, lastName=@lname, dateOfBirth=@dob, gender=@gen, " +
                    "address=@add, email=@mail, mobilePhone=@mPhn, homePhone=@hPhn, parentName=@pName, nic=@nic, contactNo=@cNo WHERE regNo = @regNo", cnct);
                cmd.Parameters.AddWithValue("@regNo", cmbregno.Text);
                cmd.Parameters.AddWithValue("@fname", txtfullname.Text);
                cmd.Parameters.AddWithValue("@lname", txtlastname.Text);
                cmd.Parameters.AddWithValue("@dob", dtpdob.Value);
                cmd.Parameters.AddWithValue("@gen", g);
                cmd.Parameters.AddWithValue("@add", txtaddress.Text);
                cmd.Parameters.AddWithValue("@mail", txtemail.Text);
                cmd.Parameters.AddWithValue("@mPhn", int.Parse(txtmobilephone.Text));
                cmd.Parameters.AddWithValue("@hPhn", int.Parse(txthomeohone.Text));
                cmd.Parameters.AddWithValue("@pName", txtparentname.Text);
                cmd.Parameters.AddWithValue("@nic", txtnic.Text);
                cmd.Parameters.AddWithValue("@cNo", int.Parse(txtcntactno.Text));

                cmd.ExecuteNonQuery();
                clear();
                MessageBox.Show("Record Updated successfully!", "Update Student", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);


            }
            LoadRegistrationNumbers();

        }

        private void LoadRegistrationNumbers()
        {
            using (SqlConnection cnct = new SqlConnection(ConnectionString))
            {
                try
                {
                    cnct.Open();
                    cmbregno.Items.Clear();
                    string query = "SELECT regNo FROM Registration";

                    using (SqlCommand command = new SqlCommand(query, cnct))
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                cmbregno.Items.Add(reader["regNo"].ToString());
                            }
                        }
                        else
                        {
                            MessageBox.Show("No registration numbers found in the database.", "Load Registration Numbers", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Load Registration Numbers", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmStudent_Registration_Load(object sender, EventArgs e)
        {

        }

        private void btndelete_Click(object sender, EventArgs e)
        {
            if (cmbregno.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a registration number to delete.", "Delete Student", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure, Do you really want to delete this record?", "Delete    ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                using (SqlConnection cnct = new SqlConnection(ConnectionString))
                {
                    cnct.Open();
                    string query = "DELETE FROM Registration WHERE regNo = @RegistrationNumber";

                    using (SqlCommand command = new SqlCommand(query, cnct))
                    {
                        command.Parameters.AddWithValue("@RegistrationNumber", cmbregno.SelectedItem.ToString());

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record deleted successfully!", "Delete Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            clear();
                            LoadRegistrationNumbers();
                        }
                        else
                        {
                            MessageBox.Show("No records found for the selected registration number.", "Delete Student", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            

        }

        private void llblexit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure, Do you really want to Exit...?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
          
        }

        private void llbllogoout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
            frmLogin frmLogin = new frmLogin();
            frmLogin.Show();
        }
    }
}
