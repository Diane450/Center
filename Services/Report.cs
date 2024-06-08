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

namespace Center.Services
{
    public class Report
    {
        public Document PdfDoc { get; set; } = null!;

        public DateOnly[] DateRange { get; set; }

        public List<IssuingMagazine> DispensingDrugList { get; set; } = null!;

        public List<ReceivingMagazine> ReceivingDrugList { get; set; } = null!;


        public Report(DateOnly[] range)
        {
            DateRange = range;
        }

        public void GetReportData()
        {
            try
            {
                DispensingDrugList = DBCall.GetIssuingMagazinData(DateRange);
                ReceivingDrugList = DBCall.GetReceivingMagazinData(DateRange);
            }
            catch (Exception ex)
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

        public async Task CreateReport(ReportWindow window)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Сохранить отчет как",
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter { Name = "PDF", Extensions = { "pdf" } }
                }
            };
            var result = await saveFileDialog.ShowAsync(window);
            
            if (result != null)
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                PdfDoc = new Document(PageSize.A4, 40f, 40f, 60f, 60f);

                try
                {
                    using (var fs = new FileStream(result, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        PdfWriter.GetInstance(PdfDoc, fs);
                        PdfDoc.Open();

                        string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "times.TTF");
                        BaseFont fgBaseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                        Font fgFont = new Font(fgBaseFont, 14, Font.NORMAL, new BaseColor(0, 0, 0));

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
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error creating PDF report", ex);
                }
            }
        }

        private void AddHeaderTable(Font fgFont)
        {
            var headerTable = new PdfPTable(new[] { .75f })
            {
                HorizontalAlignment = 0,
                WidthPercentage = 75,
                DefaultCell = { MinimumHeight = 22f },
            };

            PdfPCell cell = new PdfPCell(new Phrase($"Дата: {DateTime.Now.ToString("dd.MM.yyyy")}", fgFont));
            cell.Border = Rectangle.NO_BORDER;
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
                var dispensingDrugsTable = new PdfPTable(new[] { 1.5f, .75f, .75f })
                {
                    HorizontalAlignment = 0,
                    WidthPercentage = 100,
                    DefaultCell = { MinimumHeight = 22f },
                };
                PdfPCell title1 = new PdfPCell(new Phrase("Название", fgFont));
                dispensingDrugsTable.AddCell(title1);

                PdfPCell title2 = new PdfPCell(new Phrase("Количество", fgFont));
                dispensingDrugsTable.AddCell(title2);

                PdfPCell title3 = new PdfPCell(new Phrase("Итоговая сумма", fgFont));
                dispensingDrugsTable.AddCell(title3);


                foreach (var item in DispensingDrugList)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(item.Magazin.Title, fgFont));
                    dispensingDrugsTable.AddCell(cell);

                    PdfPCell cell2 = new PdfPCell(new Phrase(item.Count.ToString(), fgFont));
                    dispensingDrugsTable.AddCell(cell2);

                    PdfPCell cell3 = new PdfPCell(new Phrase($"{item.TotalSum} ₽", fgFont));
                    dispensingDrugsTable.AddCell(cell3);
                }
                PdfDoc.Add(dispensingDrugsTable);


                var headerTable = new PdfPTable(new[] { .75f })
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

                PdfPCell cell1 = new PdfPCell(new Phrase($"Итого за период: {DispensingDrugList.Sum(x => x.TotalSum)} ₽", fgFont));
                cell1.Border = Rectangle.NO_BORDER;
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
                var receivingMagazinTable = new PdfPTable(new[] { 1f, .75f })
                {
                    HorizontalAlignment = 0,
                    WidthPercentage = 100,
                    DefaultCell = { MinimumHeight = 22f },
                };

                PdfPCell title1 = new PdfPCell(new Phrase("Название", fgFont));
                receivingMagazinTable.AddCell(title1);

                PdfPCell title2 = new PdfPCell(new Phrase("Количество", fgFont));
                receivingMagazinTable.AddCell(title2);

                foreach (var item in ReceivingDrugList)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(item.Magazin.Title, fgFont));
                    receivingMagazinTable.AddCell(cell);

                    PdfPCell cell2 = new PdfPCell(new Phrase(item.Count.ToString(), fgFont));
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
