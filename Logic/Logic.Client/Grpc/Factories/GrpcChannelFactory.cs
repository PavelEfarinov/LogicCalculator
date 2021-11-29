using Grpc.Net.Client;

namespace Logic.Client.Grpc.Factories
{
	public static class GrpcChannelFactory
	{
		private static string _serverUrl;
		public static void Init(string serverUrl)
		{
			_serverUrl = serverUrl;
		}

		private static GrpcChannel _serverChannel;
		public static GrpcChannel ServerChannel => _serverChannel ??= GrpcChannel.ForAddress(_serverUrl);
	}
}
