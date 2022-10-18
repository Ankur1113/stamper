using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Interfaces
{
   
    public interface IDocuments
    {
        string GenerateFileName(string fileName);

        string UploadFile(IFormFile file, string folderPath, string filename = null);

        MemoryStream DownloadFile(string fileName);

        bool DeleteFile(string folderName, string fileName);
        bool BackupFile(string sourceFilePath, string folderName);

        string GetPath(string folderName);

        string GetBackupPath(string folderName);

        bool isFileExists(string fileName);

        bool CopyFile(string source, string destination);

        void CreateFileDirectory(string filePath);
    }
}
