using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Windows.Forms;
using Microsoft.Win32;



namespace System_Info
{
  

    class Program
    {
        static string hostname = Environment.MachineName;
        static string today = string.Format("text-{0:yyyy-MM-dd-hh-mm-ss-tt}", DateTime.Now);
        string filepath = "C:\\Tools\\System_Information_" + hostname + "_" + today+".txt";
        string defstatus;
        string rtstatus;
        string username;

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

        public void addTextToFile(string content)
        {
            File.AppendAllText(filepath, content+"\r\n");
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
                addTextToFile("========== Auto Start Programs ==========");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Console.WriteLine(queryObj["Name"]);
                    addTextToFile(queryObj["Name"].ToString());
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
                addTextToFile("========== Drive Information ==========");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if(Convert.ToInt64(queryObj["Size"]) > 1073741823)
                    {
                        addTextToFile(("Drive Letter: " + queryObj["DeviceID"]));
                        Console.WriteLine("Drive Letter: " + queryObj["DeviceID"]);
                        string freespace = FormatBytes( Convert.ToInt64(queryObj["FreeSpace"]) );

                        addTextToFile(("FreeSpace: " + String.Format("{0:N0}", freespace.ToString())));
                        Console.WriteLine("FreeSpace: "+String.Format("{0:N0}",freespace.ToString()));
                        string totalsize = FormatBytes(Convert.ToInt64(queryObj["Size"]));

                        addTextToFile(("Total Drive Capacity: " + String.Format("{0:N0}", totalsize.ToString())));
                        Console.WriteLine("Total Drive Capacity: " + String.Format("{0:N0}", totalsize.ToString()));
                        double intfree = Convert.ToInt64(queryObj["FreeSpace"]) / 1024.0;
                        double intsize = Convert.ToInt64(queryObj["Size"]) / 1024.0;
                        double percentfree = (double)(intfree / intsize * 100);
                        addTextToFile(("Percent Free: " + Math.Round(percentfree, 2) + "%"));
                        Console.WriteLine("Percent Free: " + Math.Round(percentfree,2)  + "%");
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
                addTextToFile("========== Network Information ==========");
                Console.WriteLine("========== Network Information ==========");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if (queryObj["IPAddress"] != null)
                    {
                        string[] addresses = (string[])queryObj["IPAddress"];
                        string[] gateways = (string[])queryObj["DefaultIPGateway"];
                        addTextToFile(("Description: " + queryObj["Description"]));
                        Console.WriteLine("Description: " + queryObj["Description"]);
                        addTextToFile(("DHCP Enabled: " + queryObj["DHCPEnabled"]));
                        Console.WriteLine("DHCP Enabled: " + queryObj["DHCPEnabled"]);
                        addTextToFile(("IP Address: " + addresses.ElementAt(0)));
                        Console.WriteLine("IP Address: " + addresses.ElementAt(0));
                        addTextToFile(("Default Gateway: " + gateways.ElementAt(0)));
                        Console.WriteLine("Default Gateway: " + gateways.ElementAt(0));
                    }
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }
       
        public void GetAV()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\SecurityCenter2",
                    "SELECT * from AntiVirusProduct");
                addTextToFile(("========== Anti Virus Products and Status=========="));
                Console.WriteLine("========== Anti Virus Products and Status==========");
                
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    addTextToFile((queryObj["DisplayName"]).ToString());
                    Console.WriteLine(queryObj["DisplayName"]);
                    addTextToFile(("----------------------------------"));
                    Console.WriteLine("----------------------------------");
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
                    addTextToFile(("Definition Status: " + defstatus));
                    Console.WriteLine("Definition Status: " + defstatus);
                    addTextToFile("Real-time Protection Status: " + rtstatus + "\n");
                    Console.WriteLine("Real-time Protection Status: " + rtstatus +"\n");
                }
                
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
        }

        public void GetSystemInfo()
        {
            addTextToFile(("========== System Information =========="));
            Console.WriteLine("========== System Information ==========");
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * from Win32_SystemEnclosure");
                
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    addTextToFile(("Serial Number: " + queryObj["SerialNumber"]));
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
                    addTextToFile(("BIOS Manufacturer: " + queryObj["Manufacturer"]));
                    Console.WriteLine("BIOS Manufacturer: " + queryObj["Manufacturer"]);
                    addTextToFile(("BIOS Version: " + bioses.ElementAt(0)));
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
                    addTextToFile(("Model: " + queryObj["Version"]));
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
                    addTextToFile(("System Type: " + queryObj["SystemType"]));
                    Console.WriteLine("System Type: " + queryObj["SystemType"]);
                    addTextToFile(("Number of Physical Processors: " + queryObj["NumberOfProcessors"]));
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
                    addTextToFile(("Operating System: " + queryObj["Caption"]));
                    Console.WriteLine("Operating System: " + queryObj["Caption"]);
                    addTextToFile(("Operating System Build Number " + queryObj["BuildNumber"]));
                    Console.WriteLine("Operating System Build Number " + queryObj["BuildNumber"]);
                    addTextToFile(("Last Boot Time: " + txtDate + " " + txtTime));
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
                        addTextToFile(("Current logged on user: " + OwnerInfo[1] + "\\" + OwnerInfo[0]));
                        Console.WriteLine("Current logged on user: " + OwnerInfo[1] +"\\" + OwnerInfo[0]);
                    }
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
            addTextToFile("========== Memory Information ==========");
            Console.WriteLine("========== Memory Information ==========");
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * from Win32_PhysicalMemory");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    addTextToFile(("Location: " + queryObj["BankLabel"]));
                    Console.WriteLine("Location: " + queryObj["BankLabel"]);
                    string size = FormatBytes(Convert.ToInt64(queryObj["Capacity"]));
                    addTextToFile(("Memory Size: " + String.Format("{0:N0}", size.ToString())));
                    Console.WriteLine("Memory Size: " + String.Format("{0:N0}", size.ToString()));
                    addTextToFile(("Memory Clock Speed: " + queryObj["Speed"]));
                    Console.WriteLine("Memory Clock Speed: " + queryObj["Speed"]);
                    addTextToFile(("Memory Serial Number: " + queryObj["SerialNumber"]));
                    Console.WriteLine("Memory Serial Number: " + queryObj["SerialNumber"]);
                    addTextToFile(("Memory Part Number: " + queryObj["PartNumber"]));
                    Console.WriteLine("Memory Part Number: " + queryObj["PartNumber"]);
                    addTextToFile(("Manufacturer: " + queryObj["Manufacturer"]));
                    Console.WriteLine("Manufacturer: " + queryObj["Manufacturer"]);
                }
            }
            catch (ManagementException e)
            {
                MessageBox.Show("An error occurred while querying for WMI data: " + e.Message);
            }
            addTextToFile("========== Battery Information ==========");
            Console.WriteLine("========== Battery Information ==========");
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * from Win32_Battery");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                   
                    int charge = Convert.ToInt32(queryObj["EstimatedChargeRemaining"]);
                    addTextToFile(("Estimated Charge Remaining: " + charge));
                    Console.WriteLine("Estimated Charge Remaining: " + charge);
                    addTextToFile(("Battery Status: " + queryObj["Status"]));
                    Console.WriteLine("Battery Status: " + queryObj["Status"]);
                    addTextToFile(("Caption: " + queryObj["Caption"]));
                    Console.WriteLine("Caption: " + queryObj["Caption"]);
                    int runtime = Convert.ToInt32(queryObj["EstimatedRunTime"]);
                    runtime = runtime / 60;
                    addTextToFile(("Estimated Run Time: " + runtime));
                    Console.WriteLine("Estimated Run Time: " + runtime);
                    string userRoot = "HKEY_LOCAL_MACHINE";
                    string subkey = "SYSTEM\\CurrentControlSet\\Control\\IDConfigDB\\CurrentDockInfo";
                    string keyName = userRoot + "\\" + subkey;
                    object dock = Registry.GetValue(keyName, "DockingState", null);
                    int dockstate = Convert.ToInt32(dock);
                    if(runtime > 10000 && dockstate == 1)
                    {
                        addTextToFile("Estimated Run Time: Connected to AC adapter and docked");
                        Console.WriteLine("Estimated Run Time: Connected to AC adapter and docked");
                    }
                    else if (runtime > 10000 && dockstate != 1)
                    {
                        addTextToFile("Estimated Run Time: Connected to AC adapter and not docked");
                        Console.WriteLine("Estimated Run Time: Connected to AC adapter and not docked");
                    }
                    else
                    {
                        addTextToFile("Estimated Run Time: " + runtime + "hrs");
                        Console.WriteLine("Estimated Run Time: " + runtime + "hrs");
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
            //string hostname = Environment.MachineName;
            //string today = string.Format("text-{0:yyyy-MM-dd-hh-mm-ss-tt}", DateTime.Now);
            //string filepath = "C:\\Tools\\System_Information_" + hostname + "_" + today+".txt";
            //if (!File.Exists(filepath))
            //{
            //File.Create(filepath);
            //}
            //else
            //{
            //File.Delete(filepath);
            //}
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
