using CommandLine;
using Logic.Dto.Models;

namespace LogicClient.CommandLineOptions
{
	[Verb("add")]
	public class AddOptions
	{
		[Value(0, Required = true, MetaName = "NodeName", HelpText = "Name of the new node")] public string NodeName { get; set; }
		[Value(1, Required = true, MetaName = "NodeType", HelpText = "")] public NodeType NodeType { get; set; }
	}
}
