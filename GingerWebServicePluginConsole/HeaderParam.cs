namespace GingerWebServicePluginConsole
{
    public class HeaderParam
    {
        public string Key { get; set; }
        public string Value { get; set; }        

        public HeaderParam(string Key, string Value)
        {
            this.Key = Key;
            this.Value = Value;
        }
    }
}
