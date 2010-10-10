namespace TouchSamples.PhotoViewer.ViewModels
{
    public class Picture
    {
        private readonly string m_Path;

        public Picture(string path)
        {
            m_Path = path;
        }

        public string Path
        {
            get { return m_Path; }
        }
    }
}