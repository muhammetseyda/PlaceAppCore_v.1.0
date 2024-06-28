using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClosedXML.Excel; // ClosedXML kütüphanesini ekleyin
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Mvc;
using PlaceApp.Data;
using PlaceApp.Models;

public class ExcelController : Controller
{
    private readonly AppDbContext _context;

    public ExcelController(AppDbContext context)
    {
        _context = context;
    }
    public IActionResult ExportToExcel()
    {

        // Verileri bir liste olarak toplayın, bu liste tabloyu temsil edecektir.
        var data = _context.Places.ToList();

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Tablo Verileri");

            // Başlık satırını ekleyin
            var headerRow = worksheet.Row(1);
            headerRow.Style.Font.Bold = true;
            headerRow.Cell(1).Value = "Sıra";
            headerRow.Cell(2).Value = "Name";
            headerRow.Cell(3).Value = "Category";
            headerRow.Cell(4).Value = "City";
            headerRow.Cell(5).Value = "Address";
            headerRow.Cell(6).Value = "Went";
            headerRow.Cell(7).Value = "RetryWent";
            headerRow.Cell(8).Value = "Insta Link";
            headerRow.Cell(9).Value = "Google Maps Link";

            // Diğer sütun başlıklarını buraya ekleyin

            // Verileri doldurun
            for (var i = 0; i < data.Count; i++)
            {
                var rowData = data[i];
                var row = worksheet.Row(i + 2); // Başlık satırından sonra başlayın

                row.Cell(1).Value = i + 1; // Sıra sütunu
                row.Cell(2).Value = rowData.Name;
                row.Cell(3).Value = rowData.Category;
                row.Cell(4).Value = rowData.City;
                row.Cell(5).Value = rowData.Address;
                row.Cell(6).Value = rowData.Went;
                row.Cell(7).Value = rowData.RetryWent;
                row.Cell(8).Value = rowData.Link1;
                row.Cell(9).Value = rowData.MapsLink;
                row.Cell(10).Value = rowData.City;
                // Diğer sütunları buraya ekleyin
            }

            // Excel dosyasını bir MemoryStream'e yazın
            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var content = stream.ToArray();

                // İndirme işlemi için dosya içeriğini döndürün
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "tablo.xlsx");
            }
        }
    }
}
