using System;

namespace MediaWikiPublisher.Converter.Ast
{
    public sealed class ImageMarkupNode : MarkupNode
    {
        private readonly string imageUri;

        public ImageMarkupNode(string imageUri)
        {
            if(imageUri == null)
            {
                throw new ArgumentNullException("imageUri");
            }

            this.imageUri = imageUri;
        }

        public string ImageUri { get { return imageUri; } }

        public override void Visit(IMarkupNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool Equals(MarkupNode other)
        {
            var otherNode = (ImageMarkupNode)other;
            return imageUri == otherNode.imageUri &&
                   base.Equals(other);
        }

        protected override int GetHashCodeOverride()
        {
            return base.GetHashCodeOverride() ^ imageUri.GetHashCode();
        }
    }
}
