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
using System.IO;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;
using System.Collections.Specialized;
using System.Threading;
using System.Net.Http;

namespace Lazy_Write_CTE
{
    public partial class Lazy_Write_CTE_Form : Form
    {
        public Lazy_Write_CTE_Form()
        {
            InitializeComponent();
            Txt_Stu_Id.Focus();
        }
        string rb1="5" , rb2 = "5" , rb3 = "5", rb4 = "5", rb5 = "5", rb6 = "5"
            , rb7 = "5", rb8 = "5", rb9 = "5", rb10 = "5", rbA = "5", rbB = "1";
        private async void Btn_Send_Click(object sender, EventArgs e)
        {
            if (Txt_Stu_Id.Text == "")
            {
                MessageBox.Show("請填寫學號");
                return;
            }
            else
            {
                var uri_login = "http://system8.ntunhs.edu.tw/myNTUNHS_student/Common/UserControls/loginModule.aspx";
                var handler_login = new HttpClientHandler() { UseCookies = true,  UseDefaultCredentials = false };
                var client_login = new HttpClient(handler_login);// { BaseAddress = baseAddress };
                client_login.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:57.0) Gecko/20100101 Firefox/57.0");
                client_login.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                client_login.DefaultRequestHeaders.Add("Keep-Alive", "timeout=600");
                client_login.DefaultRequestHeaders.ExpectContinue = false;
                var postData_login = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("txtid", Txt_Stu_Id.Text),
                    new KeyValuePair<string, string>("txtpwd", Txt_pwd.Text) ,
                    new KeyValuePair<string, string>("select", "student") ,
                };
                HttpContent content = new FormUrlEncodedContent(postData_login);
                var response = await client_login.PostAsync(uri_login, content);
                string result_login =response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                if (result_login.Split(new string[] { "_"} , StringSplitOptions.RemoveEmptyEntries).Length <2)
                {
                    MessageBox.Show("帳號或密碼錯誤");
                    return;
                }
            }
            var tempcolor = this.BackColor;
            this.BackColor = Color.Black;
            this.Text = "進行填寫中";
            List<string> Class_Url = new List<string>();
            string stu_id = Txt_Stu_Id.Text;
            string Url = "http://System1.ntunhs.edu.tw/intranetasp/evaMain/stLogin.asp?txtSTNO=" + stu_id;
            var cookieJar = new CookieContainer();
            var handler = new HttpClientHandler() { UseCookies = true , CookieContainer = cookieJar , UseDefaultCredentials =false};
            var client = new HttpClient(handler);// { BaseAddress = baseAddress };
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:57.0) Gecko/20100101 Firefox/57.0");
            client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            client.DefaultRequestHeaders.Add("Keep-Alive", "timeout=600");
            client.DefaultRequestHeaders.ExpectContinue = false;
            var resultlogin = await client.GetAsync(Url);
            client.DefaultRequestHeaders.Add("Referer", "http://system1.ntunhs.edu.tw/intranetasp/evaMain/stEval.asp");
            Url = "http://system1.ntunhs.edu.tw/intranetasp/evaMain/stDeal.asp";
            var resulttable =  await client.GetStringAsync(Url);
           
            HtmlDocument Web_Doc = new HtmlDocument();
            Web_Doc.LoadHtml(resulttable);
            foreach (HtmlNode link in Web_Doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                // Get the value of the HREF attribute
                string hrefValue = link.GetAttributeValue("href", string.Empty);
                Class_Url.Add(hrefValue);
            }
            
            for (int i  = 0; i < Class_Url.Count; i++)
            {
                Url = "http://system1.ntunhs.edu.tw/intranetasp/evaMain/stEditCdo.asp";
                string Now_Class_Url = "http://system1.ntunhs.edu.tw/intranetasp/evaMain/" + Class_Url[i];
                client.DefaultRequestHeaders.Referrer = new Uri("http://system1.ntunhs.edu.tw/intranetasp/evaMain/stDeal.asp");
                await client.GetAsync(Now_Class_Url);
                client.DefaultRequestHeaders.Referrer=new Uri(Now_Class_Url);
                var postData = new List<KeyValuePair<string, string>>
                { 
                    new KeyValuePair<string, string>("rb1", rb1),
                    new KeyValuePair<string, string>("rb2", rb2),
                    new KeyValuePair<string, string>("rb3", rb3),
                    new KeyValuePair<string, string>("rb4", rb4),
                    new KeyValuePair<string, string>("rb5", rb5),
                    new KeyValuePair<string, string>("rb6", rb6),
                    new KeyValuePair<string, string>("rb7", rb7),
                    new KeyValuePair<string, string>("rb8", rb8),
                    new KeyValuePair<string, string>("rb9", rb9),
                    new KeyValuePair<string, string>("rb10", rb10),
                    new KeyValuePair<string, string>("rbA", rbA),
                    new KeyValuePair<string, string>("rbB", rbB),
                    new KeyValuePair<string, string>("OPN_REM", "")
                };
                HttpContent content = new FormUrlEncodedContent(postData);
                var response = await client.PostAsync(Url, content);
            }
            MessageBox.Show("已填寫完畢");
            this.BackColor = tempcolor;
            this.Text = "已填寫完畢";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            int index = 5-rb.Parent.Controls.IndexOf(rb);
            if (rb.Checked)
            {
                rb1 = index.ToString();
            }
        }
        private void Group2RB_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            int index = 5-rb.Parent.Controls.IndexOf(rb);
            if (rb.Checked)
            {
                rb2 = index.ToString();
            }
        }
        private void Group3RB_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            int index =5- rb.Parent.Controls.IndexOf(rb);
            if (rb.Checked)
            {
                rb3 = index.ToString();
            }
        }
        private void Group4RB_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            int index = 5-rb.Parent.Controls.IndexOf(rb);
            if (rb.Checked)
            {
                rb4 = index.ToString();
            }
        }
        private void Group5RB_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            int index = 5-rb.Parent.Controls.IndexOf(rb);
            if (rb.Checked)
            {
                rb5 = index.ToString();
            }
        }
        private void Group6RB_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            int index = 5 - rb.Parent.Controls.IndexOf(rb);
            if (rb.Checked)
            {
                rb6 = index.ToString();
            }
        }
        private void Group7RB_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            int index = 5 - rb.Parent.Controls.IndexOf(rb);
            if (rb.Checked)
            {
                rb7 = index.ToString();
            }
        }
        private void Group8RB_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            int index = 5 - rb.Parent.Controls.IndexOf(rb);
            if (rb.Checked)
            {
                rb8 = index.ToString();
            }
        }
        private void Group9RB_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            int index = 5 - rb.Parent.Controls.IndexOf(rb);
            if (rb.Checked)
            {
                rb9 = index.ToString();
            }
        }
        private void Group10RB_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            int index = 5 - rb.Parent.Controls.IndexOf(rb);
            if (rb.Checked)
            {
                rb10 = index.ToString();
            }
        }
        private void Group11RB_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            int index = 5 - rb.Parent.Controls.IndexOf(rb);
            if (rb.Checked)
            {
                rbA = index.ToString();
            }
        }
        private void Group12RB_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            int index = 5-rb.Parent.Controls.IndexOf(rb);
            if (rb.Checked)
            {
                rbB = index.ToString();
            }
        }

    }
}
