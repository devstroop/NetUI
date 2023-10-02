using System;

namespace vc.controls.timeline.winform
{
	// Token: 0x0200000C RID: 12
	internal class TimeLine
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00002CAC File Offset: 0x00000EAC
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00002CB4 File Offset: 0x00000EB4
		public int X
		{
			get
			{
				return this._x;
			}
			set
			{
				this._x = value;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00002CBD File Offset: 0x00000EBD
		// (set) Token: 0x06000087 RID: 135 RVA: 0x00002CC5 File Offset: 0x00000EC5
		public DateTime DateTime
		{
			get
			{
				return this._dateTime;
			}
			set
			{
				if (this._dateTime != value)
				{
					this._dateTime = value;
					if (this.DateTimeChanged != null)
					{
						this.DateTimeChanged(this, value);
					}
				}
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000088 RID: 136 RVA: 0x00002CF4 File Offset: 0x00000EF4
		// (remove) Token: 0x06000089 RID: 137 RVA: 0x00002D2C File Offset: 0x00000F2C
		public event TimeLine.DateTimeChangedEventHandler DateTimeChanged;

		// Token: 0x04000044 RID: 68
		private int _x;

		// Token: 0x04000045 RID: 69
		private DateTime _dateTime = DateTime.Now;

		// Token: 0x02000011 RID: 17
		// (Invoke) Token: 0x0600010E RID: 270
		public delegate void DateTimeChangedEventHandler(TimeLine sender, DateTime value);
	}
}
