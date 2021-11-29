using System.IO;
using Logic.Dto.Models;
using Newtonsoft.Json.Linq;

namespace Logic.Client.Helpers
{
	public class SessionFileHelper
	{
		private readonly string _sessionPath;
		public SessionFileHelper(string sessionPath)
		{
			this._sessionPath = sessionPath;
		}

		public UserSessionDto GetSessionIfExists()
		{
			var fileContents = File.ReadAllText(_sessionPath);
			return JObject.Parse(fileContents).ToObject<UserSessionDto>();
		}

		public void SaveSession(UserSessionDto sessionDto)
		{
			File.WriteAllText(_sessionPath, JObject.FromObject(sessionDto).ToString());
		}

	}
}
