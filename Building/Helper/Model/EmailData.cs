namespace Building.Helper.Model
{
    public class EmailData
    {
        public const string Title = "Заявка на запрос";

        public const string Sender = "rustemsh11@yandex.ru";
        public string Recipient { get; set; }
        public string Text { get; set; }
        public string CompanyName { get; set; }
        public string Attachment { get; set; }
    }
}
