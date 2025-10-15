using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace Gaybriel_2._0
{
    public partial class Form1 : Form
    {

        DataTable dt = new DataTable();

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "884hkdboLxYjUIphOkiiNOTNDdTMB4iNV6xE16Vn",
            BasePath  = "https://fir-vsgaybriel-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new FireSharp.FirebaseClient(config);

            if(client != null)
            {
                MessageBox.Show("Conexão Estabelecida");
            }

            dt.Columns.Add("Id");
            dt.Columns.Add("Nome");
            dt.Columns.Add("Endereço");
            dt.Columns.Add("Idade");

            dataGridView1.DataSource = dt;
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            FirebaseResponse resp = await client.GetTaskAsync("Contador/Nó");
            gaybriel get = resp.ResultAs<gaybriel>();

            
            var data = new Data
            {
                ID = (Convert.ToInt32(get.cnt) + 1).ToString(),
                Nome = textBox2.Text,
                Endereço = textBox3.Text,
                Idade = textBox4.Text
            };

            SetResponse response = await client.SetTaskAsync("Informação/" + textBox1.Text,data);
            Data result = response.ResultAs<Data>();

            MessageBox.Show("Dados Inseridos: " + result.ID);

            var obj = new gaybriel
            {
                cnt = data.ID
            };

            SetResponse response1 = await client.SetTaskAsync("Contador/Nó",obj);

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.GetTaskAsync("Informação/" + textBox1.Text);
            Data obj = response.ResultAs<Data>();

            textBox1.Text = obj.ID;
            textBox2.Text = obj.Nome;
            textBox3.Text = obj.Endereço;
            textBox4.Text = obj.Idade;

            MessageBox.Show("Dados Recuperados Com Sucesso");
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            var data = new Data
            {
                ID = textBox1.Text,
                Nome = textBox2.Text,
                Endereço = textBox3.Text,
                Idade = textBox4.Text
            };

            FirebaseResponse response = await client.UpdateTaskAsync("Informação/" + textBox1.Text, data);
            Data result = response.ResultAs<Data>();
            MessageBox.Show("Atualização de Dados no ID: " + result.ID);
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.DeleteTaskAsync("Informação/" + textBox1.Text);
            MessageBox.Show("Registro de ID Excluído: " + textBox1.Text);
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.DeleteTaskAsync("Informação");
            MessageBox.Show("Todos os Elementos Excluídos / Nó de Informação Foi Excluído");
        }


        private async void button6_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            export();
        }

        private async void export()
        {
            dt.Rows.Clear();
            int i = 0;
            FirebaseResponse resp1 = await client.GetTaskAsync("Contador/Nó");
            gaybriel obj1 = resp1.ResultAs<gaybriel>();
            int cnt = Convert.ToInt32(obj1.cnt);

            while (true)
            {

                if(i == cnt)
                {
                    break;
                }

                i++;

                try
                {
                    FirebaseResponse resp2 = await client.GetTaskAsync("Informação/" + i);
                    Data obj2 = resp2.ResultAs<Data>();

                    DataRow row = dt.NewRow();

                    row["Id"] = obj2.ID;
                    row["Nome"] = obj2.Nome;
                    row["Endereço"] = obj2.Endereço;
                    row["Idade"] = obj2.Idade;

                    dt.Rows.Add(row);

                }
                catch
                {

                }
            }

            MessageBox.Show("Feito!!!");

        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Selecione Imagem";
            ofd.Filter = "Arquivo de Imagem(*,jpg) | *.jpg";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Image img = new Bitmap(ofd.FileName);
                imageBox.Image = img.GetThumbnailImage(350, 200, null, new IntPtr());
            }
        }
    }
}
