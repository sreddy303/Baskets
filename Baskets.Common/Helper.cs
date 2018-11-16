namespace Baskets.Common
{
    public static class Helper
    {
        public static int ToInt(this string value)
        {
            int i ;
            int.TryParse(value, out i);
            return i;
        }
    }
}
