using System;
using System.Collections.Generic;
using System.Drawing;

namespace NetUI.WinForms
{
	public class TimeSection
	{
		public TimeSection()
		{
			this._guid = Guid.NewGuid().ToString();
			this._dateTimeBegin = DateTime.Now.Date;
			this._dateTimeEnd = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
		}

		public string GUID
		{
			get
			{
				return this._guid;
			}
			set
			{
				this._guid = value;
			}
		}

		public TimeSection Parent { get; set; }

		public DateTime DateTimeBegin
		{
			get
			{
				return this._dateTimeBegin;
			}
			set
			{
				this._dateTimeBegin = value;
			}
		}

		public DateTime DateTimeEnd
		{
			get
			{
				return this._dateTimeEnd;
			}
			set
			{
				this._dateTimeEnd = value;
			}
		}

		public Color? Color
		{
			get
			{
				return this._color;
			}
			set
			{
				this._color = value;
			}
		}

		public List<TimeSection> Sections
		{
			get
			{
				return this._sections;
			}
			set
			{
				this._sections = value;
			}
		}
		public object Tag
		{
			get
			{
				return this._tag;
			}
			set
			{
				this._tag = value;
			}
		}

		public Channel Channel { get; internal set; }

		private string _guid = "";

		private DateTime _dateTimeBegin;

		private DateTime _dateTimeEnd;

		private Color? _color;

		private List<TimeSection> _sections = new List<TimeSection>();

		private object _tag;
	}
}
