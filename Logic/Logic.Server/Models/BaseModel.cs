using Newtonsoft.Json;

namespace Logic.Server.Models
{
	public class BaseModel
	{
		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
