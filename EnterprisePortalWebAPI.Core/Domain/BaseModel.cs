using System.Text.Json.Serialization;
namespace EnterprisePortalWebAPI.Core.Domain
{
  public class BaseModel
  {
    public string Id { get; set; } = Guid.NewGuid().ToString();
		[JsonIgnore]
		public DateTime DateCreated { get; set; }
		[JsonIgnore]
		public DateTime DateUpdated { get; set; }
  }
}
