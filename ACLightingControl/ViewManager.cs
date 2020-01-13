using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using System.Windows.Threading;
using ViewLibrary.Model.Effects;
using ViewLibrary.View;
using ViewLibrary.ViewModel;

namespace ACLightingControl
{
    public class ViewManager
    {
        // This allows code to be run on a GUI thread
        private Window _HiddenWindow;

        private System.ComponentModel.IContainer _Components;
        // The Windows system tray class
        private NotifyIcon _NotifyIcon;

        private MainWindowView _MainWindowView;
        private MainWindowViewModel _MainWindowViewModel;

        private ToolStripMenuItem _ExitMenuItem;

        public ViewManager()
        {
            _Components = new System.ComponentModel.Container();
            _NotifyIcon = new System.Windows.Forms.NotifyIcon(_Components)
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = Properties.Resources.dd_logo_white_and_black,
                Text = "dataDyne Laboratories AC Lighting Control",
                Visible = true,
            };

            _NotifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            _NotifyIcon.DoubleClick += _NotifyIcon_DoubleClick; ;
            _NotifyIcon.MouseUp += NotifyIcon_MouseUp;

            _MainWindowViewModel = new MainWindowViewModel(Dispatcher.CurrentDispatcher);

            _MainWindowViewModel.Icon = AppIcon;

            _HiddenWindow = new System.Windows.Window();
            _HiddenWindow.Hide();
        }

        private void _NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowMainWindowView();
        }

        System.Windows.Media.ImageSource AppIcon
        {
            get
            {
                System.Drawing.Icon icon = Properties.Resources.dd_logo_white_and_black;
                return Imaging.CreateBitmapSourceFromHIcon(
                    icon.Handle,
                    System.Windows.Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
        }

        private void DisplayStatusMessage(string text)
        {
            _HiddenWindow.Dispatcher.Invoke(delegate
            {
                _NotifyIcon.BalloonTipText = text;
                // The timeout is ignored on recent Windows
                _NotifyIcon.ShowBalloonTip(3000);
            });
        }
        
        private ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, string tooltipText, EventHandler eventHandler)
        {
            var item = new ToolStripMenuItem(displayText);
            if (eventHandler != null)
            {
                item.Click += eventHandler;
            }

            item.ToolTipText = tooltipText;
            return item;
        }

        private void ShowMainWindowView()
        { 
            if (_MainWindowView == null)
            {
                _MainWindowView = new MainWindowView();
                _MainWindowView.DataContext = _MainWindowViewModel;

                _MainWindowView.Closing += (s, e) => _MainWindowView = null;
                _MainWindowView.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                ElementHost.EnableModelessKeyboardInterop(_MainWindowView);
                _MainWindowView.Show();
            }
            else
            {
                _MainWindowView.Activate();
            }

            _MainWindowView.Icon = AppIcon;
        }

        private void ShowMainForm_Click(object sender, EventArgs e)
        {
            ShowMainWindowView();
        }

        private void ShowWebSite_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://datadyne.bretthewitt.net/nas.php");
        }

        private void ExitItem_Click(object sender, EventArgs e)
        {
            _NotifyIcon.Visible = false;
            _NotifyIcon.Dispose();
            EffectManager.Instance.StopEffect();
            System.Windows.Forms.Application.Exit();
        }

        private void NotifyIcon_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(_NotifyIcon, null);
            }
        }
        
        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;

            if (_NotifyIcon.ContextMenuStrip.Items.Count == 0)
            {
                _NotifyIcon.ContextMenuStrip.Items.Add(ToolStripMenuItemWithHandler("&Show AC Lighting Control", "Shows the Alienware Chroma Lighting Control Display", ShowMainForm_Click));
                _NotifyIcon.ContextMenuStrip.Items.Add(ToolStripMenuItemWithHandler("dataDyne &Website", "Navigates to the dataDyne Laboratories Web Site", ShowWebSite_Click));
                _NotifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                _ExitMenuItem = ToolStripMenuItemWithHandler("&Exit", "Exits System Tray App", ExitItem_Click);
                _NotifyIcon.ContextMenuStrip.Items.Add(_ExitMenuItem);
            }
        }
    }
}
