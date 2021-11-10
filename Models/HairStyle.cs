namespace webui.Models
{
    public class HairStyle
    {
        public int StyleId { get; set; }
        public string StyleName { get; set; }
        public string TypeId { get; set; }
        public string Description { get; set; }
        public bool IsSelected { get; set; }
        public bool IsFilter { get; set; }

    }
}