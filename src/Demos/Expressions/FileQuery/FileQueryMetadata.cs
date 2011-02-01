using System;

namespace Demos.Expressions.FileQuery
{
    public class FileQueryMetadata
    {
        public FileQueryMetadata()
        {
            GetType().GetProperties().ForEach(prop => prop.SetValue(this, Activator.CreateInstance(prop.PropertyType, prop.Name), null));
        }

        public FileQueryProperty<DateTime> CreationTime { get; private set; }
        public FileQueryProperty<string> DirectoryName { get; private set; }
        public FileQueryProperty<string> Extension { get; private set; }
        public FileQueryProperty<string> FullName { get; private set; }
        public FileQueryProperty<DateTime> LastAccessTime { get; private set; }
        public FileQueryProperty<DateTime> LastWriteTime { get; private set; }
        public FileQueryProperty<long> Length { get; private set; }
        public FileQueryProperty<string> Name { get; private set; }
    }

    public interface IFileMetadataProperty
    {
        string Name { get; }
        Type Type { get; }
        object ValueFrom(FileMetadata fileMetadata);
    }

    public class FileQueryProperty<T> : IFileMetadataProperty
    {
        public FileQueryProperty(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
        public Type Type { get { return typeof(T); } }

        public static FileQuerySpecification operator ==(FileQueryProperty<T> fileQueryProperty, T comparand)
        {
            return FileQuerySpecification.Equals(fileQueryProperty, comparand);
        }

        public static FileQuerySpecification operator !=(FileQueryProperty<T> fileQueryProperty, T comparand)
        {
            return FileQuerySpecification.Equals(fileQueryProperty, comparand, true);
        }

        public object ValueFrom(FileMetadata fileMetadata)
        {
            return fileMetadata.GetType().GetProperty(Name).GetValue(fileMetadata, null);
        }
    }
}