using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using LiveSplit.DarkSouls.Controls;
using LiveSplit.DarkSouls.Data;
using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;

namespace LiveSplit.DarkSouls
{
	public class SoulsComponent : IComponent
	{
		public const string DisplayName = "Dark Souls Autosplitter";

		private TimerModel timer;
		private SplitCollection splitCollection;
		private SoulsSettings settings;
		private SoulsMasterControl masterControl;

		public SoulsComponent()
		{
			splitCollection = new SplitCollection();
			settings = new SoulsSettings(splitCollection);
			masterControl = new SoulsMasterControl();
		}

		public string ComponentName => DisplayName;

		public float HorizontalWidth => 0;
		public float MinimumHeight => 0;
		public float VerticalHeight => 0;
		public float MinimumWidth => 0;
		public float PaddingTop => 0;
		public float PaddingBottom => 0;
		public float PaddingLeft => 0;
		public float PaddingRight => 0;

		public IDictionary<string, Action> ContextMenuControls => null;

		public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
		{
		}

		public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
		{
		}

		public Control GetSettingsControl(LayoutMode mode)
		{
			return masterControl;
		}

		public XmlNode GetSettings(XmlDocument document)
		{
			return settings.Save(document);
		}

		public void SetSettings(XmlNode settings)
		{
			this.settings.Load(settings);

			masterControl.Refresh(splitCollection.Splits, this.settings);
		}

		public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
		{
			if (timer == null)
			{
				timer = new TimerModel();
				timer.CurrentState = state;

				state.OnSplit += (sender, args) => { splitCollection.OnSplit(); };
				state.OnUndoSplit += (sender, args) => { splitCollection.OnUndoSplit(); };
				state.OnSkipSplit += (sender, args) => { splitCollection.OnSkipSplit(); };
				state.OnReset += (sender, value) => { splitCollection.OnReset(); };
			}
		}

		public void Refresh()
		{
		}

		public void Dispose()
		{
		}
	}
}
