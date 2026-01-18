using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
#nullable enable

namespace PrisonerDilemma
{
    public class Slider
    {
        // Responsible for a slider and its text box.
        // Updates the text box if the slider is moved and vice versa
        // The value is always available as a float between minValue and maxValue
        // If it is disabled, the slider and text box are disabled but the value remains accessible
        // When it is changed, it first calls PermitValueChange to see if the change is allowed
        // If not, it reverts to the previous value
        public string Name { get; private set; }    // Autoproperty for the name of the slider
        public float Value { get { return _value; } set { setValue(value); } }
        public bool Enable { get { return _enabled; } set { enableMe(value); } }
        public string LabelText { get { return label.Text; } set { label.Text = value; } }
        public delegate void ValueChangedDelegate(float newValue);
        ValueChangedDelegate? ValueChanged;
        public delegate bool PermitValueChangeDelegate(float newValue);
        PermitValueChangeDelegate? PermitValueChange;
        
        // HMI objects, I have to keep the first two because they update each other
        private readonly TrackBar trackBar;
        private readonly TextBox textBox;
        private readonly Label label;   // I have to update its text

        // Other instantiation and initialisation
        private float minValue;
        private float maxValue;
        private float deltaValue;
        private string textFormat;

        // Backing stores
        private float _value;
        private bool _enabled = true;

        public Slider(SliderConstruction P)
        {
            // Initialise the local copies of the HMIs
            trackBar = P.TrackBar;
            textBox = P.TextBox;
            label = P.Label;

            // Initialise properties from the construction parameters
            Name = P.Name;
            _value = P.InitialValue;
            Enable = true;
            PermitValueChange = null;
            ValueChanged = null;

            // Set up the track bar
            trackBar.Minimum = 0;
            trackBar.Maximum = P.NPosns-1;
            if (P.NPosns < 2)
                throw new ArgumentException("Number of positions for slider must be at least 2"); 
            deltaValue = (P.MaxValue - P.MinValue) / (P.NPosns-1);
            trackBar.SmallChange = 1;
            trackBar.LargeChange = (int)Math.Sqrt(P.NPosns);
            trackBar.ValueChanged += trackBar_ValueChanged;

            // Set up the text box
            textFormat = P.TextFormat;
            textBox.Leave += textBox_Leave;
            updateTextBox();

            // and the label
            LabelText = P.InitLabelText ?? LabelText;    // Setter handles it

            // Initialise the two delegates
            ValueChanged = P.ValueChanged;
            PermitValueChange = P.PermitValueChange;

            minValue = P.MinValue;
            maxValue = P.MaxValue;
            deltaValue = (maxValue - minValue) / (P.NPosns - 1);
        }

        private void setValue(float PValue)
        {
            if (PValue < minValue || PValue > maxValue)
                throw new ArgumentOutOfRangeException($"Value {PValue} is out of range [{minValue},{maxValue}] for slider {Name}");
            _value = PValue;
            updateTextBox();
            updateTrackBar();
        }

        private void trackBar_ValueChanged(object PSender, EventArgs PE)
        {   // The value has changed from the track bar - calculate the new value and update the text box
            _value = minValue + ((TrackBar)PSender).Value * deltaValue;
            if (_value > maxValue) _value = maxValue; // Just in case of rounding errors
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
            textBox.Text = _value.ToString(textFormat);
        }

        private void updateTrackBar()
        {
            int trackbarValue = (int)Math.Round((_value - minValue) / deltaValue);
            if (trackbarValue < trackBar.Minimum || trackbarValue > trackBar.Maximum) 
                throw new Exception("Calculated trackbar value out of range");
            trackBar.Value = trackbarValue;
        }

        private void enableMe(bool PEnable)
        {
            _enabled = PEnable;
            trackBar.Enabled = PEnable;
            textBox.Enabled = PEnable;
        }
    }

    public struct SliderConstruction
    {   // Parameters required to create a slider
        public string Name;
        public TrackBar TrackBar;
        public TextBox TextBox;
        public Label Label;
        public float MinValue;
        public float InitialValue;
        public float MaxValue;
        public int NPosns;
        public string TextFormat;
        public string? InitLabelText;
        public Slider.PermitValueChangeDelegate? PermitValueChange;
        public Slider.ValueChangedDelegate? ValueChanged;
    }
}
