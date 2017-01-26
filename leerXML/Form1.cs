using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Data.SqlClient;

namespace leerXML
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection("Data Source = ATALAYA-STD;" + "Initial Catalog = CSRAPP ; Integrated Security = true; MultipleActiveResultSets=true;");
        //SqlConnection con = new SqlConnection("Data Source=MEXQ-SERVER4;Initial Catalog=MEXQAppJulio;Persist Security Info=False;User ID=sa;Password=P@ssw0rd; MultipleActiveResultSets=true;");
        string periodo,ruta,insertaD;
        string UUID,idNota,folio,serie,fecha,cliente,rfc;
        SqlCommand comando,cmd;
        XmlReader reader;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLeer_Click(object sender, EventArgs e)
        {
            //try
            //{
                UUID = string.Empty;
                periodo = txtRuta.Text;
                string query = "EXEC vData '" + periodo + "'";
                comando = new SqlCommand(query, con);
                con.Open();
                SqlDataReader leer = comando.ExecuteReader();
                if (leer.HasRows)
                {
                    while (leer.Read())
                    {
                        ruta = leer["Location"].ToString();
                        idNota = leer["NoteID"].ToString();
                        reader = XmlReader.Create(ruta);
                            while (reader.Read())
                            {
                                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "tfd:TimbreFiscalDigital"))
                                    {
                                        if (reader.HasAttributes)
                                        {
                                            UUID = reader.GetAttribute("UUID");
                                            fecha = reader.GetAttribute("FechaTimbrado");
                                            insertaD = "INSERT into xmldata (UUID,FECHA_TIMBRADO) " +
                                                " VALUES ('" + UUID + "','" + fecha + "');";
                                            //insertaD = "insertRecords'" + UUID + "','" + folio + "','" + serie + "','" + fecha + "','" + cliente + "','" + rfc + "','" + idNota + "','" + ruta + "'";
                                            cmd = new SqlCommand(insertaD, con);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                                          
                            
                                
                                //if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "cfdi:Comprobante"))
                                //{
                                //    if (reader.HasAttributes)
                                //    {
                                //        folio = reader.GetAttribute("folio");
                                //        serie = reader.GetAttribute("serie");
                                //    }

                                //}
                                //else
                                //{
                                //    break;
                                //}
                                //if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "cfdi:Emisor"))
                                //{
                                //    if (reader.HasAttributes)
                                //    {
                                //        rfc = reader.GetAttribute("rfc");
                                //        cliente = reader.GetAttribute("nombre");
                                //    }
                                //}
                                //else
                                //{
                                //    break;
                                //}
                            //}
                        ruta = string.Empty;
                    }
                    MessageBox.Show("Datos guardados exitosamente!!!");
                }
                else
                {
                    MessageBox.Show("No se encontro información =(");
                }
                con.Close();
                txtRuta.Text = string.Empty;
            //}
            //catch (Exception err)
            //{
            //    //Console.WriteLine("Error reading from {0}. Message = {1}", ruta);
            //    Console.WriteLine("error: ", err.Message);
            //    throw;
            //}
            //finally {
            //    Console.ReadLine();
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*string rutaArchivo = string.Empty;
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                rutaArchivo = ofd.FileName;
            }
            txtRuta.Text = rutaArchivo;
            string query = "SELECT ad.BatNbr,ad.PerPost,"+
                                    "att.NoteID,att.NameOfFile,att.Location"+
                                    "FROM CSRAPP.dbo.apdoc ad"+
                                    "INNER JOIN CSRDEV.dbo.attachment att"+
                                    "ON ad.NoteID=att.NoteID"+
                                    "WHERE att.NoteID='3593' AND att.Location LIKE '%.xml'";
            periodo = txtRuta.Text;
            string query = "EXEC vData '"+periodo+"'";
            SqlCommand comando = new SqlCommand(query, con);
            con.Open();
            SqlDataReader leer = comando.ExecuteReader();
            if (leer.Read() == true)
            {
                //MessageBox.Show("Registro encontrado");
                txtRuta.Text = leer["Location"].ToString();
            }
            else {
                //MessageBox.Show("Registro NO encontrado");
                txtRuta.Text = "";
            }
            con.Close();*/
        }
    }
}
