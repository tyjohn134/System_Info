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
        //Helper function to convert bytes to varying sizes
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

        //Gets all programs running upon start up
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

        //Gets drive information -- freespace, percent free, drive letter
        public void GetDriveInfo()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * from win32_logicaldisk where DriveType = '3'");
                Console.WriteLine("========== Drive Information ==========");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if(Convert.ToInt64(queryObj["Size"]) > 1073741823)
                    {
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


        public void GetIP()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * from Win32_NetworkAdapterConfiguration");
                Console.WriteLine("========== Network Information ==========");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if (queryObj["IPAddress"] != null)
                    {
                        string[] addresses = (string[])queryObj["IPAddress"];
                        string[] gateways = (string[])queryObj["DefaultIPGateway"];
                        Console.WriteLine("Description: " + queryObj["Description"]);
                        Console.WriteLine("DHCP Enabled: " + queryObj["DHCPEnabled"]);
                        Console.WriteLine("IP Address: " + addresses.ElementAt(0));
                        Console.WriteLine("Default Gateway: " + gateways.ElementAt(0));
                    }
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }
        string defstatus;
        string rtstatus;
        public void GetAV()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\SecurityCenter2",
                    "SELECT * from AntiVirusProduct");
                Console.WriteLine("========== Anti Virus Products and Status==========");
                
                foreach (ManagementObject queryObj in searcher.Get())
                { 
                    Console.WriteLine(queryObj["DisplayName"]);
                    var productstate = queryObj["productState"];
                    string productstate1 = productstate.ToString();
                    switch (productstate1)
                    {
                        case "262144":
                            defstatus = "Up to date";
                            rtstatus = "Disabled";
                            break;
                        case "262160":
                            defstatus = "Out of date";
                            rtstatus = "Disabled";
                            break;
                        case "266240":
                            defstatus = "Up to date";
                            rtstatus = "Enabled";
                            break;
                        case "266256":
                            defstatus = "Out of date";
                            rtstatus = "Enabled";
                            break;
                        case "393216":
                            defstatus = "Up to date";
                            rtstatus = "Disabled";
                            break;
                        case "393232":
                            defstatus = "Out of date";
                            rtstatus = "Disabled";
                            break;
                        case "393488":
                            defstatus = "Out of date";
                            rtstatus = "Disabled";
                            break;
                        case "397312":
                           defstatus = "Up to date";
                            rtstatus = "Enabled";
                            break;
                        case "397328":
                            defstatus = "Out of date";
                            rtstatus = "Enabled";
                            break;
                        case "397584":
                            defstatus = "Out of date";
                            rtstatus = "Enabled";
                            break;
                        case "397568":
                            defstatus = "Up to date";
                            rtstatus = "Enabled";
                            break;
                        case "393472":
                            defstatus = "Up to date";
                            rtstatus = "Disabled";
                            break;
                        default:
                            defstatus = "Unknown";
                            rtstatus = "Unknown";
                            break;
                    }
                    Console.WriteLine("Definition Status: " + defstatus);
                    Console.WriteLine("Real-time Protection Status: " + rtstatus);
                }
                
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }

        public void GetSystemInfo()
        {
            Console.WriteLine("========== System Information ==========");
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * from Win32_SystemEnclosure");
                
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Console.WriteLine("Serial Number: " + queryObj["SerialNumber"]);
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * from Win32_BIOS");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    string[] bioses = (string[])queryObj["BIOSVersion"];
                    Console.WriteLine("BIOS Manufacturer: " + queryObj["Manufacturer"]);
                    Console.WriteLine("BIOS Version: " + bioses.ElementAt(0));
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * from Win32_ComputerSystemProduct");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Console.WriteLine("Model: " + queryObj["Version"]);
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * from Win32_ComputerSystem");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Console.WriteLine("System Type: " + queryObj["SystemType"]);
                    Console.WriteLine("Number of Physical Processors: " + queryObj["NumberOfProcessors"]);
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * from Win32_OperatingSystem");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    
                    DateTime dtBootTime = ManagementDateTimeConverter.ToDateTime(queryObj["LastBootUpTime"].ToString());

                    // display the start time and date
                    string txtDate = dtBootTime.ToLongDateString();
                    string txtTime = dtBootTime.ToLongTimeString();
                    Console.WriteLine("Operating System: " + queryObj["Caption"]);
                    Console.WriteLine("Operating System Build Number " + queryObj["BuildNumber"]);
                    Console.WriteLine("Last Boot Time: " + txtDate + " " + txtTime);
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
            try
            {
                ManagementObjectSearcher Processes = new ManagementObjectSearcher("SELECT * FROM Win32_Process Where Name='explorer.exe'");

                foreach (ManagementObject Process in Processes.Get())
                {
                    if (Process["ExecutablePath"] != null)
                    {
                        string ExecutablePath = Process["ExecutablePath"].ToString();

                        string[] OwnerInfo = new string[2];
                        Process.InvokeMethod("GetOwner", OwnerInfo);

                        Console.WriteLine("Current logged on user: " + OwnerInfo[1] +"\\" + OwnerInfo[0]);
                    }
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }

            Console.WriteLine("\n========== Memory Information ==========");
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * from Win32_PhysicalMemory");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Console.WriteLine("Location: " + queryObj["BankLabel"]);
                    string size = FormatBytes(Convert.ToInt64(queryObj["Capacity"]));
                    Console.WriteLine("Memory Size: " + String.Format("{0:N0}", size.ToString()));
                    Console.WriteLine("Memory Clock Speed: " + queryObj["Speed"]);
                    Console.WriteLine("Memory Serial Number: " + queryObj["SerialNumber"]);
                    Console.WriteLine("Memory Part Number: " + queryObj["PartNumber"]);
                    Console.WriteLine("Manufacturer: " + queryObj["Manufacturer"]);
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }

         
            //TODO: add Battery Information -- estiamtedchargeremaing, status and runtime
        }


        static void Main(string[] args)
        {
            Program n = new Program();
            n.GetIP();
            n.GetAV();
            n.GetSystemInfo();
            n.GetDriveInfo();
            n.GetStartups();
            
           
           
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
 
}
