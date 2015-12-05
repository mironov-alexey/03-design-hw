using System.Collections.Generic;
using _03_design_hw.Statistics;

namespace _03_design_hw.CloudGenerator
{
    public interface ICloudData
    {
        IEnumerable<Tag> GetTags(Statistic statistic);
    }
}