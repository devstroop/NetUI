using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace vc.controls.timeline.winform
{
	// Token: 0x02000009 RID: 9
	public class ProgressBarEx
	{
		// Token: 0x06000022 RID: 34 RVA: 0x000020CC File Offset: 0x000002CC
		private ProgressBarEx()
		{
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000213C File Offset: 0x0000033C
		internal ProgressBarEx(UCTimeBar parent, Control canvas)
		{
			this._parent = parent;
			this._canvas = canvas;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000021B8 File Offset: 0x000003B8
		// (set) Token: 0x06000025 RID: 37 RVA: 0x000021C0 File Offset: 0x000003C0
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

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000021C9 File Offset: 0x000003C9
		// (set) Token: 0x06000027 RID: 39 RVA: 0x000021D1 File Offset: 0x000003D1
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

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000028 RID: 40 RVA: 0x000021DA File Offset: 0x000003DA
		// (set) Token: 0x06000029 RID: 41 RVA: 0x000021E4 File Offset: 0x000003E4
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

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002235 File Offset: 0x00000435
		// (set) Token: 0x0600002B RID: 43 RVA: 0x00002240 File Offset: 0x00000440
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

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002C RID: 44 RVA: 0x000022B2 File Offset: 0x000004B2
		// (set) Token: 0x0600002D RID: 45 RVA: 0x000022BA File Offset: 0x000004BA
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

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002E RID: 46 RVA: 0x000022C3 File Offset: 0x000004C3
		// (set) Token: 0x0600002F RID: 47 RVA: 0x000022CB File Offset: 0x000004CB
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

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000022D4 File Offset: 0x000004D4
		// (set) Token: 0x06000031 RID: 49 RVA: 0x000022DC File Offset: 0x000004DC
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

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000022E5 File Offset: 0x000004E5
		// (set) Token: 0x06000033 RID: 51 RVA: 0x000022ED File Offset: 0x000004ED
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

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000034 RID: 52 RVA: 0x000022F6 File Offset: 0x000004F6
		// (set) Token: 0x06000035 RID: 53 RVA: 0x000022FE File Offset: 0x000004FE
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

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002307 File Offset: 0x00000507
		// (set) Token: 0x06000037 RID: 55 RVA: 0x0000230F File Offset: 0x0000050F
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

		// Token: 0x1700000F RID: 15
		// (set) Token: 0x06000038 RID: 56 RVA: 0x00002318 File Offset: 0x00000518
		public Color ProgressInfoColor
		{
			set
			{
				this._brushProgressInfo = new SolidBrush(value);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002326 File Offset: 0x00000526
		// (set) Token: 0x0600003A RID: 58 RVA: 0x0000232E File Offset: 0x0000052E
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

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600003B RID: 59 RVA: 0x00002337 File Offset: 0x00000537
		// (set) Token: 0x0600003C RID: 60 RVA: 0x0000233F File Offset: 0x0000053F
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

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600003D RID: 61 RVA: 0x00002348 File Offset: 0x00000548
		// (remove) Token: 0x0600003E RID: 62 RVA: 0x00002380 File Offset: 0x00000580
		public event ProgressBarValueChangedEventHandler ValueChanged;

		// Token: 0x0600003F RID: 63 RVA: 0x000023B8 File Offset: 0x000005B8
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

		// Token: 0x04000005 RID: 5
		private Control _canvas;

		// Token: 0x04000006 RID: 6
		private UCTimeBar _parent;

		// Token: 0x04000007 RID: 7
		private int _minNum;

		// Token: 0x04000008 RID: 8
		private int _maxNum = 100;

		// Token: 0x04000009 RID: 9
		private int _value;

		// Token: 0x0400000A RID: 10
		private int _valuePercent;

		// Token: 0x0400000B RID: 11
		private Color _cleBegin = Color.Orange;

		// Token: 0x0400000C RID: 12
		private Color _clrEnd = Color.OrangeRed;

		// Token: 0x0400000D RID: 13
		private int _borderWidth = 1;

		// Token: 0x0400000E RID: 14
		private Color _borderColor = Color.Red;

		// Token: 0x0400000F RID: 15
		private bool _showProgessInfo;

		// Token: 0x04000010 RID: 16
		private bool _showTitle;

		// Token: 0x04000011 RID: 17
		private Brush _brushProgressInfo = new SolidBrush(Color.Silver);

		// Token: 0x04000012 RID: 18
		private string _title = "";

		// Token: 0x04000013 RID: 19
		private Brush _titleBrush = new SolidBrush(Color.OrangeRed);
	}
}
