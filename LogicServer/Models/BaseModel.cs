using Newtonsoft.Json;

namespace LogicServer.Models
{
	public class BaseModel
	{
		public override string ToString()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
