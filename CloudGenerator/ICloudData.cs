using System.Collections.Generic;

namespace _03_design_hw.CloudGenerator
{
    public interface ICloudData
    {
        IEnumerable<Tag> GetTags();
    }
}