namespace BlazorToDoList.Extensions
{
    public static class StringsExtensions
    {
        public static string EncodeBase64(this string value)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(value);
            return System.Convert.ToBase64String(plainTextBytes); 
        }
        
        public static string DecodeBase64(this string value)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(value);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes); 
        }
    }
}