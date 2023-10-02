using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using NetUI.WinForms.Properties;

namespace NetUI.WinForms
{
    public sealed class UCTimeBar : UserControl
    {
        [Obsolete]
        public UCTimeBar()
        {
            this.InitializeComponent();
            this.Channels = new List<Channel>();
            this.Channels.Add(new Channel());
            this.DoubleBuffered = true;
            this.Init(TimeUnits.Minute);
        }

        [Obsolete]
        private void Init(TimeUnits timeUnit)
        {
            this._option = new TimeBarOptions(this);
            base.MouseWheel += this.UCTimeBar_MouseWheel;
            this._progressBar = new ProgressBarEx(this, this);
            this._cursorDrag = new Cursor(Resources.hand.GetHicon());
            this._cursorPressed = new Cursor(Resources.handPressed.GetHicon());
            this._curTimeLine.DateTimeChanged += this._curTimeLine_DateTimeChanged;
            this._timeUnit = timeUnit;
            this._dateTimeBegin = UCTimeBar.DateTimeOrigin;
            this.Reset();
            this._curTimeLine = new TimeLine
            {
                X = 100
            };
            this._curTimeLine.DateTime = this.PosToDateTime(this._curTimeLine.X);
            this._timerForMouseStayDelay = new Timer();
            this._timerForMouseStayDelay.Interval = 500;
            this._timerForMouseStayDelay.Tick += this._timerForMouseStayDelay_Tick;
            this._timerForMouseStayDelay.Enabled = true;
        }

        private void Reset()
        {
            this._originPos = 0;
            this._spaceLeft = 0;
            this.CaleParam();
            this._curTimeLine.DateTime = this.PosToDateTime(this._curTimeLine.X);
            this.RefreshBar();
        }

        private void CaleParam()
        {
            if (this._mode == TimeBarModes.Time)
            {
                switch (this._timeUnit)
                {
                    case TimeUnits.Minute:
                        this._secondsPerPix = 1.0;
                        return;
                    case TimeUnits.Hour:
                        this._secondsPerPix = 60.0;
                        return;
                    case TimeUnits.Day:
                        this._secondsPerPix = 1440.0;
                        return;
                    case TimeUnits.Month:
                        this._secondsPerPix = 43200.0;
                        break;
                    case TimeUnits.Year:
                        this._secondsPerPix = 525600.0;
                        break;
                    default:
                        return;
                }
            }
        }

        // Token: 0x060000A0 RID: 160 RVA: 0x000030C6 File Offset: 0x000012C6
        private void DrawProgressBar(PaintEventArgs e)
        {
            this._progressBar.ReDraw(e.Graphics);
        }

        // Token: 0x060000A1 RID: 161 RVA: 0x000030DC File Offset: 0x000012DC
        private void DrawTimeBar(PaintEventArgs e)
        {
            this._option._upRoleY = 0;
            this._option._upRoleY += ((this._option._showScale && this._option._showUpScale) ? ((this._option._showUpScaleValue ? ((int)this._option._scaleValueSize.Height + this._option._scaleHeight) : this._option._scaleHeight) + 2) : 0);
            e.Graphics.DrawLine(this._option._penRole, new Point(0, this._option._upRoleY), new Point(base.Width, this._option._upRoleY));
            e.Graphics.DrawLine(this._option._penRole, new Point(0, this._option.BottomRoleY), new Point(base.Width, this._option.BottomRoleY));
            this._option._brushRoleBody = new LinearGradientBrush(new Point(0, this._option.UpRoleY), new Point(0, this._option.BottomRoleY), this._option._roleBodyColor1, this._option._roleBodyColor2);
            e.Graphics.FillRectangle(this._option._brushRoleBody, new Rectangle(0, this._option._upRoleY + 1, base.Width, this._option.BottomRoleY - this._option.UpRoleY));
            if (this._option._showScale)
            {
                if (this._timeUnit == TimeUnits.Month)
                {
                    this.DrawMonthScale(e.Graphics);
                }
                else
                {
                    this.DrawScale(e.Graphics);
                }
            }
            this.DrawChannels(e.Graphics);
            if (this._option._showTitle && !string.IsNullOrEmpty(this._option._title))
            {
                e.Graphics.DrawString(this._option._title, this._option._fontTitle, this._option._titleBrush, new PointF(1f, (float)(this._option._upRoleY + 2)));
            }
            if (this._isRoleDraging && this._sectionNearPointer != null)
            {
                this._sectionNearPointer.DateTimeBegin = this.PosToDateTime(this._curTimeLine.X).AddSeconds(-this._secondsLeftOfPointerRange);
                this._sectionNearPointer.DateTimeEnd = this.PosToDateTime(this._curTimeLine.X).AddSeconds(this._secondsRightOfPointerRange);
                int x = this.DateTimeToPos(this._sectionNearPointer.DateTimeBegin);
                int x2 = this.DateTimeToPos(this._sectionNearPointer.DateTimeEnd);
                e.Graphics.DrawLine(new Pen(Brushes.Red, 1f)
                {
                    DashStyle = DashStyle.Custom,
                    DashPattern = new float[]
                    {
                        1f,
                        2f
                    }
                }, new Point(x, this._option.UpRoleY + (int)((double)(base.Height - this._option.UpRoleY) * 0.1)), new Point(x, base.Height - (int)((double)(base.Height - this._option.UpRoleY) * 0.1)));
                e.Graphics.DrawLine(new Pen(Brushes.Red, 1f)
                {
                    DashStyle = DashStyle.Custom,
                    DashPattern = new float[]
                    {
                        1f,
                        2f
                    }
                }, new Point(x2, this._option.UpRoleY + (int)((double)(base.Height - this._option.UpRoleY) * 0.1)), new Point(x2, base.Height - (int)((double)(base.Height - this._option.UpRoleY) * 0.1)));
            }
            this.DrawCurrentTimeLine(e.Graphics);
            if (this._option._showMouseTimeLine)
            {
                e.Graphics.DrawString(this._mouseTimeLine.DateTime.ToString("yyyy-MM-dd HH:mm:ss"), SystemFonts.DefaultFont, this._option._brushMouseString, new PointF((float)(this._mouseTimeLine.X - 65), (float)(this._option.BottomRoleY - 12)));
                e.Graphics.DrawLine(this._option._penMouseLine, new Point(this._mouseTimeLine.X, this._option._upRoleY - this._option._mousePointOnRoleHeight), new Point(this._mouseTimeLine.X, base.Height - this._option._bottomRoleBottomSpace));
                return;
            }
            if (this._option._mouseScaleHeight > 0 && this._mousePositionX > 0)
            {
                e.Graphics.DrawLine(this._option._penMouseScale, new Point(this._mousePositionX, this._option._upRoleY - this._option._mouseScaleHeight), new Point(this._mousePositionX, this._option._upRoleY));
            }
        }

        private void DrawChannels(Graphics g)
        {
            if (g == null || this.Channels.Count == 0)
            {
                return;
            }
            this._option._brushSection = new LinearGradientBrush(new Point(0, this._option._upRoleY + 2), new Point(0, this._option.BottomRoleY - 2), this._option.SectionColor1, this._option.SectionColor2);
            int num = (base.Height - this._option.UpRoleY - (this.Channels.Count + 1) * this._sectionSpace) / this.Channels.Count;
            for (int i = 0; i < this.Channels.Count; i++)
            {
                float num2 = (float)((i + 1) * this._sectionSpace + i * num + this._option.UpRoleY);
                for (int j = 0; j < this.Channels[i].Sections.Count; j++)
                {
                    int num3;
                    int num4;
                    if (this.CalePosXForDraw(this.Channels[i].Sections[j].DateTimeBegin, this.Channels[i].Sections[j].DateTimeEnd, out num3, out num4))
                    {
                        Brush brush = (this.Channels[i].Sections[j].Color == null) ? this._option._brushSection : new SolidBrush(this.Channels[i].Sections[j].Color.Value);
                        g.FillRectangle(brush, new Rectangle(num3, (int)num2, num4 - num3, num));
                        int num5 = num - this._option._sectionPaddingY * 2 - (this.Channels[i].Sections[j].Sections.Count - 1) * this._option._subSectionSpace;
                        for (int k = 0; k < this.Channels[i].Sections[j].Sections.Count; k++)
                        {
                            TimeSection timeSection = this.Channels[i].Sections[j].Sections[k];
                            if (timeSection.DateTimeBegin < timeSection.DateTimeEnd && this.CalePosXForDraw(timeSection.DateTimeBegin, timeSection.DateTimeEnd, out num3, out num4))
                            {
                                brush = ((timeSection.Color == null) ? this._option._brushSection : new SolidBrush(timeSection.Color.Value));
                                g.FillRectangle(brush, new Rectangle(num3, (int)num2 + this._option._sectionPaddingY + j * (num5 + this._option._subSectionSpace), num4 - num3, num5));
                            }
                        }
                    }
                }
                g.DrawString(this.Channels[i].Title, this._option._fontTitle, this._option._titleBrush, new PointF(2f, num2 + ((float)num - 12f) / 2f));
            }
        }

        private bool CalePosXForDraw(DateTime begin, DateTime end, out int posBegin, out int posEnd)
        {
            posBegin = (posEnd = 0);
            if (begin >= end)
            {
                return false;
            }
            posBegin = this.DateTimeToPos(begin);
            posEnd = this.DateTimeToPos(end);
            if (posEnd <= 0 || posBegin > base.Width)
            {
                return false;
            }
            if (posBegin < 0)
            {
                posBegin = 0;
            }
            if (posEnd > base.Width)
            {
                posEnd = base.Width;
            }
            if (posBegin == posEnd)
            {
                posEnd = posBegin + 1;
            }
            return true;
        }

        // Token: 0x060000A4 RID: 164 RVA: 0x000039BC File Offset: 0x00001BBC
        private void DrawScale(Graphics g)
        {
            int num = 0;
            int i = this._spaceLeft;
            while (i < base.Width)
            {
                i = this._spaceLeft + num * this._space;
                int scalePosY = this._option._upRoleY - this._option._scaleHeight;
                if (this._option._showUpScale)
                {
                    g.DrawLine(this._option._penUnitPoint, new Point(i, this._option._upRoleY), new Point(i, this._option._upRoleY - this._option._scaleHeight));
                    if (this._timeUnit == TimeUnits.Minute || this._timeUnit == TimeUnits.Hour)
                    {
                        for (int j = 1; j < 6; j++)
                        {
                            g.DrawLine(this._option._penUnitPointSub, new Point(i + j * 10, this._option._upRoleY), new Point(i + j * 10, this._option._upRoleY - this._option._scaleHeight + 1));
                        }
                        for (int k = 1; k < 6; k++)
                        {
                            g.DrawLine(this._option._penUnitPointSub, new Point(i - k * 10, this._option._upRoleY), new Point(i - k * 10, this._option._upRoleY - this._option._scaleHeight + 1));
                        }
                    }
                }
                if (this._option._showBottomScale)
                {
                    g.DrawLine(this._option._penUnitPoint, new Point(i, base.Height - this._option._bottomRoleBottomSpace), new Point(i, base.Height - this._option._bottomRoleBottomSpace + this._option._scaleHeight));
                }
                num++;
                this.DrawScaleValue(g, i, scalePosY);
            }
        }

        // Token: 0x060000A5 RID: 165 RVA: 0x00003B8C File Offset: 0x00001D8C
        private void DrawMonthScale(Graphics g)
        {
            int num = 0;
            int i = this._spaceLeft;
            DateTime dateTime = this.PosToDateTime(0);
            while (i < base.Width)
            {
                if (num == 0)
                {
                    if (this._spaceLeft == 0)
                    {
                        i = 0;
                    }
                    else
                    {
                        int num2 = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
                        i = ((dateTime.Hour == 0) ? ((num2 - dateTime.Day + 1) * 2) : ((num2 - dateTime.Day + 1) * 2 - 1));
                        dateTime = dateTime.AddMonths(1);
                    }
                }
                else
                {
                    int num3 = DateTime.DaysInMonth(dateTime.Year, dateTime.Month) * 2;
                    i += num3;
                    dateTime = dateTime.AddMonths(1);
                }
                int scalePosY = this._option._upRoleY - this._option._scaleHeight;
                if (this._option._showUpScale)
                {
                    g.DrawLine(this._option._penUnitPoint, new Point(i, this._option._upRoleY), new Point(i, this._option._upRoleY - this._option._scaleHeight));
                    g.DrawLine(this._option._penUnitPointSub, new Point(i + 28, this._option._upRoleY), new Point(i + 28, this._option._upRoleY - this._option._scaleHeight + 1));
                }
                num++;
                this.DrawScaleValue(g, i, scalePosY);
            }
        }

        private void DrawScaleValue(Graphics g, int scalePosX, int scalePosY)
        {
            if ((this._option._showUpScale && this._option._showUpScaleValue) || (this._option._showBottomScale && this._option._showBottomScaleValue))
            {
                switch (this._timeUnit)
                {
                    case TimeUnits.Minute:
                        {
                            DateTime dateTime = this.PosToDateTime(scalePosX);
                            if (dateTime.Second == 0)
                            {
                                if (this._option._showUpScale && this._option._showUpScaleValue)
                                {
                                    g.DrawString(dateTime.ToString("HH:mm:ss"), this._option._scalcFont, this._option._brushScalcString, new PointF((float)(scalePosX - 18), (float)(scalePosY - 15)));
                                }
                                if (this._option._showBottomScale)
                                {
                                    bool showBottomScaleValue = this._option._showBottomScaleValue;
                                    return;
                                }
                            }
                            break;
                        }
                    case TimeUnits.Hour:
                        {
                            DateTime dateTime2 = this.PosToDateTime(scalePosX);
                            if (this._option._showUpScale && this._option._showUpScaleValue)
                            {
                                if (dateTime2.Hour == 0)
                                {
                                    g.DrawString(dateTime2.ToString("dd MMM yyyy"), this._option._scalcFontBold, this._option._brushScalcString, new PointF((float)(scalePosX - 16), (float)(scalePosY - 15)));
                                }
                                else
                                {
                                    g.DrawString(dateTime2.ToString("h:mm tt"), this._option._scalcFont, this._option._brushScalcString, new PointF((float)(scalePosX - 9), (float)(scalePosY - 15)));
                                }
                            }
                            if (this._option._showBottomScale)
                            {
                                bool showBottomScaleValue2 = this._option._showBottomScaleValue;
                                return;
                            }
                            break;
                        }
                    case TimeUnits.Day:
                        {
                            DateTime dateTime3 = this.PosToDateTime(scalePosX);
                            if (this._option._showUpScale && this._option._showUpScaleValue)
                            {
                                if (dateTime3.Day == 1)
                                {
                                    g.DrawString(dateTime3.ToString("MMM yyyy"), this._option._scalcFontBold, this._option._brushScalcString, new PointF((float)(scalePosX - 22), (float)(scalePosY - 15)));
                                }
                                else
                                {
                                    g.DrawString(dateTime3.ToString("dd MMM"), this._option._scalcFont, this._option._brushScalcString, new PointF((float)(scalePosX - 18), (float)(scalePosY - 15)));
                                }
                            }
                            if (this._option._showBottomScale)
                            {
                                bool showBottomScaleValue3 = this._option._showBottomScaleValue;
                                return;
                            }
                            break;
                        }
                    case TimeUnits.Month:
                        {
                            DateTime dateTime4 = this.PosToDateTime(scalePosX);
                            if (this._option._showUpScale && this._option._showUpScaleValue)
                            {
                                if (dateTime4.Month == 1)
                                {
                                    g.DrawString(dateTime4.ToString("yyyy"), this._option._scalcFontBold, this._option._brushScalcString, new PointF((float)(scalePosX - 16), (float)(scalePosY - 15)));
                                }
                                else
                                {
                                    g.DrawString(dateTime4.ToString("MMM"), this._option._scalcFont, this._option._brushScalcString, new PointF((float)(scalePosX - 10), (float)(scalePosY - 15)));
                                }
                            }
                            if (this._option._showBottomScale)
                            {
                                bool showBottomScaleValue4 = this._option._showBottomScaleValue;
                            }
                            break;
                        }

                    case TimeUnits.Year:
                        {
                            DateTime dateTime5 = this.PosToDateTime(scalePosX);
                            if (this._option._showUpScale && this._option._showUpScaleValue)
                            {
                                g.DrawString(dateTime5.Year.ToString(), this._option._scalcFontBold, this._option._brushScalcString, new PointF((float)(scalePosX - 22), (float)(scalePosY - 15)));
                            }
                            if (this._option._showBottomScale)
                            {
                                bool showBottomScaleValue5 = this._option._showBottomScaleValue;
                            }
                            break;
                        }

                    default:
                        return;
                }
            }
        }
        private void DrawCurrentTimeLine(Graphics g)
        {
            int x = this._curTimeLine.X;
            Point pt = new Point(x, 0);
            Point pt2 = new Point(x, base.Height);
            g.DrawLine(this._option._penCurrentTimeLine, pt, pt2);
            if (this._option._showCurrentTimeLineString)
            {
                string text = this._curTimeLine.DateTime.ToString("yyyy-MM-dd HH:mm:ss");
                g.MeasureString(text, SystemFonts.DefaultFont);
                g.DrawString(text, SystemFonts.DefaultFont, this._option._brushCurrentTimeLineInfo, new PointF((float)(this._curTimeLine.X - 65), (float)this._option._upRoleY));
            }
        }

        private DateTime PosToDateTime(int pos)
        {
            double value = this._secondsPerPix * (double)(pos - this._originPos);
            return UCTimeBar.DateTimeOrigin.AddSeconds(value);
        }

        private int DateTimeToPos(DateTime dt)
        {
            return (int)((dt - UCTimeBar.DateTimeOrigin).TotalSeconds / this._secondsPerPix) + this._originPos;
        }

        private void SetCurrentTimeValue(DateTime dt)
        {
            TimeSpan timeSpan = dt - this._curTimeLine.DateTime;
            this._curTimeLine.DateTime = dt;
            this.MoveRole(timeSpan.TotalSeconds);
            this.RefreshBar();
        }

        private void MoveRole(double seconds)
        {
            double num = seconds / this._secondsPerPix;
            if (num < 0.0)
            {
                this._spaceLeft -= (int)(num % (double)this._space);
                if (this._spaceLeft >= this._space)
                {
                    this._spaceLeft -= this._space;
                }
                this._originPos -= (int)num;
                return;
            }
            this._spaceLeft -= (int)(num % (double)this._space);
            if (this._spaceLeft <= 0)
            {
                this._spaceLeft = this._space + this._spaceLeft;
            }
            this._originPos -= (int)num;
        }

        private List<TimeSection> GetSectionsByPos(Point p)
        {
            if (p.Y <= this._option._upRoleY || p.Y >= this._option.BottomRoleY)
            {
                return new List<TimeSection>();
            }
            DateTime dt = this.PosToDateTime(p.X);
            return this.GetAllSections().FindAll((TimeSection sec) => dt >= sec.DateTimeBegin && dt <= sec.DateTimeEnd);
        }

        private void RefreshBar()
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new Action(this.RefreshBar));
                return;
            }
            base.Invalidate();
        }

        private void UCTimeBar_Paint(object sender, PaintEventArgs e)
        {
            if (this._mode == TimeBarModes.Time)
            {
                this.DrawTimeBar(e);
            }
            else
            {
                this.DrawProgressBar(e);
            }
            if (this._option._selected && this._option._drawSelectedBorder)
            {
                e.Graphics.DrawRectangle(this._option._penSelected, new Rectangle(0, 0, base.Width - 1, base.Height - 1));
            }
        }

        private void UCTimeBar_MouseMove(object sender, MouseEventArgs e)
        {
            this._mousePositionX = e.Location.X;
            if (this._mode == TimeBarModes.Time)
            {
                if (e.Button == MouseButtons.Left && this._dragable)
                {
                    if (this.Cursor != this._cursorPressed)
                    {
                        this.Cursor = this._cursorPressed;
                    }
                    if (this._option.CurrentTimeLineDragable && Math.Abs(e.Location.X - this._curTimeLine.X) < 20)
                    {
                        this._isCurrentTimeLineDraging = true;
                        this._isRoleDraging = false;
                        if (e.Location.X <= 20 || e.Location.X > base.Width - 20)
                        {
                            return;
                        }
                        this._curTimeLine.X += e.Location.X - this._lastDragStartX;
                        this._curTimeLine.DateTime = this.PosToDateTime(this._curTimeLine.X);
                        if (this.TimeLineDraging != null)
                        {
                            this.TimeLineDraging(this, this._curTimeLine.DateTime, null);
                        }
                    }
                    else
                    {
                        this._isCurrentTimeLineDraging = false;
                        int num = e.Location.X - this._lastDragStartX;
                        if (num == 0)
                        {
                            return;
                        }
                        this._isRoleDraging = true;
                        if (e.Location.X > this._lastDragStartX)
                        {
                            this._spaceLeft += num;
                            if (this._spaceLeft >= this._space)
                            {
                                this._spaceLeft -= this._space;
                            }
                        }
                        else if (e.Location.X < this._lastDragStartX)
                        {
                            this._spaceLeft += num;
                            if (this._spaceLeft <= 0)
                            {
                                this._spaceLeft = this._space + this._spaceLeft;
                            }
                        }
                        this._originPos += num;
                        DateTime dateTime = this._curTimeLine.DateTime;
                        this._curTimeLine.DateTime = this.PosToDateTime(this._curTimeLine.X);
                        if (this.RoleDraging != null)
                        {
                            this.RoleDraging(this, dateTime, this._curTimeLine.DateTime, null);
                        }
                    }
                    base.Invalidate();
                    this._lastDragStartX = e.Location.X;
                }
                else if (this.Cursor != this._cursorDrag)
                {
                    this.Cursor = this._cursorDrag;
                }
                if (this._option._showMouseTimeLine)
                {
                    this._mouseTimeLine.X = e.Location.X;
                    this._mouseTimeLine.DateTime = this.PosToDateTime(e.Location.X);
                    base.Invalidate();
                }
                if (this._option._mouseScaleHeight > 0)
                {
                    base.Invalidate(new Rectangle(this._mousePositionX - 30, this._option._upRoleY - this._option._mouseScaleHeight - 2, 60, this._option._mouseScaleHeight + 4));
                }
                if (this._option._isMouseStayDelayEventEnable)
                {
                    this._mouseStayDelayCount = 0;
                }
            }
        }

        private void UCTimeBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (this._mode == TimeBarModes.Time)
            {
                this._lastDragStartX = e.Location.X;
                this.Cursor = this._cursorPressed;
            }
        }
        private void UCTimeBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (this._mode == TimeBarModes.Time)
            {
                if (this._isRoleDraging)
                {
                    this._isRoleDraging = false;
                    if (this.AfterRoleDraged != null)
                    {
                        this.AfterRoleDraged(this, this._curTimeLine.DateTime, this.GetSectionsByPos(new Point(this._curTimeLine.X, e.Y)));
                    }
                }
                if (this._isCurrentTimeLineDraging)
                {
                    this._isCurrentTimeLineDraging = false;
                    if (this.AfterTimeLineDraged != null)
                    {
                        this.AfterTimeLineDraged(this, this._curTimeLine.DateTime, this.GetSectionsByPos(new Point(this._curTimeLine.X, e.Y)));
                    }
                }
                this.Cursor = this._cursorDrag;
            }
        }

        private void UCTimeBar_MouseHover(object sender, EventArgs e)
        {
            if (this._mode == TimeBarModes.Time && this.MouseHoverEx != null)
            {
                this.MouseHoverEx(this, this.PosToDateTime(base.PointToClient(Control.MousePosition).X), this.GetSectionsByPos(base.PointToClient(Control.MousePosition)));
            }
        }

        private void UCTimeBar_MouseLeave(object sender, EventArgs e)
        {
            this._mousePositionX = -100;
            bool flag = false;
            if (this._option._showMouseTimeLine)
            {
                this._mouseTimeLine.X = -1000;
                this._mouseTimeLine.DateTime = this.PosToDateTime(-1000);
                flag = true;
            }
            if (this._option._mouseScaleHeight > 0)
            {
                flag = true;
            }
            if (flag)
            {
                base.Invalidate();
            }
        }

        private void UCTimeBar_DoubleClick(object sender, EventArgs e)
        {
            if (this._mode == TimeBarModes.Time && this.DoubleClickEx != null)
            {
                this.DoubleClickEx(this, this.PosToDateTime(base.PointToClient(Control.MousePosition).X), this.GetSectionsByPos(base.PointToClient(Control.MousePosition)));
            }
        }
        private void UCTimeBar_MouseClick(object sender, MouseEventArgs e)
        {
            if (this._mode == TimeBarModes.Time)
            {
                if (this.MouseClickEx != null)
                {
                    this.MouseClickEx(this, this.PosToDateTime(e.Location.X), this.GetSectionsByPos(e.Location), e);
                    return;
                }
            }
            else if (this.MouseClickEx != null)
            {
                this.MouseClickEx(this, DateTime.MinValue, null, e);
            }
        }

        [Obsolete]
        private void UCTimeBar_MouseWheel(object sender, MouseEventArgs e)
        {
            if (this._mode == TimeBarModes.Time)
            {
                if (e.Delta > 0)
                {
                    switch (this._timeUnit)
                    {
                        case TimeUnits.Minute:
                            break;
                        case TimeUnits.Hour:
                            this.TimeUnit = TimeUnits.Minute;
                            return;
                        case TimeUnits.Day:
                            this.TimeUnit = TimeUnits.Hour;
                            return;
                        case TimeUnits.Month:
                            this.TimeUnit = TimeUnits.Day;
                            return;
                        case TimeUnits.Year: 
                            this.TimeUnit = TimeUnits.Year;
                            return;
                        default:
                            return;
                    }
                }
                else
                {
                    switch (this._timeUnit)
                    {
                        case TimeUnits.Minute:
                            this.TimeUnit = TimeUnits.Hour;
                            return;
                        case TimeUnits.Hour:
                            this.TimeUnit = TimeUnits.Day;
                            return;
                        case TimeUnits.Day:
                            this.TimeUnit = TimeUnits.Month;
                            return;
                        case TimeUnits.Year:
                            this._timeUnit = TimeUnits.Year;
                            return;
                        default:
                            return;
                    }
                }
            }
        }

        // Token: 0x060000B7 RID: 183 RVA: 0x00004950 File Offset: 0x00002B50
        private void _timerForMouseStayDelay_Tick(object sender, EventArgs e)
        {
            if (this._mode == TimeBarModes.Time && this._option._isMouseStayDelayEventEnable)
            {
                this._mouseStayDelayCount += this._timerForMouseStayDelay.Interval;
                if (this._mouseStayDelayCount == 1500)
                {
                    Point point = base.PointToClient(Control.MousePosition);
                    if (point.X == this._mouseLastPosX)
                    {
                        return;
                    }
                    this._mouseLastPosX = point.X;
                    if (point.X >= 0 && point.X <= base.Width && point.Y >= 0 && point.Y <= base.Height && this.MouseStayed != null)
                    {
                        this.MouseStayed(this, this.PosToDateTime(base.PointToClient(Control.MousePosition).X), this.GetSectionsByPos(base.PointToClient(Control.MousePosition)));
                    }
                }
            }
        }

        // Token: 0x060000B8 RID: 184 RVA: 0x00004A39 File Offset: 0x00002C39
        private void _curTimeLine_DateTimeChanged(TimeLine sender, DateTime value)
        {
            if (this.CurrentTimeChanged != null)
            {
                this.CurrentTimeChanged(this, value, null);
            }
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

        public TimeBarModes Mode
        {
            get
            {
                return this._mode;
            }
            set
            {
                this._mode = value;
                if (value == TimeBarModes.Progress)
                {
                    this.Cursor = Cursors.Hand;
                }
                this.RefreshBar();
            }
        }

        [Obsolete]
        public TimeUnits TimeUnit
        {
            get
            {
                return this._timeUnit;
            }
            set
            {
                if (this._mode == TimeBarModes.Time)
                {
                    DateTime dateTime = this._curTimeLine.DateTime;
                    this._timeUnit = value;
                    this.Reset();
                    this.CurrentTime = dateTime;
                    this.RefreshBar();
                    if (this.TimeUnitChanged != null)
                    {
                        this.TimeUnitChanged(this, value);
                    }
                }
            }
        }

        [Obsolete("This property has expired and is no longer recommended for use.")]
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

        [Obsolete("This property has expired, please use the SetPointerValue and GetPointerValue methods instead.")]
        public DateTime CurrentTime
        {
            get
            {
                return this._curTimeLine.DateTime;
            }
            set
            {
                if (!this._isRoleDraging)
                {
                    this.SetCurrentTimeValue(value);
                }
            }
        }

        public int CurrentTimeLinePos
        {
            get
            {
                return this._curTimeLine.X;
            }
        }


        public DateTime DateTimeLeft
        {
            get
            {
                return this.PosToDateTime(0);
            }
            set
            {
                DateTime dateTime = this.PosToDateTime(0);
                if (dateTime != value)
                {
                    this.MoveRole((value - dateTime).TotalSeconds);
                    this._curTimeLine.DateTime = this.PosToDateTime(this._curTimeLine.X);
                    this.RefreshBar();
                }
            }
        }


        public ProgressBarEx ProgressBar
        {
            get
            {
                return this._progressBar;
            }
            set
            {
                this._progressBar = value;
            }
        }
        
        public TimeBarOptions Option
        {
            get
            {
                return this._option;
            }
        }

        public bool Dragable
        {
            get
            {
                return this._dragable;
            }
            set
            {
                this._dragable = value;
            }
        }

        public object UserData
        {
            get
            {
                return this._userData;
            }
            set
            {
                this._userData = value;
            }
        }

        public object UserData2
        {
            get
            {
                return this._userData2;
            }
            set
            {
                this._userData2 = value;
            }
        }

        public List<TimeSection> Sections
        {
            get
            {
                return this.GetAllSections();
            }
        }

        public List<Channel> Channels { get; private set; }
        public event RoleEventHandler TimeLineDraging;
        public event RoleEventHandler AfterTimeLineDraged;
        public event RoleDragingEventHandler RoleDraging;
        public event RoleEventHandler AfterRoleDraged;
        public event RoleEventHandler MouseHoverEx;
        public event RoleEventHandler DoubleClickEx;
        public event RoleMouseEventHandler MouseClickEx;
        public event RoleEventHandler MouseStayed;
        public event TimeUnitChangedEventHandler TimeUnitChanged;
        public event RoleEventHandler CurrentTimeChanged;
        public void AddChannel(Channel ch)
        {
            if (ch == null)
            {
                return;
            }
            this.Channels.Add(ch);
            this.RefreshBar();
        }

        public void InsertChannel(Channel ch, int index)
        {
            if (ch != null && index >= 0 && index <= this.Channels.Count)
            {
                this.Channels.Insert(index, ch);
                this.RefreshBar();
            }
        }

        public void RemoveChannel(Channel ch)
        {
            if (this.Channels.Contains(ch))
            {
                this.Channels.Remove(ch);
                this.RefreshBar();
            }
        }

        public void RemoveChannel(int index)
        {
            if (index >= 0 && index < this.Channels.Count)
            {
                this.Channels.RemoveAt(index);
                this.RefreshBar();
            }
        }

        public void RemoveChannel(string channelId)
        {
            Channel channel = this.Channels.Find((Channel c) => c.Id == channelId);
            if (channel != null)
            {
                this.Channels.Remove(channel);
                this.RefreshBar();
            }
        }

        public void ClearChannels()
        {
            this.Channels.Clear();
            this.RefreshBar();
        }

        public bool AddSection(TimeSection sec, bool refresh = true)
        {
            if (sec.DateTimeBegin >= sec.DateTimeEnd)
            {
                return false;
            }
            if (this.Channels.Count == 0)
            {
                this.Channels.Add(new Channel());
            }
            this.AddSection(sec, 0);
            if (refresh)
            {
                this.RefreshBar();
            }
            return true;
        }

        public void AddSection(TimeSection sec, Channel ch)
        {
            if (ch != null)
            {
                ch.Sections.Add(sec);
                sec.Channel = ch;
                this.RefreshBar();
            }
        }

        public void AddSection(TimeSection sec, string chId)
        {
            Channel channel = this.Channels.Find((Channel c) => c.Id == chId);
            if (channel != null)
            {
                channel.Sections.Add(sec);
                sec.Channel = channel;
                this.RefreshBar();
            }
        }

        public void AddSection(TimeSection sec, int channelIndex)
        {
            if (channelIndex >= 0 && channelIndex <= this.Channels.Count - 1)
            {
                this.Channels[channelIndex].Sections.Add(sec);
                sec.Channel = this.Channels[channelIndex];
                this.RefreshBar();
            }
        }

        public bool AddSections(List<TimeSection> secs)
        {
            if (secs == null || secs.Count == 0)
            {
                return false;
            }
            if (this.Channels.Count == 0)
            {
                this.Channels.Add(new Channel());
            }
            this.Channels[0].Sections.AddRange(secs);
            foreach (TimeSection timeSection in secs)
            {
                timeSection.Channel = this.Channels[0];
            }
            this.RefreshBar();
            return true;
        }

        public bool AddSections(List<TimeSection> secs, Channel ch)
        {
            if (secs == null || secs.Count == 0 || ch == null)
            {
                return false;
            }
            ch.Sections.AddRange(secs);
            foreach (TimeSection timeSection in secs)
            {
                timeSection.Channel = ch;
            }
            this.RefreshBar();
            return true;
        }

        public bool AddSections(List<TimeSection> secs, string chId)
        {
            if (secs == null || secs.Count == 0 || string.IsNullOrWhiteSpace(chId))
            {
                return false;
            }
            Channel channel = this.Channels.Find((Channel c) => c.Id == chId);
            if (channel == null)
            {
                return false;
            }
            channel.Sections.AddRange(secs);
            foreach (TimeSection timeSection in secs)
            {
                timeSection.Channel = channel;
            }
            this.RefreshBar();
            return true;
        }

        public bool AddSections(List<TimeSection> secs, int channelIndex)
        {
            if (secs == null || secs.Count == 0)
            {
                return false;
            }
            if (channelIndex >= 0 && channelIndex <= this.Channels.Count - 1)
            {
                this.Channels[channelIndex].Sections.AddRange(secs);
                foreach (TimeSection timeSection in secs)
                {
                    timeSection.Channel = this.Channels[channelIndex];
                }
                this.RefreshBar();
                return true;
            }
            return false;
        }

        public void ClearSections()
        {
            foreach (Channel channel in this.Channels)
            {
                channel.Sections.Clear();
            }
            this.RefreshBar();
        }

        public void ClearSections(Channel ch)
        {
            if (ch != null)
            {
                ch.Sections.Clear();
                this.RefreshBar();
            }
        }

        public void ClearSections(string chId)
        {
            Channel channel = this.Channels.Find((Channel c) => c.Id == chId);
            if (channel != null)
            {
                channel.Sections.Clear();
                this.RefreshBar();
            }
        }

        public void ClearSections(int channelIndex)
        {
            if (channelIndex >= 0 && channelIndex <= this.Channels.Count - 1)
            {
                this.Channels[channelIndex].Sections.Clear();
                this.RefreshBar();
            }
        }

        public List<TimeSection> GetSectionContainsDateTime(DateTime dt)
        {
            List<TimeSection> list = new List<TimeSection>();
            Predicate<TimeSection> temp = null;
            foreach (Channel channel in this.Channels)
            {
                List<TimeSection> sections = channel.Sections;
                Predicate<TimeSection> match;
                if ((match = temp) == null)
                {
                    match = (temp = ((TimeSection s) => s.DateTimeBegin <= dt && s.DateTimeEnd >= dt));
                }
                List<TimeSection> collection = sections.FindAll(match);
                list.AddRange(collection);
            }
            return list;
        }

        public TimeSection GetSection(string guid)
        {
            Predicate<TimeSection> temp = null;
            foreach (Channel channel in this.Channels)
            {
                List<TimeSection> sections = channel.Sections;
                Predicate<TimeSection> match;
                if ((match = temp) == null)
                {
                    match = (temp = ((TimeSection s) => s.GUID == guid));
                }
                TimeSection timeSection = sections.Find(match);
                if (timeSection != null)
                {
                    return timeSection;
                }
            }
            return null;
        }

        public List<TimeSection> GetSections(string guid)
        {
            List<TimeSection> list = new List<TimeSection>();
            Predicate<TimeSection> temp = null;
            foreach (Channel channel in this.Channels)
            {
                List<TimeSection> sections = channel.Sections;
                Predicate<TimeSection> match;
                if ((match = temp) == null)
                {
                    match = (temp = ((TimeSection m) => m.GUID == guid));
                }
                List<TimeSection> list2 = sections.FindAll(match);
                if (list2.Count > 0)
                {
                    list.AddRange(list2);
                }
            }
            return list;
        }

        public List<TimeSection> GetAllSections()
        {
            List<TimeSection> list = new List<TimeSection>();
            for (int i = 0; i < this.Channels.Count; i++)
            {
                if (this.Channels[i].Sections.Count > 0)
                {
                    list.AddRange(this.Channels[i].Sections);
                }
            }
            return list;
        }

        [Obsolete("This method has been deprecated and there is no alternative. Users can implement the function of this method by themselves based on the existing interface.")]
        public void MoveToSection()
        {
        }
        [Obsolete("This method has been expired, please use SetPointerPos method instead.")]
        public void MoveTimeLineTo(DateTime dt)
        {
            this._curTimeLine.X = this.DateTimeToPos(dt);
            this._curTimeLine.DateTime = dt;
            this.RefreshBar();
        }
        [Obsolete("This method has been expired, please use SetPointerPos method instead.")]
        public void MoveTimeLineTo(int posX)
        {
            if (posX < 10)
            {
                posX = 10;
            }
            if (posX > base.Width - 10)
            {
                posX = base.Width - 10;
            }
            this._curTimeLine.X = posX;
            this._curTimeLine.DateTime = this.PosToDateTime(posX);
            this.RefreshBar();
        }
        public void SetPointerPos(int posX)
        {
            if (this.Mode != TimeBarModes.Time)
            {
                return;
            }
            if (this._isRoleDraging)
            {
                return;
            }
            if (posX < 10)
            {
                posX = 10;
            }
            if (posX > base.Width - 10)
            {
                posX = base.Width - 10;
            }
            this._curTimeLine.X = posX;
            this._curTimeLine.DateTime = this.PosToDateTime(posX);
            this.RefreshBar();
        }
        public void SetPointerPos(DateTime dt)
        {
            if (this.Mode != TimeBarModes.Time)
            {
                return;
            }
            if (this._isRoleDraging)
            {
                return;
            }
            this._curTimeLine.X = this.DateTimeToPos(dt);
            this._curTimeLine.DateTime = dt;
            this.RefreshBar();
        }
        public void SetPointerValue(DateTime dt)
        {
            if (this.Mode != TimeBarModes.Time)
            {
                return;
            }
            if (!this._isRoleDraging)
            {
                this.SetCurrentTimeValue(dt);
            }
        }
        public DateTime GetPointerValue()
        {
            return this._curTimeLine.DateTime;
        }
        public void SetPointerRangMarker(bool enable, double secondsLeft = 0.0, double secondsRight = 0.0)
        {
            if (!enable)
            {
                this._sectionNearPointer = null;
                return;
            }
            this._secondsLeftOfPointerRange = secondsLeft;
            this._secondsRightOfPointerRange = secondsRight;
            this._sectionNearPointer = new TimeSection
            {
                Color = new Color?(Color.FromArgb(32, 255, 0, 0)),
                DateTimeBegin = this.GetPointerValue().AddSeconds(-secondsLeft),
                DateTimeEnd = this.GetPointerValue().AddSeconds(secondsRight)
            };
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            base.SuspendLayout();
            this.AllowDrop = true;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Name = "UCTimeBar";
            base.Size = new Size(729, 137);
            base.Paint += this.UCTimeBar_Paint;
            base.DoubleClick += this.UCTimeBar_DoubleClick;
            base.MouseClick += this.UCTimeBar_MouseClick;
            base.MouseDown += this.UCTimeBar_MouseDown;
            base.MouseLeave += this.UCTimeBar_MouseLeave;
            base.MouseHover += this.UCTimeBar_MouseHover;
            base.MouseMove += this.UCTimeBar_MouseMove;
            base.MouseUp += this.UCTimeBar_MouseUp;
            base.ResumeLayout(false);
        }


        private TimeSection _sectionNearPointer;
        private double _secondsLeftOfPointerRange;
        private double _secondsRightOfPointerRange;
        private string _guid = "";
        private Cursor _cursorDrag;
        private Cursor _cursorPressed;
        private TimeBarOptions _option;
        private Timer _timerForMouseStayDelay;
        private ProgressBarEx _progressBar;
        private TimeBarModes _mode;
        private int _mousePositionX = -100;
        private int _lastDragStartX;
        private TimeLine _curTimeLine = new TimeLine
        {
            X = 300
        };
        private TimeLine _mouseTimeLine = new TimeLine
        {
            X = -1000
        };
        private int _spaceLeft;
        private int _space = 60;

        private double _secondsPerPix;

        private static readonly DateTime DateTimeOrigin = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));

        private int _originPos;

        private DateTime _dateTimeBegin = DateTime.Now;

        private TimeUnits _timeUnit;

        private int _curSectionIndex;

        private bool _isRoleDraging;

        private bool _isCurrentTimeLineDraging;

        private const int _mouseStayDelayMS = 1500;

        private int _mouseStayDelayCount;

        private bool _dragable = true;

        private object _userData;

        private object _userData2;

        private int _mouseLastPosX;

        private int _sectionSpace = 3;

        private int _rolePadding = 4;

        private IContainer components;
    }
}
