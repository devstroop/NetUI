using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace vc.controls.timeline.winform.Properties
{
	// Token: 0x02000010 RID: 16
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x06000107 RID: 263 RVA: 0x00005A6D File Offset: 0x00003C6D
		internal Resources()
		{
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00005A75 File Offset: 0x00003C75
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Resources.resourceMan == null)
				{
					Resources.resourceMan = new ResourceManager("vc.controls.timeline.winform.Properties.Resources", typeof(Resources).Assembly);
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00005AA1 File Offset: 0x00003CA1
		// (set) Token: 0x0600010A RID: 266 RVA: 0x00005AA8 File Offset: 0x00003CA8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00005AB0 File Offset: 0x00003CB0
		internal static Bitmap hand
		{
			get
			{
				return (Bitmap)Resources.ResourceManager.GetObject("hand", Resources.resourceCulture);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00005ACB File Offset: 0x00003CCB
		internal static Bitmap handPressed
		{
			get
			{
				return (Bitmap)Resources.ResourceManager.GetObject("handPressed", Resources.resourceCulture);
			}
		}

		// Token: 0x04000080 RID: 128
		private static ResourceManager resourceMan;

		// Token: 0x04000081 RID: 129
		private static CultureInfo resourceCulture;
	}
}
