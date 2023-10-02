using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NetUI.WinForms
{
	public class ProgressBarEx
	{
		private ProgressBarEx()
		{
		}

		internal ProgressBarEx(UCTimeBar parent, Control canvas)
		{
			this._parent = parent;
			this._canvas = canvas;
		}

		public int MinNum
		{
			get
			{
				return this._minNum;
			}
			set
			{
				this._minNum = value;
			}
		}

		public int MaxNum
		{
			get
			{
				return this._maxNum;
			}
			set
			{
				this._maxNum = value;
			}
		}
		public int Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				if (value > this._maxNum)
				{
					value = this._maxNum;
				}
				this._value = value;
				this._canvas.Invalidate();
				if (this.ValueChanged != null)
				{
					this.ValueChanged(this._parent, value);
				}
			}
		}
		public int ValuePercent
		{
			get
			{
				return this._valuePercent;
			}
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				if (value > 100)
				{
					value = 100;
				}
				this._valuePercent = value;
				double num = (double)(this._maxNum - this._minNum) * (double)this._valuePercent / 100.0;
				this._value = (int)num;
				this._canvas.Invalidate();
				if (this.ValueChanged != null)
				{
					this.ValueChanged(this._parent, value);
				}
			}
		}
		public Color ColorBegin
		{
			get
			{
				return this._cleBegin;
			}
			set
			{
				this._cleBegin = value;
			}
		}
		public Color ColorEnd
		{
			get
			{
				return this._clrEnd;
			}
			set
			{
				this._clrEnd = value;
			}
		}

		public int BorderWidth
		{
			get
			{
				return this._borderWidth;
			}
			set
			{
				this._borderWidth = value;
			}
		}

		public Color BorderColor
		{
			get
			{
				return this._borderColor;
			}
			set
			{
				this._borderColor = value;
			}
		}

		public bool ShowProgessInfo
		{
			get
			{
				return this._showProgessInfo;
			}
			set
			{
				this._showProgessInfo = value;
			}
		}

		internal bool ShowTitle
		{
			get
			{
				return this._showTitle;
			}
			set
			{
				this._showTitle = value;
			}
		}
		public Color ProgressInfoColor
		{
			set
			{
				this._brushProgressInfo = new SolidBrush(value);
			}
		}
		internal string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				this._title = value;
			}
		}

		public Brush BrushTitle
		{
			get
			{
				return this._titleBrush;
			}
			set
			{
				this._titleBrush = value;
			}
		}

		public event ProgressBarValueChangedEventHandler ValueChanged;

		internal void ReDraw(Graphics g)
		{
			if (this._canvas == null)
			{
				return;
			}
			if (this._maxNum > this._minNum)
			{
				Brush brush = new LinearGradientBrush(new Point(0, 0), new Point(this._canvas.Width, 0), this._cleBegin, this._clrEnd);
				double num = (double)(this._value - this._minNum) / (double)(this._maxNum - this._minNum);
				double num2 = (double)(this._canvas.Width - this._borderWidth * 2) * num;
				Rectangle rect = new Rectangle(this._borderWidth, this._borderWidth, (int)num2, this._canvas.Height - this._borderWidth * 2);
				g.FillRectangle(brush, rect);
				if (this._showProgessInfo)
				{
					g.DrawString(string.Format("{0}/100", (int)(num * 100.0)), new Font(new FontFamily("arial"), 7f, FontStyle.Regular), this._brushProgressInfo, new PointF((float)(this._canvas.Width - 42 - this._borderWidth), (float)this._borderWidth));
				}
			}
			if (this._borderWidth > 0 && this._borderWidth < this._canvas.Height / 2 && this._borderWidth < this._canvas.Width / 2)
			{
				g.DrawRectangle(new Pen(this._borderColor, (float)this._borderWidth), 0, 0, this._canvas.Width - this._borderWidth, this._canvas.Height - this._borderWidth);
			}
			if (this._showTitle)
			{
				g.DrawString(this._title, new Font(new FontFamily("arial"), 7f, FontStyle.Regular), this._titleBrush, new PointF(2f, 0f));
			}
		}

		private Control _canvas;

		private UCTimeBar _parent;

		private int _minNum;

		private int _maxNum = 100;

		private int _value;

		private int _valuePercent;

		private Color _cleBegin = Color.Orange;

		private Color _clrEnd = Color.OrangeRed;

		private int _borderWidth = 1;

		private Color _borderColor = Color.Red;

		private bool _showProgessInfo;

		private bool _showTitle;

		private Brush _brushProgressInfo = new SolidBrush(Color.Silver);

		private string _title = "";

		private Brush _titleBrush = new SolidBrush(Color.OrangeRed);
	}
}
