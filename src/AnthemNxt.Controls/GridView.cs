using System;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using ASP = System.Web.UI.WebControls;
using AnthemNxt.Core;

namespace AnthemNxt.Controls
{
	/// <summary>
	/// Creates an updatable control that displays the values of a data source in a table where each column represents a field and each row represents a record. The GridView control allows you to select, sort, and edit these items using callbacks.
	/// </summary>
	[ToolboxBitmap(typeof(ASP.GridView))]
	public class GridView : ASP.GridView,
		IUpdatableControl,
		ICallbackControl,
		ICallbackContainerControl
	{
		#region Unique Anthem control code

		/// <summary>
		/// Excecutes <see cref="System.Web.UI.WebControls.GridView.OnDataPropertyChanged"/>, 
		/// then sets <see cref="UpdateAfterCallBack"/> to true.
		/// </summary>
		protected override void OnDataPropertyChanged()
		{
			base.OnDataPropertyChanged();
			UpdateAfterCallBack = true;
		}

		private const string parentTagName = "div";
		/// <summary>
		/// Renders the server control wrapped in an additional element so that the
		/// element.innerHTML can be updated after a callback.
		/// </summary>
		protected override void Render(HtmlTextWriter writer)
		{
			if(EnableCallBack && AddCallBacks)
				EventHandlerManager.AddChildCallBacks(this,
					EnabledDuringCallBack,
					TextDuringCallBack,
					PreCallBackFunction,
					PostCallBackFunction,
					CallBackCancelledFunction);

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

		#region ICallBackControl implementation

		/// <summary>
		/// Gets or sets the javascript function to execute on the client if the callback is
		/// cancelled. See <see cref="PreCallBackFunction"/>.
		/// </summary>
		[Category("Anthem")]
		[DefaultValue("")]
		[Description("The javascript function to call on the client if the callback is cancelled.")]
		public virtual string CallBackCancelledFunction
		{
			get
			{
				string text = (string)ViewState["CallBackCancelledFunction"];
				if(text == null)
					return string.Empty;
				else
					return text;
			}
			set
			{
				ViewState["CallBackCancelledFunction"] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the control uses callbacks instead of postbacks to post data to the server.
		/// </summary>
		/// <value>
		/// 	<strong>true</strong> if the the control uses callbacks; otherwise,
		/// <strong>false</strong>. The default is <strong>true</strong>.
		/// </value>
		[Category("Anthem")]
		[DefaultValue(true)]
		[Description("True if this control uses callbacks instead of postbacks to post data to the server.")]
		public virtual bool EnableCallBack
		{
			get
			{
				object obj = this.ViewState["EnableCallBack"];
				if(obj == null)
					return true;
				else
					return (bool)obj;
			}
			set
			{
				ViewState["EnableCallBack"] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the control is enabled on the client during callbacks.
		/// </summary>
		/// <value>
		/// 	<strong>true</strong> if the the control is enabled; otherwise,
		/// <strong>false</strong>. The default is <strong>true</strong>.
		/// </value>
		/// <remarks>Not all HTML elements support this property.</remarks>
		[Category("Anthem")]
		[DefaultValue(true)]
		[Description("True if this control is enabled on the client during callbacks.")]
		public virtual bool EnabledDuringCallBack
		{
			get
			{
				object obj = this.ViewState["EnabledDuringCallBack"];
				if(obj == null)
					return true;
				else
					return (bool)obj;
			}
			set
			{
				ViewState["EnabledDuringCallBack"] = value;
			}
		}


		/// <summary>
		/// Gets or sets the javascript function to execute on the client after the callback
		/// response is received.
		/// </summary>
		/// <remarks>
		/// The callback response is passed into the PostCallBackFunction as the one and only
		/// parameter.
		/// </remarks>
		/// <example>
		/// 	<code lang="JScript" description="This example shows a PostCallBackFunction that displays the error if there is one.">
		/// function AfterCallBack(result) {
		///   if (result.error != null &amp;&amp; result.error.length &gt; 0) {
		///     alert(result.error);
		///   }
		/// }
		///     </code>
		/// </example>
		[Category("Anthem")]
		[DefaultValue("")]
		[Description("The javascript function to call on the client after the callback response is received.")]
		public virtual string PostCallBackFunction
		{
			get
			{
				string text = (string)this.ViewState["PostCallBackFunction"];
				if(text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				ViewState["PostCallBackFunction"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the javascript function to execute on the client before the callback
		/// is made.
		/// </summary>
		/// <remarks>The function should return false on the client to cancel the callback.</remarks>
		[Category("Anthem")]
		[DefaultValue("")]
		[Description("The javascript function to call on the client before the callback is made.")]
		public virtual string PreCallBackFunction
		{
			get
			{
				string text = (string)this.ViewState["PreCallBackFunction"];
				if(text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				ViewState["PreCallBackFunction"] = value;
			}
		}

		/// <summary>Gets or sets the text to display on the client during the callback.</summary>
		/// <remarks>
		/// If the HTML element that invoked the callback has a text value (such as &lt;input
		/// type="button" value="Run"&gt;) then the text of the element is updated during the
		/// callback, otherwise the associated &lt;label&gt; text is updated during the callback.
		/// If the element does not have a text value, and if there is no associated &lt;label&gt;,
		/// then this property is ignored.
		/// </remarks>
		[Category("Anthem")]
		[DefaultValue("")]
		[Description("The text to display during the callback.")]
		public virtual string TextDuringCallBack
		{
			get
			{
				string text = (string)this.ViewState["TextDuringCallBack"];
				if(text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				ViewState["TextDuringCallBack"] = value;
			}
		}

		#endregion

		#region ICallBackContainerControl implementation

		/// <summary>
		/// Gets or sets a value indicating whether the control should convert child control postbacks into callbacks.
		/// </summary>
		/// <value>
		/// 	<strong>true</strong> if the control should convert child control postbacks to
		/// callbacks; otherwise, <strong>false</strong>. The default is
		/// <strong>true</strong>.
		/// </value>
		/// <remarks>
		/// Only controls that <see cref="AnthemNxt.Manager"/> recognizes will be converted.
		/// </remarks>
		[Category("Anthem")]
		[DefaultValue(true)]
		[Description("True if this control should convert child control postbacks into callbacks.")]
		public virtual bool AddCallBacks
		{
			get
			{
				object obj = this.ViewState["AddCallBacks"];
				if(obj == null)
					return true;
				else
					return (bool)obj;
			}
			set
			{
				ViewState["AddCallBacks"] = value;
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