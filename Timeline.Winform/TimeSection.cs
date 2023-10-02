using System;
using System.Collections.Generic;
using System.Drawing;

namespace vc.controls.timeline.winform
{
	// Token: 0x0200000D RID: 13
	public class TimeSection
	{
		// Token: 0x0600008B RID: 139 RVA: 0x00002D74 File Offset: 0x00000F74
		public TimeSection()
		{
			this._guid = Guid.NewGuid().ToString();
			this._dateTimeBegin = DateTime.Now.Date;
			this._dateTimeEnd = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600008C RID: 140 RVA: 0x00002DE6 File Offset: 0x00000FE6
		// (set) Token: 0x0600008D RID: 141 RVA: 0x00002DEE File Offset: 0x00000FEE
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

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00002DF7 File Offset: 0x00000FF7
		// (set) Token: 0x0600008F RID: 143 RVA: 0x00002DFF File Offset: 0x00000FFF
		public TimeSection Parent { get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000090 RID: 144 RVA: 0x00002E08 File Offset: 0x00001008
		// (set) Token: 0x06000091 RID: 145 RVA: 0x00002E10 File Offset: 0x00001010
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

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00002E19 File Offset: 0x00001019
		// (set) Token: 0x06000093 RID: 147 RVA: 0x00002E21 File Offset: 0x00001021
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

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00002E2A File Offset: 0x0000102A
		// (set) Token: 0x06000095 RID: 149 RVA: 0x00002E32 File Offset: 0x00001032
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

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00002E3B File Offset: 0x0000103B
		// (set) Token: 0x06000097 RID: 151 RVA: 0x00002E43 File Offset: 0x00001043
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

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00002E4C File Offset: 0x0000104C
		// (set) Token: 0x06000099 RID: 153 RVA: 0x00002E54 File Offset: 0x00001054
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

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00002E5D File Offset: 0x0000105D
		// (set) Token: 0x0600009B RID: 155 RVA: 0x00002E65 File Offset: 0x00001065
		public Channel Channel { get; internal set; }

		// Token: 0x04000047 RID: 71
		private string _guid = "";

		// Token: 0x04000049 RID: 73
		private DateTime _dateTimeBegin;

		// Token: 0x0400004A RID: 74
		private DateTime _dateTimeEnd;

		// Token: 0x0400004B RID: 75
		private Color? _color;

		// Token: 0x0400004C RID: 76
		private List<TimeSection> _sections = new List<TimeSection>();

		// Token: 0x0400004D RID: 77
		private object _tag;
	}
}
