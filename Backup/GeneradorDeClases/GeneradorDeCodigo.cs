using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace GeneradorDeClases
{
    public partial class GeneradorDeCodigo : Form
    {

        public static DataTable Tablas = null;
        public static DataTable TablaDatos = null;

        public GeneradorDeCodigo()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void CrearCodigo()
        {
            using (StreamWriter sw = new StreamWriter("TestFile.txt"))
            {
                // Add some text to the file.
                sw.Write("This is the ");
                sw.WriteLine("header for the file.");
                sw.WriteLine("-------------------");
                // Arbitrary objects can also be written to the file.
                sw.Write("The date is: ");
                sw.WriteLine(DateTime.Now);
            }

        }


        public void GuardarClase()
        {
            string FILE_NAME = "D:/MyFile.txt";
            if (File.Exists(FILE_NAME))
            {
                Console.WriteLine("{0} already exists.", FILE_NAME);
                return;
            }
            using (StreamWriter sw = File.CreateText(FILE_NAME))
            {
                sw.WriteLine("This is my file.");
                sw.WriteLine("I can write ints {0} or floats {1}, and so on.",
                    1, 4.2);
                sw.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (TablaDatos == null)
            //{
            //    CrearDataTable();
            //}

            //DataRow dr;
            //dr = TablaDatos.NewRow();
            //dr["Atributo"] = txtAtributo.Text;
            //dr["Tipo"] = txtTipo.Text;
            //TablaDatos.Rows.Add(dr);

            //GridAtributos.DataSource = TablaDatos;
        }

        private void CrearDataTable()
        {

            DataTable Tabla = new DataTable();
            Tabla.Columns.Add(new DataColumn("Atributo"));
            Tabla.Columns.Add(new DataColumn("Tipo"));
            TablaDatos = Tabla;
        }



        private void LlenarTabla()
        {

            string CadenaConexion = txtCadenadeConexion.Text;


            string SQL = "SELECT  distinct TABLE_NAME " +
                                "FROM INFORMATION_SCHEMA.columns ORDER BY TABLE_NAME  " +
                                " SELECT  TABLE_NAME AS TABLA, COLUMN_NAME AS ATRIBUTO," +
                                "CASE IS_NULLABLE WHEN 'YES' THEN '?' ELSE '' END AS ESNULO,DATA_TYPE AS TIPODATO, case DATA_TYPE  when 'int'	" +
                                "	then 'Int32'  when 'decimal'	then 'Decimal'  when 'numeric'	then 'Decimal'" +
                                "  when 'money'	then 'Decimal'  when 'smallint' then 'int'  when 'tinyint'	then 'int'" +
                                "  when 'bit'		then 'int'  when 'float'	then 'double'  when 'bigint'	then 'double' " +
                                " when 'datetime' then 'DateTime'  when 'smalldatetime' then 'DateTime'  else 'string'" +
                                " end as TIPOC, case DATA_TYPE  when 'int'		then 'int'  when 'decimal'	then 'dcm'  when" +
                                " 'numeric'	then 'dcm'  when 'money'	then 'dcm'  when 'smallint' then 'int'  when 'tinyint'	" +
                                "then 'int'  when 'bit'		then 'int'  when 'float'	then 'dbl'  when 'bigint'	then 'dbl'  when" +
                                " 'datetime' then 'dt'  when 'smalldatetime' then 'dt'  else 'str' end as ABREVIADO, " +
                                " ISNULL(CAST(CHARACTER_MAXIMUM_LENGTH AS VARCHAR),'')MAXIMO," +
                                " ISNULL((SELECT COLUMNPROPERTY( OBJECT_ID(TABLE_NAME),COLUMN_NAME,'IsIdentity')),0)ESIDENTITY " +
                                " FROM INFORMATION_SCHEMA.columns ORDER BY TABLE_NAME,TIPOC";


            DataSet ds = new DataSet();
            SqlConnection objConexion = new SqlConnection(CadenaConexion);
            objConexion.Open();

            SqlCommand objComando = new SqlCommand(SQL, objConexion);
            SqlDataAdapter objAdapter = new SqlDataAdapter(objComando);
            objAdapter.Fill(ds);
            Tablas = ds.Tables[0];
            TablaDatos = ds.Tables[1];
        }

        private void CrearArchivoClases()
        {
                foreach (DataRow dr1 in Tablas.Rows)
            {

                string NombreEntidad = dr1["TABLE_NAME"].ToString();


                if (!Directory.Exists(txtRuta.Text + @"\Clases\"))
                {
                    Directory.CreateDirectory(txtRuta.Text + @"\Clases\");
                }
              

                string FILE_NAME = txtRuta.Text + @"\Clases\" + NombreEntidad + ".cs";
                if (File.Exists(FILE_NAME))
                {
                    File.Delete(FILE_NAME);
                }
                using (StreamWriter sw = File.CreateText(FILE_NAME))
                {
                    sw.WriteLine("using System;");
                    sw.WriteLine("using System.Text;");
                    sw.WriteLine("using System.Data;");
                    sw.WriteLine("using System.Collections.Generic;");
                    sw.WriteLine("");
                    sw.WriteLine("");
                    sw.WriteLine(" namespace Entidades");
                    sw.WriteLine("{");
                    sw.WriteLine("");
                    sw.WriteLine("");


                    sw.WriteLine(" public class {0}", NombreEntidad);
                    sw.WriteLine("{");
                    sw.WriteLine("");
                    foreach (DataRow dr in TablaDatos.Rows)
                    {
                        if (NombreEntidad == dr["TABLA"].ToString())
                        {
                            sw.WriteLine("private {0}{1} {2}{3} ;", dr["TIPOC"].ToString(), dr["ESNULO"].ToString(), dr["ABREVIADO"].ToString(), dr["ATRIBUTO"].ToString());
                        }
                    }
                    sw.WriteLine("");
                    sw.WriteLine("");
                    foreach (DataRow dr in TablaDatos.Rows)
                    {
                        if (NombreEntidad == dr["TABLA"].ToString())
                        {
                            sw.WriteLine("public {0}{1} {2}", dr["TIPOC"].ToString(), dr["ESNULO"].ToString(), dr["ATRIBUTO"].ToString());
                            sw.WriteLine("{");
                            sw.WriteLine("");
                            sw.WriteLine("get");
                            sw.WriteLine("{");
                            sw.WriteLine("return {0}{1} ;", dr["ABREVIADO"].ToString(), dr["ATRIBUTO"].ToString());
                            sw.WriteLine("}");
                            sw.WriteLine("");
                            sw.WriteLine("set");
                            sw.WriteLine("{");
                            sw.WriteLine("{0}{1} = value;", dr["ABREVIADO"].ToString(), dr["ATRIBUTO"].ToString());
                            sw.WriteLine("}");
                            sw.WriteLine("");
                            sw.WriteLine("}");
                            sw.WriteLine("");
                            sw.WriteLine("");
                        }
                    }

                    sw.WriteLine("}");
                    sw.WriteLine("}");
                    sw.Close();
                }

            }
        }

        private void CrearArchivoProcedimientosGuardar()
        {
            foreach (DataRow dr1 in Tablas.Rows)
            {
                string NombreEntidad = dr1["TABLE_NAME"].ToString();

                if (!Directory.Exists(txtRuta.Text + @"\Procedimientos\"))
                {
                    Directory.CreateDirectory(txtRuta.Text + @"\Procedimientos\");
                }
             

                string FILE_NAME = txtRuta.Text + @"\Procedimientos\" + "Guardar" + NombreEntidad + ".sql";
                if (File.Exists(FILE_NAME))
                {
                    File.Delete(FILE_NAME);
                }
                using (StreamWriter sw = File.CreateText(FILE_NAME))
                {
                    sw.WriteLine("IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'P', N'PC'))", "Guardar"+ NombreEntidad);
                    sw.WriteLine("DROP PROCEDURE [dbo].[{0}]", "Guardar" + NombreEntidad);
                    sw.WriteLine("GO");
                    sw.WriteLine("");
                    sw.WriteLine("");
                    sw.WriteLine("CREATE PROCEDURE [dbo].[{0}]", "Guardar" + NombreEntidad);
                    sw.WriteLine("");
                    sw.WriteLine("");

                    int loop = 0;
                    
                    bool ultimo = false;

                    foreach (DataRow dr in TablaDatos.Rows)
                    {
                        loop ++;
                        if (NombreEntidad == dr["TABLA"].ToString())
                        {
                            if (dr["ESIDENTITY"].ToString() == "1")
                            {
                                continue;
                            }
                            if (TablaDatos.Rows.Count != loop)
                            {
                                if (TablaDatos.Rows[loop]["TABLA"].ToString() != NombreEntidad)
                                {
                                    ultimo = true;
                                }
                                
                            }
                            else
                            {
                                    ultimo = true;
                            }

                            

                            if (String.IsNullOrEmpty(dr["MAXIMO"].ToString()))
                            {
                                sw.WriteLine("@{0}      {1}" +(ultimo?"":","), dr["ATRIBUTO"].ToString(), dr["TIPODATO"].ToString());
                            }
                            else
                            {
                                sw.WriteLine("@{0}      {1}({2})"+ (ultimo ? "" : ","), dr["ATRIBUTO"].ToString(), dr["TIPODATO"].ToString(), dr["MAXIMO"].ToString());
                            }
                            

                        }
                    }

                    sw.WriteLine("");
                    sw.WriteLine("");
                    sw.WriteLine("AS");
                    sw.WriteLine("");
                    sw.WriteLine("");
                    sw.WriteLine("INSERT INTO {0}", NombreEntidad);
                    sw.WriteLine("");
                    string Campos = String.Empty;
                    string Parametros = String.Empty;
                    foreach (DataRow dr in TablaDatos.Rows)
                    {
                        if (dr["ESIDENTITY"].ToString() == "1")
                        {
                            continue;
                        }
                        if (NombreEntidad == dr["TABLA"].ToString())
                        {
                            Campos = Campos + dr["ATRIBUTO"].ToString() + ',';
                            Parametros = Parametros + "@" + dr["ATRIBUTO"].ToString() + ',';
                        }
                    }
                    Campos = Campos.Remove(Campos.Length - 1);
                    Parametros = Parametros.Remove(Parametros.Length - 1);

                    sw.WriteLine("({0})", Campos);
                    sw.WriteLine("VALUES", Campos);
                    sw.WriteLine("({0})", Parametros);
                    sw.Close();
                }

            }
        }

        private void CrearArchivoProcedimientosConsultar()
        {
            foreach (DataRow dr1 in Tablas.Rows)
            {
                string NombreEntidad = dr1["TABLE_NAME"].ToString();

                if (!Directory.Exists(txtRuta.Text + @"\Procedimientos\"))
                {
                    Directory.CreateDirectory(txtRuta.Text + @"\Procedimientos\");
                }
             

                string FILE_NAME = txtRuta.Text + @"\Procedimientos\" + "Consultar" + NombreEntidad + ".sql";
                if (File.Exists(FILE_NAME))
                {
                    File.Delete(FILE_NAME);
                }
                using (StreamWriter sw = File.CreateText(FILE_NAME))
                {
                    sw.WriteLine("IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[{0}]') AND type in (N'P', N'PC'))", "Consultar" + NombreEntidad);
                    sw.WriteLine("DROP PROCEDURE [dbo].[{0}]", "Consultar" + NombreEntidad);
                    sw.WriteLine("GO");
                    sw.WriteLine("");
                    sw.WriteLine("");
                    sw.WriteLine("CREATE PROCEDURE [dbo].[{0}]", "Consultar" + NombreEntidad);
                    sw.WriteLine("");
                    sw.WriteLine("");
                    int loop = 0;
                    bool ultimo = false;

                    foreach (DataRow dr in TablaDatos.Rows)
                    {
                        loop++;
                        if (NombreEntidad == dr["TABLA"].ToString())
                        {

                            if (TablaDatos.Rows.Count != loop)
                            {
                                if (TablaDatos.Rows[loop]["TABLA"].ToString() != NombreEntidad)
                                {
                                    ultimo = true;
                                }

                            }
                            else
                            {
                                ultimo = true;
                            }



                   
                            if (String.IsNullOrEmpty(dr["MAXIMO"].ToString()))
                            {
                                sw.WriteLine("@{0}      {1} = NULL" + (ultimo ? "" : ","), dr["ATRIBUTO"].ToString(), dr["TIPODATO"].ToString());
                            }
                            else
                            {
                                sw.WriteLine("@{0}      {1}({2}) = NULL" + (ultimo ? "" : ","), dr["ATRIBUTO"].ToString(), dr["TIPODATO"].ToString(), dr["MAXIMO"].ToString());
                            }

                        }
                    }

                    loop = 0;
                    ultimo = false;

                    sw.WriteLine("");
                    sw.WriteLine("");
                    sw.WriteLine("AS");
                    sw.WriteLine("");
                    sw.WriteLine("");
                    string Campos = String.Empty;
                    string Parametros = String.Empty;
                    foreach (DataRow dr in TablaDatos.Rows)
                    {
                        if (NombreEntidad == dr["TABLA"].ToString())
                        {
                            Campos = Campos + dr["ATRIBUTO"].ToString() + ',';
                        }
                    }
                    Campos = Campos.Remove(Campos.Length - 1);
                    sw.WriteLine("SELECT {0} FROM {1}", Campos, NombreEntidad);
                    sw.WriteLine("WHERE");
                    foreach (DataRow dr in TablaDatos.Rows)
                    {
                        loop++;
                     

                        if (NombreEntidad == dr["TABLA"].ToString())
                        {

                            if (TablaDatos.Rows.Count != loop)
                            {
                                if (TablaDatos.Rows[loop]["TABLA"].ToString() != NombreEntidad)
                                {
                                    ultimo = true;
                                }

                            }
                            else
                            {
                                ultimo = true;
                            }

                            sw.WriteLine("{0} = ISNULL({1},{2}) " + (ultimo ? "" : "AND"),dr["ATRIBUTO"].ToString(), "@" + dr["ATRIBUTO"].ToString(), dr["ATRIBUTO"].ToString());
                        }
                    }
                    sw.Close();
                }

            }
        }

        private void btnGenerarCodigo_Click(object sender, EventArgs e)
        {
            LlenarTabla();
            CrearArchivoClases();
            CrearArchivoProcedimientosGuardar();
            CrearArchivoProcedimientosConsultar();
        }

        private void btnBucar_Click(object sender, EventArgs e)
        {
            BuscarFolder.ShowDialog();
            txtRuta.Text = BuscarFolder.SelectedPath;
        }

    }
}


