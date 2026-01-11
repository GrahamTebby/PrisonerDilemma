using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PrisonerDilemma
{
    internal class Slider
    {
        // Responsible for a slider and its text box.
        // Updates the text box if the slider is moved and vice versa
        // The value is always available as a float between minValue and maxValue
        // If it is disabled, the slider and text box are disabled but the value remains accessible

        public string Name { get; private set; }
        public float Value { get; private set; }
        public bool Enable { get { return _enabled; } set { enableMe(value); } }
        public string LabelText { get; set; }

        public struct SliderParms
        {   // Parameters required to create a slider
            public string Name;
            public System.Windows.Forms.TrackBar TrackBar;
            public System.Windows.Forms.TextBox TextBox;
            public System.Windows.Forms.Label Label;
            public string LabelText;
            public float MinValue;
            public float InitialValue;
            public float MaxValue;
            public string TextFormat;
        }

        private readonly System.Windows.Forms.TrackBar trackBar;
        private readonly System.Windows.Forms.TextBox textBox;
        private float minValue = 0F;
        private float maxValue = 1F;
        private string textFormat;
        private bool _enabled = true;

        public Slider(SliderParms P)
        {
            Name = P.Name;
            LabelText = P.LabelText;
            trackBar = P.TrackBar;
            textBox = P.TextBox;
            minValue = P.MinValue;
            Value = P.InitialValue;
            maxValue = P.MaxValue;
            Enable = true;
            textFormat = P.TextFormat;
            // Event handlers
            trackBar.ValueChanged += trackBar_ValueChange;
            textBox.Leave += textBox_Leave;
            // Set track bar range
            trackBar.Minimum = 0;
            trackBar.Maximum = 100; // Using 0.01 increments
            trackBar.SmallChange = 1;
            trackBar.LargeChange = 10;
        }

        private void trackBar_ValueChange(object PSender, EventArgs PE)
        {   // The value has changed from the track bar - calculate the new value and update the text box
            float range = maxValue - minValue;
            Value = minValue + (float)(range * trackBar.Value) / trackBar.Maximum;
            updateTextBox();
        }

        private void textBox_Leave(object PSender, EventArgs PE)
        {   // The text box has lost focus - validate and update the track bar
            if (trackBar == null) return; // There is no trackbar to update
            if (float.TryParse(textBox.Text, out float newValue))
            {   // We have parsed the new value
                if (newValue >= minValue && newValue <= maxValue)
                {   // Valid input - update value and track bar
                    Value = newValue;
                    updateTrackBar();
                }
            }
            // If we get here, the input was either valid or invalid; update the text box either way
            updateTextBox();
        }

        private void updateTextBox()
        {
            textBox.Text = Value.ToString(textFormat);
        }

        private void updateTrackBar()
        {
            float range = maxValue - minValue;
            trackBar.Value = (int)(((Value - minValue) * trackBar.Maximum) / range);
        }

        private void enableMe(bool PEnable)
        {
            _enabled = PEnable;
            trackBar.Enabled = PEnable;
            textBox.Enabled = PEnable;
        }
    }
}
