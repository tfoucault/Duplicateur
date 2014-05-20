using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;

namespace Duplicateur
{

    public class FileManager
    {

        private List<DriveInfo> usbList = new List<DriveInfo>();
        
        
        public FileManager()
        {
           this.dinfo();
        }


        public List<DriveInfo> getUsbList()
        {
            return usbList;
        }

        public void dinfo()
        {
            try
            {
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                foreach (DriveInfo d in allDrives)
                {
                    if (d.IsReady == true && d.DriveType == DriveType.Removable)
                    {
                        /*string ko = d.VolumeLabel;
                        string dt = System.Convert.ToString(d.DriveType);

                        MessageBox.Show(System.Convert.ToString(var_dump(d, 1)));*/

                        usbList.Add(d);   
                    }
                }
            }
            catch { MessageBox.Show("Error Fetching Drive Info", "Error"); }
        }
    }
}
