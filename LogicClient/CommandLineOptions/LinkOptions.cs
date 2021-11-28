using CommandLine;

namespace LogicClient.CommandLineOptions
{
	[Verb("link")]
	public class LinkOptions
	{
		[Value(0, Required = true, MetaName = "SourceNode", HelpText = "Name of the node to get value from")]
		public string SourceNodeName { get; set; }

		[Value(1, Required = true, MetaName = "DestinationNode", HelpText = "Name of the node to get value to")]
		public string DestinationNodeName { get; set; }
	}
}
