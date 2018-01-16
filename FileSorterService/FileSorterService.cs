using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Diagnostics;
using System.ServiceProcess;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using FileSorterService.Extensions;

namespace FileSorterService
{
    public partial class FileSorterService : ServiceBase
    {

        private FileSystemWatcher fileWatcher;
        private const string music = "C:\\Users\\Lukas\\Music\\";
        private static string[] destinationFolders;
        private static string[] extensions;

        public FileSorterService()
        {
            InitializeComponent();
        }

        public void DebugStart() {
            this.OnStart(null);
        }

        [PermissionSet(SecurityAction.Demand, Name="FullTrust")]
        protected override void OnStart(string[] args)
        {
            var destFolders = new List<string>();
            var ext = new List<string>();
            for (int i = 0; i < args.Length; ++i) {
                if (i % 2 == 0) {
                    destFolders.Add(args[i]);
                }
                else
                {
                    ext.Add(args[i]);
                }
            }

            destinationFolders = destFolders.ToArray();
            extensions = ext.ToArray();
            InitializeWatcher(args);

        }

        private void InitializeWatcher(string[] args) {
            if (args == null)
                fileWatcher = new FileSystemWatcher("C:\\Users\\Lukas\\Downloads");
            else
            {
                this.fileWatcher = new FileSystemWatcher(args[0]);
            }

            fileWatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
           | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            this.fileWatcher.Created += new FileSystemEventHandler(OnFileCreated);
            this.fileWatcher.Changed += new FileSystemEventHandler(OnFileChanged);
            this.fileWatcher.EnableRaisingEvents = true;
        }


        private static void OnFileCreated(object o, FileSystemEventArgs args) {
            Console.WriteLine(o.ToString());
            Console.WriteLine(args.Name);
            //System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "file_created_event.txt");
        }

        private static void OnFileChanged(object source, FileSystemEventArgs args) {
            /*Console.WriteLine(args.Name);
            Console.WriteLine(args.ChangeType);*/

            //
            string extension = Path.GetExtension(args.Name);

            if (extension == ".tmp" || extension == ".crdownload") {
                return;
            }

            int index = extensions.IndexOf(extension);

            bool result = Task.Run(() => FileNotUsed(args.FullPath)).Result;
            //if true, file is ready to move
            

            if (result && index != -1) {
                Console.WriteLine(args.FullPath + " file ready to move.");
                File.Move(args.FullPath, destinationFolders[index] + args.Name);
            }
        }

        private static async Task<bool> FileNotUsed(string path) {
            if (!File.Exists(path))
                return false;
            while (FileLocked(path)) {
                await Task.Delay(5000);
            }
            return true;
        }

        private static bool FileLocked(string path) {
            FileStream file = null;
            try
            {
                file = File.Open(path, FileMode.Open);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            catch (IOException) {
                return true;
            }
            finally
            {
                if (file != null)
                    file.Close();
            }
            return false;
        }

        protected override void OnStop()
        {
        }

        
    }
}
