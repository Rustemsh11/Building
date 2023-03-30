using Leaf.xNet;
using AngleSharp.Html.Parser;
using AngleSharp.Html.Dom;
using Building.Helper.Model;

namespace Building.Helper
{
    public class StroyPriceParsing
    {
        private const string LINK = "https://stroyprice.ru/kazan/";
        public static Dictionary<string, string> CatalogNameAndLink = new Dictionary<string, string>(); // Коллекция с ключом назнванием каталога и со знаением ссылки этого каталога
        public static Dictionary<string, string> MaterialNameAndLink = new Dictionary<string, string>(); // Коллекция с ключом назнванием материала и со знаением ссылки этого материала


        /// <summary>
        /// заполняем коллекцию CatalogNameAndLink с главной страницы сайта
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetAllCatalogLink()
        {
            HtmlParser htmlParser = new HtmlParser();

            var page = htmlParser.ParseDocument(GetStroyPricePage(LINK));

            ChekDictionaryKey(CatalogNameAndLink, page);


            return CatalogNameAndLink;
        }
        private static void ChekDictionaryKey(Dictionary<string, string> verifiableDictionary, IHtmlDocument page)
        {
            foreach (var item in page.QuerySelectorAll("a.island.island_shadow.catalog-section__item"))
            {
                if (verifiableDictionary.ContainsKey(item.QuerySelector("p").TextContent))
                {
                    continue;
                }
                verifiableDictionary.Add(item.QuerySelector("p").TextContent, item.GetAttribute("href"));

            }

        }
        /// <summary>
        /// переходим в нужый каталог и заполняем коллекцию MaterialNameAndLink с материалами нужного каталога
        /// </summary>
        /// <param name="catalogName">название каталога</param>
        /// <returns></returns>
        private static Dictionary<string, string> GetAllMaterialLink(string catalogName)
        {
            string getMaterialLink = null;
            foreach (var item in CatalogNameAndLink)
            {

                if (item.Key == catalogName)
                {
                    getMaterialLink = GetStroyPricePage("https://stroyprice.ru" + item.Value);
                    break;
                }
            }
            HtmlParser htmlParser = new HtmlParser();
            var page = htmlParser.ParseDocument(getMaterialLink);

            ChekDictionaryKey(MaterialNameAndLink, page);

            return MaterialNameAndLink;
        }

        /// <summary>
        /// получаем ссылку на страницу со всеми товарами нужного материала
        /// </summary>
        /// <param name="catalogName">название каталога</param>
        /// <param name="materialName">название материала</param>
        /// <returns></returns>
        public static string GetParsingLink(string catalogName, string materialName)
        {
            GetAllCatalogLink();
            GetAllMaterialLink(catalogName);
            string link = null;
            foreach (var item in MaterialNameAndLink)
            {

                if (item.Key == materialName)
                {
                    link = string.Format("https://stroyprice.ru{0}", item.Value);
                    break;
                }
            }
            return link;
        }
        public static string GetStroyPricePage(string link)
        {
            Leaf.xNet.HttpRequest response = new Leaf.xNet.HttpRequest();
            return response.Get(link).ToString();
        }
        public static IList<ParsingData> ParsTovar(string response)
        {
            HtmlParser htmlParser = new HtmlParser();
            var doc = htmlParser.ParseDocument(response);

            List<ParsingData> parsingData = new List<ParsingData>();


            foreach (var item in doc.QuerySelectorAll("div.companies-item.companies-item_modern.island"))
            {

                var tt = item.OuterHtml;

                string s = "";
                if (item.QuerySelector("div.companies-item__open-wrp>.companies-item__open-c>div>div>span") != null)
                {
                    s = item.QuerySelector("div.companies-item__open-wrp>.companies-item__open-c>div>div>span").TextContent;
                }

                parsingData.Add(new ParsingData()
                {
                    Name = item.QuerySelector("div.companies-item__open-wrp>.companies-item__open-c>a.link.link_muted.text.text_md.text_bold.companies-item__open-inline.modal-orders-capture__btn-modal").TextContent,
                    CompanyName = item.QuerySelector("div.companies-item__open-wrp>.companies-item__open-c>.companies-item__open-line>a>span.text").TextContent,
                    RatingCompany = item.QuerySelector("div.companies-item__open-wrp>.companies-item__open-c>.companies-item__open-line>.companies-item__open-line>p").TextContent,
                    Price = item.QuerySelector("div.companies-item__open-wrp>.companies-item__open-r>a").TextContent,
                    Location = item.QuerySelector("div.companies-item__open-wrp>.companies-item__open-r>.companies-item__open-line").GetAttribute("title"),
                    Comment = s,
                    Image = item.QuerySelector("div.companies-item__open-wrp>.companies-item__open-l>.gallery-compact>.gallery-compact__wrp>a").GetAttribute("href"),
                    UrlToCompany = "https://stroyprice.ru/" + item.QuerySelector("div.companies-item__open-wrp>.companies-item__open-c>.companies-item__open-line>a").GetAttribute("href"),
                    //Phone=item.QuerySelector("div.modal-container>.modal-container__wrp>.modal-container__cell>.modal.modal_size_middle.modal-form.phone-call__modal>.modal__content.modal-form__content>.modal-form__wrp>.modal-form__form-side>.modal-form__form>.phone-call__phone-desc>a>span>.phone-call__phone-number").TextContent
                });
            }

            return parsingData;
        }
    }
    //{
    //    private const string LINK = "https://stroyprice.ru/kazan/";
    //    public static Dictionary<string, string> CatalogNameAndLink = new Dictionary<string, string>(); // Коллекция с ключом назнванием каталога и со знаением ссылки этого каталога
    //    public static Dictionary<string, string> MaterialNameAndLink = new Dictionary<string, string>(); // Коллекция с ключом назнванием материала и со знаением ссылки этого материала

    //    public StroyPriceParsing()
    //    {
    //        //GetAllCatalogLink();
    //    }
    //    /// <summary>
    //    /// заполняем коллекцию CatalogNameAndLink с главной страницы сайта
    //    /// </summary>
    //    /// <returns></returns>
    //    public static Dictionary<string, string> GetAllCatalogLink()
    //    {
    //        HtmlParser htmlParser = new HtmlParser();

    //        var doc = htmlParser.ParseDocument(GetStroyPrice(LINK));


    //        foreach (var item in doc.QuerySelectorAll("a.island.island_shadow.catalog-section__item"))
    //        {
    //            if (CatalogNameAndLink.ContainsKey(item.QuerySelector("p").TextContent))
    //            {
    //                continue;
    //            }
    //            CatalogNameAndLink.Add(item.QuerySelector("p").TextContent, item.GetAttribute("href"));

    //        }
    //        return CatalogNameAndLink;
    //    }

    //    /// <summary>
    //    /// переходим в нужый каталог и заполняем коллекцию MaterialNameAndLink с материалами нужного каталога
    //    /// </summary>
    //    /// <param name="catalogName">название каталога</param>
    //    /// <returns></returns>
    //    private static Dictionary<string, string> GetAllMaterialLink(string catalogName)
    //    {
    //        string getMaterialLink = null;
    //        foreach (var item in CatalogNameAndLink)
    //        {

    //            if(item.Key== catalogName)
    //            {
    //                getMaterialLink = GetStroyPrice("https://stroyprice.ru" + item.Value);
    //                break;
    //            }
    //        }
    //        HtmlParser htmlParser = new HtmlParser();
    //        var doc = htmlParser.ParseDocument(getMaterialLink);
    //        foreach (var item in doc.QuerySelectorAll("a.island.island_shadow.catalog-section__item"))
    //        {
    //            if(MaterialNameAndLink.ContainsKey(item.QuerySelector("p").TextContent))
    //            {
    //                continue;
    //            }
    //            MaterialNameAndLink.Add(item.QuerySelector("p").TextContent, item.GetAttribute("href"));

    //        }
    //        return MaterialNameAndLink;
    //    }

    //    /// <summary>
    //    /// получаем ссылку на страницу со всеми товарами нужного материала
    //    /// </summary>
    //    /// <param name="catalogName">название каталога</param>
    //    /// <param name="materialName">название материала</param>
    //    /// <returns></returns>
    //    public static string GetParsingLink(string catalogName, string materialName)
    //    {
    //        //GetAllCatalogLink();
    //        GetAllMaterialLink(catalogName);
    //        string link = null;
    //        foreach (var item in MaterialNameAndLink)
    //        {

    //            if (item.Key == materialName)
    //            {
    //                link = string.Format("https://stroyprice.ru{0}",item.Value);
    //                break;
    //            }
    //        }
    //        return link;
    //    }
    //    public static string GetStroyPrice(string link)
    //    {
    //        Leaf.xNet.HttpRequest response = new Leaf.xNet.HttpRequest();
    //        return response.Get(link).ToString();
    //    }
    //    public static IList<ParsingData> ParsTovar(string response)
    //    {
    //        HtmlParser htmlParser = new HtmlParser();
    //        var doc = htmlParser.ParseDocument(response);

    //        List<ParsingData> parsingData = new List<ParsingData>();


    //        foreach (var item in doc.QuerySelectorAll("div.companies-item.companies-item_modern.island"))
    //        {

    //            var tt = item.OuterHtml;

    //            string s = "";
    //            if(item.QuerySelector("div.companies-item__open-wrp>.companies-item__open-c>div>div>span")!=null)
    //            {
    //                s = item.QuerySelector("div.companies-item__open-wrp>.companies-item__open-c>div>div>span").TextContent;
    //            }

    //            parsingData.Add(new ParsingData()
    //            {
    //                Name = item.QuerySelector("div.companies-item__open-wrp>.companies-item__open-c>a.link.link_muted.text.text_md.text_bold.companies-item__open-inline.modal-orders-capture__btn-modal").TextContent,
    //                CompanyName = item.QuerySelector("div.companies-item__open-wrp>.companies-item__open-c>.companies-item__open-line>a>span.text").TextContent,
    //                RatingCompany = item.QuerySelector("div.companies-item__open-wrp>.companies-item__open-c>.companies-item__open-line>.companies-item__open-line>p").TextContent,
    //                Price = item.QuerySelector("div.companies-item__open-wrp>.companies-item__open-r>a").TextContent,
    //                Location = item.QuerySelector("div.companies-item__open-wrp>.companies-item__open-r>.companies-item__open-line").GetAttribute("title"),
    //                Comment = s,
    //                Image = item.QuerySelector("div.companies-item__open-wrp>.companies-item__open-l>.gallery-compact>.gallery-compact__wrp>a").GetAttribute("href"),
    //                UrlToCompany = "https://stroyprice.ru/" + item.QuerySelector("div.companies-item__open-wrp>.companies-item__open-c>.companies-item__open-line>a").GetAttribute("href"),
    //                //Phone=item.QuerySelector("div.modal-container>.modal-container__wrp>.modal-container__cell>.modal.modal_size_middle.modal-form.phone-call__modal>.modal__content.modal-form__content>.modal-form__wrp>.modal-form__form-side>.modal-form__form>.phone-call__phone-desc>a>span>.phone-call__phone-number").TextContent
    //            }) ;
    //        }

    //        return parsingData;
    //    }
    //}
}
