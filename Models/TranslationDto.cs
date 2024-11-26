namespace Microservices.Common.Models
{
    public class TranslationDto
    {
        public TranslationDto(string arabic, string english)
        {
            Ar = arabic;
            En = english;
        }
        public string Ar { get; set; }
        public string En { get; set; }
    }
}