﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;

namespace Duplicateur
{
    class Usb {
        private const int INVALID_HANDLE_VALUE = -1;
        private const int GENERIC_READ = unchecked((int)0x80000000);
        private const int GENERIC_WRITE = unchecked((int)0x40000000);
        private const int FILE_SHARE_READ = unchecked((int)0x00000001);
        private const int FILE_SHARE_WRITE = unchecked((int)0x00000002);
        private const int OPEN_EXISTING = unchecked((int)3);
        private const int FSCTL_LOCK_VOLUME = unchecked((int)0x00090018);
        private const int FSCTL_DISMOUNT_VOLUME = unchecked((int)0x00090020);
        private const int IOCTL_STORAGE_EJECT_MEDIA = unchecked((int)0x002D4808);
        private const int IOCTL_STORAGE_MEDIA_REMOVAL = unchecked((int)0x002D4804);

        [DllImport("kernel32")]
        private static extern int CloseHandle(IntPtr handle);

        [DllImport("kernel32")]
        private static extern int DeviceIoControl
            (IntPtr deviceHandle, uint ioControlCode,
              IntPtr inBuffer, int inBufferSize,
              IntPtr outBuffer, int outBufferSize,
              ref int bytesReturned, IntPtr overlapped);

        [DllImport("kernel32")]
        private static extern IntPtr CreateFile(
            string lpFileName,
            int dwDesiredAccess,
            int dwShareMode,
            IntPtr lpSecurityAttributes,
            int dwCreationDisposition,
            int dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        public char driveLetter;

        public bool isSelected = false;
        public bool copyToRoot = true;
        private string destinationPath = "";
        public bool createFolder = true;
        private string folderToCreate = "Duplication";
        public bool sendNotification = false;
        private string notifMailAddress = "";

        public string getFolderToCreate() { return this.folderToCreate; }
        public string getDestinationPath() { return this.destinationPath; }
        public string getNotifMailAddress() { return this.notifMailAddress; }

        public void setFolderToCreate(String name)
        {
            this.folderToCreate = name;
        }

        public void setDestinationPath(String path)
        {
            this.destinationPath = path;
        }

        public void setNotifMailAddress(string mailAddress)
        {
            this.notifMailAddress = mailAddress;
        }

        public String getTotalSizeStr() {

            //Objet d'informations sur le périphérique amovible
            DriveInfo di = new DriveInfo(this.driveLetter + ":");

            //On récupere la taille totale en octets
            long tSize = di.TotalSize;

            //Taille totale de la clé en MO
            Decimal dSize = tSize / (1024 * 1024);
            dSize = Math.Round(dSize);

            return dSize + " Mo";
        }

        public long getTotalSize()
        {
            //Objet d'informations sur le périphérique amovible
            DriveInfo di = new DriveInfo(this.driveLetter + ":");

            return di.TotalSize;
        }

        public long getFreeSpace()
        {
            //Objet d'informations sur le périphérique amovible
            DriveInfo di = new DriveInfo(this.driveLetter + ":");

            return di.TotalFreeSpace;
        }

        public String getFreeSpaceStr()
        {
            //Objet d'informations sur le périphérique amovible
            DriveInfo di = new DriveInfo(this.driveLetter + ":");

            //On récupere la taille dispo en octets
            long fSize = di.TotalFreeSpace;

            //Taille dispo de la clé en MO
            Decimal dSize = fSize / (1024 * 1024);
            dSize = Math.Round(dSize);

            return dSize + " Mo";
        }

        public String getFormat()
        {
            //Objet d'informations sur le périphérique amovible
            DriveInfo di = new DriveInfo(this.driveLetter + ":");

            //On récupere le type du format (NTFS, exFAT, ...)
            return di.DriveFormat;
        }

        public Usb(char driveLetter)
        {
            this.driveLetter = driveLetter;
        }

        public void EjectDrive()
        {
            string path = "\\\\.\\" + this.driveLetter + ":";

            IntPtr handle = CreateFile(path, GENERIC_READ, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
            Console.WriteLine(handle);
            if ((long)handle == -1)
            {
                MessageBox.Show("Unable to open drive " + driveLetter);
                return;
            }

            int dummy = 0;

            DeviceIoControl(handle, IOCTL_STORAGE_EJECT_MEDIA, IntPtr.Zero, 0,
                IntPtr.Zero, 0, ref dummy, IntPtr.Zero);

            CloseHandle(handle);

            MessageBox.Show("OK to remove drive.");
        }
        #region SetLabel

        /// <summary>
        /// set a drive label to the desired value
        /// </summary>
        /// <param name="driveLetter">drive letter. Example : 'A', 'B', 'C', 'D', ..., 'Z'.</param>
        /// <param name="label">label for the drive</param>
        /// <returns>true if success, false if failure</returns>
        public static bool SetLabel(char driveLetter, string label = "")
        {
            #region args check

            if (!Char.IsLetter(driveLetter))
            {
                return false;
            }
            if (label == null)
            {
                label = "";
            }

            #endregion
            try
            {
                DriveInfo di = DriveInfo.GetDrives()
                                        .Where(d => d.Name.StartsWith(driveLetter.ToString()))
                                        .FirstOrDefault();
                di.VolumeLabel = label;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region FormatDrive

        /// <summary>
        /// Format a drive using the best available method
        /// </summary>
        /// <param name="driveLetter">drive letter. Example : 'A', 'B', 'C', 'D', ..., 'Z'.</param>
        /// <param name="label">label for the drive</param>
        /// <param name="fileSystem">file system. Possible values : "FAT", "FAT32", "EXFAT", "NTFS", "UDF".</param>
        /// <param name="quickFormat">quick formatting?</param>
        /// <param name="enableCompression">enable drive compression?</param>
        /// <param name="clusterSize">cluster size (default=null for auto). Possible value depends on the file system : 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, ...</param>
        /// <returns>true if success, false if failure</returns>
        public bool FormatDrive(string label = "", string fileSystem = "NTFS", bool quickFormat = true, bool enableCompression = false, int? clusterSize = null)
        {
            return FormatDrive_CommandLine(this.driveLetter, label, fileSystem, quickFormat, enableCompression, clusterSize);
        }

        #endregion

        #region FormatDrive_CommandLine

        /// <summary>
        /// Format a drive using Format.com windows file
        /// </summary>
        /// <param name="driveLetter">drive letter. Example : 'A', 'B', 'C', 'D', ..., 'Z'.</param>
        /// <param name="label">label for the drive</param>
        /// <param name="fileSystem">file system. Possible values : "FAT", "FAT32", "EXFAT", "NTFS", "UDF".</param>
        /// <param name="quickFormat">quick formatting?</param>
        /// <param name="enableCompression">enable drive compression?</param>
        /// <param name="clusterSize">cluster size (default=null for auto). Possible value depends on the file system : 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, ...</param>
        /// <returns>true if success, false if failure</returns>
        public static bool FormatDrive_CommandLine(char driveLetter, string label = "", string fileSystem = "NTFS", bool quickFormat = true, bool enableCompression = false, int? clusterSize = null)
        {
            #region args check

            if (!Char.IsLetter(driveLetter) ||
                !IsFileSystemValid(fileSystem))
            {
                return false;
            }

            #endregion
            bool success = false;
            string drive = driveLetter + ":";
            try
            {
                var di                     = new DriveInfo(drive);
                var psi                    = new ProcessStartInfo();
                psi.FileName               = "format.com";
                psi.WorkingDirectory       = Environment.SystemDirectory;
                psi.Arguments              = "/FS:" + fileSystem +
                                             " /Y" +
                                             " /V:" + label +
                                             (quickFormat ? " /Q" : "") +
                                             ((fileSystem == "NTFS" && enableCompression) ? " /C" : "") +
                                             (clusterSize.HasValue ? " /A:" + clusterSize.Value : "") +
                                             " " + drive;
                psi.UseShellExecute        = false;
                psi.CreateNoWindow         = true;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardInput  = true;
                var formatProcess          = Process.Start(psi);
                var swStandardInput        = formatProcess.StandardInput;
                swStandardInput.WriteLine();
                formatProcess.WaitForExit();
                success = true;
            }
            catch (Exception) { }
            return success;
        }

        #endregion

        #region FormatDrive_Shell32

        #region interop

        // http://msdn.microsoft.com/en-us/library/windows/desktop/bb762169(v=vs.85).aspx
        [DllImport("shell32.dll")]
        private static extern uint SHFormatDrive(IntPtr hwnd, uint drive, SHFormatFlags fmtID, SHFormatOptions options);

        private enum SHFormatFlags : uint
        {
            SHFMT_ID_DEFAULT = 0xFFFF,
            /// <summary>
            /// A general error occured while formatting. This is not an indication that the drive cannot be formatted though.
            /// </summary>
            SHFMT_ERROR      = 0xFFFFFFFF,
            /// <summary>
            /// The drive format was cancelled by user/OS.
            /// </summary>
            SHFMT_CANCEL     = 0xFFFFFFFE,
            /// <summary>
            /// A serious error occured while formatting. The drive is unable to be formatted by the OS.
            /// </summary>
            SHFMT_NOFORMAT   = 0xFFFFFFD
        }

        [Flags]
        private enum SHFormatOptions : uint
        {
            /// <summary>
            /// Full formatting
            /// </summary>
            SHFMT_OPT_COMPLETE = 0x0,
            /// <summary>
            /// Quick Format
            /// </summary>
            SHFMT_OPT_FULL     = 0x1,
            /// <summary>
            /// MS-DOS System Boot Disk
            /// </summary>
            SHFMT_OPT_SYSONLY  = 0x2
        }

        #endregion

        /// <summary>
        /// Format a drive using Shell32.dll
        /// </summary>
        /// <param name="driveLetter">drive letter. Example : 'A', 'B', 'C', 'D', ..., 'Z'.</param>
        /// <param name="label">label for the drive</param>
        /// <param name="quickFormat">quick formatting?</param>
        /// <returns>true if success, false if failure</returns>
        [Obsolete("Unsupported by Microsoft nowadays. Prefer the FormatDrive() or FormatDrive_CommandLine() methods")]
        public static bool FormatDrive_Shell32(char driveLetter, string label = "", bool quickFormat = true)
        {
            #region args check

            if (!Char.IsLetter(driveLetter))
            {
                return false;
            }

            #endregion
            bool success = false;
            string drive = driveLetter + ":";
            try
            {
                var di           = new DriveInfo(drive);
                var bytes        = Encoding.ASCII.GetBytes(di.Name.ToCharArray());
                uint driveNumber = Convert.ToUInt32(bytes[0] - Encoding.ASCII.GetBytes(new[] { 'A' })[0]);
                var options      = SHFormatOptions.SHFMT_OPT_COMPLETE;
                if (quickFormat)
                    options = SHFormatOptions.SHFMT_OPT_FULL;

                uint returnCode = SHFormatDrive(IntPtr.Zero, driveNumber, SHFormatFlags.SHFMT_ID_DEFAULT, options);
                if (returnCode == (uint)SHFormatFlags.SHFMT_ERROR)
                    throw new Exception("An error occurred during the format. This does not indicate that the drive is unformattable.");
                else if (returnCode == (uint)SHFormatFlags.SHFMT_CANCEL)
                    throw new OperationCanceledException("The format was canceled.");
                else if (returnCode == (uint)SHFormatFlags.SHFMT_NOFORMAT)
                    throw new IOException("The drive cannot be formatted.");

                SetLabel(driveLetter, label);
                success = true;
            }
            catch (Exception) { }
            return success;
        }

        #endregion

        #region FormatDrive_Win32Api

        // http://msdn.microsoft.com/en-us/library/aa394515(VS.85).aspx
        
        /// <summary>
        /// Format a drive using Win32 API
        /// </summary>
        /// <param name="driveLetter">drive letter. Example : 'A', 'B', 'C', 'D', ..., 'Z'.</param>
        /// <param name="label">label for the drive</param>
        /// <param name="fileSystem">file system. Possible values : "FAT", "FAT32", "EXFAT", "NTFS", "UDF".</param>
        /// <param name="quickFormat">quick formatting?</param>
        /// <param name="enableCompression">enable drive compression?</param>
        /// <param name="clusterSize">cluster size. Possible value depends on the file system : 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, ...</param>
        /// <returns>true if success, false if failure</returns>
        [Obsolete("Might have troubles formatting ram drives. Prefer the FormatDrive() or FormatDrive_CommandLine() methods")]
        public static bool FormatDrive_Win32Api(char driveLetter, string label = "", string fileSystem = "NTFS", bool quickFormat = true, bool enableCompression = false, int clusterSize = 8192)
        {
            #region args check
            if (!Char.IsLetter(driveLetter) ||
                !IsFileSystemValid(fileSystem))
            {
                return false;
            }

            #endregion
            bool success = false;
            try
            {
                var moSearcher = new ManagementObjectSearcher(@"SELECT * FROM Win32_Volume WHERE DriveLetter='" + driveLetter + ":'");
                foreach (ManagementObject mo in moSearcher.Get())
                {
                    mo.InvokeMethod("Format", new object[] { fileSystem, quickFormat, clusterSize, label, enableCompression });
                    success = true;
                }
            }
            catch (Exception)
            {
                success = false;
            }
            return success;
        }

        #endregion

        #region IsFileSystemValid

        /// <summary>
        /// test if the provided filesystem value is valid
        /// </summary>
        /// <param name="fileSystem">file system. Possible values : "FAT", "FAT32", "EXFAT", "NTFS", "UDF".</param>
        /// <returns>true if valid, false if invalid</returns>
        public static bool IsFileSystemValid(string fileSystem)
        {
            #region args check

            if (fileSystem == null)
            {
                return false;
            }

            #endregion
            switch (fileSystem)
            {
                case "FAT":
                case "FAT32":
                case "EXFAT":
                case "NTFS":
                case "UDF":
                    return true;
                default:
                    return false;
            }
        }
        #endregion
    }
}
