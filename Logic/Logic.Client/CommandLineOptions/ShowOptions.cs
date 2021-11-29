using CommandLine;

namespace Logic.Client.CommandLineOptions
{
	[Verb("show")]
	public class ShowOptions
	{
		[Value(0, Required = true, MetaName = "NodeName", HelpText = "Name of the node to show")] public string NodeName { get; set; }
	}
}
