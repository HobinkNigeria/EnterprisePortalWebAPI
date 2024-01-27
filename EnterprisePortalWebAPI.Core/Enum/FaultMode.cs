namespace EnterprisePortalWebAPI.Core.Enum
{
    public enum FaultMode
    {
        CLIENT_INVALID_ARGUMENT,
        SERVER,
        TIMEOUT,
        REQUESTED_ENTITY_NOT_FOUND,
        INVALID_OBJECT_STATE,
        UNAUTHORIZED,
        GATEWAY_ERROR,
        RECORD_EXISTS,
        DUPLICATE_RECORD, 
        AUTHENTICATION_FAILED,
        NONE
    }
}
