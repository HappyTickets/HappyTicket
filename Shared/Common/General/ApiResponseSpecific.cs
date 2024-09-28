namespace Shared.Common.General
{
    public class ApiResponseSpecific : ApiResponse
    {
        public string ExcelFileName { get; set; }

        public byte[] ExcelFileContent { get; set; }
    }
}
