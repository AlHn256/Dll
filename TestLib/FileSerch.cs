namespace TestLib
{
    public class FileSerch
    {
        public string[] GetFiles(MatchType filter)
        {
            string[] filtr = ["*.jpg", "*.bmp", "*.png"];
            string[] files = Directory.GetFiles("D:\\Development", filtr[0],
                new EnumerationOptions
                {
                    IgnoreInaccessible = true,
                    RecurseSubdirectories = true
                });
            return files;
        }

        public void GetDirs()
        {
            string[] dirs = Directory.GetDirectories("D:\\Development", "*",
                new EnumerationOptions
                {

                    IgnoreInaccessible = true,
                    RecurseSubdirectories = true
                });

        }
    }
}
