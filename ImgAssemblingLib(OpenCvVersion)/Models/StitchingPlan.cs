using System;
using System.Collections.Generic;
using WinFormsApp1.Enum;

namespace ImgAssemblingLib.Models
{
    [Serializable]
    internal class StitchingPlan
    {
        public string Dir { get; set; } = string.Empty;
        public int FilesNumber { get; set; }
        public List<SelectedFiles> SelectedFiles { get; set; }
        public EnumDirection? Direction { get; set; }
        public StitchingPlan (string dir, List<SelectedFiles> selectedFiles, EnumDirection? direction)
        {
            Dir = dir;
            FilesNumber = selectedFiles.Count;
            SelectedFiles = selectedFiles;
            Direction = direction;
        }

        public StitchingPlan()
        {
        }
    }
}
