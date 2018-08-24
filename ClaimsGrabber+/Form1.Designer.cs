namespace ClaimsGrabber_
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.drpApplications = new System.Windows.Forms.ComboBox();
            this.lblApplications = new System.Windows.Forms.Label();
            this.lblEnvironment = new System.Windows.Forms.Label();
            this.drpEnvironments = new System.Windows.Forms.ComboBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.gridSaml = new System.Windows.Forms.DataGridView();
            this.btnDownloadXml = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridSaml)).BeginInit();
            this.SuspendLayout();
            // 
            // drpApplications
            // 
            this.drpApplications.Enabled = false;
            this.drpApplications.FormattingEnabled = true;
            this.drpApplications.Location = new System.Drawing.Point(95, 49);
            this.drpApplications.Name = "drpApplications";
            this.drpApplications.Size = new System.Drawing.Size(284, 21);
            this.drpApplications.TabIndex = 1;
            this.drpApplications.SelectedIndexChanged += new System.EventHandler(this.drpApplications_SelectedIndexChanged);
            // 
            // lblApplications
            // 
            this.lblApplications.AutoSize = true;
            this.lblApplications.Location = new System.Drawing.Point(22, 52);
            this.lblApplications.Name = "lblApplications";
            this.lblApplications.Size = new System.Drawing.Size(67, 13);
            this.lblApplications.TabIndex = 0;
            this.lblApplications.Text = "Applications:";
            // 
            // lblEnvironment
            // 
            this.lblEnvironment.AutoSize = true;
            this.lblEnvironment.Location = new System.Drawing.Point(22, 15);
            this.lblEnvironment.Name = "lblEnvironment";
            this.lblEnvironment.Size = new System.Drawing.Size(69, 13);
            this.lblEnvironment.TabIndex = 5;
            this.lblEnvironment.Text = "Environment:";
            this.lblEnvironment.Click += new System.EventHandler(this.lblEnvironment_Click);
            // 
            // drpEnvironments
            // 
            this.drpEnvironments.FormattingEnabled = true;
            this.drpEnvironments.Location = new System.Drawing.Point(95, 12);
            this.drpEnvironments.Name = "drpEnvironments";
            this.drpEnvironments.Size = new System.Drawing.Size(192, 21);
            this.drpEnvironments.TabIndex = 6;
            this.drpEnvironments.SelectedIndexChanged += new System.EventHandler(this.drpEnvironments_SelectedIndexChanged);
            // 
            // btnGo
            // 
            this.btnGo.Enabled = false;
            this.btnGo.Location = new System.Drawing.Point(25, 90);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 2;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(123, 90);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(0, 13);
            this.lblResult.TabIndex = 7;
            // 
            // gridSaml
            // 
            this.gridSaml.AllowUserToAddRows = false;
            this.gridSaml.AllowUserToDeleteRows = false;
            this.gridSaml.AllowUserToOrderColumns = true;
            this.gridSaml.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSaml.Location = new System.Drawing.Point(12, 121);
            this.gridSaml.Name = "gridSaml";
            this.gridSaml.ReadOnly = true;
            this.gridSaml.Size = new System.Drawing.Size(737, 163);
            this.gridSaml.TabIndex = 8;
            // 
            // btnDownloadXml
            // 
            this.btnDownloadXml.Enabled = false;
            this.btnDownloadXml.Location = new System.Drawing.Point(25, 297);
            this.btnDownloadXml.Name = "btnDownloadXml";
            this.btnDownloadXml.Size = new System.Drawing.Size(111, 23);
            this.btnDownloadXml.TabIndex = 9;
            this.btnDownloadXml.Text = "Download Xml";
            this.btnDownloadXml.UseVisualStyleBackColor = true;
            this.btnDownloadXml.Click += new System.EventHandler(this.btnDownloadXml_Click_1);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Lucida Sans", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.lblStatus.Location = new System.Drawing.Point(458, 287);
            this.lblStatus.MaximumSize = new System.Drawing.Size(300, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(45, 14);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "Status:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 338);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnDownloadXml);
            this.Controls.Add(this.gridSaml);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.drpEnvironments);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.lblEnvironment);
            this.Controls.Add(this.drpApplications);
            this.Controls.Add(this.lblApplications);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.gridSaml)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox drpApplications;
        private System.Windows.Forms.Label lblApplications;
        private System.Windows.Forms.Label lblEnvironment;
        private System.Windows.Forms.ComboBox drpEnvironments;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.DataGridView gridSaml;
        private System.Windows.Forms.Button btnDownloadXml;
        private System.Windows.Forms.Label lblStatus;

    }
}

