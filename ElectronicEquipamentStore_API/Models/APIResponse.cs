using System.Net;

namespace ElectronicEquipamentStore_API.Models
{
    public class APIResponse
    {
        public HttpStatusCode statusCode { get; set; }
        public bool IsSuccessful { get; set; }
        public List<string> ErrorMessages { get; set; }

        public object Result { get; set; }
    }
}
