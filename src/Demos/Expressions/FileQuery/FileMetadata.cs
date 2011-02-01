using System;
using System.IO;

namespace Demos.Expressions.FileQuery
{
    public class FileMetadata
    {
        public FileMetadata(FileInfo fileInfo)
        {
            CreationTime = fileInfo.CreationTime;
            DirectoryName = fileInfo.DirectoryName;
            Extension = fileInfo.Extension;
            FullName = fileInfo.FullName;
            LastAccessTime = fileInfo.LastAccessTime;
            LastWriteTime = fileInfo.LastWriteTime;
            Length = fileInfo.Length;
            Name = fileInfo.Name;
        }

        public DateTime CreationTime { get; private set; }
        public string DirectoryName { get; private set; }
        public string Extension { get; private set; }
        public string FullName { get; private set; }
        public DateTime LastAccessTime { get; private set; }
        public DateTime LastWriteTime { get; private set; }
        public long Length { get; private set; }
        public string Name { get; private set; }
    }
}