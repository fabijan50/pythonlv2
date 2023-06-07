using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Pythonlv2WinFormApp
{
    public partial class Form1 : Form
    {
        TcpClient client;
        Random random = new Random();
        string message;
        public Form1()
        {
            InitializeComponent();
        }

        public double generateRandomDouble(double lB, double uB)
        {
            double randomDouble = random.NextDouble();
            double randomDoubleInRange = randomDouble * (uB - lB) + lB;
            return randomDoubleInRange;
        }
        public string getRandomColor() {
            int randomInt = random.Next(1,7);
            string mColor;
            switch(randomInt)
            {
                case 1:
                    mColor = "red";
                    break;
                case 2:
                    mColor = "blue";
                    break;
                case 3:
                    mColor = "black";
                    break;
                case 4:
                    mColor = "green";
                    break;
                case 5:
                    mColor = "yellow";
                    break;
                case 6:
                    mColor = "white";
                    break;
                default:
                    mColor = "red";
                    break;
            }
            return mColor;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void BTN_Connect_Click(object sender, EventArgs e)
        {             
            client = new TcpClient(txtBhostname.Text, Int32.Parse(txtBhostport.Text));

            NetworkStream stream = client.GetStream();
            byte[] data = new byte[1024];
            int bytesRead = stream.Read(data, 0, data.Length);
            string message = Encoding.ASCII.GetString(data, 0, bytesRead);

            if (message == "Connection established.")
            {
                ConnectionPanel.BackColor = Color.Green;
            }         

        }

        private void BTN_CloseConnection_Click(object sender, EventArgs e)
        {                 
            if (client != null && client.Connected)
            {
                client.Close();
                ConnectionPanel.BackColor = Color.Red;
            }
            
        }

        private void BTN_Lrandom_Click(object sender, EventArgs e)
        {
            txtBL1X.Text= ((int)generateRandomDouble(5, 795)).ToString();
            txtBL1Y.Text= ((int)generateRandomDouble(5, 595)).ToString();
            txtBL2X.Text= ((int)generateRandomDouble(5, 795)).ToString();
            txtBL2Y.Text= ((int)generateRandomDouble(5, 595)).ToString();

            txtBLboja.Text = getRandomColor();

        }

        private void BTN_Lsend_Click(object sender, EventArgs e)
        {
            if (client != null && client.Connected)
            {
                message = String.Format("Line " + txtBLboja.Text + " " + txtBL1X.Text + " " + txtBL1Y.Text + " " + txtBL2X.Text + " " + txtBL2Y.Text + "\n");
                int byteCount = Encoding.ASCII.GetByteCount(message);
                byte[] sendData = new byte[byteCount];
                sendData = Encoding.ASCII.GetBytes(message);
                NetworkStream stream = client.GetStream();
                stream.Write(sendData, 0, sendData.Length);
            }
        }

        private void BTN_Trandom_Click(object sender, EventArgs e)
        {
            txtBT1X.Text= ((int)generateRandomDouble(5, 795)).ToString();
            txtBT1Y.Text= ((int)generateRandomDouble(5, 595)).ToString();
            txtBT2X.Text= ((int)generateRandomDouble(5, 795)).ToString();
            txtBT2Y.Text= ((int)generateRandomDouble(5, 595)).ToString();
            txtBT3X.Text= ((int)generateRandomDouble(5, 795)).ToString();
            txtBT3Y.Text= ((int)generateRandomDouble(5, 595)).ToString();

            txtBTboja.Text = getRandomColor();

        }

        private void BTN_Tsend_Click(object sender, EventArgs e)
        {
            if (client != null && client.Connected)
            {
                message = String.Format("Triangle " + txtBTboja.Text + " " + txtBT1X.Text + " " + txtBT1Y.Text + " " + txtBT2X.Text + " " + txtBT2Y.Text + " " + txtBT3X.Text + " " + txtBT3Y.Text+ "\n");
                int byteCount = Encoding.ASCII.GetByteCount(message);   
                byte[] sendData = new byte[byteCount]; 
                sendData = Encoding.ASCII.GetBytes(message); 
                NetworkStream stream = client.GetStream(); 
                stream.Write(sendData, 0, sendData.Length);
            }
        }

        private void BTN_Pravorandom_Click(object sender, EventArgs e)
        {
            txtBPpolozajX.Text= ((int)generateRandomDouble(5, 795)).ToString();
            txtBPpolozajY.Text= ((int)generateRandomDouble(5, 595)).ToString();
            double granica1 = Double.Parse(txtBPpolozajX.Text);
            double granica2 = Double.Parse(txtBPpolozajY.Text);
            txtBPvisina.Text= ((int)generateRandomDouble(5, (595-granica2))).ToString();      
            txtBPsirina.Text= ((int)generateRandomDouble(5, (795-granica1))).ToString();      

            txtBPravoBoja.Text= getRandomColor();


        }

        private void BTN_Pravosend_Click(object sender, EventArgs e)
        {
            if (client != null && client.Connected)
            {
                message = String.Format("Rectangle " + txtBPravoBoja.Text + " " + txtBPpolozajX.Text + " " + txtBPpolozajY.Text + " " + txtBPvisina.Text + " " + txtBPsirina.Text+ "\n");
                int byteCount = Encoding.ASCII.GetByteCount(message);  
                byte[] sendData = new byte[byteCount]; 
                sendData = Encoding.ASCII.GetBytes(message);
                NetworkStream stream = client.GetStream(); 
                stream.Write(sendData, 0, sendData.Length);
            }
        }

        private void BTN_Krandom_Click(object sender, EventArgs e)
        {
            txtBKX.Text= ((int)generateRandomDouble(5, 795)).ToString();
            txtBKY.Text= ((int)generateRandomDouble(5, 595)).ToString();

            double x = Double.Parse(txtBKX.Text);
            double y = Double.Parse(txtBKY.Text);
            double[] udaljenosti = new double[4];
            udaljenosti[0]= x;
            udaljenosti[1] = 800 - x;
            udaljenosti[2] = y;
            udaljenosti[3] = 600 - y;

            txtBKradijus.Text= ((int)generateRandomDouble(5, udaljenosti.Min())).ToString();
            txtBKboja.Text= getRandomColor();
        }

        private void BTN_Ksend_Click(object sender, EventArgs e)
        {
            if (client != null && client.Connected)
            {
                message = String.Format("Circle " + txtBKboja.Text + " " + txtBKX.Text + " " + txtBKY.Text + " " + txtBKradijus.Text + "\n");
                int byteCount = Encoding.ASCII.GetByteCount(message);   
                byte[] sendData = new byte[byteCount]; 
                sendData = Encoding.ASCII.GetBytes(message); 
                NetworkStream stream = client.GetStream(); 
                stream.Write(sendData, 0, sendData.Length);
            }
        }

        private void BTN_ElipsaRandom_Click(object sender, EventArgs e)
        {
            txtBEX.Text= ((int)generateRandomDouble(5, 795)).ToString();
            txtBEY.Text= ((int)generateRandomDouble(5, 595)).ToString();
            double x = Double.Parse(txtBEX.Text);
            double y = Double.Parse(txtBEY.Text);
            double udaljenostx1= x;
            double udaljenostx2 = 800 - x;
            double udaljenosty1 = y;
            double udaljenosty2 = 600 - y;

            if(udaljenostx1>udaljenostx2)
                txtBEradijus1.Text = ((int)generateRandomDouble(5, udaljenostx2)).ToString();
            else
                txtBEradijus1.Text = ((int)generateRandomDouble(5, udaljenostx1)).ToString();

            if (udaljenosty1>udaljenosty2)
                txtBEradijus2.Text = ((int)generateRandomDouble(5, udaljenosty2)).ToString();
            else
                txtBEradijus2.Text = ((int)generateRandomDouble(5, udaljenosty1)).ToString();

            txtBEboja.Text= getRandomColor();

        }

        private void BTN_Esend_Click(object sender, EventArgs e)
        {
            if (client != null && client.Connected)
            {
                message = String.Format("Ellipse " + txtBEboja.Text + " " + txtBEX.Text + " " + txtBEY.Text + " " + txtBEradijus1.Text + " " + txtBEradijus2.Text + "\n");
                int byteCount = Encoding.ASCII.GetByteCount(message);   
                byte[] sendData = new byte[byteCount]; 
                sendData = Encoding.ASCII.GetBytes(message); 
                NetworkStream stream = client.GetStream(); 
                stream.Write(sendData, 0, sendData.Length);
            }
        }

        private void BTN_Polyaddpoint_Click(object sender, EventArgs e)
        {
            PolyListBox.Items.Add(txtBPolyX.Text +" "+ txtBPolyY.Text);
        }

        private void BTN_Polyrandom_Click(object sender, EventArgs e)
        {
            PolyListBox.Items.Clear();
         
            int brojvrhova = random.Next(4, 10);
            List<Point> vrhovi = new List<Point>();
            while (brojvrhova > 0)
            {
                double X = ((int)generateRandomDouble(5, 795));
                double Y = ((int)generateRandomDouble(5, 595));
                Point vrh = new Point((int)X, (int)Y);
                vrhovi.Add(vrh);
                brojvrhova--;
            }

            vrhovi.Sort((a, b) => ClockwiseComparer(vrhovi[0], a, b));

            int ClockwiseComparer(Point center, Point a, Point b)
            {
                double angleA = Math.Atan2(a.Y - center.Y, a.X - center.X);
                double angleB = Math.Atan2(b.Y - center.Y, b.X - center.X);
                if (angleA < angleB)
                    return -1;
                if (angleA > angleB)
                    return 1;
                return 0;
            }

            foreach (Point vrh in vrhovi)
            {
                PolyListBox.Items.Add(vrh.X.ToString() + " " + vrh.Y.ToString());
            }          

            txtBPolyboja.Text= getRandomColor();
        }
        
        private void BTN_Polysend_Click(object sender, EventArgs e)
        {
            if (client != null && client.Connected)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in PolyListBox.Items)
                {
                   sb.AppendLine(item.ToString());
                }
                string listBoxContents = sb.ToString().TrimEnd();
                string[] lines = listBoxContents.Split('\n');
                string modifiedMessage = "Polygon " + txtBPolyboja.Text + " " + string.Join(" ", lines);

                message = modifiedMessage + "\n";
                int byteCount = Encoding.ASCII.GetByteCount(message); 
                byte[] sendData = new byte[byteCount];
                sendData = Encoding.ASCII.GetBytes(message); 
                NetworkStream stream = client.GetStream();
                stream.Write(sendData, 0, sendData.Length);
            }
        }
    }
}
