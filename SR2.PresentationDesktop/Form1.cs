using SR2.DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SR2.PresentationDesktop
{
    public partial class Form1 : Form
    {
        private readonly HttpClient client;
        public Form1()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:34978/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            HttpResponseMessage respone = await client.GetAsync("api/Brands");
            listView1.Items.Clear();
            if (respone.IsSuccessStatusCode)
            {
                var brands = await respone.Content.ReadAsAsync<List<Brand>>();
                foreach(var item in brands)
                {
                    var li = listView1.Items.Add(item.BrandId.ToString());
                    li.SubItems.Add(item.BrandName);
                    li.SubItems.Add(item.Description);
                }
            }
            else
            {
                var errorMsg = respone.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                MessageBox.Show(errorMsg);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Brand field is required");
                return;
            }
            var brand = new Brand
            {
                BrandName = textBox1.Text,
                Description = textBox2.Text
            };
            HttpResponseMessage response = await client.PostAsJsonAsync<Brand>("api/Brands", brand);
            if (response.IsSuccessStatusCode)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                MessageBox.Show("The record was saved");
                Form1_Load(sender, e);
            }
            else
            {
                var errorMsg = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                MessageBox.Show(errorMsg);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var result = MessageBox.Show("Do you want to delete this record?","Confirmation", 
                    MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    var id = listView1.SelectedItems[0].Text;
                    HttpResponseMessage response = await client.DeleteAsync("api/Brands/" + id);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("This record was deleted");
                        Form1_Load(sender,e);
                    }
                    else
                    {
                        var errorMsg = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        MessageBox.Show(errorMsg);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select an item");
            }
        }
    }
}
