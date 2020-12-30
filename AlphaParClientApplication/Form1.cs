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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ServiceReference1.ServiceClient client = new ServiceReference1.ServiceClient();
            try
            {
                string userToken = client.LoginWithAD(textBox1.Text, textBox2.Text);
                //label1.Text = string.Format("Welcome {0}", client.GetUsernameByToken(userToken));

                var form2 = new Form2(userToken);
                form2.Location = this.Location;
                form2.StartPosition = FormStartPosition.Manual;
                form2.FormClosing += delegate { this.Show(); };
                form2.Show();
                this.Hide();
            }
            catch (FaultException<ServiceReference1.ServiceFault> fault)
            {
                label1.Text = fault.Detail.Message;
            }
        }

        private bool checkStringIsNotError(string returnString)
        {
            return !returnString.Contains("Error:");
        }
    }
}
