using System.Collections.Generic;

namespace _03_design_hw
{
    public interface ICloudData
    {
        int Width{ get; }
        int Height{ get; }
        IEnumerable<Tag> GetTags();
    }
}