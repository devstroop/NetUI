using System;
using System.Drawing;

namespace vc.controls.timeline.winform
{
	// Token: 0x0200000A RID: 10
	public class TimeBarOptions
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00002588 File Offset: 0x00000788
		internal TimeBarOptions(UCTimeBar owner)
		{
			this._owner = owner;
			this._titleSize = this._owner.CreateGraphics().MeasureString("测试", this._fontTitle);
			this._scaleValueSize = this._owner.CreateGraphics().MeasureString("22", this._scalcFontBold);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000027A8 File Offset: 0x000009A8
		private TimeBarOptions()
		{
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000042 RID: 66 RVA: 0x0000297D File Offset: 0x00000B7D
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00002985 File Offset: 0x00000B85
		public bool CurrentTimeLineDragable
		{
			get
			{
				return this._currentTimeLineDragable;
			}
			set
			{
				this._currentTimeLineDragable = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000044 RID: 68 RVA: 0x0000298E File Offset: 0x00000B8E
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00002996 File Offset: 0x00000B96
		public Font FontTitle
		{
			get
			{
				return this._fontTitle;
			}
			set
			{
				this._fontTitle = value;
				this._titleSize = this._owner.CreateGraphics().MeasureString("测试", this._fontTitle);
				this._owner.Invalidate();
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000046 RID: 70 RVA: 0x000029CB File Offset: 0x00000BCB
		// (set) Token: 0x06000047 RID: 71 RVA: 0x000029D3 File Offset: 0x00000BD3
		public Font FontScalcMain
		{
			get
			{
				return this._scalcFontBold;
			}
			set
			{
				this._scalcFontBold = value;
				this._scaleValueSize = this._owner.CreateGraphics().MeasureString("22", this._scalcFontBold);
				this._owner.Invalidate();
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00002A08 File Offset: 0x00000C08
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00002A10 File Offset: 0x00000C10
		public Font FontScalcSub
		{
			get
			{
				return this._scalcFont;
			}
			set
			{
				this._scalcFont = value;
				this._owner.Invalidate();
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002A24 File Offset: 0x00000C24
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00002A2C File Offset: 0x00000C2C
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

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002A35 File Offset: 0x00000C35
		// (set) Token: 0x0600004D RID: 77 RVA: 0x00002A3D File Offset: 0x00000C3D
		public Pen PenRoleLine
		{
			get
			{
				return this._penRole;
			}
			set
			{
				this._penRole = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002A46 File Offset: 0x00000C46
		// (set) Token: 0x0600004F RID: 79 RVA: 0x00002A4E File Offset: 0x00000C4E
		public Pen PenUnitMain
		{
			get
			{
				return this._penUnitPoint;
			}
			set
			{
				this._penUnitPoint = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00002A57 File Offset: 0x00000C57
		// (set) Token: 0x06000051 RID: 81 RVA: 0x00002A5F File Offset: 0x00000C5F
		public Pen PenUnitSub
		{
			get
			{
				return this._penUnitPointSub;
			}
			set
			{
				this._penUnitPointSub = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00002A68 File Offset: 0x00000C68
		// (set) Token: 0x06000053 RID: 83 RVA: 0x00002A70 File Offset: 0x00000C70
		public Pen PenCurrentTimeLine
		{
			get
			{
				return this._penCurrentTimeLine;
			}
			set
			{
				this._penCurrentTimeLine = value;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00002A79 File Offset: 0x00000C79
		// (set) Token: 0x06000055 RID: 85 RVA: 0x00002A81 File Offset: 0x00000C81
		public Brush BrushCurrentTimeLineInfo
		{
			get
			{
				return this._brushCurrentTimeLineInfo;
			}
			set
			{
				this._brushCurrentTimeLineInfo = value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00002A8A File Offset: 0x00000C8A
		// (set) Token: 0x06000057 RID: 87 RVA: 0x00002A92 File Offset: 0x00000C92
		public Pen PenMouseTimeLine
		{
			get
			{
				return this._penMouseLine;
			}
			set
			{
				this._penMouseLine = value;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00002A9B File Offset: 0x00000C9B
		// (set) Token: 0x06000059 RID: 89 RVA: 0x00002AA3 File Offset: 0x00000CA3
		public Brush BrushMouseTimeLineInfo
		{
			get
			{
				return this._brushMouseString;
			}
			set
			{
				this._brushMouseString = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002AAC File Offset: 0x00000CAC
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00002AB4 File Offset: 0x00000CB4
		public Color SectionColor1
		{
			get
			{
				return this._sectionColor1;
			}
			set
			{
				this._sectionColor1 = value;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600005C RID: 92 RVA: 0x00002ABD File Offset: 0x00000CBD
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00002AC5 File Offset: 0x00000CC5
		public Color SectionColor2
		{
			get
			{
				return this._sectionColor2;
			}
			set
			{
				this._sectionColor2 = value;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600005E RID: 94 RVA: 0x00002ACE File Offset: 0x00000CCE
		// (set) Token: 0x0600005F RID: 95 RVA: 0x00002AD6 File Offset: 0x00000CD6
		public Brush BrushScalcValue
		{
			get
			{
				return this._brushScalcString;
			}
			set
			{
				this._brushScalcString = value;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00002ADF File Offset: 0x00000CDF
		// (set) Token: 0x06000061 RID: 97 RVA: 0x00002AE7 File Offset: 0x00000CE7
		public Color RoleBodyColor1
		{
			get
			{
				return this._roleBodyColor1;
			}
			set
			{
				this._roleBodyColor1 = value;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000062 RID: 98 RVA: 0x00002AF0 File Offset: 0x00000CF0
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00002AF8 File Offset: 0x00000CF8
		public Color RoleBodyColor2
		{
			get
			{
				return this._roleBodyColor2;
			}
			set
			{
				this._roleBodyColor2 = value;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00002B01 File Offset: 0x00000D01
		// (set) Token: 0x06000065 RID: 101 RVA: 0x00002B09 File Offset: 0x00000D09
		public Pen PenSelected
		{
			get
			{
				return this._penSelected;
			}
			set
			{
				this._penSelected = value;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00002B12 File Offset: 0x00000D12
		// (set) Token: 0x06000067 RID: 103 RVA: 0x00002B1A File Offset: 0x00000D1A
		public bool ShowTitle
		{
			get
			{
				return this._showTitle;
			}
			set
			{
				this._showTitle = value;
				this._owner.ProgressBar.ShowTitle = value;
				this._owner.Invalidate();
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00002B3F File Offset: 0x00000D3F
		// (set) Token: 0x06000069 RID: 105 RVA: 0x00002B47 File Offset: 0x00000D47
		public bool ShowScale
		{
			get
			{
				return this._showScale;
			}
			set
			{
				this._showScale = value;
				this._owner.Invalidate();
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00002B5B File Offset: 0x00000D5B
		// (set) Token: 0x0600006B RID: 107 RVA: 0x00002B63 File Offset: 0x00000D63
		public bool ShowUpScale
		{
			get
			{
				return this._showUpScale;
			}
			set
			{
				this._showUpScale = value;
				this._owner.Invalidate();
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00002B77 File Offset: 0x00000D77
		// (set) Token: 0x0600006D RID: 109 RVA: 0x00002B7F File Offset: 0x00000D7F
		public bool ShowBottomScale
		{
			get
			{
				return this._showBottomScale;
			}
			set
			{
				this._showBottomScale = value;
				this._owner.Invalidate();
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00002B93 File Offset: 0x00000D93
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00002B9B File Offset: 0x00000D9B
		public bool ShowCurrentTimeLineString
		{
			get
			{
				return this._showCurrentTimeLineString;
			}
			set
			{
				this._showCurrentTimeLineString = value;
				this._owner.Invalidate();
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00002BAF File Offset: 0x00000DAF
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00002BB7 File Offset: 0x00000DB7
		public bool ShowUpScaleValue
		{
			get
			{
				return this._showUpScaleValue;
			}
			set
			{
				this._showUpScaleValue = value;
				this._owner.Invalidate();
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00002BCB File Offset: 0x00000DCB
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00002BD3 File Offset: 0x00000DD3
		public bool ShowBottomScaleValue
		{
			get
			{
				return this._showBottomScaleValue;
			}
			set
			{
				this._showBottomScaleValue = value;
				this._owner.Invalidate();
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00002BE7 File Offset: 0x00000DE7
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00002BEF File Offset: 0x00000DEF
		public bool ShowMouseTimeLine
		{
			get
			{
				return this._showMouseTimeLine;
			}
			set
			{
				this._showMouseTimeLine = value;
				this._owner.Invalidate();
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00002C03 File Offset: 0x00000E03
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00002C0B File Offset: 0x00000E0B
		public bool Selected
		{
			get
			{
				return this._selected;
			}
			set
			{
				this._selected = value;
				this._owner.Invalidate();
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00002C1F File Offset: 0x00000E1F
		// (set) Token: 0x06000079 RID: 121 RVA: 0x00002C27 File Offset: 0x00000E27
		public bool DrawSelectedBorder
		{
			get
			{
				return this._drawSelectedBorder;
			}
			set
			{
				this._drawSelectedBorder = value;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00002C30 File Offset: 0x00000E30
		public int UpRoleY
		{
			get
			{
				return this._upRoleY;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00002C38 File Offset: 0x00000E38
		public int BottomRoleY
		{
			get
			{
				return this._owner.Height - this._bottomRoleBottomSpace;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00002C4C File Offset: 0x00000E4C
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00002C54 File Offset: 0x00000E54
		public string Title
		{
			get
			{
				return this._title;
			}
			set
			{
				this._title = value;
				this._owner.ProgressBar.Title = value;
				this._owner.Invalidate();
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00002C79 File Offset: 0x00000E79
		// (set) Token: 0x0600007F RID: 127 RVA: 0x00002C81 File Offset: 0x00000E81
		public bool IsMouseStayDelayEventEnable
		{
			get
			{
				return this._isMouseStayDelayEventEnable;
			}
			set
			{
				this._isMouseStayDelayEventEnable = value;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00002C8A File Offset: 0x00000E8A
		// (set) Token: 0x06000081 RID: 129 RVA: 0x00002C92 File Offset: 0x00000E92
		public int MouseScaleHeight
		{
			get
			{
				return this._mouseScaleHeight;
			}
			set
			{
				this._mouseScaleHeight = value;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00002C9B File Offset: 0x00000E9B
		// (set) Token: 0x06000083 RID: 131 RVA: 0x00002CA3 File Offset: 0x00000EA3
		public Pen PenMouseScale
		{
			get
			{
				return this._penMouseScale;
			}
			set
			{
				this._penMouseScale = value;
			}
		}

		// Token: 0x04000015 RID: 21
		private UCTimeBar _owner;

		// Token: 0x04000016 RID: 22
		internal string _title = "";

		// Token: 0x04000017 RID: 23
		internal SizeF _titleSize;

		// Token: 0x04000018 RID: 24
		internal SizeF _scaleValueSize;

		// Token: 0x04000019 RID: 25
		internal bool _isMouseStayDelayEventEnable;

		// Token: 0x0400001A RID: 26
		internal bool _selected;

		// Token: 0x0400001B RID: 27
		internal bool _drawSelectedBorder;

		// Token: 0x0400001C RID: 28
		internal int _scaleHeight = 3;

		// Token: 0x0400001D RID: 29
		internal int _mousePointOnRoleHeight;

		// Token: 0x0400001E RID: 30
		internal Brush _titleBrush = Brushes.Yellow;

		// Token: 0x0400001F RID: 31
		internal Pen _penSelected = new Pen(Color.RoyalBlue, 1f);

		// Token: 0x04000020 RID: 32
		internal Pen _penRole = new Pen(Color.Black, 1f);

		// Token: 0x04000021 RID: 33
		internal Color _roleBodyColor1 = Color.Silver;

		// Token: 0x04000022 RID: 34
		internal Color _roleBodyColor2 = Color.FromArgb(64, 64, 64);

		// Token: 0x04000023 RID: 35
		internal Brush _brushRoleBody = new SolidBrush(Color.FromArgb(64, 64, 64));

		// Token: 0x04000024 RID: 36
		internal Pen _penUnitPoint = new Pen(Color.Black, 2f);

		// Token: 0x04000025 RID: 37
		internal Pen _penUnitPointSub = new Pen(Color.Black, 1f);

		// Token: 0x04000026 RID: 38
		internal Brush _brushScalcString = new SolidBrush(Color.Black);

		// Token: 0x04000027 RID: 39
		internal Pen _penCurrentTimeLine = new Pen(Color.Black, 1f);

		// Token: 0x04000028 RID: 40
		internal Brush _brushCurrentTimeLineInfo = new SolidBrush(Color.Black);

		// Token: 0x04000029 RID: 41
		internal Pen _penMouseLine = new Pen(Color.Red, 1f);

		// Token: 0x0400002A RID: 42
		internal Brush _brushMouseString = Brushes.Black;

		// Token: 0x0400002B RID: 43
		internal Brush _brushSection = Brushes.RoyalBlue;

		// Token: 0x0400002C RID: 44
		internal Font _scalcFont = new Font("Arial", 8f);

		// Token: 0x0400002D RID: 45
		internal Font _scalcFontBold = new Font("Arial", 8f, FontStyle.Bold);

		// Token: 0x0400002E RID: 46
		internal Font _fontTitle = new Font(new FontFamily("Arial"), 8f, FontStyle.Regular);

		// Token: 0x0400002F RID: 47
		internal int _upRoleY = 20;

		// Token: 0x04000030 RID: 48
		internal int _bottomRoleBottomSpace;

		// Token: 0x04000031 RID: 49
		internal bool _showMouseTimeLine;

		// Token: 0x04000032 RID: 50
		internal bool _showUpScale = true;

		// Token: 0x04000033 RID: 51
		internal bool _showBottomScale;

		// Token: 0x04000034 RID: 52
		internal bool _showScale = true;

		// Token: 0x04000035 RID: 53
		internal bool _showUpScaleValue = true;

		// Token: 0x04000036 RID: 54
		internal bool _showBottomScaleValue = true;

		// Token: 0x04000037 RID: 55
		internal bool _showCurrentTimeLineString = true;

		// Token: 0x04000038 RID: 56
		internal bool _showTitle = true;

		// Token: 0x04000039 RID: 57
		internal int _mouseScaleHeight = 5;

		// Token: 0x0400003A RID: 58
		internal Pen _penMouseScale = Pens.Red;

		// Token: 0x0400003B RID: 59
		internal int _maxSectionLevel;

		// Token: 0x0400003C RID: 60
		internal int _subSectionSpace = 1;

		// Token: 0x0400003D RID: 61
		internal int _sectionPaddingY = 1;

		// Token: 0x0400003E RID: 62
		private bool _currentTimeLineDragable;

		// Token: 0x0400003F RID: 63
		private Color _sectionColor1 = Color.FromArgb(100, Color.Red);

		// Token: 0x04000040 RID: 64
		private Color _sectionColor2 = Color.FromArgb(180, Color.Red);
	}
}
