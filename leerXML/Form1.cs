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

namespace leerXML
{
    public partial class Form1 : Form
    {
        //SqlConnection con = new SqlConnection("Data Source = ATALAYA-STD;" + "Initial Catalog = CSRAPP ; Integrated Security = true; MultipleActiveResultSets=true;");
        SqlConnection con = new SqlConnection("Data Source=MEXQ-SERVER4;Initial Catalog=MEXQAppJulio;Persist Security Info=False;User ID=sa;Password=P@ssw0rd; MultipleActiveResultSets=true;");
        string periodo, ruta, insertaD, insertaLog;
        string a = string.Empty, m = string.Empty;
        string err = string.Empty;
        string  idNota, folio, serie, fecha, cliente, rfc;
        string rfcEmisor, nombreEmisor, rfcReceptor, nombreReceptor, tipoComprobante, fSerie, fFolio, nombreImpuesto, UUID,metodoPago;
        double subTotal, impuestoT,Total,trasladoIVA;
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

        //private void btnLeer_Click(object sender, EventArgs e)
        //{
        //    a = cbAnio.Text;
        //    valorMes();
        //    UUID = string.Empty;
        //    periodo = a + m;
        //    string query = "EXEC vData '" + periodo + "'";
        //    comando = new SqlCommand(query, con);
        //    con.Open();
        //    SqlDataReader leer = comando.ExecuteReader();
        //    if (leer.HasRows)
        //    {
        //        while (leer.Read())
        //        {
        //            ruta = leer["Location"].ToString();
        //            idNota = leer["NoteID"].ToString();
        //            reader = XmlReader.Create(ruta);
        //            try
        //            {
        //                //INICIA LECTURA DE XML
        //                while (reader.Read())
        //                {
        //                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "tfd:TimbreFiscalDigital"))
        //                    {
        //                        if (reader.HasAttributes)
        //                        {
        //                            UUID = reader.GetAttribute("UUID");
        //                            fecha = reader.GetAttribute("FechaTimbrado");
        //                            insertaD = "insertRecords'" + UUID + "','" + folio + "','" + serie + "','" + fecha + "','" + cliente + "','" + rfc + "','" + idNota + "','" + ruta + "'";
        //                            cmd = new SqlCommand(insertaD, con);
        //                            cmd.ExecuteNonQuery();
        //                        }
        //                    }
        //                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "cfdi:Comprobante"))
        //                    {
        //                        if (reader.HasAttributes)
        //                        {
        //                            folio = reader.GetAttribute("folio");
        //                            serie = reader.GetAttribute("serie");
        //                        }
        //                    }
        //                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "cfdi:Emisor"))
        //                    {
        //                        if (reader.HasAttributes)
        //                        {
        //                            rfc = reader.GetAttribute("rfc");
        //                            cliente = reader.GetAttribute("nombre");
        //                        }
        //                    }
        //                    if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "cfdi:Conceptos"))
        //                    {
        //                        if (reader.HasAttributes)
        //                        {
        //                            rfc = reader.GetAttribute("rfc");
        //                            cliente = reader.GetAttribute("nombre");
        //                        }
        //                    }
        //                }
        //                //TERMINA LECTURA DE XML
        //            }
        //            catch (Exception ex)
        //            {
        //                //System.IO.File.AppendAllText(@"C:\Users\Public\Documents\logxml.txt", ruta + "\r\n" + ex.Message + "\r\n");
        //                err = err + ex.Message + "\n" + ruta;

        //                insertaLog = "INSERT into logxml (NoteID,Location,error)  VALUES (@idn,@ruta,@error);";
        //                SqlCommand command = new SqlCommand(insertaLog, con);
        //                command.Parameters.AddWithValue("@idn", idNota);
        //                command.Parameters.AddWithValue("@ruta", ruta);
        //                command.Parameters.AddWithValue("@error", ex.Message);
        //                command.ExecuteNonQuery();
        //            }
        //            ruta = string.Empty;
        //        }
        //        if(err.Length>0)
        //        {
        //            MessageBox.Show(err, "Errores de lectura", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //        MessageBox.Show("Datos guardados exitosamente!!!");
        //    }
        //    else
        //    {
        //        MessageBox.Show("No se encontro información =(");
        //    }
        //    con.Close();
        //    cbAnio.SelectedIndex = -1;
        //    cbMes.SelectedIndex = -1;
        //}

        private void btnLeer_Click(object sender, EventArgs e)
        {
            subTotal = 0.0D;
            impuestoT = 0.0D;
            Total = 0.0D;
            a = cbAnio.Text;
            valorMes();
            UUID = string.Empty;
            periodo = a + m;
            string query = "EXEC vData '" + periodo + "'";
            comando = new SqlCommand(query, con);
            con.Open();
            SqlDataReader leer = comando.ExecuteReader();

            reader = XmlReader.Create("C:\\Users\\cvalenciano\\Desktop\\CE_LAyout\\xmlTest\\algunos.xml");
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
                                metodoPago = reader.GetAttribute("metodoDePago");//OBTENER EL FOLIO DEL COMPROBANTE
                            }
                            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "cfdi:Impuestos"))
                            {
                                impuestoT = Math.Round(double.Parse(reader.GetAttribute("totalImpuestosTrasladados")),6);//OBTENER IMPORTE
                            } 
                            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "cfdi:Concepto"))
                            {
                                subTotal += Math.Round(double.Parse(reader.GetAttribute("importe")),6);//OBTENER IMPORTE
                            }
                            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "cfdi:Traslado"))
                            {
                                nombreImpuesto = reader.GetAttribute("impuesto");//OBTENER IMPUESTO
                                trasladoIVA = Math.Round(double.Parse(reader.GetAttribute("importe")), 6);//OBTENER IMPORTE IVA
                            }
                            if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "tfd:TimbreFiscalDigital"))
                            {
                                UUID = reader.GetAttribute("UUID");//OBTENER UUID
                            }
                        }
                        Total = Math.Round(subTotal + impuestoT,6);
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
                MessageBox.Show("RFCEmisor: "+rfcEmisor+
                    "\nNombre Emiso: "+nombreEmisor+
                    "\nRFC Receptor: "+
                    rfcReceptor + "\nNombre Receptor: " + nombreReceptor + "\nTipo: " + tipoComprobante + "\nSerie: " + fSerie + "    Folio:" + fFolio +
                    "\nSubtotal: " + subTotal.ToString() + "\nImpuestos Trasladados: " + impuestoT + "\nImpuesto: " + nombreImpuesto + "\nTotal: " + Total.ToString() +
                    "\nUUID: "+UUID+"\nMetodo de pago: "+metodoPago+"\nTraslado de IVA: "+trasladoIVA);
            con.Close();
            cbAnio.SelectedIndex = -1;
            cbMes.SelectedIndex = -1;
        }

        private void btnVLogs_Click(object sender, EventArgs e)
        {
            Logs logs = new Logs();
            logs.Show();
        }
    }
}
