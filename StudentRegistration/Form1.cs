using System.Data.SqlClient;

namespace StudentRegistration
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadDataGrid();
        }

        SqlConnection con = new SqlConnection("Data Source=DESKTOP-LG43OQS; Initial Catalog=students; User Id=sa; Password=marvino12796");
        SqlCommand cmd;
        SqlDataReader read;
        string id;
        bool Mode = true; // means to add records otherwise update
        string sql;

        public void LoadDataGrid()
        {
            try
            {
                sql = "select * from student";
                cmd = new SqlCommand(sql, con);
                con.Open();

                read = cmd.ExecuteReader();

                dataGridView1.Rows.Clear();

                while (read.Read())
                {
                    dataGridView1.Rows.Add(read[0], read[1], read[2], read[3]);
                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void getID(String id)
        {
            sql = "select * from student where id = '" + id + "'  ";
            cmd = new SqlCommand(sql, con);
            con.Open();
            read = cmd.ExecuteReader();

            while (read.Read())
            {
                textName.Text = read[1].ToString();
                textCourse.Text = read[2].ToString();
                textFee.Text = read[3].ToString();
            }
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string name = textName.Text;
            string course = textCourse.Text;
            string fee = textFee.Text;

            if (Mode == true)
            {
                sql = "insert into student(stname,course,fee) values(@stname,@course,@fee)";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@stname", name);
                cmd.Parameters.AddWithValue("@course", course);
                cmd.Parameters.AddWithValue("@fee", fee);
                MessageBox.Show("Record Added!");
                cmd.ExecuteNonQuery();

                textName.Clear();
                textCourse.Clear();
                textFee.Clear();
                textName.Focus();
            }
            else
            {
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql = "update student set stname = @stname, course = @course, fee = @fee where id = @id";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@stname", name);
                cmd.Parameters.AddWithValue("@course", course);
                cmd.Parameters.AddWithValue("@fee", fee);
                cmd.Parameters.AddWithValue("@id", id);
                MessageBox.Show("Record Updated!");
                cmd.ExecuteNonQuery();

                textName.Clear();
                textCourse.Clear();
                textFee.Clear();
                textName.Focus();
                button2.Text = "Save";
                Mode = true;
            }
            con.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Edit"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                getID(id);
                button2.Text = "Edit";
            }
            else if(e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                sql = "delete from student where  = @id ";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id ", id);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Deleted!");
                con.Close();   
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadDataGrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textName.Clear();
            textCourse.Clear();
            textFee.Clear();
            textName.Focus();
            button2.Text = "Save";
            Mode = true;
        }
    }
}