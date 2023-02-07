namespace BlazorAppPowerBI.Data
{
    public class UserInfoGlobalClass
    {
        public string id { get; private set; } = "";
        public string name { get; private set; } = "";

        public static UserInfoGlobalClass ob { get; set; }

        public event EventHandler UserChangedEvent;

        public void SetId(String ID)
        {
           id = ID;
        }
        public void SetName(String Name)
        {
            name = Name;
        }
    }
}
