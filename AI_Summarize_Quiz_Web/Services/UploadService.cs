namespace AI_Summarize_Quiz_Web.Services
{
    public class UploadService
    {
        private readonly string _targetFolderPath;

        public UploadService(string targetFolderPath)
        {
            _targetFolderPath = targetFolderPath;
        }

        public async Task<string> UploadPdfAsync(IFormFile file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            if (!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("File must be a PDF.", nameof(file));

            // Ensuring the target directory exists
            Directory.CreateDirectory(_targetFolderPath);

            // Create a unique file name to avoid conflicts
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(_targetFolderPath, uniqueFileName);

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath; // Returns the path where the file was saved
        }
    }
}
