using Newtonsoft.Json;
using RestSharp;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tesseract;

namespace VideoFromImage
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        public class Root
        {
            public string image { get; set; }
            public string value { get; set; }
        }

        public class RegisterClass
        {
            public string account { get; set; }
            public string password { get; set; }
            public string confirm_Password { get; set; }
            public object moneyPassword { get; set; }
            public string name { get; set; }
            public string countryCode { get; set; }
            public object mobile { get; set; }
            public object email { get; set; }
            public object sex { get; set; }
            public string birthday { get; set; }
            public object idNumber { get; set; }
            public object qqAccount { get; set; }
            public object groupBank { get; set; }
            public object bankName { get; set; }
            public object bankProvince { get; set; }
            public object bankCity { get; set; }
            public object bankAccount { get; set; }
            public string checkCodeEncrypt { get; set; }
            public string checkCode { get; set; }
            public bool isRequiredMoneyPassword { get; set; }
            public object dealerAccount { get; set; }
            public object parentAccount { get; set; }
            public object adInfo { get; set; }
        }

        public string RandomName()
        {
            string[] lines = File.ReadAllLines("data/name.txt");
            Random rand = new Random();
            return lines[rand.Next(lines.Length)];
        }

        private static readonly string[] VietnameseSigns = new string[]

    {

        "aAeEoOuUiIdDyY",

        "áàạảãâấầậẩẫăắằặẳẵ",

        "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",

        "éèẹẻẽêếềệểễ",

        "ÉÈẸẺẼÊẾỀỆỂỄ",

        "óòọỏõôốồộổỗơớờợởỡ",

        "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",

        "úùụủũưứừựửữ",

        "ÚÙỤỦŨƯỨỪỰỬỮ",

        "íìịỉĩ",

        "ÍÌỊỈĨ",

        "đ",

        "Đ",

        "ýỳỵỷỹ",

        "ÝỲỴỶỸ"

    };

        public static string ConvertName(string str)

        {

            for (int i = 1; i < VietnameseSigns.Length; i++)

            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)

                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }

            return str.ToLower();

        }

        static string RemoveSpace(string input)
        {
            return input.Replace(" ", "");
        }

        public string GenUsername()
        {
            Random random = new Random();
            int ranNum = random.Next(100, 1000);
            string username = string.Empty;
            username = RemoveSpace(ConvertName(RandomName()));
            username += ranNum.ToString();

            if (username.Length > 15)
            {
                username = username.Substring(0, Math.Min(username.Length, 15));
                return username;
            }
            else
            {
                return username;
            }

        }

        public void SaveInfo(string user)
        {
            string dt = user + "|" + "thanh999999999";
            File.AppendAllText("data/acc.txt", dt + Environment.NewLine); ;
        }

        public string SerialJson(string code, string code_encrypt)
        {
            RegisterClass r = new RegisterClass();
            r.account = GenUsername();
            r.password = "thanh999999999";
            r.confirm_Password = "thanh999999999";
            r.moneyPassword = null;
            r.name = RandomName();
            r.countryCode = "84";
            r.mobile = null;
            r.email = null;
            r.sex = null;
            r.birthday = "2000/06/13";
            r.idNumber = null;
            r.qqAccount = null;
            r.groupBank = null;
            r.bankName = null;
            r.bankProvince = null;
            r.bankCity = null;
            r.bankAccount = null;
            r.checkCodeEncrypt = code_encrypt;
            r.checkCode = code;
            r.isRequiredMoneyPassword = false;
            r.dealerAccount = null;
            r.parentAccount = null;
            r.adInfo = null;
            string text = JsonConvert.SerializeObject(r);

            return text;
        }

        public void SaveImage(string base64)
        {
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64)))
            {
                using (Bitmap bm2 = new Bitmap(ms))
                {
                    bm2.Save("code.jpg");
                }
            }
        }

        public string RegconizeCode()
        {
            string imagePath = @"code.jpg";
            string path = @"D:\vs2019\VideoFromImage\bin\Debug\tessdata-4.1.0";

            using (var engine = new TesseractEngine(path, "eng", EngineMode.Default))
            {

                engine.SetVariable("tessedit_pageseg_mode", "PSM.Auto");

                using (var img = Pix.LoadFromFile(imagePath))
                {
                    using (var page = engine.Process(img))
                    {
                        string extractedText = page.GetText();
                        string result = Regex.Replace(extractedText, @"\t|\n|\r", "");

                        return result;
                    }
                }
            }
        }

        public async Task<string> GetCode()
        {
            var options = new RestClientOptions()
            {
                MaxTimeout = -1,
            };
            var client = new RestClient(options);
            var request = new RestRequest("https://88hello88.com/api/0.0/Home/GetCaptchaForRegister", Method.Post);
            request.AddHeader("Accept", "application/json, text/plain, */*");
            request.AddHeader("Accept-Language", "en-US,en;q=0.9,vi;q=0.8,ar;q=0.7,de;q=0.6");
            request.AddHeader("Cookie", "AWSALB=PWg44b6FCJ05Xk4FVG9QmvQJsFt2EbvQp7WftZ5lykT/wISdlGJGfbLt/z13ZvFTKqVDNq8KBTRJe/SKWTZYUzQseg9PKTLTMVg83PZNJ3yHmFikD4w6HaDXdxvc; AWSALBCORS=PWg44b6FCJ05Xk4FVG9QmvQJsFt2EbvQp7WftZ5lykT/wISdlGJGfbLt/z13ZvFTKqVDNq8KBTRJe/SKWTZYUzQseg9PKTLTMVg83PZNJ3yHmFikD4w6HaDXdxvc; nohostname_ip=4935F7D1AG1268974B95BC");
            RestResponse response = await client.ExecuteAsync(request);
            Root parsed_json = JsonConvert.DeserializeObject<Root>(response.Content);
            var codeEncrypt = parsed_json.value;
            SaveImage(parsed_json.image);

            RegconizeCode();


            textBox1.Text = RegconizeCode();
            textBox2.Text = codeEncrypt;
            return codeEncrypt;


        }

        async void Register()
        {
            string code_encrypt = await GetCode();
            string code = RegconizeCode().Trim();

            if (code != "" && code_encrypt != "")
            {

                var options = new RestClientOptions()
                {
                    MaxTimeout = -1,
                    //Proxy = new WebProxy("103.6.223.2:3128")
                };
                var client = new RestClient(options);
                var request = new RestRequest("https://88hello88.com/api/1.0/member/register", Method.Post);
                request.AddHeader("Accept", "application/json, text/plain, */*");
                request.AddHeader("Accept-Language", "en-US,en;q=0.9,vi;q=0.8,ar;q=0.7,de;q=0.6");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Cookie", "AWSALB=XqLBEvBdQcQ8+peqZxG0S2HMznrPchuBCsT04oDWKApjK8h/INqLG3AgQe3XPBTygYXVb2J3GRqwiQGUl865OuRsppl1nFxhYtCZzIPELYaZfh81ktpauQvfKyEV; AWSALBCORS=XqLBEvBdQcQ8+peqZxG0S2HMznrPchuBCsT04oDWKApjK8h/INqLG3AgQe3XPBTygYXVb2J3GRqwiQGUl865OuRsppl1nFxhYtCZzIPELYaZfh81ktpauQvfKyEV; nohostname_ip=4935F7D1AG126897594F8C");
                var body = SerialJson(code, code_encrypt);
                request.AddStringBody(body, DataFormat.Json);
                RestResponse response = await client.ExecuteAsync(request);
                RegClass.Root parsed_json = JsonConvert.DeserializeObject<RegClass.Root>(response.Content);
                RegisterClass json = JsonConvert.DeserializeObject<RegisterClass>(body);
                string user = json.account;

                if (parsed_json.Code == 200)
                {
                    SaveInfo(user);
                }
                else
                {
                    MessageBox.Show("Lỗi! Vui lòng thử lại.");
                    MessageBox.Show(response.Content);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for(int i =0;i< numThread.Value;i++)
            {
                Register();
            }

        }
    }

}
