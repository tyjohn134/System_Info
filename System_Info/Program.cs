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
                    if((int)queryObj["Size"] > 1073741823)
                    {
                        Console.WriteLine("\n");
                        Console.WriteLine("========== Drive Information ==========");
                        Console.WriteLine(queryObj["DeviceID"]);
                        int freespace = (int)queryObj["FreeSpace"] / 1024;
                        

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
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
 
}
