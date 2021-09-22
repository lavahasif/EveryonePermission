using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace EveryonePermission
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            // SetDraw();
            var appExePath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            Console.Write("path" + appExePath);
            SetControl(appExePath);
            SetDraw();
            Console.WriteLine("Choose by press anything select the folder if you are using 'open Dialog'");
            Console.WriteLine("2 Console");
            Console.WriteLine("Any open Dialog");
            var readLine = Console.ReadLine();

            if (readLine.Equals("2"))
            {
                SetPermissionConsole();
            }
            else
            {
                OpenDialog();
                Console.ReadLine();
            }
        }

        private static void SetPermissionConsole()
        {
            var isout = false;

            do
            {
                Console.WriteLine("Please Enter path");
                var readLine = Console.ReadLine();

                if (readLine.Length < 2)
                {
                    Console.WriteLine("Please Enter proper destination");
                }

                string[] files;
                try
                {
                    files = Directory.GetFiles(readLine);
                    foreach (string s in files)
                    {
                        // Create the FileInfo object only when needed to ensure
                        // the information is as current as possible.
                        System.IO.FileInfo fi = null;
                        try
                        {
                            fi = new System.IO.FileInfo(s);
                            SetFileControl2(fi.FullName);
                        }
                        catch (System.IO.FileNotFoundException e)
                        {
                            // To inform the user and continue is
                            // sufficient for this demonstration.
                            // Your application may require different behavior.
                            Console.WriteLine(e.Message);
                        }

                        Console.WriteLine("{0} : {1}", fi.Name, fi.Directory);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }


                // SetFileControl(appExePath);
                // SetFileControl2(@"D:\SheraccHotel\teatime32.mdf");
            } while (isout);
        }

        static void OpenDialog()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                foreach (var path in Directory.GetFiles(fbd.SelectedPath))
                {
                    try
                    {
                        Console.WriteLine(path);
                        SetFileControl2(path);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    Console.WriteLine(path); // full path
                    // Console.WriteLine(System.IO.Path.GetFileName(path)); // file name
                }
            }
        }

        public static void SetDraw()
        {
            Console.WriteLine(
                "\n=========================================================================================================");

            Console.Write(
                "Instruction:\n" +
                "1.copy paste the application which directory file you want set as permission every one \n " +
                "2.run as admin \n  " +
                "3.If you have any difficulty please copy this file other directory \n and Enter the folder path Here\n "
            );

            Console.WriteLine(
                "=========================================================================================================");
        }

        public static void SetControl(string path)
        {
            DirectorySecurity sec = Directory.GetAccessControl(path);

// Using this instead of the "Everyone" string means we work on non-English systems.
            SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            sec.AddAccessRule(new FileSystemAccessRule(everyone,
                FileSystemRights.Modify | FileSystemRights.Synchronize,
                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None,
                AccessControlType.Allow));
            Directory.SetAccessControl(path, sec);
        }

        public static void SetFileControl(string path)
        {
            var readOnlyCollection = Microsoft.VisualBasic.FileIO.FileSystem.GetFiles(path,
                Microsoft.VisualBasic.FileIO.SearchOption.SearchAllSubDirectories,
                new string[] {"*.mdf", "*.ldf,*.MDF,*.LDF"});
            foreach (var s in readOnlyCollection)
            {
                // Get a FileSecurity object that represents the
// current security settings.
                var fileName = Path.Combine(path, s);
                FileSecurity fSecurity = File.GetAccessControl(fileName);
                SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
// Add the FileSystemAccessRule to the security settings.
                fSecurity.AddAccessRule(new FileSystemAccessRule(everyone,
                    FileSystemRights.Modify | FileSystemRights.Synchronize,
                    InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None,
                    AccessControlType.Allow));

// Set the new access settings.
                File.SetAccessControl(fileName, fSecurity);
            }
        }

        public static void SetFileControl2(string fileName)
        {
            FileSecurity fSecurity = File.GetAccessControl(fileName);

// Add the FileSystemAccessRule to the security settings.
            SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            fSecurity.AddAccessRule(new FileSystemAccessRule(everyone,
                FileSystemRights.Modify | FileSystemRights.Synchronize,
                AccessControlType.Allow));

// Set the new access settings.
            File.SetAccessControl(fileName, fSecurity);
        }
    }

    static class Draw
    {
        public static string DrawInConsoleBox(this string s)
        {
            string ulCorner = "╔";
            string llCorner = "╚";
            string urCorner = "╗";
            string lrCorner = "╝";
            string vertical = "║";
            string horizontal = "═";

            string[] lines = s.Split(new char[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);


            int longest = 0;
            foreach (string line in lines)
            {
                if (line.Length > longest)
                    longest = line.Length;
            }

            int width = longest + 2; // 1 space on each side


            string h = string.Empty;
            for (int i = 0; i < width; i++)
                h += horizontal;

            // box top
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(ulCorner + h + urCorner);

            // box contents
            foreach (string line in lines)
            {
                double dblSpaces = (((double) width - (double) line.Length) / (double) 2);
                int iSpaces = Convert.ToInt32(dblSpaces);

                if (dblSpaces > iSpaces) // not an even amount of chars
                {
                    iSpaces += 1; // round up to next whole number
                }

                string beginSpacing = "";
                string endSpacing = "";
                for (int i = 0; i < iSpaces; i++)
                {
                    beginSpacing += " ";

                    if (!(iSpaces > dblSpaces &&
                          i == iSpaces - 1)) // if there is an extra space somewhere, it should be in the beginning
                    {
                        endSpacing += " ";
                    }
                }

                // add the text line to the box
                sb.AppendLine(vertical + beginSpacing + line + endSpacing + vertical);
            }

            // box bottom
            sb.AppendLine(llCorner + h + lrCorner);

            // the finished box
            return sb.ToString();
        }
    }
}