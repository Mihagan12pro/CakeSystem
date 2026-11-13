using NPOI.SS.UserModel;

namespace CakeSystem.Application.Extensions
{
    internal static class CellExtensions
    {
        public static object GetObjectCellValue(this ICell cell)
        {
            switch(cell.CellType)
            {
                case CellType.String:
                    return cell.StringCellValue;

                case CellType.Numeric:
                    return cell.NumericCellValue;
            }

            return null;
        }
    }
}
