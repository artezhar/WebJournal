using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;
using System.Reflection;

namespace WebZhurnal.Models
{
    public class ExcelManager
    {
        private ExcelPackage package;

        public List<List<string>> GetRows(int max=1000)
        {
            List<List<string>> result = new List<List<string>>();
            ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
            for (int i = workSheet.Dimension.Start.Row;
                i <= Math.Min(workSheet.Dimension.End.Row, max);
                i++)
            {
                result.Add(new List<string>());
                for (int j = workSheet.Dimension.Start.Column;
                        j <= workSheet.Dimension.End.Column;
                        j++)
                {
                    result.Last().Add(workSheet.GetValue<string>(i, j));
                }
            }
             return result;
        }

        public List<string> GetRow(int i)
        {
            ExcelWorksheet workSheet = package.Workbook.Worksheets[1];
            List<string> result = new List<string>();
            for (int j = workSheet.Dimension.Start.Column;
                        j <= workSheet.Dimension.End.Column;
                        j++)
            {
                result.Add(workSheet.GetValue<string>(i, j));
            }
            return result;
        }

        public ExcelManager(string path)
        {
            package = new ExcelPackage(new FileInfo(path));
        }


    }
}
