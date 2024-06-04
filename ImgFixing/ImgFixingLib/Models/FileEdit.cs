using System.Text;
using System.Security.Cryptography;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text.Json;

namespace ImgFixingLib.Models
{
    class FileEdit
    {
        private string JsonSaveFile = "SaveJson.txt";
        public bool IsErr { get; set; } = false;
        public string ErrText { get; set; } = string.Empty;
        public string TextMessag { get; set; } = string.Empty;
        private static string[] FileFilter {  get; set; }

        public FileEdit()
        {
        }
        public FileEdit(string[] fileFilter ) {
            FileFilter = fileFilter;
        }

        private bool SetErr(string err)
        {
            ErrText = err;
            return false;
        }

        public string AutoLoade()
        {
            string LoadeInfo = string.Empty;
            string[] FiletoLoad = GetAutoSaveFilesList();

            foreach (string LFile in FiletoLoad)
            {
                if (File.Exists(LFile))
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader(LFile))
                        {
                            LoadeInfo = sr.ReadToEnd();
                            sr.Close();
                        }
                    }
                    catch (Exception e) { SetExeption(e); }
                }
            }
            return LoadeInfo;
        }

        private bool SetExeption(Exception e)
        {
            IsErr = true;
            ErrText = e.Message;
            return false;
        }

        public bool AutoSave(string[] Info)
        {
            string[] FiletoSave = GetAutoSaveFilesList();
            if (Info.Length == 0 || FiletoSave.Length == 0)
                 return SetErr("Err AutoSave.Info.Length == 0 || FiletoSave.Length == 0!!");

            string str = string.Empty;
            foreach (string txt in Info) str += txt + "\r";

            foreach (string FtoSave in FiletoSave)
                if (ChkFile(FtoSave)) SetFileString(FtoSave, str);
            
            return false;
        }

        public bool AutoSave<T>(T obj)
        {
            string[] FiletoSave = GetAutoSaveFilesList();
            if (obj == null || FiletoSave.Length == 0) return SetErr("Err AutoSave.obj == null || FiletoSave.Length == 0!!!");
            foreach (string FtoSave in FiletoSave) SaveJson<T>(FtoSave, obj);
            return true;
        }

        public bool ChkDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                DirectoryInfo tmpdir = new DirectoryInfo(dir);
                try
                {
                    tmpdir.Create();
                }
                catch (Exception e) { SetExeption(e); }
                if (Directory.Exists(dir)) return true;
            }
            else return true;
            return false;
        }

        public bool ChkFile(string file)
        {
            if (!File.Exists(file))
            {
                try
                {
                    using (FileStream fs = File.Create(file))
                    {
                        if (File.Exists(file)) return true;
                    }
                }
                catch (Exception e) { SetExeption(e); }
                return false;
            }

            return true;
        }

        public string ComputeMD5Checksum(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] fileData = new byte[fs.Length];
                fs.Read(fileData, 0, (int)fs.Length);
                byte[] checkSum = md5.ComputeHash(fileData);
                return BitConverter.ToString(checkSum);
            }
        }

        public string DirFile(string Dir, string File)
        {
            if (string.IsNullOrEmpty(Dir) || string.IsNullOrEmpty(File)) return string.Empty;
            if (Dir[Dir.Length - 1] == '\\') Dir = Dir.Substring(0, Dir.Length - 1);
            if (File[0] == '\\') File = File.Substring(1);
            return Dir + "\\" + File;
        }

        public bool DirRename(string Dir, string NewDir)
        {
            DirectoryInfo CorDir = new DirectoryInfo(Dir);
            CorDir.MoveTo(NewDir);
            if (CorDir.Exists) return true;
            else return false;
        }
        public bool FileRename(string File, string NewFile)
        {
            FileInfo CorFile = new FileInfo(File);
            CorFile.MoveTo(NewFile);
            if (CorFile.Exists) return true;
            else return false;
        }
        internal bool IsSameDisk(string Dir, string Dir2)
        {
            if (Dir != null && Dir2 != null)
            {
                if (Dir.Length > 3 && Dir2.Length > 3)
                {
                    if (Dir.IndexOf(@":\") == 1 && Dir2.IndexOf(@":\") == 1)
                    {
                        Dir = Dir.ToLower();
                        Dir2 = Dir2.ToLower();
                        if (Dir[0] == Dir2[0]) return true;
                    }
                }
            }
            return false;
        }
        internal bool IsSameDir(string DirFrom, string DirTo)
        {
            if (DirFrom != null && DirTo != null)
            {
                if (DirFrom.Length > 1 && DirTo.Length > 1)
                {
                    if (DirFrom.IndexOf(DirTo) != -1 || DirTo.IndexOf(DirFrom) != -1) return true;
                }
            }
            return false;
        }
        internal string GetAutoLoadeFirstFile()
        {
            string LoadeFile = "";
            string[] FiletoLoad = GetAutoSaveFilesList();

            foreach (string LFile in FiletoLoad)
            {
                if (File.Exists(LFile))
                {
                    LoadeFile = LFile;
                    break;
                }
            }
            return LoadeFile;
        }
        public string[] GetAutoSaveFilesList()
        {
            string ApplicationFileName = Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Split('\\').Last()) + ".inf";
            string AdditionalFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName.Split('\\').Last();
            string[] AutoSaveFiles = new string[] { @"C:\Windows\Temp", @"D:", @"E:", @"C:" };
            List<string> AutoSaveFilesList = new List<string>() 
            { 
                Directory.GetCurrentDirectory() + "\\" + ApplicationFileName , 
                AdditionalFolder + "\\" + ApplicationFileName 
            };
            foreach (string elem in AutoSaveFiles)
                AutoSaveFilesList.Add(elem + "\\" + ApplicationFileName);

            return AutoSaveFilesList.ToArray();
        }
        public List<string> GetFileList(string file)
        {
            List<string> FileList = new List<string>();
            if (File.Exists(file))
                FileList = File.ReadAllLines(file).ToList();
            return FileList;
        }
        public List<string> GetFileList(string file, int nEncoding)
        {
            string encoding = "utf-8";
            if (nEncoding == 1)
                encoding = "windows-1251";

            if (encoding == null || encoding.Length == 0) return GetFileList(file);

            List<string> FileList = new List<string>();
            if (File.Exists(file))
                FileList = File.ReadAllLines(file, Encoding.GetEncoding(encoding)).ToList();
            return FileList;
        }
        public List<string> GetFileList(string file, string encoding)
        {
            if (encoding == null || encoding.Length == 0) return GetFileList(file);

            List<string> FileList = new List<string>();
            if (File.Exists(file))
                FileList = File.ReadAllLines(file, Encoding.GetEncoding(encoding)).ToList();
            return FileList;
        }
        public bool SetFileList(string file, List<string> fileList, int nEncoding)
        {
            if (nEncoding == 1)
                return SetFileList(file, fileList, "windows-1251");
            else
                return SetFileList(file, fileList);
        }
        public bool SetFileList(string file, List<string> fileList, string encoding = "utf-8")
        {
            try
            {
                FileStream f1 = new FileStream(file, FileMode.Truncate, FileAccess.Write, FileShare.Read);
                using (StreamWriter sw = new StreamWriter(f1, Encoding.GetEncoding(encoding)))
                {
                    foreach (string txt in fileList) sw.WriteLine(txt);
                }
            }
            catch (Exception e)
            {
                SetExeption(e);
                return false;
            }
            return true;
        }
        public bool SetFileString(string file, string text)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Truncate, FileAccess.Write, FileShare.Read);
                using (StreamWriter writetext = new StreamWriter(fs))
                {
                    writetext.WriteLine(text);
                }
            }
            catch (Exception e)
            {
                SetExeption(e);
                return false;
            }
            return true;
        }

        public FileInfo[] SearchFiles()=>SearchFiles(GetDefoltDirectory());
        public FileInfo[] SearchFiles(string dir)
        {
            if(FileFilter.Length == 0)return SearchFiles(dir, ["*.*"]);
            else return SearchFiles(dir, FileFilter);
        }

        public FileInfo[] SearchFiles(string dir, string[] filter, int Lv = 0)
        {
            //Lv
            //0 All filles
            //1 TopDirectoryOnly
            //2 From TopDirectoryOnly to 1DirLv
            //3 From TopDirectoryOnly to 2DirLv
            //-1 Jist DirL1 1
            //-2 Jist DirL1 2

            if (string.IsNullOrEmpty(dir)) dir = AppDomain.CurrentDomain.BaseDirectory;

            FileInfo[] fileList = [];

            if (Directory.Exists(dir))
            {
                DirectoryInfo DI = new DirectoryInfo(dir);

                if (Lv == 0) fileList = filter.SelectMany(fi => DI.GetFiles(fi, SearchOption.AllDirectories)).Distinct().ToArray();
                else fileList = filter.SelectMany(fi => DI.GetFiles(fi, SearchOption.TopDirectoryOnly)).Distinct().ToArray();
            }
            return fileList;
        }
        public string GetDefoltDirectory() => AppDomain.CurrentDomain.BaseDirectory;
        public bool DelAllFileFromDir() => DelAllFileFromDir(GetDefoltDirectory());
        public bool DelAllFileFromDir(string rezultDir)
        {
            bool rezult = true;
            var fileList = SearchFiles(rezultDir);
            if (fileList != null)
            {
                foreach (var f in fileList)
                {
                    File.Delete(f.FullName);
                    if (File.Exists(f.FullName)) rezult = false;
                }
            }
            return rezult;
        }
        internal bool DelAllFileFromList(FileInfo[] fileList)
        {
            bool rezult = true;
            if (fileList == null) return false;
            if (fileList.Length == 0) return false;

            foreach (var f in fileList)
            {
                File.Delete(f.FullName);
                if (File.Exists(f.FullName)) rezult = false;
            }
            return rezult;
        }
        public bool CheckAccessToFile(string file)
        {
            try
            {
                //System.Security.AccessControl.FileSecurity ds = File.GetAccessControl(file);

                // Attempt to get a list of security permissions from the folder. 
                // This will raise an exception if the path is read only or do not have access to view the permissions. 

                var fInfo = new FileInfo(file);
                FileSecurity fSecurity = fInfo.GetAccessControl();
                SecurityIdentifier usersSid = new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null);
                FileSystemRights fileRights = FileSystemRights.Read | FileSystemRights.Synchronize; //All read only file usually have Synchronize added automatically when allowing access, refer the msdn doc link below
                var rules = fSecurity.GetAccessRules(true, true, usersSid.GetType()).OfType<FileSystemAccessRule>();
                var hasRights = rules.Where(r => r.FileSystemRights == fileRights).Any();
                return true;
            }
            catch (UnauthorizedAccessException e)
            {
                string text = e.Message;
                return false;
            }
        }
        public bool IsFolder(string href)
        {
            FileAttributes attr = File.GetAttributes(href);
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory) return true; 
            else return false;
        }
        public bool LoadeJson<T>(out T obj) => LoadeJson(GetJsonDefoltSaveFile(),out obj);
        public bool LoadeJson<T>(string file,out T obj)
        {
            obj = default(T);
            if (File.Exists(file))
            {
                try
                {
                    // Open the text file using a stream reader.
                    using (var sr = new StreamReader(file))
                    {
                        // Read the stream as a string, and write the string to the console.
                        string? jsonString = sr.ReadToEnd();
                        if (jsonString != null)
                        {
                            obj = JsonSerializer.Deserialize<T>(jsonString);
                            return true;
                        }
                    }
                }
                catch (IOException e)
                {
                    return false;
                }
            }
            return false;
        }

        
        public string GetJsonDefoltSaveFile() => GetDefoltDirectory() + JsonSaveFile;
        public bool SaveJson<T>(T obj) => SaveJson<T>(GetJsonDefoltSaveFile(),  obj);
        public bool SaveJson<T>(string file, T obj)
        {
            try
            {
                using FileStream createStream = File.Create(file);
                JsonSerializer.Serialize(createStream, obj);
            }
            catch (IOException e)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> SaveJsonAsync<T>(string file,T obj)
        {
            try
            {
                await using FileStream createStream = File.Create(file);
                await JsonSerializer.SerializeAsync(createStream, obj);
            }
            catch (IOException e)
            {
                return false;
            }
            return true;
        }

        public bool CheckFileName(string dir) => true;
        public bool FixFileName(string dir) => true;

        public void ClearInformation()
        {
            ErrText = string.Empty;
            IsErr = false;
            TextMessag = string.Empty;
        }
        public async Task<bool> FindCopyAndDel(string Dir)
        {
            if (!Directory.Exists(Dir)) return false;

            string rezulText = string.Empty;
            long maxLenghtFile = 16777216;
            List<CopyList> CheckFileList = new List<CopyList>();
            FileList fileList = new FileList(Dir, maxLenghtFile);

            string text = string.Empty;
            rezulText += "Start search" + "\nDir - " + Dir;
            await Task.Run(() => { fileList.MadeList(); });
            CheckFileList = fileList.GetList();
            rezulText += "\nFinish " + CheckFileList.Count();

            int i = 0, j = 0;
            for (i = 0; i < CheckFileList.Count() - 1; i++)
            {
                if (CheckFileList[i].Copy > -1) continue;
                string heshI = CheckFileList[i].Hesh;
                long fileLength = CheckFileList[i].FileLength;


                for (j = i + 1; j < CheckFileList.Count(); j++)
                {
                    if (CheckFileList[j].Copy > -1) continue;
                    if (fileLength != 0)
                    {
                        if (CheckFileList[j].Copy == -1 && fileLength == CheckFileList[j].FileLength)
                        {
                            CheckFileList[i].Copy = i;
                            CheckFileList[j].Copy = i;
                        }
                    }
                    else
                    {
                        if (CheckFileList[j].Copy == -1 && heshI == CheckFileList[j].Hesh)
                        {
                            CheckFileList[i].Copy = i;
                            CheckFileList[j].Copy = i;
                        }
                    }
                }
            }

            var copyList = CheckFileList.Where(x => x.Copy != -1).OrderBy(y => y.Copy).ToList();
            if (copyList.Count > 0)
            {
                i = -1;
                int nDelFiles = 0;
                foreach (var elem in copyList)
                {
                    if (i == elem.Copy)
                    {
                        elem.ForDel = true;

                        File.Delete(elem.File);
                        if (!File.Exists(elem.File)) nDelFiles++;
                        if (elem.FileLength == 0) text += "\n" + i + " " + elem.Copy + " " + elem.File + " " + elem.Hesh + "  - DELETED by HeshCOPY";
                        else text += "\n" + i + " " + elem.Copy + " " + elem.File + " " + elem.FileLength + "  - DELETED by LengthCOPY";

                    }
                    else
                    {
                        if (elem.FileLength == 0) text += "\n" + i + " " + elem.Copy + " " + elem.File + " " + elem.Hesh + "  -  HeshCOPY";
                        else text += "\n" + i + " " + elem.Copy + " " + elem.File + " " + elem.FileLength + "  -  LengthCOPY";
                    }
                    i = elem.Copy;
                }
                if (nDelFiles > 0) text += "\n" + nDelFiles + " Deleted Files!!!";
            }

            if (copyList.Count == 0) TextMessag = "Kопий Nет!";
            else TextMessag = text;

            return true;
        }
    }
}