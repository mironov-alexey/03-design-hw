using System.Drawing;
using _03_design_hw.Loaders;

namespace _03_design_hw.CloudGenerator
{
    public class SimpleCloudGenerator : ICloudGenerator
    {
        private const int MaxImageSize = 5000;
        private readonly Settings _settings;
        private readonly ICloudData _tags;

        public SimpleCloudGenerator(ICloudData tags, Settings settings)
        {
            _tags = tags;
            _settings = settings;
        }

        public Image GenerateCloudImage()
        {
            using (var preReleaseImage = GeneratePreReleaseImage())
            {
                var releaseImage = new Bitmap(_settings.Width, _settings.Height);
                using (var releaseGraphics = Graphics.FromImage(releaseImage))
                {
                    releaseGraphics.Clear(Color.White);
                    releaseGraphics.DrawImage(preReleaseImage, 0, 0);
                    return releaseImage;
                }
            }
        }

        private Image GeneratePreReleaseImage()
        {
            var img = new Bitmap(MaxImageSize, MaxImageSize);
            using (var graphics = Graphics.FromImage(img))
            {
                foreach (var tag in _tags.GetTags())
                    graphics.DrawString(tag.Word.WordString, tag.Font, new SolidBrush(tag.Color), tag.Location.X,
                        tag.Location.Y);
                return img;
            }
        }
    }
}