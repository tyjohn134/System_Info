using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Windows.Forms;



namespace System_Info
{
  

    class Program
    {
        private static string FormatBytes(long bytes)
        {
            string[] Suffix = { "B", "KB", "MB", "GB", "TB" };
            int i;
            double dblSByte = bytes;
            for (i = 0; i < Suffix.Length && bytes >= 1024; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }

            return String.Format("{0:0.##} {1}", dblSByte, Suffix[i]);
        }
        public void GetStartups()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_StartupCommand");
                Console.WriteLine("========== Auto Start Programs ==========");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Console.WriteLine(queryObj["Name"]);
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }

        public void GetDriveInfo()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * from win32_logicaldisk where DriveType = '3'");
                Console.WriteLine("========== Auto Start Programs ==========");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if(Convert.ToInt64(queryObj["Size"]) > 1073741823)
                    {
                        Console.WriteLine("\n");
                        Console.WriteLine("========== Drive Information ==========");
                        Console.WriteLine("Drive Letter: " + queryObj["DeviceID"]);
                        string freespace = FormatBytes( Convert.ToInt64(queryObj["FreeSpace"]) );
                        Console.WriteLine("FreeSpace: "+String.Format("{0:N0}",freespace.ToString()));
                        string totalsize = FormatBytes(Convert.ToInt64(queryObj["Size"]));
                        Console.WriteLine("Total Drive Capacity: " + String.Format("{0:N0}", totalsize.ToString()));
                        double intfree = Convert.ToInt64(queryObj["FreeSpace"]) / 1024.0;
                        double intsize = Convert.ToInt64(queryObj["Size"]) / 1024.0;
                        int percentfree = (int)(intfree / intsize * 100);
                        Console.WriteLine("Percent Free: " + String.Format("{0:N0}", percentfree.ToString() )+ "%");
                    }
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }

        static void Main(string[] args)
        {
            Program n = new Program();
            n.GetStartups();
            n.GetDriveInfo();
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
 
}
