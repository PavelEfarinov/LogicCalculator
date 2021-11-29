using System;
using Google.Protobuf.WellKnownTypes;
using Logic.Proto;

namespace Logic.Utils
{
	public static class NullableHelper
	{
		public static bool? ToBool(this NullableBool nullableBool)
		{
			switch (nullableBool.KindCase)
			{
				case NullableBool.KindOneofCase.Data:
					return nullableBool.Data;
				case NullableBool.KindOneofCase.Null:
					return null;
				default:
					throw new Exception("Wrong NullableBool format");
			}
		}

		public static NullableBool ToNullable(this bool? nullableBool)
		{
			return nullableBool.HasValue
				? new NullableBool()
				{
					Data = nullableBool.Value
				}
				: new NullableBool()
				{
					Null = NullValue.NullValue
				};
		}

		public static string ToPrintString(this NullableBool nullableBool)
		{
			switch (nullableBool.KindCase)
			{
				case NullableBool.KindOneofCase.Data:
					return nullableBool.Data.ToString();
				case NullableBool.KindOneofCase.Null:
					return "Null";
				default:
					throw new Exception("Wrong NullableBool format");
			}
		}
	}
}
