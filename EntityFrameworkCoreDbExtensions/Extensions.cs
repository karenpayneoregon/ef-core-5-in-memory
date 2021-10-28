using System.Windows.Forms;

namespace EntityFrameworkCoreDbExtensions
{
    public static class Extensions
    {
        public static bool CurrentIsValid(this BindingSource source)
            => source.DataSource is not null && source.Current is not null;
    }
}