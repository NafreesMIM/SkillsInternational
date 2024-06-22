using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace SkillsInternational
{
    public partial class frmLogin : Form
    {
        SqlConnection cnct = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=Student;Integrated Security=True");
        DataTable dt;
        SqlCommand cmd;
        public frmLogin()
        {
            InitializeComponent();
            textBox6.Select();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }
        private void btnlogin_Click(object sender, EventArgs e)
        {
            cnct.Open();
            string log = "SELECT * FROM Logins WHERE username=@uname AND password=@pswd";
            cmd = new SqlCommand(log, cnct);
            cmd.Parameters.AddWithValue("@uname", textBox6 .Text);
            cmd.Parameters.AddWithValue("@pswd", textBox5.Text);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            dt = new DataTable();
            adapter.Fill(dt);

            // You should execute the query before checking the results
            int rowCount = dt.Rows.Count;

            cnct.Close();
            if (textBox6.Text != string.Empty && textBox5.Text != string.Empty)
            {
                if (rowCount > 0)
                {
                    //MessageBox.Show("Login Success!", "Login", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmStudent_Registration register = new frmStudent_Registration();
                    register.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("invalid Login credentials, please check Username and Password and try again.", "invalid login Details", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox6.Clear();
                    textBox5.Clear();
                    textBox6.Select();
                }
            }
            else
            {
                MessageBox.Show("Please Enter Your Username and Password!", "Worning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox6.Select();
            }


        }
        private void btnclear_Click(object sender, EventArgs e)
{
            textBox6.Clear();
            textBox5.Clear();
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure, Do you really want to Exit...?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
           

        }
    }
}
