using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Discord_Event_time
{
    public partial class Form1 : Form
    {
        private readonly string _settingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Discord Event time",
            "window.settings"
        );

        public Form1()
        {
            InitializeComponent();

            this.Load += Form1_Load;
            this.FormClosing += Form1_FormClosing;

            // Set the clock icon
            try
            {
                this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
            catch
            {
                // If loading fails, use default
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            if (version != null)
            {
                label2.Text = $"{label2.Text} v{version.Major}.{version.Minor}.{version.Build}";
            }
            LoadWindowPosition();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveWindowPosition();
        }

        private void LoadWindowPosition()
        {
            try
            {
                if (!File.Exists(_settingsPath))
                    return;

                var lines = File.ReadAllLines(_settingsPath);
                if (lines.Length < 5)
                    return;

                if (!int.TryParse(lines[0], out var x)) return;
                if (!int.TryParse(lines[1], out var y)) return;
                if (!int.TryParse(lines[2], out var w)) return;
                if (!int.TryParse(lines[3], out var h)) return;
                var state = lines[4];

                var rect = new Rectangle(x, y, w, h);

                // Validate that at least part of the window will be on a screen
                bool visible = Screen.AllScreens.Any(s => s.WorkingArea.IntersectsWith(rect));
                if (!visible)
                    return;

                this.StartPosition = FormStartPosition.Manual;
                this.Bounds = rect;

                if (string.Equals(state, "Maximized", StringComparison.OrdinalIgnoreCase))
                    this.WindowState = FormWindowState.Maximized;
                else
                    this.WindowState = FormWindowState.Normal;
            }
            catch
            {
                // Ignore and use default position
            }
        }

        private void SaveWindowPosition()
        {
            try
            {
                var dir = Path.GetDirectoryName(_settingsPath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                var bounds = this.WindowState == FormWindowState.Normal ? this.Bounds : this.RestoreBounds;
                var state = this.WindowState == FormWindowState.Maximized ? "Maximized" : "Normal";

                File.WriteAllLines(_settingsPath, new[]
                {
                    bounds.X.ToString(),
                    bounds.Y.ToString(),
                    bounds.Width.ToString(),
                    bounds.Height.ToString(),
                    state
                });
            }
            catch
            {
                // Ignore failures to save settings
            }
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime localDateTime = dateTimePicker.Value;
                DateTime utcDateTime = localDateTime.ToUniversalTime();
                long unixTimestamp = ((DateTimeOffset)utcDateTime).ToUnixTimeSeconds();

                // Show GMT/Zulu time at the start
                string gmtTime = utcDateTime.ToString("d MMM yyyy HH:mm");
                string prefix = $"GMT/Zulu: {gmtTime} your time is ";

                string timeFormat = chkFull.Checked ? "f" : "t";  // :f for full, :t for short time
                string formatted = $"<t:{unixTimestamp}:{timeFormat}>";

                var parts = new System.Collections.Generic.List<string>();
                parts.Add(formatted);
                if (chkRelative.Checked) parts.Add($"<t:{unixTimestamp}:R>");
                
                string output = prefix + string.Join(" ", parts);
                outputBox.Text = output;
            }
            catch (Exception ex)
            {
                try
                {
                    toolStripStatusLabel1.Text = $"Error: {ex.Message}";
                    statusTimer.Start();
                }
                catch
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(outputBox.Text))
                return;

            try
            {
                Clipboard.SetText(outputBox.Text);
                toolStripStatusLabel1.Text = "Copied to clipboard!";
                statusTimer.Start();
            }
            catch (Exception ex)
            {
                try
                {
                    toolStripStatusLabel1.Text = $"Copy failed: {ex.Message}";
                    statusTimer.Start();
                }
                catch
                {
                    MessageBox.Show($"Copy failed: {ex.Message}");
                }
            }
        }

        private void statusTimer_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = string.Empty;
            statusTimer.Stop();
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            if (version != null)
            {
                label2.Text = $"{label2.Text} v{version.Major}.{version.Minor}.{version.Build}";
            }
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {

        }

        private void chkFull_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkRelative_CheckedChanged(object sender, EventArgs e)
        {
            // Regenerate the output when checkbox is toggled
            generateButton_Click(null, null);
        }
    }
}
