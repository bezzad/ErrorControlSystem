using System;
using System.Globalization;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace WaitSplash
{
    /// <summary>
    /// Interaction logic for Splash.xaml
    /// </summary>
    [UIPermissionAttribute(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
    public partial class Splash : Window
    {
        #region Properties

        protected object SyncLocker = new object();
        protected int ShowTimes = 0;
        protected long TickCount;
        protected DispatcherTimer TickCounter;
        protected bool SrcInitialized;
        public System.Windows.Forms.Control OwnerControl;

        #endregion

        #region Constructors

        public Splash()
        {
            this.InitializeComponent();

            this.SourceInitialized += delegate
            {
                SrcInitialized = true;

                CenterToParent();
            };

            TickCounter = new DispatcherTimer();
            TickCounter.Tick += TickCounter_Tick;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Centers to parent.
        /// </summary>
        /// <param name="ctrl">The <see cref="System.Windows.Forms.Control"/> control.</param>
        public void CenterToParent(System.Windows.Forms.Control ctrl)
        {
            // Get the handle to the non-WPF owner window
            IntPtr ownerWindowHandle = ctrl.Handle; // Get hWnd for non-WPF window

            // Set the owned WPF window’s owner with the non-WPF owner window
            WindowInteropHelper helper = new WindowInteropHelper(this);
            helper.Owner = ownerWindowHandle; 

            // Center window
            // Note - Need to use HwndSource to get handle to WPF owned window,
            //        and the handle only exists when SourceInitialized has been
            //        raised
            if (SrcInitialized)
            {
                // Get WPF size and location for non-WPF owner window
                int ownerLeft = ctrl.Left; // Get non-WPF owner’s Left
                int ownerWidth = ctrl.Width; // Get non-WPF owner’s Width
                int ownerTop = ctrl.Top; // Get non-WPF owner’s Top
                int ownerHeight = ctrl.Height; // Get non-WPF owner’s Height

                // Get transform matrix to transform non-WPF owner window
                // size and location units into device-independent WPF
                // size and location units
                HwndSource source = HwndSource.FromHwnd(helper.Handle);
                if (source == null || source.CompositionTarget == null) return;

                System.Windows.Media.Matrix matrix = source.CompositionTarget.TransformFromDevice;
                System.Windows.Point ownerWPFSize = matrix.Transform(new System.Windows.Point(ownerWidth, ownerHeight));
                System.Windows.Point ownerWPFPosition = matrix.Transform(new System.Windows.Point(ownerLeft, ownerTop));

                // Center WPF window
                this.WindowStartupLocation = WindowStartupLocation.Manual;
                this.Left = ownerWPFPosition.X + (ownerWPFSize.X - this.Width) / 2;
                this.Top = ownerWPFPosition.Y + (ownerWPFSize.Y - this.Height) / 2;

            }
        }

        /// <summary>
        /// Centers to parent.
        /// </summary>
        /// <param name="win">The <see cref="System.Windows.Window"/> window.</param>
        public void CenterToParent(System.Windows.Window win)
        {
            // Center WPF window
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = win.Left + (win.Width - this.Width) / 2;
            this.Top = win.Top + (win.Height - this.Height) / 2;
        }

        /// <summary>
        /// Centers to <see cref="System.Windows.Window"/> or <see cref="System.Windows.Forms.Control"/> parent.
        /// </summary>
        public void CenterToParent()
        {
            if (OwnerControl != null)
            {
                CenterToParent(OwnerControl);
            }
            else if (Owner != null)
            {
                CenterToParent(Owner);
            }
        }

        void TickCounter_Tick(object sender, System.EventArgs e)
        {
            lblTimeNo.Text = ((Environment.TickCount - TickCount) / 1000).ToString(CultureInfo.InvariantCulture);
        }


        /// <summary>
        /// Starts the wait splash and shown that
        /// </summary>
        public void Start()
        {
            Task.Run(() =>
            {
                lock (SyncLocker)
                {
                    if (++ShowTimes == 1)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            TickCount = Environment.TickCount;
                            TickCounter.Start();
                            this.Show();
                        });
                    }
                }
            });
        }

        /// <summary>
        /// Stop Wait Splash, and close that
        /// </summary>
        public void Stop()
        {
            Task.Run(() =>
            {
                lock (SyncLocker)
                {
                    if (--ShowTimes <= 0)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            this.Hide();
                            TickCounter.Stop();
                        });
                    }
                }
            });
        }

        #endregion
    }
}
