using System;
using System.Drawing;

namespace NetUI.WinForms
{
    public class TimeBarOptions
    {
        internal TimeBarOptions(UCTimeBar owner)
        {
            this._owner = owner;
            this._titleSize = this._owner.CreateGraphics().MeasureString("测试", this._fontTitle);
            this._scaleValueSize = this._owner.CreateGraphics().MeasureString("22", this._scalcFontBold);
        }

        private TimeBarOptions()
        {
        }

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

        public int UpRoleY
        {
            get
            {
                return this._upRoleY;
            }
        }

        public int BottomRoleY
        {
            get
            {
                return this._owner.Height - this._bottomRoleBottomSpace;
            }
        }

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

        private UCTimeBar _owner;

        internal string _title = "";
        internal SizeF _titleSize;
        internal SizeF _scaleValueSize;
        internal bool _isMouseStayDelayEventEnable;
        internal bool _selected;
        internal bool _drawSelectedBorder;
        internal int _scaleHeight = 3;
        internal int _mousePointOnRoleHeight;
        internal Brush _titleBrush = Brushes.Yellow;
        internal Pen _penSelected = new Pen(Color.RoyalBlue, 1f);
        internal Pen _penRole = new Pen(Color.Black, 1f);
        internal Color _roleBodyColor1 = Color.Silver;
        internal Color _roleBodyColor2 = Color.FromArgb(64, 64, 64);
        internal Brush _brushRoleBody = new SolidBrush(Color.FromArgb(64, 64, 64));
        internal Pen _penUnitPoint = new Pen(Color.Black, 2f);
        internal Pen _penUnitPointSub = new Pen(Color.Black, 1f);
        internal Brush _brushScalcString = new SolidBrush(Color.Black);
        internal Pen _penCurrentTimeLine = new Pen(Color.Black, 1f);
        internal Brush _brushCurrentTimeLineInfo = new SolidBrush(Color.Black);
        internal Pen _penMouseLine = new Pen(Color.Red, 1f);
        internal Brush _brushMouseString = Brushes.Black;
        internal Brush _brushSection = Brushes.RoyalBlue;
        internal Font _scalcFont = new Font("Arial", 8f);
        internal Font _scalcFontBold = new Font("Arial", 8f, FontStyle.Bold);
        internal Font _fontTitle = new Font(new FontFamily("Arial"), 8f, FontStyle.Regular);
        internal int _upRoleY = 20;
        internal int _bottomRoleBottomSpace;
        internal bool _showMouseTimeLine;
        internal bool _showUpScale = true;
        internal bool _showBottomScale;
        internal bool _showScale = true;
        internal bool _showUpScaleValue = true;
        internal bool _showBottomScaleValue = true;
        internal bool _showCurrentTimeLineString = true;
        internal bool _showTitle = true;
        internal int _mouseScaleHeight = 5;
        internal Pen _penMouseScale = Pens.Red;
        internal int _maxSectionLevel;
        internal int _subSectionSpace = 1;
        internal int _sectionPaddingY = 1;
        private bool _currentTimeLineDragable;
        private Color _sectionColor1 = Color.FromArgb(100, Color.Red);
        private Color _sectionColor2 = Color.FromArgb(180, Color.Red);
    }
}
