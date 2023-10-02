using System;
using System.Collections.Generic;

namespace NetUI.WinForms
{
	public class Channel
	{
		public Channel()
		{
			this.Sections = new List<TimeSection>();
			this.Id = Guid.NewGuid().ToString();
		}
		public string Id { get; private set; }
		public string Title { get; set; }
		public List<TimeSection> Sections { get; set; }
		public object Tag { get; set; }
	}
}
