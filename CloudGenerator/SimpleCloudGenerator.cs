using System.Drawing;
using Color = System.Drawing.Color;


namespace _03_design_hw.CloudGenerator
{
    public class SimpleCloudGenerator : ICloudGenerator
    {
        private const int MaxImageSize = 5000;
        private readonly ICloudData _tags;
        public SimpleCloudGenerator(ICloudData tags)
        {
            _tags = tags;
        }

        public Image GeneratePreReleaseImage()
        {
            var img = new Bitmap(MaxImageSize, MaxImageSize);
            using (Graphics graphics = Graphics.FromImage(img))
            {
                foreach (var tag in _tags.GetTags())
                    graphics.DrawString(tag.Word.WordString, tag.Font, new SolidBrush(tag.Color), tag.Location.X, tag.Location.Y);
                return img;
            }
        }

        public Image GenerateCloudImage()
        {
            using (var preReleaseImage = GeneratePreReleaseImage())
            {
                var releaseImage = new Bitmap(_tags.Width, _tags.Height);
                using (Graphics releaseGraphics = Graphics.FromImage(releaseImage))
                {
                    releaseGraphics.Clear(Color.White);
                    releaseGraphics.DrawImage(preReleaseImage, 0, 0);
                    return releaseImage;
                }
            }
        }
    }
}
