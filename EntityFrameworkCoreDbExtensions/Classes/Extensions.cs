using System.Windows.Forms;

namespace EntityFrameworkCoreDbExtensions.Classes
{
    public static class Extensions
    {
        /// <summary>
        /// Ensure zero exceptions from nulls
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool CurrentIsValid(this BindingSource source)
            => source.DataSource is not null && source.Current is not null;
    }
}