using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GutenbergApp.Json
{
    public class BookItemJson
    {
        public int id { get; set; }

        public string title { get; set; }

        public List<AuthorJson> authors { get; set; }

        public List<AuthorJson> translators { get; set; }

        public List<string> subjects { get; set; }

        public List<string> bookshelves { get; set; }

        public List<string> languages { get; set; }

        public bool copyright { get; set; }

        public string media_type { get; set; }

        public FormatsJson formats { get; set; }

        public int download_count { get; set; }
    }

    public class AuthorJson
    {
        public string name { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? birth_year { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? death_year { get; set; }
    }

    public class FormatsJson
    {
        [JsonPropertyName("application/x-mobipocket-ebook")]
        public string applicationxmobipocketebook { get; set; }

        [JsonPropertyName("application/epub+zip")]
        public string applicationepubzip { get; set; }

        [JsonPropertyName("text/html")]
        public string texthtml { get; set; }

        [JsonPropertyName("application/octet-stream")]
        public string applicationoctetstream { get; set; }

        [JsonPropertyName("image/jpeg")]
        public string imagejpeg { get; set; }

        [JsonPropertyName("text/plain")]
        public string textplain { get; set; }

        [JsonPropertyName("text/plain; charsetus-ascii")]
        public string textplaincharsetusascii { get; set; }

        [JsonPropertyName("application/rdf+xml")]
        public string applicationrdfxml { get; set; }

        [JsonPropertyName("text/html; charsetutf-8")]
        public string texthtmlcharsetutf8 { get; set; }

        [JsonPropertyName("text/plain; charsetutf-8")]
        public string textplaincharsetutf8 { get; set; }

        [JsonPropertyName("text/html; charsetiso-8859-1")]
        public string texthtmlcharsetiso88591 { get; set; }

        [JsonPropertyName("text/plain; charsetiso-8859-1")]
        public string textplaincharsetiso88591 { get; set; }
    }
}
