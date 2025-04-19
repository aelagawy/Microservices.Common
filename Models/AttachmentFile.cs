namespace Microservices.Common.Models
{
    public class AttachmentFile
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public string Type { get; set; }
        public long LastModified { get; set; }
        public string Base64Type { get; set; }
        public string Base64String { get; set; }
        public byte[] ByteArrayContent { get { return Convert.FromBase64String(Base64String); } }

        public AttachmentFile MapAttachmentFile(string fileUrl, byte[] byteArray)
        {
            Name = fileUrl.Substring(fileUrl.LastIndexOf('/') + 1);
            Base64String = Convert.ToBase64String(byteArray);
            Type = GetFileType(Base64String);
            Base64Type = $"data:{Type};base64,";
            return this;
        }

        private static string GetFileType(string base64String)
        {
            return base64String[..5].ToUpper() switch
            {
                "IVBOR" => "image/png",
                "/9J/4" => "image/jpg",
                "AAAAF" => "video/mp4",
                "JVBER" => "application/pdf",
                "AAABA" => "image/vnd.microsoft.icon",
                "UMFYI" => "application/vnd.rar",
                "E1XYD" => "application/rtf",
                "U1PKC" => "text/plain",
                "MQOWM" or "77U/M" => "srt",
                _ => string.Empty,
            };
        }
    }
}