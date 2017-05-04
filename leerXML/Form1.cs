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
using System.Globalization;
using System.IO;

namespace leerXML
{
    public partial class Form1 : Form
    {
        //SqlConnection con = new SqlConnection("Data Source = ATALAYA-STD;" + "Initial Catalog = CSRAPP ; Integrated Security = true; MultipleActiveResultSets=true;"); 
        SqlConnection con = new SqlConnection("Data Source=MEXQ-SERVER4;Initial Catalog=MEXQAppPr;Persist Security Info=False;User ID=sa;Password=P@ssw0rd; MultipleActiveResultSets=true;");
        string periodo, ruta, insertaD, insertaLog;
        string a = string.Empty, m = string.Empty;
        string err = string.Empty;
        string rfcEmisor, nombreEmisor, rfcReceptor, nombreReceptor, tipoComprobante, fSerie, fFolio, UUID, metodoPago, fechaTimbrado, IVA, idNota;
        string subTotal,Total;
        string d_Totalxml, d_rfcEmisor, d_nombreProveedor;
        string P1, fechaPol,tipoPoliza, folio, clase, idDiarioP, conceptoP, sistOrig, impresa, ajuste, guidP;
        string M1, idCuenta, referencia, tipoMonto, importe, idDiariom1, importeME, conceptoM1, idSegneg, guidM1;
        string AM, uuidAM;
        string AD, uuidAD;
        string linea1;
        //string[] polizaDatos;
        string[] M1Datos;
        string[] AMDatos;
        string[] ADDatos;
        int[] polizaLong = { 2, 8, 4, 9, 1, 10, 100, 3, 1, 1, 36 };
        int[] M1Long = { 2, 30, 20, 1, 20, 10, 20, 100, 4, 36 };
        int[] AMLong = { 2, 36 };
        int[] ADLong = { 2, 36 };
        string now = DateTime.Now.ToString("yyMMdd_hhmm");
        SqlCommand comando, cmd;
        XmlReader reader;

        public Form1()
        {
            InitializeComponent();
            llenarCombo_anio();
            llenarCombo_mes();
        }

        
        private void llenarCombo_anio()
        {
            String sDate = DateTime.Now.ToString();
            DateTime datevalue = (Convert.ToDateTime(sDate.ToString()));
            String anio = datevalue.Year.ToString();
            cbAnio.Items.Clear();
            cbAnio.Items.Add(anio);
            cbAnio.Items.Add(Int32.Parse(anio) - 1);
            cbAnio.Items.Add(Int32.Parse(anio) - 2);
            cbAnio.Items.Add(Int32.Parse(anio) - 3);
            cbAnio.Items.Add(Int32.Parse(anio) - 4);
            cbAnio.Items.Add(Int32.Parse(anio) - 5);
        }

        private void llenarCombo_mes()
        {
            cbMes.Items.Clear();
            cbMes.Items.Add("Enero");
            cbMes.Items.Add("Febrero");
            cbMes.Items.Add("Marzo");
            cbMes.Items.Add("Abril");
            cbMes.Items.Add("Mayo");
            cbMes.Items.Add("Junio");
            cbMes.Items.Add("Julio");
            cbMes.Items.Add("Agosto");
            cbMes.Items.Add("Septiembre");
            cbMes.Items.Add("Octubre");
            cbMes.Items.Add("Noviembre");
            cbMes.Items.Add("Diciembre");
        }

        private string getmPago(string mPago)
        {
            return new String(mPago.Where(Char.IsDigit).ToArray());
        }


        private void valorMes()
        {
            switch (cbMes.Text)
            {
                case "Enero":
                    m = "01";
                    break;
                case "Febrero":
                    m = "02";
                    break;
                case "Marzo":
                    m = "03";
                    break;
                case "Abril":
                    m = "04";
                    break;
                case "Mayo":
                    m = "05";
                    break;
                case "Junio":
                    m = "06";
                    break;
                case "Julio":
                    m = "07";
                    break;
                case "Agosto":
                    m = "08";
                    break;
                case "Septiembre":
                    m = "09";
                    break;
                case "Octubre":
                    m = "10";
                    break;
                case "Noviembre":
                    m = "11";
                    break;
                case "Diciembre":
                    m = "12";
                    break;
                default:
                    break;
            }

        }

        //OBSOLETO
       /* private void btnLeer_Click(object sender, EventArgs e)
        {
            a = cbAnio.Text;
            valorMes();
            UUID = string.Empty;
            periodo = a + m;
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
                    try
                    {
                        //INICIA LECTURA DE XML
                        while (reader.Read())
                        {
                            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "tfd:TimbreFiscalDigital"))
                            {
                                if (reader.HasAttributes)
                                {
                                    UUID = reader.GetAttribute("UUID");
                                    fecha = reader.GetAttribute("FechaTimbrado");
                                    insertaD = "insertRecords'" + UUID + "','" + folio + "','" + serie + "','" + fecha + "','" + cliente + "','" + rfc + "','" + idNota + "','" + ruta + "'";
                                    cmd = new SqlCommand(insertaD, con);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "cfdi:Comprobante"))
                            {
                                if (reader.HasAttributes)
                                {
                                    folio = reader.GetAttribute("folio");
                                    serie = reader.GetAttribute("serie");
                                }
                            }
                            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "cfdi:Emisor"))
                            {
                                if (reader.HasAttributes)
                                {
                                    rfc = reader.GetAttribute("rfc");
                                    cliente = reader.GetAttribute("nombre");
                                }
                            }
                            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "cfdi:Conceptos"))
                            {
                                if (reader.HasAttributes)
                                {
                                    rfc = reader.GetAttribute("rfc");
                                    cliente = reader.GetAttribute("nombre");
                                }
                            }
                        }
                        //TERMINA LECTURA DE XML
                    }
                    catch (Exception ex)
                    {
                        //System.IO.File.AppendAllText(@"C:\Users\Public\Documents\logxml.txt", ruta + "\r\n" + ex.Message + "\r\n");
                        err = err + ex.Message + "\n" + ruta;

                        insertaLog = "INSERT into logxml (NoteID,Location,error)  VALUES (@idn,@ruta,@error);";
                        SqlCommand command = new SqlCommand(insertaLog, con);
                        command.Parameters.AddWithValue("@idn", idNota);
                        command.Parameters.AddWithValue("@ruta", ruta);
                        command.Parameters.AddWithValue("@error", ex.Message);
                        command.ExecuteNonQuery();
                    }
                    ruta = string.Empty;
                }
                if (err.Length > 0)
                {
                    MessageBox.Show(err, "Errores de lectura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                MessageBox.Show("Datos guardados exitosamente!!!");
            }
            else
            {
                MessageBox.Show("No se encontro información =(");
            }
            con.Close();
            cbAnio.SelectedIndex = -1;
            cbMes.SelectedIndex = -1;
        }*/

        //REAL CODING
        private void btnLeer_Click(object sender, EventArgs e)
        {
            a = cbAnio.Text;
            valorMes();
            UUID = string.Empty;
            periodo = a + m;
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
                    try
                    {
                        //INICIA LECTURA DE XML
                        while (reader.Read())
                        {
                            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "cfdi:Emisor"))
                            {
                                rfcEmisor = reader.GetAttribute("rfc");//OBTENER RFC DEL EMISOR
                                nombreEmisor = reader.GetAttribute("nombre");//OBTENER NOMBRE DEL EMISOR

                            }
                            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "cfdi:Receptor"))
                            {
                                rfcReceptor = reader.GetAttribute("rfc");//OBTENER RFC DEL RECEPTOR
                                nombreReceptor = reader.GetAttribute("nombre");//OBTENER NOMBRE DEL EMISOR
                            }
                            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "cfdi:Comprobante"))
                            {
                                tipoComprobante = reader.GetAttribute("tipoDeComprobante");//OBTENER EL TIPO DEL COMPROBANTE
                                fSerie = reader.GetAttribute("serie");//OBTENER LA SERIE DEL COMPROBANTE
                                fFolio = reader.GetAttribute("folio");//OBTENER EL FOLIO DEL COMPROBANTE
                                metodoPago = getmPago(reader.GetAttribute("metodoDePago"));//OBTENER EL FOLIO DEL COMPROBANTE
                                subTotal = reader.GetAttribute("subTotal");//OBTENER IMPORTE SUBTOTAL
                                Total = reader.GetAttribute("total");//OBTENER IMPORTE TOTAL
                            }
                            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "cfdi:Impuestos"))
                            {
                                IVA = reader.GetAttribute("totalImpuestosTrasladados");//OBTENER IVA
                                //impuestoT = reader.GetAttribute("totalImpuestosTrasladados");//OBTENER IMPORTE
                            }
                            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "tfd:TimbreFiscalDigital"))
                            {
                                UUID = reader.GetAttribute("UUID");//OBTENER UUID
                                fechaTimbrado = reader.GetAttribute("FechaTimbrado");//OBTRENER FECHA TIMBRADO
                            }
                        }
                        //GUARDAR EN BD
                        insertaD = "insertRecords '" + idNota + "','" + rfcEmisor + "','" + nombreEmisor + "','" + rfcReceptor + "','" + nombreReceptor + "','" + tipoComprobante + "','" + metodoPago + "','" + fSerie + "','" + fFolio + "','" + subTotal + "','" + IVA + "','" + Total + "','" + UUID + "','" + fechaTimbrado + "','" + ruta + "'";
                        cmd = new SqlCommand(insertaD, con);
                        cmd.ExecuteNonQuery();
                        //TERMINA LECTURA DE XML
                    }
                    catch (Exception ex)
                    {
                        //System.IO.File.AppendAllText(@"C:\Users\Public\Documents\logxml.txt", ruta + "\r\n" + ex.Message + "\r\n");
                        err = err + ex.Message + "\n" + ruta;
                        insertaLog = "INSERT into logxml (NoteID,Location,error)  VALUES (@idn,@ruta,@error);";
                        SqlCommand command = new SqlCommand(insertaLog, con);
                        command.Parameters.AddWithValue("@idn", idNota);
                        command.Parameters.AddWithValue("@ruta", ruta);
                        command.Parameters.AddWithValue("@error", ex.Message);
                        command.ExecuteNonQuery();
                    }
                    ruta = string.Empty;
                }
                //MOSTRAR VENTANA DE ERRORES
                //if (err.Length > 0)
                {
                    //MessageBox.Show(err, "Errores de lectura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show("Hubo errores de lectura durante el proceso, consulte la ventana de logs para mas información", "Errores de lectura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                MessageBox.Show("Datos guardados exitosamente!!!");
            }
            else
            {
                MessageBox.Show("No se encontro información");
            }
            con.Close();
            cbAnio.SelectedIndex = -1;
            cbMes.SelectedIndex = -1;
        }
        
          //PARA PRUEBAS
          /*private void btnLeer_Click(object sender, EventArgs e)
          {
              a = cbAnio.Text;
              valorMes();
              UUID = string.Empty;
              periodo = a + m;
              string query = "EXEC vData '" + periodo + "'";
              comando = new SqlCommand(query, con);
              con.Open();
              SqlDataReader leer = comando.ExecuteReader();

              reader = XmlReader.Create("C:\\Users\\cvalenciano\\Desktop\\CE_LAyout\\xmlTest\\4347-f10857281-[ni] ADT JUNIO.xml");
                      try
                      {
                          //INICIA LECTURA DE XML
                          while (reader.Read())
                          {
                              if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "cfdi:Emisor"))
                              {
                                  rfcEmisor = reader.GetAttribute("rfc");//OBTENER RFC DEL EMISOR
                                  nombreEmisor = reader.GetAttribute("nombre");//OBTENER NOMBRE DEL EMISOR
                              }
                              if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "cfdi:Receptor"))
                              {
                                  rfcReceptor = reader.GetAttribute("rfc");//OBTENER RFC DEL RECEPTOR
                                  nombreReceptor = reader.GetAttribute("nombre");//OBTENER NOMBRE DEL EMISOR
                              }
                              if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "cfdi:Comprobante"))
                              {
                                  tipoComprobante = reader.GetAttribute("tipoDeComprobante");//OBTENER EL TIPO DEL COMPROBANTE
                                  fSerie = reader.GetAttribute("serie");//OBTENER LA SERIE DEL COMPROBANTE
                                  fFolio = reader.GetAttribute("folio");//OBTENER EL FOLIO DEL COMPROBANTE
                                  //metodoPago = reader.GetAttribute("metodoDePago").Substring(0, 2);//OBTENER EL FOLIO DEL COMPROBANTE
                                  metodoPago = getmPago(reader.GetAttribute("metodoDePago"));//OBTENER EL FOLIO DEL COMPROBANTE
                                  //getmPago(metodoPago);
                                  subTotal = reader.GetAttribute("subTotal");//OBTENER IMPORTE SUBTOTAL
                                  Total = reader.GetAttribute("total");//OBTENER IMPORTE TOTAL
                              }
                              if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "cfdi:Concepto"))
                              {
                                  subTotal += double.Parse(reader.GetAttribute("importe"));//OBTENER IMPORTE
                              }
                              if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "tfd:TimbreFiscalDigital"))
                              {
                                  UUID = reader.GetAttribute("UUID");//OBTENER UUID
                                  fechaTimbrado = reader.GetAttribute("FechaTimbrado");//OBTRENER FECHA TIMBRADO
                              }
                          }
                          //TERMINA LECTURA DE XML
                      }
                      catch (Exception ex)
                      {
                          //System.IO.File.AppendAllText(@"C:\Users\Public\Documents\logxml.txt", ruta + "\r\n" + ex.Message + "\r\n");
                          err = err + ex.Message + "\n" + ruta;
                      }
                      ruta = string.Empty;
                  if (err.Length > 0)
                  {
                      MessageBox.Show(err, "Errores de lectura", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  }
                  MessageBox.Show("RFCEmisor: "+rfcEmisor+"\nNombre Emiso: "+nombreEmisor+
                                                     "\nRFC Receptor: "+rfcReceptor + "\nNombre Receptor: " + nombreReceptor + "\nTipo: " + tipoComprobante + "\nSerie: " + fSerie + "    Folio:" + fFolio +
                                                      "\nSubtotal: " + subTotal.ToString() + "\nTotal: " + Total.ToString() +
                                                      "\nUUID: "+UUID+"\nMetodo de pago: "+metodoPago);
              con.Close();
              cbAnio.SelectedIndex = -1;
              cbMes.SelectedIndex = -1;
          }*/

        private void btnVLogs_Click(object sender, EventArgs e)
        {
            Logs logs = new Logs();
            logs.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Logs logs = new Logs();
            logs.Show();
        }

        private string hl(string cadena, int n)
        {
            int numberOfLetters = cadena.Length;//Calcula la letras del string
            int ne = n - numberOfLetters;//Calcular espacios en blanco a usar
            if (ne < 0)
                ne = 0;
            string eBlanco = new string(' ', ne);//Asiga los espacion en blanco a la variable eBlanco
            return cadena + eBlanco;//Regresa el string con los espacios en blanco
        }


        private void btnGenerar_Click(object sender, EventArgs e)
        {
            //string consulta = "SELECT * FROM XMLDATA WHERE rfc_emisor='HDM001017AS1'";
            string consulta = "SELECT xd.uuid, xd.total, xd.serie,xd.folio,xd.rfc_emisor,xd.nombre_emisor,xd.fecha_timbrado,"+
                                        "ad.PerPost , ad.RefNbr, ad.BatNbr "+
                                        "FROM xmldata as xd "+
                                        "inner join apdoc as ad "+
                                        "on xd.noteid= ad.noteid "+
                                        "where ad.Perpost = '201601' "+
                                        "ORDER BY ad.PerPost;";
            comando = new SqlCommand(consulta, con);
            con.Open();
            SqlDataReader reader = comando.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    P1 = "P";
                    fechaPol = reader["fecha_timbrado"].ToString().Substring(0,10);
                    fechaPol = fechaPol.Replace("-", string.Empty);
                    tipoPoliza = "10";
                    folio = reader["folio"].ToString();
                    if (!(folio.Length < 9))
                        folio = folio.Substring(0, 9);
                    clase = "C";
                    idDiarioP = "IDP";
                    conceptoP = reader["serie"].ToString() + reader["folio"].ToString() +" "+ reader["nombre_emisor"].ToString();
                    sistOrig = "SO";
                    impresa = "I";
                    ajuste = "A";
                    guidP = reader["uuid"].ToString();
                    string[] polizaDatos = { P1, fechaPol, tipoPoliza, folio, clase, idDiarioP, conceptoP, sistOrig, impresa, ajuste, guidP };
                    for (int i = 0; i < polizaLong.Length; i++)
                    {
                        linea1 = linea1 + hl(polizaDatos[i], polizaLong[i]);
                    }
                    linea1 = linea1 + "\r\n";
                }
                /*ESCRIBIR EN EL DOCUMENTO*/
                string folder = @"C:\output\";
                string path = folder +now + ".txt";
                // This text is added only once to the file.
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    string createText = linea1 + Environment.NewLine;
                    File.WriteAllText(path, createText);
                }

                /*This text is always added, making the file longer over time
                if it is not deleted.*/
                //string appendText = "This is extra text" + Environment.NewLine;
                //File.AppendAllText(path, appendText);

                // Open the file to read from.
                string readText = File.ReadAllText(path);
                Console.WriteLine(readText);
                /*ESCRIBIR EN EL DOCUMENTO*/
                MessageBox.Show("Datos guardados exitosamente!!!");
            }
            else
            {
                MessageBox.Show("No se encontro información");
            }
            con.Close();
        }
    }
}
