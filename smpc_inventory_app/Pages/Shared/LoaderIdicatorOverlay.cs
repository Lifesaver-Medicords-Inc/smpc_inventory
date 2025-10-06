using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Inventory_SMPC.Pages;
using smpc_inventory_app.Properties;

namespace smpc_invemtory_app.Pages.Shared
{
    public partial class LoaderIndicatorOverlay : UserControl
    {
        private static LoaderIndicatorOverlay _instance;
        private Timer _rotationTimer;
        private float _rotationAngle = 0f;
        private Image _originalImage;

        private LoaderIndicatorOverlay()
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;
            this.BackColor = Color.Transparent;
            this.Visible = true;
            this.Enabled = true;

            // Make sure pictureBox is centered
            pictureBox.BackColor = Color.Transparent;
            pictureBox.Anchor = AnchorStyles.None;

            // Spinner image
            if (pictureBox.Image == null)
                pictureBox.Image = Resources.spinner;
            _originalImage = pictureBox.Image;

            pictureBox.Location = new Point(
                (this.ClientSize.Width - pictureBox.Width) / 2,
                (this.ClientSize.Height - pictureBox.Height) / 2
            );

            // Rotate animation
            _rotationTimer = new Timer
            {
                Interval = 30
            };
            _rotationTimer.Tick += RotatePictureBox;
            _rotationTimer.Start();

            // Resize handler
            this.Resize += (s, e) =>
            {
                pictureBox.Location = new Point(
                    (this.ClientSize.Width - pictureBox.Width) / 2,
                    (this.ClientSize.Height - pictureBox.Height) / 2
                );
            };
        }

        public static void ShowOverlay()
        {
            var parentForm = Application.OpenForms
                .OfType<SMPC>()
                .FirstOrDefault();

            if (parentForm == null)
            {
                return;
            }

            // Add overlay to the main content panel
            Control overlayTarget = parentForm.Controls["MainContentPanel"] ?? parentForm;

            if (_instance == null || _instance.Parent == null)
            {
                _instance = new LoaderIndicatorOverlay();
                overlayTarget.Controls.Add(_instance);
            }

            // Bring to top
            overlayTarget.Controls.SetChildIndex(_instance, 0);
            _instance.Visible = true;
            _instance.BringToFront();

            // Force redraw
            overlayTarget.Invalidate(true);
            overlayTarget.Update();
        }

        public static void HideOverlay()
        {

            if (_instance != null && _instance.Parent != null)
            {
                _instance._rotationTimer?.Stop();
                _instance._rotationTimer?.Dispose();

                if (_instance.pictureBox.Image != null &&
                    _instance.pictureBox.Image != _instance._originalImage)
                {
                    _instance.pictureBox.Image.Dispose();
                }

                _instance.Parent.Controls.Remove(_instance);
                _instance.Dispose();
                _instance = null;
            }

        }

        private void RotatePictureBox(object sender, EventArgs e)
        {
            if (_originalImage == null)
                return;

            _rotationAngle += 5f;
            if (_rotationAngle >= 360f)
                _rotationAngle -= 360f;

            if (pictureBox.Image != null && pictureBox.Image != _originalImage)
            {
                pictureBox.Image.Dispose();
            }

            pictureBox.Image = RotateImage(_originalImage, _rotationAngle);
        }

        private Bitmap RotateImage(Image img, float angle)
        {
            Bitmap rotatedBmp = new Bitmap(img.Width, img.Height);
            rotatedBmp.SetResolution(img.HorizontalResolution, img.VerticalResolution);

            using (Graphics g = Graphics.FromImage(rotatedBmp))
            {
                g.TranslateTransform(img.Width / 2, img.Height / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-img.Width / 2, -img.Height / 2);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, new Point(0, 0));
            }

            return rotatedBmp;
        }
    }
}
