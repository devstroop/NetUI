using System;
using System.Collections.Generic;

namespace NetUI.WinForms
{
	public delegate void RoleDragingEventHandler(UCTimeBar timeBar, DateTime dateTimeOld, DateTime dateTimeNew, List<TimeSection> sections);
}
