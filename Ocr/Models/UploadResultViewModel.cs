using System;
namespace HackathonEq2018Ocr.Models
{
    public class UploadResultViewModel
    {
        public UploadResultViewModel()
        {
        }

        public long FileSize { get; set; }
        public string FileName { get; set; }
        public string FileText { get; set; }
    }
}
