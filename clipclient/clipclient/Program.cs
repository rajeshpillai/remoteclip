using System;
using System.Windows.Forms;
using System.Net.Http;
using Keystroke.API;
using System.Text;
using Newtonsoft.Json;

namespace clipclient
{
    // REF: https://github.com/fabriciorissetto/KeystrokeAPI/blob/master/Keystroke.ConsoleAppTest/Program.cs
    class Program
    {
        /*
         * Current thread must be set to single thread apartment (STA) mode before OLE calls can be made. 
         * Ensure that your Main function has STAThreadAttribute marked on it.
         */
        [STAThread]
        static void Main(string[] args)
        {
            string ClipboardContent = "";
            string API_URL = "http://localhost:8888/";

            HttpClient client = new HttpClient();

            using (var api = new KeystrokeAPI())
            {
                api.CreateKeyboardHook(async (character) => {
                    Console.Write(character);

                    if (Clipboard.ContainsText())
                    {
                        if (ClipboardContent != Clipboard.GetText())
                        {
                            ClipboardContent = Clipboard.GetText();
                            Console.WriteLine(ClipboardContent);

                            object data = new
                            {
                                content = ClipboardContent 
                            };


                            var content = new StringContent(JsonConvert.SerializeObject(data), UnicodeEncoding.UTF8, "application/json");
                            var response = await client.PostAsync(API_URL + "uploadclip", content);
                            var result = await response.Content.ReadAsStringAsync();

                            Console.WriteLine(result);

                        }
                    }

                });
                //This call starts the windows message loop for you, but you will need to reference the 
                //System.Windows.Forms.dll in your project.
                Application.Run();
            }
        }
    }
}
