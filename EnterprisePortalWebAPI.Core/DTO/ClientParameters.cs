namespace EnterprisePortalWebAPI.Core.DTO
{
	public class ClientParameters
	{
		const int maxPageSize = 1000000;
		public int PageNumber { get; set; } = 1;
		private int _pageSize = 10000;
		public int PageSize
		{
			get
			{
				return _pageSize;
			}
			set
			{
				_pageSize = (value > maxPageSize) ? maxPageSize : value;
			}
		}
	}
}
