using Building.Domain.Entity;
using Microsoft.Office.Interop.Word;
using Range = Microsoft.Office.Interop.Word.Range;

namespace Building.Helper
{
    public  class WordDocument
    {
        public string SuplyerName { get; set; }
        public string SuplyerINN { get; set; }
        public string SuplyerKPP { get; set; }
        public string CompanyName { get; set; }
        public string CompanyINN { get; set; }
        public string CompanyKPP { get; set; }
        public string SnabName { get; set; }
        public string SnabPhone { get; set; }
        public string Email { get; set; }
        public string BuildingName { get; set; }
        public string BuildingAddress { get; set; }
        public string ResponseDate { get; set; }
        public IEnumerable<QueryDetail> QueryDetails { get; set; }

        public void Create()
        {
            Application application = new Application();
            Document document = application.Documents.Add(Visible: true);
            Paragraph paragraph = document.Paragraphs.Add();
            document.Paragraphs.Format.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
            document.Paragraphs.Format.LineSpacingRule = WdLineSpacing.wdLineSpaceMultiple;

            Range range = document.Paragraphs[1].Range;
            range.Text = string.Format("Кому: {0}\vИнн: {1} КПП: {2}\v\v От: {3}\vИнн: {4} КПП: {5}\vКонтактное лицо: {6}\vТелефон: {7}\v Email: {8}"
                , SuplyerName, SuplyerINN, SuplyerKPP, CompanyName, CompanyINN, CompanyKPP, SnabName, SnabPhone, Email);
            range.Font.Name = "Times New Roman";
            range.Font.Size = 14;

            Paragraph paragraph1 = document.Paragraphs.Add();
            document.Paragraphs[2].Format.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            Range range2 = document.Paragraphs[2].Range;
            range2.Text = "Запрос о предоставлении ценовой информации";
            range2.Font.Name = "Times New Roman";
            range2.Font.Size = 20;
            range2.Font.Bold = 0;

            Paragraph paragraph2 = document.Paragraphs.Add();
            document.Paragraphs[3].Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            Range range3 = document.Paragraphs[3].Range;
            range3.Text = "      Прошу предоставить информацию по ценам и условиям поставки на указанные ниже материалы и услуги, а также включить в счет стоимость доставки. В случае, если возможна отсрочка платежа, указать количество дней отсрочки и необходимый процент предоплаты.";
            range3.Font.Name = "Times New Roman";
            range3.Font.Size = 14;
            Paragraph paragraph4 = document.Paragraphs.Add();
            document.Paragraphs[4].Format.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
            Range range4 = document.Paragraphs[4].Range;
            range4.Text = string.Format("       Объект: {0}\n       Адрес доставки: {1}\n       Срок подачи предложения до: {2}\n",BuildingName,BuildingAddress, ResponseDate);
            range4.Font.Name = "Times New Roman";
            range4.Font.Size = 14;


            Paragraph paragraph5 = document.Paragraphs.Add();
            Range range5 = document.Paragraphs[7].Range;
            Table table = document.Tables.Add(range5, 1, 5, WdTableFieldSeparator.wdSeparateByCommas);
            range5 = table.Cell(1, 1).Range;
            range5.Text = "Каталог";
            range5.Font.Name = "Times New Roman";
            range5.Font.Size = 14;
            range5.Font.Bold = 1;
            range5 = table.Cell(1, 2).Range;
            range5.Text = "Материал";
            range5.Font.Name = "Times New Roman";
            range5.Font.Size = 14;
            range5.Font.Bold = 1;
            range5 = table.Cell(1, 3).Range;
            range5.Text = "Номенклатура";
            range5.Font.Name = "Times New Roman";
            range5.Font.Size = 14;
            range5.Font.Bold = 1;
            range5 = table.Cell(1, 4).Range;
            range5.Text = "Количество";
            range5.Font.Name = "Times New Roman";
            range5.Font.Size = 14;
            range5.Font.Bold = 1;
            range5 = table.Cell(1, 5).Range;
            range5.Text = "Ед.измерения";
            range5.Font.Name = "Times New Roman";
            range5.Font.Size = 14;
            range5.Font.Bold = 1;
            int i = 2;
            foreach (var item in QueryDetails)
            {
                table.Rows.Add();
                range5 = table.Cell(i, 1).Range;
                range5.Text = item.Material.Catalog.Name;
                range5.Font.Name = "Times New Roman";
                range5.Font.Size = 14;
                range5.Font.Bold = 0;
                range5 = table.Cell(i, 2).Range;
                range5.Text = item.Material.Name;
                range5.Font.Name = "Times New Roman";
                range5.Font.Size = 14;
                range5.Font.Bold = 0;
                range5 = table.Cell(i, 3).Range;
                range5.Text = item.Name;
                range5.Font.Name = "Times New Roman";
                range5.Font.Size = 14;
                range5.Font.Bold = 0;
                range5 = table.Cell(i, 4).Range;
                range5.Text = item.Count.ToString();
                range5.Font.Name = "Times New Roman";
                range5.Font.Size = 14;
                range5.Font.Bold = 0;
                range5 = table.Cell(i, 5).Range;
                range5.Text = item.Measure;
                range5.Font.Name = "Times New Roman";
                range5.Font.Size = 14;
                range5.Font.Bold = 0;
                i++;
            }

            document.SaveAs(@"C:\Users\агроном\Desktop\test.docx");
            document.Close();

            application.Quit();



        }
    }
}
