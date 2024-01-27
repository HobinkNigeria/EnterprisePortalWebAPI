using EnterprisePortalWebAPI.Core.Enum;

namespace EnterprisePortalWebAPI.Core.DTO
{
    public class ErrorResponse
    {
        public string? ResponseCode { get; set; }
        public string? ResponseDescription { get; set; }

        public static T Create<T>(FaultMode fault, string errorCode, string errorMessage) where T : BasicResponse, new()
        {
            return new T
            {
                IsSuccessful = false,
                FaultType = fault,
                Error = new ErrorResponse
                {
                    ResponseCode = errorCode,
                    ResponseDescription = errorMessage
                }
            };
        }

        public override string ToString()
        {
            return $"{ResponseCode} - {ResponseDescription}";
        }
    }
}
