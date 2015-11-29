using System.Drawing;
using Color = System.Drawing.Color;


namespace _03_design_hw
{
    public class SimpleCloudGenerator : ICloudGenerator
    {
        private const int MaxImageSize = 5000;
        private readonly CloudDataGenerator _cloudData;
        public SimpleCloudGenerator(CloudDataGenerator cloudData)
        {
            _cloudData = cloudData;
        }

        public Image GeneratePreReleaseImage()
        {
            var img = new Bitmap(MaxImageSize, MaxImageSize);
            using (Graphics graphics = Graphics.FromImage(img))
            {
                foreach (var tag in _cloudData.GetTagsSequence())
                    graphics.DrawString(tag.Word.WordString, tag.Font, new SolidBrush(tag.Color), tag.Location.X, tag.Location.Y);
                return img;
            }
        }

        public Image GenerateCloudImage()
        {
            using (var preReleaseImage = GeneratePreReleaseImage())
            {
                var releaseImage = new Bitmap(_cloudData.Width, _cloudData.Height);
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
