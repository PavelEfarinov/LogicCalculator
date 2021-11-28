using Grpc.Net.Client;

namespace LogicClient.Grpc.Factories
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
