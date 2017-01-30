using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace leerXML
{
    public partial class Logs : Form
    {
        SqlConnection con = new SqlConnection("Data Source=MEXQ-SERVER4;Initial Catalog=MEXQAppJulio;Persist Security Info=False;User ID=sa;Password=P@ssw0rd; MultipleActiveResultSets=true;");

        public Logs()
        {
            InitializeComponent();
            loadData();
        }
        private void loadData()
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.Connection = con;
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "SELECT NoteID,Error,Location,DateCreated FROM logxml;";
            SqlDataAdapter sqlDataAdap = new SqlDataAdapter(sqlCmd);
            DataTable dtRecord = new DataTable();
            sqlDataAdap.Fill(dtRecord);
            dgLogs.DataSource = dtRecord;
            DataGridViewColumn noteID = dgLogs.Columns[0]; noteID.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            DataGridViewColumn Error = dgLogs.Columns[1]; Error.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            DataGridViewColumn Location = dgLogs.Columns[2]; Location.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            DataGridViewColumn Fecha = dgLogs.Columns[3]; Fecha.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
        }
    }
}
