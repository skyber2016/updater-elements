using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Markup;

namespace Update.Classes.WebBrowserOverlayWF;

public partial class WebBrowserOverlay : Window, IComponentConnector
{
	private FrameworkElement _placementTarget;

	public static WebBrowserOverlay Current;

	public WebBrowser WebBrowser => _wb;

	public WebBrowserOverlay(FrameworkElement placementTarget)
	{
		InitializeComponent();
		Current = this;
		_placementTarget = placementTarget;
		Window owner = Window.GetWindow(placementTarget);
		owner.LocationChanged += delegate
		{
			OnSizeLocationChanged();
		};
		_placementTarget.SizeChanged += delegate
		{
			OnSizeLocationChanged();
		};
		if (owner.IsVisible)
		{
			base.Owner = owner;
			Show();
			return;
		}
		owner.IsVisibleChanged += delegate
		{
			if (owner.IsVisible)
			{
				base.Owner = owner;
				Show();
			}
		};
	}

	protected override void OnClosing(CancelEventArgs e)
	{
		base.OnClosing(e);
		if (!e.Cancel)
		{
			base.Dispatcher.BeginInvoke((Action)delegate
			{
				base.Owner.Close();
			});
		}
	}

	private void OnSizeLocationChanged()
	{
		Point point = _placementTarget.TranslatePoint(default(Point), base.Owner);
		Point point2 = new Point(_placementTarget.ActualWidth, _placementTarget.ActualHeight);
		HwndSource obj = (HwndSource)PresentationSource.FromVisual(base.Owner);
		HwndTarget compositionTarget = obj.CompositionTarget;
		point = compositionTarget.TransformToDevice.Transform(point);
		point2 = compositionTarget.TransformToDevice.Transform(point2);
		Win32.POINT lpPoint = new Win32.POINT(point);
		Win32.ClientToScreen(obj.Handle, ref lpPoint);
		Win32.POINT pOINT = new Win32.POINT(point2);
		Win32.MoveWindow(((HwndSource)PresentationSource.FromVisual(this)).Handle, lpPoint.X, lpPoint.Y, pOINT.X, pOINT.Y, bRepaint: true);
	}
}
