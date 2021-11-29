using CommandLine;

namespace Logic.Client.CommandLineOptions
{
	[Verb("set")]
	public class SetOptions
	{
		[Value(0, Required = true, MetaName = "NodeName", HelpText = "Name of the new node")]
		public string NodeName { get; set; }

		[Value(1, Required = true, MetaName = "NodeValue", HelpText = "Value of the node to set")]
		public bool? Value { get; set; }
	}
}
