using System;

namespace MediaWikiPublisher.Converter.Ast
{
    public abstract class MarkupNode : IEquatable<MarkupNode>
    {
        public void Accept(IMarkupNodeVisitor visitor)
        {
            Visit(visitor);
        }

        public abstract void Visit(IMarkupNodeVisitor visitor);

        public override int GetHashCode()
        {
            return GetHashCodeOverride();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((MarkupNode)obj);
        }

        public virtual bool Equals(MarkupNode other)
        {
            return true;
        }

        protected virtual int GetHashCodeOverride()
        {
            return (base.GetHashCode() * 397);
        }
        
        public static bool operator ==(MarkupNode left, MarkupNode right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MarkupNode left, MarkupNode right)
        {
            return !Equals(left, right);
        }
    }
}
