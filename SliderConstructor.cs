using System.Windows.Forms;

namespace PrisonerDilemma
{
    internal class SliderConstructor
    {
        public string Name { get; set; }
        public TrackBar TrackBar { get; set; }
        public TextBox TextBox { get; set; }
        public Label Label { get; set; }
        public string LabelText { get; set; }
        public float MinValue { get; set; }
        public float InitialValue { get; set; }
        public float MaxValue { get; set; }
        public int NPosns { get; set; }
        public string TextFormat { get; set; }
        public string InitLabelText { get; set; }
        public object PermitValueChange { get; set; }
        public object ValueChanged { get; set; }
    }
}