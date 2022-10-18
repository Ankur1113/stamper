using Demo.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace RoomBooking.WebApi.Common
{
    public class Documents : IDocuments
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly string DocumentPath = string.Empty;
        private readonly string DocumentPathBackup = AppSettings.DocumentPathBackup;

        public Documents(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
            DocumentPath = _appEnvironment.WebRootPath;
        }

        public bool BackupFile(string sourceFilePath, string folderName)
        {
            bool blnBackUpFile = false;
            try
            {
                string strBackUpFilePath = GetBackupPath(folderName);

                //if folder not exist then Create folder
                CreateFileDirectory(strBackUpFilePath);

                strBackUpFilePath = Path.Combine(strBackUpFilePath, GenerateFileName(Path.GetFileName(sourceFilePath)));
                CopyFile(sourceFilePath, strBackUpFilePath);

                blnBackUpFile = true;
            }
            catch (Exception ex)
            {
                blnBackUpFile = false;
                throw ex;
            }
            return blnBackUpFile;
        }

        public void CreateFileDirectory(string filePath)
        {
            try
            {
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteFile(string folderName, string fileName)
        {
            bool blnDeleteFile = false;
            try
            {
                var vFullPath = Path.Combine(folderName, fileName);

                if (isFileExists(vFullPath))
                {
                    File.Delete(vFullPath);
                    blnDeleteFile = true;
                }
            }
            catch (Exception ex)
            {
                blnDeleteFile = false;
                throw ex;
            }
            return blnDeleteFile;
        }

        public MemoryStream DownloadFile(string fileName)
        {
            try
            {
                var vMemoryStream = new MemoryStream();
                if (File.Exists(fileName))
                {
                    using (var vFileStream = new FileStream(fileName, FileMode.Open))
                    {
                        vFileStream.CopyTo(vMemoryStream);
                    }
                    vMemoryStream.Position = 0;
                }
                return vMemoryStream;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GenerateFileName(string fileName)
        {
            try
            {
                string strFileName = string.Empty;
                string[] strName = fileName.Split('.');
                strFileName = strName[0] + "_" + DateTime.UtcNow.ToString("yyyyMMdd") + "." + strName[strName.Length - 1];
                return strFileName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetBackupPath(string folderName)
        {
            try
            {
                return Path.Combine(DocumentPathBackup, folderName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string GetPath(string folderName)
        {
            try
            {
                return Path.Combine(DocumentPath, folderName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool CopyFile(string source, string destination)
        {
            try
            {
                bool blnCopyFile = false;
                if (isFileExists(source))
                {
                    File.Copy(source, destination, true);
                    blnCopyFile = true;
                }
                return blnCopyFile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool isFileExists(string fileName)
        {
            try
            {
                bool blnFileExists = false;
                if (File.Exists(fileName))
                    blnFileExists = true;

                return blnFileExists;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string UploadFile(IFormFile file, string folderPath, string filename = null)
        {
            string StrValue = string.Empty;
            try
            {
                var vPathToSave = GetPath(folderPath);

                if (file.Length > 0)
                {
                    //if folder not exist then Create folder
                    CreateFileDirectory(vPathToSave);

                    //var vFileName = file.FileName;
                    string vFileName = string.Empty;
                    if (filename != null)
                    {
                        vFileName = $"{filename}" + Path.GetExtension(file.FileName);
                    }
                    else
                    {
                        vFileName = $"{DateTime.UtcNow.ToString("yyyyMMddHHmmssfffffff")}" + Path.GetExtension(file.FileName);
                    }

                    //string vFileName1 = vFileName;
                    //var name = vFileName1.Split('.');

                    //String filename = name[0];
                    //String fileext = name[name.Length-1];

                    var vFullPath = Path.Combine(vPathToSave, vFileName);

                    if (isFileExists(vFullPath))
                        BackupFile(vFullPath, folderPath);

                    using (var vFileStream = new FileStream(vFullPath, FileMode.Create))
                    {
                        file.CopyTo(vFileStream);
                    }
                    StrValue = Path.Combine(folderPath, vFileName);
                }
            }
            catch (Exception ex)
            {
                StrValue = "";
                throw ex;
            }
            return StrValue;
        }
    }
}
