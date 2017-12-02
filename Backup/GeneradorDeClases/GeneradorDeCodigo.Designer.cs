namespace GeneradorDeClases
{
    partial class GeneradorDeCodigo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblCadenaConexion = new System.Windows.Forms.Label();
            this.txtCadenadeConexion = new System.Windows.Forms.TextBox();
            this.btnGenerarCodigo = new System.Windows.Forms.Button();
            this.txtRuta = new System.Windows.Forms.TextBox();
            this.lblRuta = new System.Windows.Forms.Label();
            this.BuscarFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.btnBucar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblCadenaConexion
            // 
            this.lblCadenaConexion.AutoSize = true;
            this.lblCadenaConexion.Location = new System.Drawing.Point(12, 51);
            this.lblCadenaConexion.Name = "lblCadenaConexion";
            this.lblCadenaConexion.Size = new System.Drawing.Size(108, 13);
            this.lblCadenaConexion.TabIndex = 1;
            this.lblCadenaConexion.Text = "CadenaDe Conexion:";
            // 
            // txtCadenadeConexion
            // 
            this.txtCadenadeConexion.Location = new System.Drawing.Point(128, 48);
            this.txtCadenadeConexion.Name = "txtCadenadeConexion";
            this.txtCadenadeConexion.Size = new System.Drawing.Size(263, 20);
            this.txtCadenadeConexion.TabIndex = 2;
            this.txtCadenadeConexion.Text = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=BDCentroTecn" +
                "icoAutomotriz;Data Source=SALAF407-12";
            // 
            // btnGenerarCodigo
            // 
            this.btnGenerarCodigo.Location = new System.Drawing.Point(316, 132);
            this.btnGenerarCodigo.Name = "btnGenerarCodigo";
            this.btnGenerarCodigo.Size = new System.Drawing.Size(75, 23);
            this.btnGenerarCodigo.TabIndex = 8;
            this.btnGenerarCodigo.Text = "Generar";
            this.btnGenerarCodigo.UseVisualStyleBackColor = true;
            this.btnGenerarCodigo.Click += new System.EventHandler(this.btnGenerarCodigo_Click);
            // 
            // txtRuta
            // 
            this.txtRuta.Location = new System.Drawing.Point(128, 74);
            this.txtRuta.Name = "txtRuta";
            this.txtRuta.Size = new System.Drawing.Size(263, 20);
            this.txtRuta.TabIndex = 10;
            // 
            // lblRuta
            // 
            this.lblRuta.AutoSize = true;
            this.lblRuta.Location = new System.Drawing.Point(12, 77);
            this.lblRuta.Name = "lblRuta";
            this.lblRuta.Size = new System.Drawing.Size(77, 13);
            this.lblRuta.TabIndex = 9;
            this.lblRuta.Text = "Ruta Archivos:";
            // 
            // btnBucar
            // 
            this.btnBucar.Location = new System.Drawing.Point(396, 72);
            this.btnBucar.Name = "btnBucar";
            this.btnBucar.Size = new System.Drawing.Size(29, 23);
            this.btnBucar.TabIndex = 11;
            this.btnBucar.Text = "...";
            this.btnBucar.UseVisualStyleBackColor = true;
            this.btnBucar.Click += new System.EventHandler(this.btnBucar_Click);
            // 
            // GeneradorDeCodigo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 234);
            this.Controls.Add(this.btnBucar);
            this.Controls.Add(this.txtRuta);
            this.Controls.Add(this.lblRuta);
            this.Controls.Add(this.btnGenerarCodigo);
            this.Controls.Add(this.txtCadenadeConexion);
            this.Controls.Add(this.lblCadenaConexion);
            this.Name = "GeneradorDeCodigo";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCadenaConexion;
        private System.Windows.Forms.TextBox txtCadenadeConexion;
        private System.Windows.Forms.Button btnGenerarCodigo;
        private System.Windows.Forms.TextBox txtRuta;
        private System.Windows.Forms.Label lblRuta;
        private System.Windows.Forms.FolderBrowserDialog BuscarFolder;
        private System.Windows.Forms.Button btnBucar;
    }
}

