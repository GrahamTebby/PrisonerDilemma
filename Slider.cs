using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Xsl;
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
        public delegate void ValueChangedDelegate(float PNewValue);
        public ValueChangedDelegate? ValueChanged;
        public delegate bool PermitValueChangeDelegate(float PNewValue);
        public PermitValueChangeDelegate? PermitValueChange;

        // HMI objects, I have to keep the first two because they update each other
        protected readonly TrackBar trackBar;
        protected readonly TextBox textBox;
        protected readonly Label label;   // I have to update its text

        // Other instantiation and initialisation
        protected float minValue;
        protected float maxValue;
        protected float deltaValue;
        protected string textFormat;

        // Backing stores
        protected float _value;
        protected bool _enabled = true;

        public Slider(SliderConstruction P)
        {
            // Initialise the local copies of the HMIs
            trackBar = P.TrackBar;
            textBox = P.TextBox;
            label = P.Label;

            // Set up the track bar
            minValue = P.MinValue;
            maxValue = P.MaxValue;
            if (P.NPosns < 2)
                throw new ArgumentException("Number of positions for slider must be at least 2");
            deltaValue = (maxValue - minValue) / (P.NPosns - 1);
            trackBar.Minimum = 0;
            trackBar.Maximum = P.NPosns - 1;
            trackBar.SmallChange = 1;
            trackBar.LargeChange = (int)Math.Sqrt(P.NPosns);
            trackBar.ValueChanged += trackBar_ValueChanged;

            // Set up the text box
            textFormat = P.TextFormat;
            textBox.Leave += textBox_Leave;
            updateTextBox();

            // and the label
            LabelText = P.InitLabelText ?? LabelText;    // Setter handles it

            // Initialise the value to the initial value
            setValue(P.InitialValue);

            // Initialise the two delegates
            ValueChanged = P.ValueChanged;
            PermitValueChange = P.PermitValueChange;

            // What's left?
            Name = P.Name;
            // Don't need to set Enable as the HMI is already enabled and _enabled is initialised
        }

        protected void setValue(float PValue)
        {
            if (PValue < minValue || PValue > maxValue)
                throw new ArgumentOutOfRangeException($"Value {PValue} is out of range [{minValue},{maxValue}] for slider {Name}");
            _value = PValue;
            updateTextBox();
            updateTrackBar();
            ValueChanged?.Invoke(_value);
        }

        protected void trackBar_ValueChanged(object PSender, EventArgs PE)
        {
            trackBar_ValueChanged1(PSender, PE);
            ValueChanged?.Invoke(_value);
        }
        protected void trackBar_ValueChanged1(object PSender, EventArgs PE)
        {   // The value has changed from the track bar - calculate the new value and update the text box
            _value = minValue + ((TrackBar)PSender).Value * deltaValue;
            if (_value > maxValue) _value = maxValue; // Just in case of rounding errors
            updateTextBox();
        }

        protected void textBox_Leave(object PSender, EventArgs PE)
        {
            textBox_Leave1(PSender, PE);
            ValueChanged?.Invoke(_value);
        }


        protected void textBox_Leave1(object PSender, EventArgs PE)
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

        virtual protected void updateTextBox()
        {
            textBox.Text = _value.ToString(textFormat);
        }

        protected void updateTrackBar()
        {
            int trackbarValue = (int)Math.Round((_value - minValue) / deltaValue);
            if (trackbarValue < trackBar.Minimum || trackbarValue > trackBar.Maximum)
                throw new Exception("Calculated trackbar value out of range");
            trackBar.Value = trackbarValue;
        }

        protected void enableMe(bool PEnable)
        {
            _enabled = PEnable;
            trackBar.Enabled = PEnable;
            textBox.Enabled = PEnable;
        }

        // !!! Don't have anything about permitting the slider to give actual frame rates, not index position
        // !!! Also need to make way for timer and background worker
    }

    public class SliderFR : Slider
    {   // A slider for the frame rate

        public SliderFR(SliderConstruction P) : base(P) 
        {
            textBox.ReadOnly = false;
        }

        protected override void updateTextBox()
        {
            if (_value < 0F)
                textBox.Text = "Max";
            else
                textBox.Text =
                _value.ToString(textFormat);
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
    };
}