using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;


namespace ClaimsGrabber_
{
    public partial class Form1 : Form
    {

        Hashtable rptsToUrl = new Hashtable();
        string xmlShare = @"\\Claimsgrabberplus\ClaimsGrabber$\";
        string environmentsXml = @"\\Claimsgrabberplus\ClaimsGrabber$\Environments.xml";
        string environmentShort = string.Empty;
        SAML samlObj;

        public Form1()
        {
            InitializeComponent();

            // bind xml to dropdownlist xml
            try
            {
                PopulateEnvironmentDropDownFromXml(environmentsXml);
            }
            catch{}

        }

        private void drpApplications_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnGo.Enabled = true;
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            // Set status
            lblStatus.Text = "Working...";

            // Setup variables
            string rpt = (environmentShort + "-" + drpApplications.SelectedItem.ToString());
            string url = (string)rptsToUrl[rpt];

            // Get Attributes for responseFromServer
            try
            {
               samlObj = new SAML(url, rpt);

                // Update gridview with claims
                gridSaml.DataSource = (samlObj.Attributes);
                gridSaml.Columns["Name"].DisplayIndex = 0;
                gridSaml.Columns["Value"].DisplayIndex = 1;
                gridSaml.Columns["Description"].DisplayIndex = 2;
                gridSaml.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                gridSaml.Columns["Value"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                gridSaml.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


                // Add context menu to gridview
                ContextMenuStrip mnu = new ContextMenuStrip();
                ToolStripMenuItem mnuCopy = new ToolStripMenuItem("Copy");
                mnuCopy.Click += new EventHandler(mnuCopy_Click);
                mnu.Items.AddRange(new ToolStripItem[] { mnuCopy });
                gridSaml.ContextMenuStrip = mnu;

                btnDownloadXml.Enabled = true;
                lblResult.Text = "";

            }
            catch (Exception ex)
            {
                lblResult.Text = ex.Message;
                gridSaml.DataSource = null;
                btnDownloadXml.Enabled = false;
            }

            lblStatus.Text = "Status: " + drpApplications.SelectedItem.ToString() + " Complete";
            
        }

        private void drpEnvironments_SelectedIndexChanged(object sender, EventArgs e)
        {

            // Get Environment
            string environment = drpEnvironments.SelectedItem.ToString();
            environmentShort = environment;

            // Get RPT file details
            //string filePath = @"C:\temp\ClaimsGrabber+\" + environment.Split('.')[0] + "_RPTExport.xml";
            string filePath = xmlShare + environment + "_RPTExport.xml";
            filePath = filePath.Replace(" (Prod)", "").Replace(" (Test)", "");

            // Get RPT file locations
            drpApplications.Enabled = true;

            // Populdate drpApplications
            PopulateApplicationsDropDownFromXml(filePath);
        }

        // Populates the Applications drop down from xml
        private void PopulateApplicationsDropDownFromXml(string xmlfile)
        {
            drpApplications.Items.Clear();

            try
            {
                XElement xelement = XElement.Load(xmlfile);
                IEnumerable<XElement> applications = xelement.Elements();

                foreach (XElement xEle in applications)
                {
                    string name = (string)xEle.Attribute("name");
                    string url = (string)xEle.Attribute("url");


                    drpApplications.Items.Add(name);

                    // Populate hash table
                    string hashTableVal = (environmentShort + "-" + name);
                    if (!(rptsToUrl.ContainsKey(hashTableVal)))
                    {
                        rptsToUrl.Add(hashTableVal, url);
                    }
                    
                }

                lblResult.Text = "";

            }
            catch
            {
                lblResult.Text = ("##Error## Xml file " + xmlfile + " not found.");
                lblResult.Visible = true;
            }

        }

        // Populates the Environments drop down from xml
        private void PopulateEnvironmentDropDownFromXml(string xmlfile)
        {
            try
            {
                XElement xelement = XElement.Load(xmlfile);
                IEnumerable<XElement> environments = xelement.Elements();

                foreach (XElement xEle in environments)
                    drpEnvironments.Items.Add(xEle.Value);

                lblResult.Text = "";

            }
            catch
            {
                
                lblResult.Text = ("##Error## Xml file " + xmlfile + " not found.");
                lblResult.Visible = true;
            }

        }

        private void enableServerSideTab()
        {

        }

        private void disableServerSideTab()
        {

        }

        private void lblEnvironment_Click(object sender, EventArgs e)
        {

        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {
            string val = gridSaml.SelectedCells[0].Value.ToString();
            Clipboard.SetText(val);
        }

        private void btnDownloadXml_Click_1(object sender, EventArgs e)
        {
            // Add sts & url attribs to xml
            string rpt = (environmentShort + "-" + drpApplications.SelectedItem.ToString());
            string url = (string)rptsToUrl[rpt];

            // Open file dialog and save to file
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Xml Files | *.xml";
            saveFileDialog.DefaultExt = "xml";
            saveFileDialog.FileName = ("MySamlToken.xml");
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                samlObj.WriteToXml(saveFileDialog.FileName);
            }
        }

    }
}

