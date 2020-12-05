using System;

namespace NFTIntegration.Data
{
    public class ReportData
    {
        public Int64 ReportId { get; set; }
        public int High { get; set; }
        public int Medium { get; set; }
        public int Low { get; set; }
        public int Information { get; set; }
        public string RunDate { get; set; }
        public string ReportFileName { get; set; }
    }
}
