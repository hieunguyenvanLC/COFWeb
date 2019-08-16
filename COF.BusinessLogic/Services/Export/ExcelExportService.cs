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
        byte[] ExportExcelOutsource(PartnerDailyOrderReport partnerReport);
    }
    public class ExcelExportService : IExcelExportService
    {
        public ExcelExportService()
        {

        }
        public byte[] ExportExcelOutsource(PartnerDailyOrderReport partnerReport)
        {
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                using (var xlPackage = new ExcelPackage(stream))
                {
                    foreach (var item in partnerReport.Shops)
                    {
                        ExportDailyOrdersForShop(xlPackage, item);
                    }
                    
                    xlPackage.Save();
                }
                bytes = stream.ToArray();
            }
            return bytes;
        }

        private void ExportDailyOrdersForShop(ExcelPackage xlPackage, ShopDailyReportModel shopDaily)
        {
            var worksheet = xlPackage.Workbook.Worksheets.Add(shopDaily.Name);
            int row = 1;
            var properties = new List<string>
                    {
                        "#",
                        "Thời gian",
                        "Tên khách hàng",
                        "Số điện thoại",
                        "Địa chỉ",
                        "Nhân viên",
                        "Tổng tiền"
                    };
            int maxCol = properties.Count;
            

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
            for (int i = 0; i < shopDaily.Orders.Count; i++)
            {
                var order = shopDaily.Orders[i];

                column = 0;
                row++;
                column++;
                worksheet.Cells[row, column].Value = i + 1;

                column++;
                worksheet.Cells[row, column].Value = order.CreateDateTime;

                column++;
                worksheet.Cells[row, column].Value = order.CustomerName;

                column++;
                worksheet.Cells[row, column].Value = order.PhoneNumber;

                column++;
                worksheet.Cells[row, column].Value = order.Address;

                column++;
                worksheet.Cells[row, column].Value = order.StaffName;

                column++;
                worksheet.Cells[row, column].Value = order.TotalCost;

            }
            worksheet.Cells.AutoFitColumns(200);
        }

    }
}
