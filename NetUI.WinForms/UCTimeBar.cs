using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using NetUI.WinForms.Properties;

namespace NetUI.WinForms
{
    public class UCTimeBar : UserControl
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

        // Token: 0x060000A2 RID: 162 RVA: 0x00003600 File Offset: 0x00001800
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

        // Token: 0x060000A3 RID: 163 RVA: 0x00003948 File Offset: 0x00001B48
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
                                    g.DrawString(dateTime.ToString("HH:mm"), this._option._scalcFont, this._option._brushScalcString, new PointF((float)(scalePosX - 18), (float)(scalePosY - 15)));
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
                                    g.DrawString(dateTime2.ToString("MM-dd"), this._option._scalcFontBold, this._option._brushScalcString, new PointF((float)(scalePosX - 16), (float)(scalePosY - 15)));
                                }
                                else
                                {
                                    g.DrawString(dateTime2.Hour + "H", this._option._scalcFont, this._option._brushScalcString, new PointF((float)(scalePosX - 9), (float)(scalePosY - 15)));
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
                                    g.DrawString(dateTime3.ToString("yyyy-MM"), this._option._scalcFontBold, this._option._brushScalcString, new PointF((float)(scalePosX - 22), (float)(scalePosY - 15)));
                                }
                                else
                                {
                                    g.DrawString(dateTime3.ToString("MM-dd"), this._option._scalcFont, this._option._brushScalcString, new PointF((float)(scalePosX - 18), (float)(scalePosY - 15)));
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
                                    g.DrawString(dateTime4.Year.ToString() + "Year", this._option._scalcFontBold, this._option._brushScalcString, new PointF((float)(scalePosX - 16), (float)(scalePosY - 15)));
                                }
                                else
                                {
                                    g.DrawString(dateTime4.Month.ToString() + "Month", this._option._scalcFont, this._option._brushScalcString, new PointF((float)(scalePosX - 10), (float)(scalePosY - 15)));
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

        // Token: 0x1700003E RID: 62
        // (get) Token: 0x060000B9 RID: 185 RVA: 0x00004A51 File Offset: 0x00002C51
        // (set) Token: 0x060000BA RID: 186 RVA: 0x00004A59 File Offset: 0x00002C59
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

        // Token: 0x1700003F RID: 63
        // (get) Token: 0x060000BB RID: 187 RVA: 0x00004A62 File Offset: 0x00002C62
        // (set) Token: 0x060000BC RID: 188 RVA: 0x00004A6A File Offset: 0x00002C6A
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

        // Token: 0x17000040 RID: 64
        // (get) Token: 0x060000BD RID: 189 RVA: 0x00004A88 File Offset: 0x00002C88
        // (set) Token: 0x060000BE RID: 190 RVA: 0x00004A90 File Offset: 0x00002C90
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

        // Token: 0x17000041 RID: 65
        // (get) Token: 0x060000BF RID: 191 RVA: 0x00004AE0 File Offset: 0x00002CE0
        // (set) Token: 0x060000C0 RID: 192 RVA: 0x00004AE8 File Offset: 0x00002CE8
        [Obsolete("该属性已过期，不建议再使用。")]
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

        // Token: 0x17000042 RID: 66
        // (get) Token: 0x060000C1 RID: 193 RVA: 0x00004AF1 File Offset: 0x00002CF1
        // (set) Token: 0x060000C2 RID: 194 RVA: 0x00004AFE File Offset: 0x00002CFE
        [Obsolete("该属性已过期，请使用SetPointerValue 和 GetPointerValue方法替代。")]
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

        // Token: 0x17000043 RID: 67
        // (get) Token: 0x060000C3 RID: 195 RVA: 0x00004B0F File Offset: 0x00002D0F
        public int CurrentTimeLinePos
        {
            get
            {
                return this._curTimeLine.X;
            }
        }

        // Token: 0x17000044 RID: 68
        // (get) Token: 0x060000C4 RID: 196 RVA: 0x00004B1C File Offset: 0x00002D1C
        // (set) Token: 0x060000C5 RID: 197 RVA: 0x00004B28 File Offset: 0x00002D28
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

        // Token: 0x17000045 RID: 69
        // (get) Token: 0x060000C6 RID: 198 RVA: 0x00004B7D File Offset: 0x00002D7D
        // (set) Token: 0x060000C7 RID: 199 RVA: 0x00004B85 File Offset: 0x00002D85
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

        // Token: 0x17000046 RID: 70
        // (get) Token: 0x060000C8 RID: 200 RVA: 0x00004B8E File Offset: 0x00002D8E
        public TimeBarOptions Option
        {
            get
            {
                return this._option;
            }
        }

        // Token: 0x17000047 RID: 71
        // (get) Token: 0x060000C9 RID: 201 RVA: 0x00004B96 File Offset: 0x00002D96
        // (set) Token: 0x060000CA RID: 202 RVA: 0x00004B9E File Offset: 0x00002D9E
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

        // Token: 0x17000048 RID: 72
        // (get) Token: 0x060000CB RID: 203 RVA: 0x00004BA7 File Offset: 0x00002DA7
        // (set) Token: 0x060000CC RID: 204 RVA: 0x00004BAF File Offset: 0x00002DAF
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

        // Token: 0x17000049 RID: 73
        // (get) Token: 0x060000CD RID: 205 RVA: 0x00004BB8 File Offset: 0x00002DB8
        // (set) Token: 0x060000CE RID: 206 RVA: 0x00004BC0 File Offset: 0x00002DC0
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

        // Token: 0x1700004A RID: 74
        // (get) Token: 0x060000CF RID: 207 RVA: 0x00004BC9 File Offset: 0x00002DC9
        public List<TimeSection> Sections
        {
            get
            {
                return this.GetAllSections();
            }
        }

        // Token: 0x1700004B RID: 75
        // (get) Token: 0x060000D0 RID: 208 RVA: 0x00004BD1 File Offset: 0x00002DD1
        // (set) Token: 0x060000D1 RID: 209 RVA: 0x00004BD9 File Offset: 0x00002DD9
        public List<Channel> Channels { get; private set; }

        // Token: 0x14000003 RID: 3
        // (add) Token: 0x060000D2 RID: 210 RVA: 0x00004BE4 File Offset: 0x00002DE4
        // (remove) Token: 0x060000D3 RID: 211 RVA: 0x00004C1C File Offset: 0x00002E1C
        public event RoleEventHandler TimeLineDraging;

        // Token: 0x14000004 RID: 4
        // (add) Token: 0x060000D4 RID: 212 RVA: 0x00004C54 File Offset: 0x00002E54
        // (remove) Token: 0x060000D5 RID: 213 RVA: 0x00004C8C File Offset: 0x00002E8C
        public event RoleEventHandler AfterTimeLineDraged;

        // Token: 0x14000005 RID: 5
        // (add) Token: 0x060000D6 RID: 214 RVA: 0x00004CC4 File Offset: 0x00002EC4
        // (remove) Token: 0x060000D7 RID: 215 RVA: 0x00004CFC File Offset: 0x00002EFC
        public event RoleDragingEventHandler RoleDraging;

        // Token: 0x14000006 RID: 6
        // (add) Token: 0x060000D8 RID: 216 RVA: 0x00004D34 File Offset: 0x00002F34
        // (remove) Token: 0x060000D9 RID: 217 RVA: 0x00004D6C File Offset: 0x00002F6C
        public event RoleEventHandler AfterRoleDraged;

        // Token: 0x14000007 RID: 7
        // (add) Token: 0x060000DA RID: 218 RVA: 0x00004DA4 File Offset: 0x00002FA4
        // (remove) Token: 0x060000DB RID: 219 RVA: 0x00004DDC File Offset: 0x00002FDC
        public event RoleEventHandler MouseHoverEx;

        // Token: 0x14000008 RID: 8
        // (add) Token: 0x060000DC RID: 220 RVA: 0x00004E14 File Offset: 0x00003014
        // (remove) Token: 0x060000DD RID: 221 RVA: 0x00004E4C File Offset: 0x0000304C
        public event RoleEventHandler DoubleClickEx;

        // Token: 0x14000009 RID: 9
        // (add) Token: 0x060000DE RID: 222 RVA: 0x00004E84 File Offset: 0x00003084
        // (remove) Token: 0x060000DF RID: 223 RVA: 0x00004EBC File Offset: 0x000030BC
        public event RoleMouseEventHandler MouseClickEx;

        // Token: 0x1400000A RID: 10
        // (add) Token: 0x060000E0 RID: 224 RVA: 0x00004EF4 File Offset: 0x000030F4
        // (remove) Token: 0x060000E1 RID: 225 RVA: 0x00004F2C File Offset: 0x0000312C
        public event RoleEventHandler MouseStayed;

        // Token: 0x1400000B RID: 11
        // (add) Token: 0x060000E2 RID: 226 RVA: 0x00004F64 File Offset: 0x00003164
        // (remove) Token: 0x060000E3 RID: 227 RVA: 0x00004F9C File Offset: 0x0000319C
        public event TimeUnitChangedEventHandler TimeUnitChanged;

        // Token: 0x1400000C RID: 12
        // (add) Token: 0x060000E4 RID: 228 RVA: 0x00004FD4 File Offset: 0x000031D4
        // (remove) Token: 0x060000E5 RID: 229 RVA: 0x0000500C File Offset: 0x0000320C
        public event RoleEventHandler CurrentTimeChanged;

        // Token: 0x060000E6 RID: 230 RVA: 0x00005041 File Offset: 0x00003241
        public void AddChannel(Channel ch)
        {
            if (ch == null)
            {
                return;
            }
            this.Channels.Add(ch);
            this.RefreshBar();
        }

        // Token: 0x060000E7 RID: 231 RVA: 0x00005059 File Offset: 0x00003259
        public void InsertChannel(Channel ch, int index)
        {
            if (ch != null && index >= 0 && index <= this.Channels.Count)
            {
                this.Channels.Insert(index, ch);
                this.RefreshBar();
            }
        }

        // Token: 0x060000E8 RID: 232 RVA: 0x00005083 File Offset: 0x00003283
        public void RemoveChannel(Channel ch)
        {
            if (this.Channels.Contains(ch))
            {
                this.Channels.Remove(ch);
                this.RefreshBar();
            }
        }

        // Token: 0x060000E9 RID: 233 RVA: 0x000050A6 File Offset: 0x000032A6
        public void RemoveChannel(int index)
        {
            if (index >= 0 && index < this.Channels.Count)
            {
                this.Channels.RemoveAt(index);
                this.RefreshBar();
            }
        }

        // Token: 0x060000EA RID: 234 RVA: 0x000050CC File Offset: 0x000032CC
        public void RemoveChannel(string channelId)
        {
            Channel channel = this.Channels.Find((Channel c) => c.Id == channelId);
            if (channel != null)
            {
                this.Channels.Remove(channel);
                this.RefreshBar();
            }
        }

        // Token: 0x060000EB RID: 235 RVA: 0x00005114 File Offset: 0x00003314
        public void ClearChannels()
        {
            this.Channels.Clear();
            this.RefreshBar();
        }

        // Token: 0x060000EC RID: 236 RVA: 0x00005128 File Offset: 0x00003328
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

        // Token: 0x060000ED RID: 237 RVA: 0x00005179 File Offset: 0x00003379
        public void AddSection(TimeSection sec, Channel ch)
        {
            if (ch != null)
            {
                ch.Sections.Add(sec);
                sec.Channel = ch;
                this.RefreshBar();
            }
        }

        // Token: 0x060000EE RID: 238 RVA: 0x00005198 File Offset: 0x00003398
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

        // Token: 0x060000EF RID: 239 RVA: 0x000051E8 File Offset: 0x000033E8
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

        // Token: 0x060000F9 RID: 249 RVA: 0x00005604 File Offset: 0x00003804
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

        // Token: 0x060000FB RID: 251 RVA: 0x00005730 File Offset: 0x00003930
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

        // Token: 0x060000FC RID: 252 RVA: 0x0000578A File Offset: 0x0000398A
        [Obsolete("该方法已弃用，且没有替代方法，用户可根据现有接口自行实现该方法的功能。")]
        public void MoveToSection()
        {
        }

        // Token: 0x060000FD RID: 253 RVA: 0x0000578C File Offset: 0x0000398C
        [Obsolete("该方法已过期，请使用SetPointerPos方法替代。")]
        public void MoveTimeLineTo(DateTime dt)
        {
            this._curTimeLine.X = this.DateTimeToPos(dt);
            this._curTimeLine.DateTime = dt;
            this.RefreshBar();
        }

        // Token: 0x060000FE RID: 254 RVA: 0x000057B4 File Offset: 0x000039B4
        [Obsolete("该方法已过期，请使用SetPointerPos方法替代。")]
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

        // Token: 0x060000FF RID: 255 RVA: 0x00005808 File Offset: 0x00003A08
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

        // Token: 0x06000100 RID: 256 RVA: 0x0000586B File Offset: 0x00003A6B
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

        // Token: 0x06000101 RID: 257 RVA: 0x000058A3 File Offset: 0x00003AA3
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

        // Token: 0x06000102 RID: 258 RVA: 0x00004AF1 File Offset: 0x00002CF1
        public DateTime GetPointerValue()
        {
            return this._curTimeLine.DateTime;
        }

        // Token: 0x06000103 RID: 259 RVA: 0x000058C0 File Offset: 0x00003AC0
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

        // Token: 0x06000104 RID: 260 RVA: 0x00005935 File Offset: 0x00003B35
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Token: 0x06000105 RID: 261 RVA: 0x00005954 File Offset: 0x00003B54
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

        // Token: 0x04000054 RID: 84
        private TimeSection _sectionNearPointer;

        // Token: 0x04000055 RID: 85
        private double _secondsLeftOfPointerRange;

        // Token: 0x04000056 RID: 86
        private double _secondsRightOfPointerRange;

        // Token: 0x04000057 RID: 87
        private string _guid = "";

        // Token: 0x04000058 RID: 88
        private Cursor _cursorDrag;

        // Token: 0x04000059 RID: 89
        private Cursor _cursorPressed;

        // Token: 0x0400005A RID: 90
        private TimeBarOptions _option;

        // Token: 0x0400005B RID: 91
        private Timer _timerForMouseStayDelay;

        // Token: 0x0400005C RID: 92
        private ProgressBarEx _progressBar;

        // Token: 0x0400005D RID: 93
        private TimeBarModes _mode;

        // Token: 0x0400005E RID: 94
        private int _mousePositionX = -100;

        // Token: 0x0400005F RID: 95
        private int _lastDragStartX;

        // Token: 0x04000060 RID: 96
        private TimeLine _curTimeLine = new TimeLine
        {
            X = 300
        };

        // Token: 0x04000061 RID: 97
        private TimeLine _mouseTimeLine = new TimeLine
        {
            X = -1000
        };

        // Token: 0x04000062 RID: 98
        private int _spaceLeft;

        // Token: 0x04000063 RID: 99
        private int _space = 60;

        // Token: 0x04000064 RID: 100
        private double _secondsPerPix;

        // Token: 0x04000065 RID: 101
        private static readonly DateTime DateTimeOrigin = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));

        // Token: 0x04000066 RID: 102
        private int _originPos;

        // Token: 0x04000067 RID: 103
        private DateTime _dateTimeBegin = DateTime.Now;

        // Token: 0x04000068 RID: 104
        private TimeUnits _timeUnit;

        // Token: 0x04000069 RID: 105
        private int _curSectionIndex;

        // Token: 0x0400006A RID: 106
        private bool _isRoleDraging;

        // Token: 0x0400006B RID: 107
        private bool _isCurrentTimeLineDraging;

        // Token: 0x0400006C RID: 108
        private const int _mouseStayDelayMS = 1500;

        // Token: 0x0400006D RID: 109
        private int _mouseStayDelayCount;

        // Token: 0x0400006E RID: 110
        private bool _dragable = true;

        // Token: 0x0400006F RID: 111
        private object _userData;

        // Token: 0x04000070 RID: 112
        private object _userData2;

        // Token: 0x04000071 RID: 113
        private int _mouseLastPosX;

        // Token: 0x04000072 RID: 114
        private int _sectionSpace = 3;

        // Token: 0x04000073 RID: 115
        private int _rolePadding = 4;

        // Token: 0x0400007F RID: 127
        private IContainer components;
    }
}
