using System;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using ASP = System.Web.UI.WebControls;
using AnthemNxt.Core;

namespace AnthemNxt.Controls
{
	/// <summary>
	/// Creates an updatable control that displays a link to another Web page. 
	/// </summary>
	[ToolboxBitmap(typeof(ASP.HyperLink))]
	public class HyperLink : ASP.HyperLink, IUpdatableControl
	{
		#region Unique Anthem control code

		private const string parentTagName = "span";
		/// <summary>
		/// Renders the server control wrapped in an additional element so that the
		/// element.innerHTML can be updated after a callback.
		/// </summary>
		protected override void Render(HtmlTextWriter writer)
		{
			if(!DesignMode)
			{
				AnthemNxt.Core.Manager.WriteBeginControlMarker(writer, parentTagName, this);
			}
			if(Visible)
			{
				base.Render(writer);
			}
			if(!DesignMode)
			{
				AnthemNxt.Core.Manager.WriteEndControlMarker(writer, parentTagName, this);
			}
		}

		#endregion

		#region IUpdatableControl implementation

		/// <summary>
		/// Gets or sets a value indicating whether the control should be updated after each callback.
		/// Also see <see cref="UpdateAfterCallBack"/>.
		/// </summary>
		/// <value>
		/// 	<strong>true</strong> if the the control should be updated; otherwise,
		/// <strong>false</strong>. The default is <strong>false</strong>.
		/// </value>
		/// <example>
		/// 	<code lang="CS" description="This is normally used declaratively as shown here.">
		/// &lt;anthem:Label id="label" runat="server" AutoUpdateAfterCallBack="true" /&gt;
		///     </code>
		/// </example>
		[Category("Anthem")]
		[DefaultValue(false)]
		[Description("True if this control should be updated after each callback.")]
		public virtual bool AutoUpdateAfterCallBack
		{
			get
			{
				object obj = this.ViewState["AutoUpdateAfterCallBack"];
				if(obj == null)
					return false;
				else
					return (bool)obj;
			}
			set
			{
				if(value) UpdateAfterCallBack = true;
				ViewState["AutoUpdateAfterCallBack"] = value;
			}
		}

		private bool _updateAfterCallBack = false;

		/// <summary>
		/// Gets or sets a value which indicates whether the control should be updated after the current callback.
		/// Also see <see cref="AutoUpdateAfterCallBack"/>.
		/// </summary>
		/// <value>
		/// 	<strong>true</strong> if the the control should be updated; otherwise,
		/// <strong>false</strong>. The default is <strong>false</strong>.
		/// </value>
		/// <example>
		/// 	<code lang="CS" description="This is normally used in server code as shown here.">
		/// this.Label = "Count = " + count;
		/// this.Label.UpdateAfterCallBack = true;
		///     </code>
		/// </example>
		[Browsable(false)]
		[DefaultValue(false)]
		public virtual bool UpdateAfterCallBack
		{
			get { return _updateAfterCallBack; }
			set { _updateAfterCallBack = value; }
		}

		#endregion

		#region Common Anthem control code

		/// <summary>
		/// Raises the <see cref="System.Web.UI.Control.Load"/> event and registers the control
		/// with <see cref="AnthemNxt.Manager"/>.
		/// </summary>
		/// <param name="e">A <see cref="System.EventArgs"/>.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			AnthemNxt.Core.Manager.Register(this);
		}

		/// <summary>
		/// Forces the server control to output content and trace information.
		/// </summary>
		public override void RenderControl(HtmlTextWriter writer)
		{
			base.Visible = true;
			base.RenderControl(writer);
		}

		/// <summary>
		/// Overrides the Visible property so that AnthemNxt.Manager can track the visibility.
		/// </summary>
		/// <value>
		/// 	<strong>true</strong> if the control is rendered on the client; otherwise
		/// <strong>false</strong>. The default is <strong>true</strong>.
		/// </value>
		public override bool Visible
		{
			get
			{
				return AnthemNxt.Core.Manager.GetControlVisible(this, ViewState, DesignMode);
			}
			set { AnthemNxt.Core.Manager.SetControlVisible(ViewState, value); }
		}

		#endregion
	}
}
