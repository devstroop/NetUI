using System;

namespace NetUI.WinForms
{
	internal class TimeLine
	{
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

		public event TimeLine.DateTimeChangedEventHandler DateTimeChanged;

		private int _x;

		private DateTime _dateTime = DateTime.Now;

		public delegate void DateTimeChangedEventHandler(TimeLine sender, DateTime value);
	}
}
