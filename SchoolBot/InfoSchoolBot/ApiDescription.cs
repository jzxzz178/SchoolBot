namespace SchoolBot;

public static class ApiDescription
{
    public class ApiDescriptionAttribute : Attribute
    {
        public string Description;

        public ApiDescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}