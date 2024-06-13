using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Center.Models;
using Center.ModelsDTO;
using Avalonia.Controls;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace Center.Services
{
    public class Report(DateOnly[] range)
    {
        public Document PdfDoc { get; set; } = null!;

        public DateOnly[] DateRange { get; set; } = range;

        public List<IssuingMagazine> DispensingDrugList { get; set; } = null!;

        public List<ReceivingMagazine> ReceivingDrugList { get; set; } = null!;

        public void GetReportData()
        {
            try
            {
                DispensingDrugList = DBCall.GetIssuingMagazinData(DateRange);
                ReceivingDrugList = DBCall.GetReceivingMagazinData(DateRange);
            }
            catch
            {
                throw new Exception();
            }
        }

        private void CreateTitle(string text)
        {
            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.TTF");
            BaseFont fgBaseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            var title = new Paragraph(text, new Font(fgBaseFont, 14, Font.BOLD, new BaseColor(0, 0, 0)))
            {
                SpacingAfter = 25f,
                SpacingBefore = 25f,
                Alignment = Element.ALIGN_CENTER
            };
            PdfDoc.Add(title);
        }

        public async Task<string> CreateReport(ReportWindow window)
        {
            var storageProvider = window.StorageProvider;
            var result = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Сохранить отчет как",
                FileTypeChoices =
                [
                    new FilePickerFileType("PDF")
                    {
                        Patterns = ["*.pdf"]
                    }
                ],
                DefaultExtension = "pdf"
            });

            if (result != null)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                PdfDoc = new Document(PageSize.A4, 40f, 40f, 60f, 60f);

                try
                {
                    using var fs = await result.OpenWriteAsync();
                    PdfWriter.GetInstance(PdfDoc, fs);
                    PdfDoc.Open();

                    string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.TTF");
                    BaseFont fgBaseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    Font fgFont = new(fgBaseFont, 14, Font.NORMAL, new BaseColor(0, 0, 0));

                    var spacer = new Paragraph("")
                    {
                        SpacingAfter = 10f,
                        SpacingBefore = 10f
                    };
                    PdfDoc.Add(spacer);

                    var title = new Paragraph($"ОТЧЕТ ПРОДАЖИ И ПРИЕМА ЖУРНАЛОВ \r ОТ {DateRange[0]} ДО {DateRange[1]}", new Font(fgBaseFont, 14, Font.BOLD, new BaseColor(0, 0, 0)))
                    {
                        SpacingAfter = 25f,
                        Alignment = Element.ALIGN_CENTER
                    };
                    PdfDoc.Add(title);

                    AddHeaderTable(fgFont);
                    AddIssuingMagazinTable(fgFont);
                    AddReceivingDrugTable(fgFont);

                    PdfDoc.Close();
                    return "Отчет создан";
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return "";
        }

        private void AddHeaderTable(Font fgFont)
        {
            var headerTable = new PdfPTable([.75f])
            {
                HorizontalAlignment = 0,
                WidthPercentage = 75,
                DefaultCell = { MinimumHeight = 22f },
            };

            PdfPCell cell = new(new Phrase($"Дата: {DateTime.Now:dd.MM.yyyy}", fgFont))
            {
                Border = Rectangle.NO_BORDER
            };
            headerTable.AddCell(cell);

            cell.Phrase = new Phrase($"Подготовил: {CurrentUser.Worker.FullName}", fgFont);
            headerTable.AddCell(cell);


            PdfDoc.Add(headerTable);
        }

        private void AddIssuingMagazinTable(Font fgFont)
        {
            CreateTitle("Статистика проданных журналов за период");
            if (DispensingDrugList.Count > 0)
            {
                var dispensingDrugsTable = new PdfPTable([1.5f, .75f, .75f])
                {
                    HorizontalAlignment = 0,
                    WidthPercentage = 100,
                    DefaultCell = { MinimumHeight = 22f },
                };
                PdfPCell title1 = new(new Phrase("Название", fgFont));
                dispensingDrugsTable.AddCell(title1);

                PdfPCell title2 = new(new Phrase("Количество", fgFont));
                dispensingDrugsTable.AddCell(title2);

                PdfPCell title3 = new(new Phrase("Итоговая сумма", fgFont));
                dispensingDrugsTable.AddCell(title3);


                foreach (var item in DispensingDrugList)
                {
                    PdfPCell cell = new(new Phrase(item.Magazin.Title, fgFont));
                    dispensingDrugsTable.AddCell(cell);

                    PdfPCell cell2 = new(new Phrase(item.Count.ToString(), fgFont));
                    dispensingDrugsTable.AddCell(cell2);

                    PdfPCell cell3 = new(new Phrase($"{item.TotalSum} ₽", fgFont));
                    dispensingDrugsTable.AddCell(cell3);
                }
                PdfDoc.Add(dispensingDrugsTable);


                var headerTable = new PdfPTable([.75f])
                {
                    HorizontalAlignment = 0,
                    WidthPercentage = 75,
                    DefaultCell = { MinimumHeight = 22f },
                };

                var spacer = new Paragraph("")
                {
                    SpacingAfter = 10f,
                    SpacingBefore = 10f
                };
                PdfDoc.Add(spacer);

                PdfPCell cell1 = new(new Phrase($"Итого за период: {DispensingDrugList.Sum(x => x.TotalSum)} ₽", fgFont))
                {
                    Border = Rectangle.NO_BORDER
                };
                headerTable.AddCell(cell1);

                PdfDoc.Add(headerTable);
            }
            else
            {
                var title = new Paragraph($"Отсутствует", fgFont)
                {
                    SpacingAfter = 25f,
                    Alignment = Element.ALIGN_CENTER
                };
                PdfDoc.Add(title);
            }
        }

        private void AddReceivingDrugTable(Font fgFont)
        {
            CreateTitle("Статистика полученных журналов за период");
            if (ReceivingDrugList.Count > 0)
            {
                var receivingMagazinTable = new PdfPTable([1f, .75f])
                {
                    HorizontalAlignment = 0,
                    WidthPercentage = 100,
                    DefaultCell = { MinimumHeight = 22f },
                };

                PdfPCell title1 = new(new Phrase("Название", fgFont));
                receivingMagazinTable.AddCell(title1);

                PdfPCell title2 = new(new Phrase("Количество", fgFont));
                receivingMagazinTable.AddCell(title2);

                foreach (var item in ReceivingDrugList)
                {
                    PdfPCell cell = new(new Phrase(item.Magazin.Title, fgFont));
                    receivingMagazinTable.AddCell(cell);

                    PdfPCell cell2 = new(new Phrase(item.Count.ToString(), fgFont));
                    receivingMagazinTable.AddCell(cell2);
                }
                PdfDoc.Add(receivingMagazinTable);
            }
            else
            {
                var title = new Paragraph($"Отсутствует", fgFont)
                {
                    SpacingAfter = 25f,
                    Alignment = Element.ALIGN_CENTER
                };
                PdfDoc.Add(title);
            }
        }
    }
}
