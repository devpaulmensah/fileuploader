namespace FileUploader.API.Extensions;

public static class FileExtensions
{
    public static class DocumentTypes
    {
        public const string Passport = "passport";
        public const string GhanaCard = "ghana_card";
        public const string VoterId = "voter_id";
        public const string Nhis = "nhis";
        public const string BirthCertificate = "birth_certificate";
        public const string TestDocument= "TestDocument";
    }

    public static class FolderName
    {
        private const string Passport = "Passports";
        private const string GhanaCard = "GhanaCards";
        private const string VotersId = "VotersId";
        private const string Nhis = "NHIS";
        private const string BirthCertificate = "BirthCertificates";
        private const string Others = "Others";
        private const string TestDocuments = "TestDocuments";

        public static string GetFolderName(string documentType)
        {
            return documentType switch
            {
                DocumentTypes.Passport => Passport,
                DocumentTypes.GhanaCard => GhanaCard,
                DocumentTypes.VoterId => VotersId,
                DocumentTypes.Nhis => Nhis,
                DocumentTypes.BirthCertificate => BirthCertificate,
                DocumentTypes.TestDocument => TestDocuments,
                _ => Others
            };
        }
            
    }
    
    public static string GetFileExtension(this IFormFile file) => 
        Path.GetExtension(file.FileName);

    public static bool IsAllowed(this string extension)
    {
        var filesConfig = ConfigurationExtension.GetFilesConfig();
        return filesConfig.AllowedFilesExtensions.Contains(extension);
    }
}