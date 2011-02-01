using System;

namespace Demos.Expressions.FileQuery
{
    public abstract class FileQuerySpecification
    {
        internal class TrueFileQuerySpecification : FileQuerySpecification
        {
            public override bool Evaluate(FileMetadata fileMetadata)
            {
                return true;
            }
        }

        internal class FalseFileQuerySpecification : FileQuerySpecification
        {
            public override bool Evaluate(FileMetadata fileMetadata)
            {
                return false;
            }
        }

        private static readonly FileQuerySpecification _true = new TrueFileQuerySpecification();
        private static readonly FileQuerySpecification _false = new FalseFileQuerySpecification();

        public static EqualsQuerySpecification Equals(IFileMetadataProperty source, object comparand, bool not = false)
        {
            return new EqualsQuerySpecification(source, comparand, not);
        }

        public static FileQuerySpecification operator &(FileQuerySpecification spec1, FileQuerySpecification spec2)
        {
            return new CompoundQuerySpecification(spec1, spec2);
        }

        public static bool operator false(FileQuerySpecification spec)
        {
            return spec == _false;
        }

        public static bool operator true(FileQuerySpecification spec)
        {
            return spec == _true;
        }

        public abstract bool Evaluate(FileMetadata fileMetadata);
    }

    public class CompoundQuerySpecification : FileQuerySpecification
    {
        private readonly FileQuerySpecification _left;
        private readonly FileQuerySpecification _right;

        internal CompoundQuerySpecification(FileQuerySpecification left, FileQuerySpecification right)
        {
            _left = left;
            _right = right;
        }

        public override bool Evaluate(FileMetadata fileMetadata)
        {
            return _left.Evaluate(fileMetadata) && _right.Evaluate(fileMetadata);
        }

        public override string ToString()
        {
            return String.Format("CompoundQuery({0}, {1})", _left, _right);
        }
    }

    public class EqualsQuerySpecification : FileQuerySpecification
    {
        private readonly IFileMetadataProperty _source;
        private readonly object _comparand;
        private readonly bool _not;

        internal EqualsQuerySpecification(IFileMetadataProperty source, object comparand, bool not = false)
        {
            _source = source;
            _comparand = comparand;
            _not = not;
        }

        public override bool Evaluate(FileMetadata fileMetadata)
        {
            var target = _source.ValueFrom(fileMetadata);
            var op = target.GetType().GetMethod("op_Equality");
            if (op == null || op.ReturnType != typeof(bool))
            {
                return false;
            }
            return (bool)op.Invoke(target, new[] { _comparand }) ^ _not;
        }

        public override string ToString()
        {
            return String.Format("{0} {1}== {2}", _not ? "!" : "", _source.Name, _comparand);
        }
    }
}