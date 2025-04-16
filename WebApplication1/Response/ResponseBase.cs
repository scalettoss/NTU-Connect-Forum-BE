using System.Text.Json.Serialization;

namespace ForumBE.Response
{
    public class ResponseBase
    {
        public int Status { get; set; }
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object Data { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object Pagination { get; set; }

        public ResponseBase(int status, string message, object data = default, object pagination = default)
        {
            Status = status;
            Message = message;
            Data = data;
            Pagination = pagination;
        }

        public static ResponseBase Success(object data, object pagination = null)
        {
            return new ResponseBase(200, "Success", data, pagination);
        }

        public static ResponseBase Fail(string message, int statusCode = 400)
        {
            return new ResponseBase(statusCode, message);
        }
    }

}
