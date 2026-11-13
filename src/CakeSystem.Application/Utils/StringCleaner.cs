using System.Text;

namespace CakeSystem.Application.Utils
{
    public static class StringCleaner
    {
        public static string CleanCyrillic(string cyrillicStr)
        {
            StringBuilder sb = new StringBuilder();

            foreach(char c in cyrillicStr)
            {
                if (((int)c >= (int)'а' && (int)c <= (int)'я') || ((int)c >= (int)'А' && (int)c <= (int)'Я'))
                    sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
