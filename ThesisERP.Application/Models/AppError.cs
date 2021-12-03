using Newtonsoft.Json;

namespace ThesisERP.Application.Models
{
    public class AppError
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
