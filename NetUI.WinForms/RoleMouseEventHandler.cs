using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NetUI.WinForms
{
	public delegate void RoleMouseEventHandler(UCTimeBar sender, DateTime dateTime, List<TimeSection> section, MouseEventArgs e);
}
