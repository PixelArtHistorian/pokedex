namespace PokedexApi.Infrastructure.Response
{
    public class TranslationResponse
    {
        public Contents contents { get; set; }
        public Success success { get; set; }
    }

    public class Contents
    {
        public string text { get; set; }
        public string translated { get; set; }
        public string translation { get; set; }
    }

    public class Success
    {
        public int total { get; set; }
    }

}
