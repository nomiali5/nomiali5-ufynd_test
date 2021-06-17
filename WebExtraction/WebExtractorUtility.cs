using Jsonize;
using Jsonize.Abstractions.Configuration;
using Jsonize.Abstractions.Interfaces;
using Jsonize.Abstractions.Models;
using Jsonize.Parser;
using Jsonize.Serializer;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebExtraction
{
    public class WebExtractorUtility
    {
        public static async Task<string> ExtractJson(string html = "")
        {
            using (var client = new HttpClient())
            {
                JsonizeParser parser = new JsonizeParser(new JsonizeParserConfiguration()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    EmptyTextNodeHandling = EmptyTextNodeHandling.Ignore,
                    TextTrimHandling = TextTrimHandling.Trim,
                    ClassAttributeHandling = ClassAttributeHandling.Array
                });

                JsonizeSerializer serializer = new JsonizeSerializer();

                Jsonizer jsonizer = new Jsonizer(parser, serializer);
                return await jsonizer.ParseToStringAsync(html);
            }
        }
    }
}
