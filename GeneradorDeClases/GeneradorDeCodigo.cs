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
            try
            {
                string CadenaConexion = txtCadenadeConexion.Text;


                string SQL = "SELECT  DISTINCT TABLE_NAME FROM INFORMATION_SCHEMA.columns WHERE TABLE_NAME NOT IN('__MigrationHistory') /* AND TABLE_NAME  like 'Movilidad%'*/ ORDER BY TABLE_NAME  " +
                            "SELECT  TABLE_NAME AS TABLA, COLUMN_NAME AS ATRIBUTO," +
                            "CASE IS_NULLABLE WHEN 'YES' THEN '?' ELSE '' END AS ESNULO, " +
                            "DATA_TYPE AS TIPODATO, " +
                            "CASE DATA_TYPE  " +
                            "WHEN 'int'		THEN 'Int32'	WHEN 'decimal'			THEN 'Decimal'		WHEN 'numeric'		THEN 'decimal'" +
                            "WHEN 'money'	THEN 'Decimal'  WHEN 'smallint'			THEN 'short'		WHEN 'tinyint'		THEN 'byte'" +
                            "WHEN 'bit'		THEN 'Boolean'  WHEN 'float'			THEN 'double'		WHEN 'bigint'		THEN 'double'" +
                            "WHEN 'datetime' THEN 'DateTime' WHEN 'smalldatetime'	THEN 'DateTime'		ELSE 'string'		END AS TIPOC, " +
                            "CASE DATA_TYPE  " +
                            "WHEN 'int'		THEN 'int'		WHEN 'decimal'			THEN 'dcm'			WHEN	'numeric'	THEN 'dcm'  " +
                            "WHEN 'money'	THEN 'dcm'		WHEN 'smallint'			THEN 'int'			WHEN	'tinyint'	THEN 'int'  " +
                            "WHEN 'bit'		THEN 'int'		WHEN 'float'			THEN 'dbl'			WHEN	'bigint'	THEN 'dbl'  " +
                            "WHEN 'datetime' THEN 'dt'		WHEN 'smalldatetime'	THEN 'dt'			ELSE 'str'			END AS ABREVIADO, " +
                            "ISNULL(CAST(CHARACTER_MAXIMUM_LENGTH AS VARCHAR),'')MAXIMO," +
                            "ISNULL((SELECT COLUMNPROPERTY( OBJECT_ID(TABLE_NAME),COLUMN_NAME,'IsIdentity')),0)ESIDENTITY " +
                            "FROM INFORMATION_SCHEMA.columns WHERE TABLE_NAME NOT IN('__MigrationHistory') /* AND TABLE_NAME  like 'Movilidad%'*/ ORDER BY TABLE_NAME ;";
                //ORDER BY TABLE_NAME,TIPOC


                DataSet ds = new DataSet();
                SqlConnection objConexion = new SqlConnection(CadenaConexion);
                objConexion.Open();

                SqlCommand objComando = new SqlCommand(SQL, objConexion);
                SqlDataAdapter objAdapter = new SqlDataAdapter(objComando);
                objAdapter.Fill(ds);
                Tablas = ds.Tables[0];
                TablaDatos = ds.Tables[1];
            }
            catch (Exception exc)
            {

                MessageBox.Show("Oe no me pude conectar a la BD. Revisate el webconfig hombe!");
            }
      
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
            if (Tablas != null)
            {
                CrearArchivoVista();
                CrearEntidadesEntity();
                CrearControladorDatos();
                CrearControladorDatosGeneral();
                CrearArchivoClases();
                CrearArchivoProcedimientosGuardar();
                CrearArchivoProcedimientosConsultar();
                CrearCodeBehind();
                MessageBox.Show("Terminada la vuelta!!!", "Listo", MessageBoxButtons.OK);
            }
        }

        private void CrearCodeBehind()
        {

            foreach (DataRow dr1 in Tablas.Rows)
            {

                string NombreEntidad = dr1["TABLE_NAME"].ToString();


                if (!Directory.Exists(txtRuta.Text + @"\CodeBehind\"))
                {
                    Directory.CreateDirectory(txtRuta.Text + @"\CodeBehind\");
                }

                string FILE_NAME = txtRuta.Text + @"\CodeBehind\"+ NombreEntidad + ".cs";
                if (File.Exists(FILE_NAME))
                {
                    File.Delete(FILE_NAME);
                }
                using (StreamWriter sw = File.CreateText(FILE_NAME))
                {
                    sw.WriteLine("protected void grid{0}_SelectedIndexChanged(object sender, EventArgs e)",NombreEntidad);
                    sw.WriteLine("  {");
                    sw.WriteLine("      try");
                    sw.WriteLine("      {");
                    sw.WriteLine("          int rowindex = grid{0}.SelectedRow.RowIndex;", NombreEntidad);
                    foreach (DataRow dr in TablaDatos.Rows)
                    {
                        if (NombreEntidad == dr["TABLA"].ToString())
                        {
                            bool esDropDown = false;
                            string campo = dr["ATRIBUTO"].ToString();
                            string nombreControl = "txt" + campo;

                            if (campo.Substring(0, 2) == "fk")
                            {
                                nombreControl = "ddl" + campo.Remove(0, 5);
                                esDropDown = true;
                            }

                            if (esDropDown)
                            {
                                sw.WriteLine("          string {0} = gridActores.DataKeys[rowindex][\"{0}\"].ToString();", campo);
                                sw.WriteLine("          {0}.SelectedIndex = {0}.Items.IndexOf({0}.Items.FindByValue({1}));", nombreControl,campo);
                            }
                            else
                            {
                                sw.WriteLine("          {0}.Text = grid{1}.DataKeys[rowindex][\"{2}\"].ToString();", nombreControl,NombreEntidad,campo);
                            }
                            
                            
                        }
                    }
                    sw.WriteLine("      }");
                    sw.WriteLine("      catch (Exception exc)");
                    sw.WriteLine("      {");
                    sw.WriteLine("          MensajeError(exc.ToString());");
                    sw.WriteLine("      }");
                    sw.WriteLine("  }");
                    sw.WriteLine("");
                    sw.WriteLine("");
                    sw.WriteLine("protected void grid{0}_RowCommand(object sender, GridViewCommandEventArgs e)", NombreEntidad);
                    sw.WriteLine("  {");
                    sw.WriteLine("      try");
                    sw.WriteLine("      {");
                    sw.WriteLine("          if (e.CommandName == \"Editar{0}\")", NombreEntidad);
                    sw.WriteLine("          {");
                    sw.WriteLine("              int rowindex = Convert.ToInt32(e.CommandArgument);");
                    

                    foreach (DataRow dr in TablaDatos.Rows)
                    {
                        if (NombreEntidad == dr["TABLA"].ToString())
                        {
                            bool esDropDown = false;
                            string campo = dr["ATRIBUTO"].ToString();
                            string nombreControl = "txt" + campo;

                            if (campo.Substring(0, 2) == "fk")
                            {
                                nombreControl = "ddl" + campo.Remove(0, 5);
                                esDropDown = true;
                            }

                            if (esDropDown)
                            {
                                sw.WriteLine("              string {0} = gridActores.DataKeys[rowindex][\"{0}\"].ToString();", campo);
                                sw.WriteLine("              {0}.SelectedIndex = {0}.Items.IndexOf({0}.Items.FindByValue({1}));", nombreControl, campo);
                            }
                            else
                            {
                                sw.WriteLine("              {0}.Text = grid{1}.DataKeys[rowindex][\"{2}\"].ToString();", nombreControl, NombreEntidad, campo);
                            }


                        }
                    }
                    sw.WriteLine("          }");
                    sw.WriteLine("      }");
                    sw.WriteLine("      catch (Exception exc)");
                    sw.WriteLine("      {");
                    sw.WriteLine("          MensajeError(exc.ToString());");
                    sw.WriteLine("      }");
                    sw.WriteLine("  }");


                    sw.WriteLine("protected void btnAgregar{0}_Click(object sender, EventArgs e)", NombreEntidad);
                    sw.WriteLine("{");
                    sw.WriteLine("  try");
                    sw.WriteLine("  {");
                    sw.WriteLine("      {0} obj{0} = new {0}();", NombreEntidad);
                    foreach (DataRow dr in TablaDatos.Rows)
                    {
                        if (NombreEntidad == dr["TABLA"].ToString())
                        {
                            
                            string campo = dr["ATRIBUTO"].ToString();
                            string nombreControl = "txt" + campo;

                            if (campo.Substring(0, 2) == "fk")
                            {
                                nombreControl = "ddl" + campo.Remove(0, 5);
                                if (dr["TIPOC"].ToString() != "string")
                                {
                                    sw.WriteLine("      obj{0}.{1} = {2}.Parse({3}.SelectedItem.Value);", NombreEntidad, campo, dr["TIPOC"].ToString(), nombreControl);
                                }
                                else
                                {
                                    sw.WriteLine("      obj{0}.{1} = {2}.SelectedItem.Value;", NombreEntidad, campo, nombreControl);
                                }
                                
                                
                            }
                            else
                            {
                                if (dr["TIPOC"].ToString() != "string")
                                {
                                    sw.WriteLine("      obj{0}.{1} = {2}.Parse({3}.Text);", NombreEntidad, campo, dr["TIPOC"].ToString(), nombreControl);
                                }
                                else
                                {
                                    sw.WriteLine("      obj{0}.{1} = {2}.Text;", NombreEntidad, campo, nombreControl);
                                }
                                
                            }
                        }
                    }
                    sw.WriteLine("  }");
                    sw.WriteLine("  catch (Exception exc)");
                    sw.WriteLine("  {");
                    sw.WriteLine("      MensajeError(exc.ToString());");
                    sw.WriteLine("  }");
                    sw.WriteLine("}");
                }

                }
                        
            }

        
      

        private void CrearControladorDatos()
        {
         
            foreach (DataRow dr1 in Tablas.Rows)
            {

                string NombreEntidad = dr1["TABLE_NAME"].ToString();


                if (!Directory.Exists(txtRuta.Text + @"\ControladorDatos\"))
                {
                    Directory.CreateDirectory(txtRuta.Text + @"\ControladorDatos\");
                }

                string FILE_NAME = txtRuta.Text + @"\ControladorDatos\" + "ControladorDatos"+NombreEntidad + ".cs";
                if (File.Exists(FILE_NAME))
                {
                    File.Delete(FILE_NAME);
                }
                using (StreamWriter sw = File.CreateText(FILE_NAME))
                {
                    sw.WriteLine("using ITM.AutoMedicionCTI.AccesoDatos;");
                    sw.WriteLine("using ITM.AutoMedicionCTI.AccesoDatos.AMCTI.EntityFramework.Modelos;");
                    sw.WriteLine("using System;");
                    sw.WriteLine("using System.Collections.Generic;");
                    sw.WriteLine("using System.Configuration;");
                    sw.WriteLine("using System.Data;");
                    sw.WriteLine("using System.Data.SqlClient;");
                    sw.WriteLine("using System.Linq;");

                    sw.WriteLine("namespace ITM.AutoMedicionCTI.{0}", NombreEntidad);
                    sw.WriteLine("  {");
                    sw.WriteLine("      public class ControladorDatos{0}", NombreEntidad);
                    sw.WriteLine("      {");
                    sw.WriteLine("");
                    sw.WriteLine("      private string connectionString = ConfigurationManager.ConnectionStrings['ContextAMCTI'].ConnectionString;");
                    sw.WriteLine("");
                    
                    string parametros1 = String.Empty;
                    string parametros2 = String.Empty;
                    string parametros3 = String.Empty;
                    foreach (DataRow dr in TablaDatos.Rows)
                    {
                        if (NombreEntidad == dr["TABLA"].ToString())
                        {
                            string atributo = dr["ATRIBUTO"].ToString();
                            if (dr["ATRIBUTO"].ToString().Substring(0, 2) == "fk")
                            {
                                
                                if (dr["ATRIBUTO"].ToString().Substring(0, 2) == "fk")
                                {
                                    atributo = dr["ATRIBUTO"].ToString().Remove(0, 3);
                                }
                            }
                                //parametros1 = parametros1 + String.Format("{0}{1} {2} = null,", dr["TIPOC"].ToString(), dr["ESNULO"].ToString(), atributo);
                                parametros1 = parametros1 + String.Format("{0}? {1} = null,", dr["TIPOC"].ToString(), atributo);
                                parametros2 = parametros2 + String.Format("x.{0} == ({1} == null ? x.{0} : {1}) && ", dr["ATRIBUTO"].ToString(),atributo);
                            
                            if (dr["ATRIBUTO"].ToString().Substring(0, 2) != "id")
                            {
                                parametros3 = "                   " + parametros3 + String.Format("obj{0}.{1} = _obj{0}.{1} == null ? obj{0}.{1} : _obj{0}.{1};\n", NombreEntidad, dr["ATRIBUTO"].ToString());
                            }
                        }
                    }
                    
                    parametros1 = parametros1.Remove(parametros1.Length - 1);
                    parametros2 = parametros2.Remove(parametros2.Length - 3);
                    sw.WriteLine("      #region {0}", NombreEntidad);
                    sw.WriteLine("");
                    sw.WriteLine("      public List<{0}> Obtener{0}({1})", NombreEntidad,parametros1);
                    sw.WriteLine("      {");
                    sw.WriteLine("          List <{0}> objLista{0} = new List<{0}>();", NombreEntidad);
                    sw.WriteLine("          try");
                    sw.WriteLine("          {");
                    sw.WriteLine("              using (var dbContext = new ContextAMCTI())");
                    sw.WriteLine("              {");
                    sw.WriteLine("                  objLista{0} = dbContext.{0}.Where(x=> {1}).ToList();",NombreEntidad, parametros2);
                    sw.WriteLine("              }");
                    sw.WriteLine("          }");
                    sw.WriteLine("          catch (Exception exc)");
                    sw.WriteLine("          {");
                    sw.WriteLine("              throw exc;");
                    sw.WriteLine("          }");            
                    sw.WriteLine("          return objLista{0};", NombreEntidad);
                    sw.WriteLine("      }");

                    sw.WriteLine("");
                    sw.WriteLine("");


                    sw.WriteLine("      public void Insertar{0}({0} obj{0})", NombreEntidad);
                    sw.WriteLine("      {");
                    sw.WriteLine("          try");
                    sw.WriteLine("          {");
                    sw.WriteLine("              using (var dbContext = new ContextAMCTI())");
                    sw.WriteLine("              {");
                    sw.WriteLine("                  dbContext.{0}.Add(obj{0});", NombreEntidad);
                    sw.WriteLine("                  dbContext.SaveChanges();");
                    sw.WriteLine("              }");
                    sw.WriteLine("          }");
                    sw.WriteLine("          catch (Exception exc)");
                    sw.WriteLine("          {");
                    sw.WriteLine("              throw exc;");
                    sw.WriteLine("          }");
                    sw.WriteLine("      }");


                    sw.WriteLine("      public void Actualizar{0}({0} _obj{0})", NombreEntidad);
                    sw.WriteLine("      {");
                    sw.WriteLine("          try");
                    sw.WriteLine("          {");
                    sw.WriteLine("              using (var dbContext = new ContextAMCTI())");
                    sw.WriteLine("              {");
                    sw.WriteLine("                  {0} obj{0} = dbContext.{0}.FirstOrDefault(x => x.id{0} == _obj{0}.id{0});", NombreEntidad);
                    sw.WriteLine("                  if (obj{0} != null)", NombreEntidad);
                    sw.WriteLine("                  {");
                    sw.WriteLine(parametros3);
                    sw.WriteLine("                     dbContext.SaveChanges();");
                    sw.WriteLine("                  }");
                    sw.WriteLine("              }");
                    sw.WriteLine("          }");
                    sw.WriteLine("          catch (Exception exc)");
                    sw.WriteLine("          {");
                    sw.WriteLine("          throw exc;");
                    sw.WriteLine("          }");
                    sw.WriteLine("      }");
                    sw.WriteLine("");
                    sw.WriteLine("      #endregion");


                }

            }
        }


        private void CrearControladorDatosGeneral()
        {
            if (!Directory.Exists(txtRuta.Text + @"\CrearControladorDatosGeneral\"))
            {
                Directory.CreateDirectory(txtRuta.Text + @"\CrearControladorDatosGeneral\");
            }

            string FILE_NAME = txtRuta.Text + @"\CrearControladorDatosGeneral\" + "ControladorDatosMovilidad.cs";
            if (File.Exists(FILE_NAME))
            {
                File.Delete(FILE_NAME);
            }
            using (StreamWriter sw = File.CreateText(FILE_NAME))
            {
                sw.WriteLine("using ITM.AutoMedicionCTI.AccesoDatos;");
                sw.WriteLine("using ITM.AutoMedicionCTI.AccesoDatos.AMCTI.EntityFramework.Modelos;");
                sw.WriteLine("using System;");
                sw.WriteLine("using System.Collections.Generic;");
                sw.WriteLine("using System.Configuration;");
                sw.WriteLine("using System.Data;");
                sw.WriteLine("using System.Data.SqlClient;");
                sw.WriteLine("using System.Linq;");

                sw.WriteLine("namespace ITM.AutoMedicionCTI.{0}", "Movilidad");
                sw.WriteLine("  {");
                sw.WriteLine("      public class ControladorDatos{0}", "Movilidad");
                sw.WriteLine("      {");
                sw.WriteLine("");
                sw.WriteLine("      private string connectionString = ConfigurationManager.ConnectionStrings['ContextAMCTI'].ConnectionString;");
                sw.WriteLine("");
                foreach (DataRow dr1 in Tablas.Rows)
                {

                    string NombreEntidad = dr1["TABLE_NAME"].ToString();


                    string parametros1 = String.Empty;
                    string parametros2 = String.Empty;
                    string parametros3 = String.Empty;
                    foreach (DataRow dr in TablaDatos.Rows)
                    {
                        if (NombreEntidad == dr["TABLA"].ToString())
                        {
                            string atributo = dr["ATRIBUTO"].ToString();
                            if (dr["ATRIBUTO"].ToString().Substring(0, 2) == "fk")
                            {
                                    atributo = dr["ATRIBUTO"].ToString().Remove(0, 3);
                            }
                            parametros1 = parametros1 + String.Format("{0}? {1} = null,", dr["TIPOC"].ToString(), atributo);
                                parametros2 = parametros2 + String.Format("x.{0} == ({1} == null ? x.{0} : {1}) && ", dr["ATRIBUTO"].ToString(), atributo);
                            
                            if (dr["ATRIBUTO"].ToString().Substring(0, 2) != "id")
                            {
                                parametros3 = "                   " + parametros3 + String.Format("obj{0}.{1} = _obj{0}.{1} == null ? obj{0}.{1} : _obj{0}.{1};\n", NombreEntidad, dr["ATRIBUTO"].ToString());
                            }
                        }
                    }
                    parametros1 = parametros1.Remove(parametros1.Length - 1);
                    parametros2 = parametros2.Remove(parametros2.Length - 3);
                    sw.WriteLine("      #region {0}", NombreEntidad);
                    sw.WriteLine("");
                    sw.WriteLine("      public List<{0}> Obtener{0}({1})", NombreEntidad, parametros1);
                    sw.WriteLine("      {");
                    sw.WriteLine("          List <{0}> objLista{0} = new List<{0}>();", NombreEntidad);
                    sw.WriteLine("          try");
                    sw.WriteLine("          {");
                    sw.WriteLine("              using (var dbContext = new ContextAMCTI())");
                    sw.WriteLine("              {");
                    sw.WriteLine("                  objLista{0} = dbContext.{0}.Where(x=> {1}).ToList();", NombreEntidad, parametros2);
                    sw.WriteLine("              }");
                    sw.WriteLine("          }");
                    sw.WriteLine("          catch (Exception exc)");
                    sw.WriteLine("          {");
                    sw.WriteLine("              throw exc;");
                    sw.WriteLine("          }");
                    sw.WriteLine("          return objLista{0};", NombreEntidad);
                    sw.WriteLine("      }");

                    sw.WriteLine("");
                    sw.WriteLine("");


                    sw.WriteLine("      public void Insertar{0}({0} obj{0})", NombreEntidad);
                    sw.WriteLine("      {");
                    sw.WriteLine("          try");
                    sw.WriteLine("          {");
                    sw.WriteLine("              using (var dbContext = new ContextAMCTI())");
                    sw.WriteLine("              {");
                    sw.WriteLine("                  dbContext.{0}.Add(obj{0});", NombreEntidad);
                    sw.WriteLine("                  dbContext.SaveChanges();");
                    sw.WriteLine("              }");
                    sw.WriteLine("          }");
                    sw.WriteLine("          catch (Exception exc)");
                    sw.WriteLine("          {");
                    sw.WriteLine("              throw exc;");
                    sw.WriteLine("          }");
                    sw.WriteLine("      }");
                    sw.WriteLine("");
                    sw.WriteLine("      public void Actualizar{0}({0} _obj{0})", NombreEntidad);
                    sw.WriteLine("      {");
                    sw.WriteLine("          try");
                    sw.WriteLine("          {");
                    sw.WriteLine("              using (var dbContext = new ContextAMCTI())");
                    sw.WriteLine("              {");
                    sw.WriteLine("                  {0} obj{0} = dbContext.{0}.FirstOrDefault(x => x.id{0} == _obj{0}.id{0});", NombreEntidad);
                    sw.WriteLine("                  if (obj{0} != null)", NombreEntidad);
                    sw.WriteLine("                  {");
                    sw.WriteLine(parametros3);
                    sw.WriteLine("                     dbContext.SaveChanges();");
                    sw.WriteLine("                  }");
                    sw.WriteLine("              }");
                    sw.WriteLine("          }");
                    sw.WriteLine("          catch (Exception exc)");
                    sw.WriteLine("          {");
                    sw.WriteLine("          throw exc;");
                    sw.WriteLine("          }");
                    sw.WriteLine("      }");
                    sw.WriteLine("");
                    sw.WriteLine("");
                    sw.WriteLine("      #endregion");
                }
                sw.WriteLine("      }");
                sw.Close();
            }
        }

        private void CrearEntidadesEntity()
        {
            if (!Directory.Exists(txtRuta.Text + @"\Entidades\"))
            {
                Directory.CreateDirectory(txtRuta.Text + @"\Entidades\");
            }

            string archivoContext = txtRuta.Text + @"\Entidades\Context.cs";
            if (File.Exists(archivoContext))
            {
                File.Delete(archivoContext);
            }
                
                using (StreamWriter sw = File.CreateText(archivoContext))
                {
                    foreach (DataRow dr1 in Tablas.Rows)
                    { 
                        sw.WriteLine("public DbSet<" + dr1["TABLE_NAME"].ToString() + "> " + dr1["TABLE_NAME"].ToString() + " { get; set; }");
                    }
                sw.Close();
            }
                
    
                foreach (DataRow dr1 in Tablas.Rows)
            {

                string NombreEntidad = dr1["TABLE_NAME"].ToString();


                if (!Directory.Exists(txtRuta.Text + @"\Entidades\"))
                {
                    Directory.CreateDirectory(txtRuta.Text + @"\Entidades\");
                }


                string FILE_NAME = txtRuta.Text + @"\Entidades\" + NombreEntidad + ".cs";
                if (File.Exists(FILE_NAME))
                {
                    File.Delete(FILE_NAME);
                }
                using (StreamWriter sw = File.CreateText(FILE_NAME))
                {
                    sw.WriteLine("using System;");
                    sw.WriteLine("using System.ComponentModel.DataAnnotations;");
                    sw.WriteLine("using System.ComponentModel.DataAnnotations.Schema;");
                    sw.WriteLine("");
                    sw.WriteLine("");
                    sw.WriteLine("namespace ITM.AutoMedicionCTI.AccesoDatos.AMCTI.EntityFramework.Modelos");
                    sw.WriteLine("{");
                    sw.WriteLine("");
                    sw.WriteLine("  public partial class {0}", NombreEntidad);
                    sw.WriteLine("  {");
                    sw.WriteLine("");
                    sw.WriteLine("      [Key]");
                    sw.WriteLine("      [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]");
                    foreach (DataRow dr in TablaDatos.Rows)
                    {
                        if (NombreEntidad == dr["TABLA"].ToString())
                        {
                            string nulo1 = String.Empty;
                            string nulo2 = String.Empty;
                            if ((!String.IsNullOrEmpty(dr["ESNULO"].ToString()))&& (dr["TIPODATO"].ToString()!= "varchar" && dr["TIPODATO"].ToString() != "nvarchar") )
                            {
                                nulo1 = "Nullable <";
                                nulo2 = ">";
                            }

                            sw.WriteLine("      public {0}{1}{2} {3} ", nulo1, dr["TIPOC"].ToString(),nulo2, dr["ATRIBUTO"].ToString()+"{ get; set; }");
                        }
                    }

                    sw.WriteLine("  }");
                    sw.WriteLine("}");
                    sw.Close();
                }

            }
        }

        private void CrearArchivoVista()
        {
            foreach (DataRow dr1 in Tablas.Rows)
            {

                string NombreEntidad = dr1["TABLE_NAME"].ToString();


                if (!Directory.Exists(txtRuta.Text + @"\Vista\"))
                {
                    Directory.CreateDirectory(txtRuta.Text + @"\Vista\");
                }


                string FILE_NAME = txtRuta.Text + @"\Vista\" + NombreEntidad + ".aspx";
                if (File.Exists(FILE_NAME))
                {
                    File.Delete(FILE_NAME);
                }
                using (StreamWriter sw = File.CreateText(FILE_NAME))
                {
                    string listaCampos = String.Empty;
                    foreach (DataRow dr in TablaDatos.Rows)
                    {
                        if (NombreEntidad == dr["TABLA"].ToString())
                        {
                            string linea = String.Empty;
                            bool esDropDown = false;
                            string campo = dr["ATRIBUTO"].ToString();
                            string nombreControl = "txt" + campo;

                            listaCampos = listaCampos +","+ campo;
                            if (campo.Substring(0,2)=="fk")
                            {
                                nombreControl = "ddl" + campo.Remove(0,5);
                                esDropDown = true;
                            }

                            
                                sw.WriteLine("<div class='row'>");
                                sw.WriteLine("  <div class='col-sm-4'>");
                                sw.WriteLine("      <label>{0}</label>", campo);
                                sw.WriteLine("      <asp:RequiredFieldValidator ID = 'rfv{0}' runat = 'server' ControlToValidate = '{0}' ErrorMessage = ' * Requerido ' Font-Bold = 'true' ForeColor = '#da4932' SetFocusOnError = 'true' ValidationGroup = '{1}'></asp:RequiredFieldValidator>",nombreControl, dr["TABLA"].ToString());

                            
                            if (!esDropDown)
                            {
                                string tipoDato = dr["TIPODATO"].ToString() == "datetime" ? "TextMode = 'Date'" : "";
                                string maxLength = (!String.IsNullOrEmpty(dr["MAXIMO"].ToString()) ? "MaxLength='" + dr["MAXIMO"].ToString() + "'" : "");
                                sw.WriteLine("      <asp:TextBox ID = '{0}' runat = 'server' CssClass = 'form-control' placeholder = '{1}' "+tipoDato+maxLength+
                                    "></asp:TextBox>", nombreControl, campo);
                            }
                            else
                            {
                                sw.WriteLine("      <asp:DropDownList ID = '{0}' runat = 'server' CssClass = 'form-control' AutoPostBack = 'true' placeholder = '{0}'></asp:DropDownList>", nombreControl, campo);
                            }
                                sw.WriteLine("  </div>");
                                sw.WriteLine("</div>");
                        }
                    }
                    listaCampos = listaCampos.Remove(0,1);

                    sw.WriteLine("<div class='row'>");
                    sw.WriteLine("  <div class='col-sm-12'>");
                    sw.WriteLine("      <div class='table-responsive'>");
                    sw.WriteLine("          <asp:GridView ID = 'grid{0}' runat='server' AutoGenerateColumns='false' DataKeyNames='{1}' AllowPaging='True' AllowSorting='True' CssClass='table table - bordered bs - table' PageSize='30' HeaderStyle-BackColor='#333333' HeaderStyle-ForeColor='White' Font-Bold='true'>", NombreEntidad, listaCampos);
                    sw.WriteLine("              <Columns>");
                    sw.WriteLine("                  <asp:CommandField ShowSelectButton = 'True' SelectText='...   '><ControlStyle CssClass = 'glyphicon glyphicon-arrow-right' /></asp:CommandField>");
                    
                    foreach (DataRow dr in TablaDatos.Rows)
                    {
                        if (NombreEntidad == dr["TABLA"].ToString())
                        {
                            sw.WriteLine("                  <asp:BoundField DataField = '{0}' HeaderText='{0}' />", dr["ATRIBUTO"].ToString());
                        }
                    }
                    sw.WriteLine("              </Columns>");
                    sw.WriteLine("          </asp:GridView>");
                    sw.WriteLine("      </div>");
                    sw.WriteLine("  </div>");
                    sw.WriteLine("</div>");

                    sw.Close();
                }

            }
        }

        private void btnBucar_Click(object sender, EventArgs e)
        {
            BuscarFolder.ShowDialog();
            txtRuta.Text = BuscarFolder.SelectedPath;
        }

    }
}


