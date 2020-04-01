using COF.BusinessLogic.Models.Report;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COF.BusinessLogic.Services.Export
{
    public interface IExcelExportService
    {
        byte[] ExportExcelOutsource(ExportDailyModel partnerReport);
        byte[] ExportMonthyCakeOrDrinkCategoryRevenue(MonthlyRevenueFilterByCakeOrDrinkCategoryModel monthlyRevenue);
    }
    public class ExcelExportService : IExcelExportService
    {
        public ExcelExportService()
        {

        }
        public byte[] ExportExcelOutsource(ExportDailyModel partnerReport)
        {
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                using (var xlPackage = new ExcelPackage(stream))
                {
                    ExportDailyOrdersForShop(xlPackage, partnerReport);

                    xlPackage.Save();
                }
                bytes = stream.ToArray();
            }
            return bytes;
        }

        public byte[] ExportMonthyCakeOrDrinkCategoryRevenue(MonthlyRevenueFilterByCakeOrDrinkCategoryModel monthlyRevenue)
        {
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                using (var xlPackage = new ExcelPackage(stream))
                {
                    ExportMonthyCakeOrDrinkCategoryRevenue(xlPackage, monthlyRevenue);

                    xlPackage.Save();
                }
                bytes = stream.ToArray();
            }
            return bytes;
        }

        private void ExportDailyOrdersForShop(ExcelPackage xlPackage, ExportDailyModel shopDaily)
        {
            var worksheet = xlPackage.Workbook.Worksheets.Add("Doanh thu");
            int row = 1;

            var properties = new List<string>
                    {
                        "Tên danh mục",
                        "Tổng số sản phẩm",
                        "Doanh thu"
                    };
            int maxCol = properties.Count;

            worksheet.Cells[1, 1].Value = "Ngày";
            worksheet.Cells[1, 4].Value = "Tổng hóa đơn";
            worksheet.Cells[1, 5].Value = "Tổng doanh thu";
            worksheet.Cells[1, 6].Value = "Chi tiết";

            worksheet.Cells[2, 1].Value = DateTime.UtcNow.AddHours(7).ToString("dd/MM/yyyy");
            worksheet.Cells[2, 4].Value = shopDaily.ToDayRevenue.TotalUnit;
            worksheet.Cells[2, 4].Style.Numberformat.Format = "###,###,##0";
            worksheet.Cells[2, 5].Value = shopDaily.ToDayRevenue.TotalMoney;
            worksheet.Cells[2, 5].Style.Numberformat.Format = "###,###,##0";
            worksheet.Cells[2, 6].Value = "";

            row = 3;

            for (int i = 0; i < maxCol; i++)
            {
                worksheet.Cells[row, i + 1].Value = properties[i];
            }

            worksheet.Cells[row, 1, row, maxCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[row, 1, row, maxCol].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
            worksheet.Cells[row, 1, row, maxCol].Style.Font.Bold = true;
            worksheet.Cells[row, 1, row, maxCol].Style.WrapText = true;
            worksheet.Cells[row, 1, row, maxCol].Style.Font.Size = 11;
            worksheet.Cells[row, 1, row, maxCol].Style.Font.Name = "Calibri";
            worksheet.Cells[row, 1, row, maxCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[row, 1, row, maxCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //worksheet.View.FreezePanes(2, 1);

            var column = 0;
            for (int i = 0; i < shopDaily.ToDayRevenue.Details.Count; i++)
            {
                var category = shopDaily.ToDayRevenue.Details[i];

                column = 0;
                row++;
                //column++;
                //worksheet.Cells[row, column].Value = i + 1;

                column++;
                worksheet.Cells[row, column].Value = category.Type;

                column++;
                worksheet.Cells[row, column].Value = category.TotalUnit;
                worksheet.Cells[row, column].Style.Numberformat.Format = "###,###,##0";

                column++;
                worksheet.Cells[row, column].Value = category.TotalMoney;
                worksheet.Cells[row, column].Style.Numberformat.Format = "###,###,##0";


            }

            column = 0;
            row++;
            //column++;
            //worksheet.Cells[row, column].Value = i + 1;

            column++;
            worksheet.Cells[row, column].Value = "Tổng";


            column++;
            worksheet.Cells[row, column].Value = shopDaily.ToDayRevenue.Details.Sum(x => x.TotalUnit);
            worksheet.Cells[row, column].Style.Numberformat.Format = "###,###,##0";

            var totalRenueIncludePromotion = shopDaily.ToDayRevenue.Details.Sum(x => x.TotalMoney);

            column++;
            worksheet.Cells[row, column].Value = totalRenueIncludePromotion;
            worksheet.Cells[row, column].Style.Numberformat.Format = "###,###,##0";


            column = 0;
            row++;
            column++;
            worksheet.Cells[row, column].Value = "Khuyến mãi";


            column++;
            worksheet.Cells[row, column].Value = "";

            column++;
            worksheet.Cells[row, column].Value = ( totalRenueIncludePromotion - (decimal)shopDaily.ToDayRevenue.TotalMoney);
            worksheet.Cells[row, column].Style.Numberformat.Format = "###,###,##0";


            column = 0;
            row++;
            column++;
            worksheet.Cells[row, column].Value = "Tổng sau Khuyến mãi";


            column++;
            worksheet.Cells[row, column].Value = shopDaily.ToDayRevenue.Details.Sum(x => x.TotalUnit);
            worksheet.Cells[row, column].Style.Numberformat.Format = "###,###,##0";

            column++;
            worksheet.Cells[row, column].Value = shopDaily.ToDayRevenue.TotalMoney;
            worksheet.Cells[row, column].Style.Numberformat.Format = "###,###,##0";

            column = 0;
            row++;
            column++;
            worksheet.Cells[row, column].Value = "Tổng doanh thu bánh";


            column++;
            worksheet.Cells[row, column].Value = "";

            column++;
            var cakeFinalAmount = shopDaily.ToDayRevenue.Details.Where(x => x.TypeId == 20 || x.TypeId == 13)
                .Sum(x => x.TotalMoney);
            worksheet.Cells[row, column].Value = cakeFinalAmount;
            worksheet.Cells[row, column].Style.Numberformat.Format = "###,###,##0";

            column = 0;
            row++;
            column++;
            worksheet.Cells[row, column].Value = "Tổng doanh thu nước";


            column++;
            worksheet.Cells[row, column].Value = "";

            column++;
            
            worksheet.Cells[row, column].Value = (decimal)shopDaily.ToDayRevenue.TotalMoney - cakeFinalAmount;
            worksheet.Cells[row, column].Style.Numberformat.Format = "###,###,##0";

            worksheet.Cells.AutoFitColumns(200);
        }
        private void ExportMonthyCakeOrDrinkCategoryRevenue(ExcelPackage xlPackage, MonthlyRevenueFilterByCakeOrDrinkCategoryModel monthlyRevenue)
        {
            var worksheet = xlPackage.Workbook.Worksheets.Add("Doanh thu");
            int row = 1;

            worksheet.Cells[$"A{row}:F{row}"].Merge = true;
            worksheet.Cells[$"A{row}:F{row}"].Value = $"THEO DÕI DOANH THU COF THÁNG {monthlyRevenue.Month}/{monthlyRevenue.Year}";
            worksheet.Cells[$"A{row}:F{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[$"A{row}:F{row}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            worksheet.Cells[$"A{row}:F{row}"].Style.Font.Size = 18;
            row++;

            var properties = new List<string>
                    {
                        "Năm",
                        "Tháng",
                        "Ngày",
                        "Doanh thu tổng",
                        "DT nước PM",
                        "DT bánh PM"
                    };
            int maxCol = properties.Count;

            for (int i = 0; i < maxCol; i++)
            {
                worksheet.Cells[row, i + 1].Value = properties[i];
                worksheet.Cells[row, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

            worksheet.Cells[row, 1, row, maxCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[row, 1, row, maxCol].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
            worksheet.Cells[row, 1, row, maxCol].Style.Font.Bold = true;
            worksheet.Cells[row, 1, row, maxCol].Style.WrapText = true;
            worksheet.Cells[row, 1, row, maxCol].Style.Font.Size = 11;
            worksheet.Cells[row, 1, row, maxCol].Style.Font.Name = "Calibri";
            worksheet.Cells[row, 1, row, maxCol].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            worksheet.Cells[row, 1, row, maxCol].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //worksheet.View.FreezePanes(2, 1);

            var column = 0;
            for (int i = 0; i < monthlyRevenue.DayRevenues.Count; i++)
            {
                var day = monthlyRevenue.DayRevenues[i];

                column = 0;
                row++;

                column++;
                worksheet.Cells[row, column].Value = monthlyRevenue.Year;
                worksheet.Cells[row, column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                column++;
                worksheet.Cells[row, column].Value = monthlyRevenue.Month;
                worksheet.Cells[row, column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                column++;
                worksheet.Cells[row, column].Value = day.Day;
                worksheet.Cells[row, column].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                column++;
                worksheet.Cells[row, column].Value = day.TotalRevenue;
                worksheet.Cells[row, column].Style.Numberformat.Format = "###,###,##0";

                column++;
                worksheet.Cells[row, column].Value = day.DrinkRevenue;
                worksheet.Cells[row, column].Style.Numberformat.Format = "###,###,##0";

                column++;
                worksheet.Cells[row, column].Value = day.CakeRevenue;
                worksheet.Cells[row, column].Style.Numberformat.Format = "###,###,##0";

            }

            row++;
            column = 0;
            worksheet.Cells[$"A{row}:C{row}"].Merge = true;
            worksheet.Cells[$"A{row}:C{row}"].Value = "Tổng";
            worksheet.Cells[$"A{row}:C{row}"].Style.Font.Bold = true;

            worksheet.Cells[$"A{row}:C{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[$"A{row}:C{row}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            worksheet.Cells[$"D{row}"].Value = monthlyRevenue.TotalRevenue;
            worksheet.Cells[$"D{row}"].Style.Numberformat.Format = "###,###,##0";
            worksheet.Cells[$"D{row}"].Style.Font.Bold = true;



            worksheet.Cells[$"E{row}"].Value = monthlyRevenue.DrinkRevenue;
            worksheet.Cells[$"E{row}"].Style.Numberformat.Format = "###,###,##0";
            worksheet.Cells[$"E{row}"].Style.Font.Bold = true;

            worksheet.Cells[$"F{row}"].Value = monthlyRevenue.CakeRevenue;
            worksheet.Cells[$"F{row}"].Style.Numberformat.Format = "###,###,##0";
            worksheet.Cells[$"F{row}"].Style.Font.Bold = true;

            worksheet.Cells.AutoFitColumns(200);
        }

    }
}
