using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestData
{
	public class RtHeader
	{
		public int idMsgType { get; set; }
	}

	public class Body
	{

	}

	public class RFooter
	{
		string msgDateTime;
		public string MsgDateTime
		{
			get
			{
				return msgDateTime;
			}
			set
			{
				msgDateTime = value;
			}
		}

		string idKeyField;
		public string idKey
		{
			get { return idKeyField; }
			set { idKeyField = value; }
		}

	}

	public class Request
	{
		public RtHeader RtHeader { get; set; }
		public Body body { get; set; }
		public RFooter RFooter { get; set; }
	}

	public class SignMsg
	{

	}
	
	public class Data
    {
		public Request Request { get; set; }
		public SignMsg SignMsg { get; set; }
    }
}
