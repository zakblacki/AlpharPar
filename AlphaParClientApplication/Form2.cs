using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlphaParClientApplication
{
    public partial class Form2 : Form
    {
        string token;
        ServiceReference1.ServiceClient serviceClient;
        ServiceReference1.Client[] clients;

        public Form2(string token)
        {
            InitializeComponent();
            ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;

            serviceClient = new ServiceReference1.ServiceClient();

            this.token = token;
            GetUsername();
            FillClientsTable();
        }

        private void GetUsername()
        {
            try
            {
                string username = serviceClient.GetUsernameByToken(token);
                label1.Text = string.Format("Connected as : {0}", username);
            }
            catch (FaultException<ServiceReference1.ServiceFault> fault)
            {
                label1.Text = fault.Detail.Message;
            }
        }

        private void FillClientsTable()
        {
            try
            {
                clients = serviceClient.GetClients(token);
                //Fill table
                dataGridView1.DataSource = new BindingList<ServiceReference1.Client>(clients);
                CreateDeleteColumn();
                dataGridView1.Columns["ID"].Visible = false;
                
            }
            catch(FaultException<ServiceReference1.ServiceFault> fault)
            {
                // Todo: Display error in fault.Detail.Message
            }
        }

        private void CreateDeleteColumn()
        {
            if (!dataGridView1.Columns.Contains("DeleteColumn"))
            {
                DataGridViewButtonColumn DeleteColumn = new DataGridViewButtonColumn();
                DeleteColumn.HeaderText = "Action";
                DeleteColumn.Name = "DeleteColumn";
                DeleteColumn.Text = "Delete";
                DeleteColumn.UseColumnTextForButtonValue = true;
                dataGridView1.Columns.Add(DeleteColumn);

                dataGridView1.CellContentClick += new DataGridViewCellEventHandler(dataGridView1_CellContentClick);
            }

            dataGridView1.Columns["DeleteColumn"].DisplayIndex = 5;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceReference1.Client client = new ServiceReference1.Client();
                client.lastname = textBox1.Text;
                client.firstname = textBox2.Text;
                client.address = textBox3.Text;
                client.phone = textBox4.Text;

                serviceClient.InsertClient(token, client);

                FillClientsTable();
            }
            catch(FaultException<ServiceReference1.ServiceFault> fault)
            {
                // Todo: Display error in fault.Detail.Message
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["DeleteColumn"].Index)
            {
                try
                {
                    bool clientIsDeleted = serviceClient.DeleteClient(token, (ServiceReference1.Client)dataGridView1.Rows[e.RowIndex].DataBoundItem);

                    FillClientsTable();
                }
                catch(FaultException<ServiceReference1.ServiceFault> fault)
                {
                    // Todo: Display error in fault.Detail.Message
                }
            }
        }
    }
}
