using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace liveTranscribe
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer? components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            startButton = new Button();
            stopButton = new Button();
            statusLabel = new Label();
            transcriptTextBox = new TextBox();
            modelLabel = new Label();
            modelComboBox = new ComboBox();
            temperatureLabel = new Label();
            temperatureNumeric = new NumericUpDown();
            beamLabel = new Label();
            beamSizeNumeric = new NumericUpDown();
            bestOfLabel = new Label();
            bestOfNumeric = new NumericUpDown();
            translateCheckBox = new CheckBox();
            autoDetectLanguageCheckBox = new CheckBox();
            languageLabel = new Label();
            languageTextBox = new TextBox();
            autodetectOnceCheckBox = new CheckBox();
            partialResultsCheckBox = new CheckBox();
            tokenDetailsCheckBox = new CheckBox();
            vadThresholdLabel = new Label();
            vadThresholdNumeric = new NumericUpDown();
            vadGapLabel = new Label();
            vadGapNumeric = new NumericUpDown();
            threadsLabel = new Label();
            threadsNumeric = new NumericUpDown();
            promptLabel = new Label();
            promptTextBox = new TextBox();
            ((System.ComponentModel.ISupportInitialize)temperatureNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)beamSizeNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bestOfNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)vadThresholdNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)vadGapNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)threadsNumeric).BeginInit();
            SuspendLayout();
            // 
            // startButton
            // 
            startButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            startButton.Location = new Point(654, 122);
            startButton.Name = "startButton";
            startButton.Size = new Size(66, 27);
            startButton.TabIndex = 0;
            startButton.Text = "Start";
            startButton.UseVisualStyleBackColor = true;
            startButton.Click += button1_Click;
            // 
            // stopButton
            // 
            stopButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            stopButton.Enabled = false;
            stopButton.Location = new Point(726, 122);
            stopButton.Name = "stopButton";
            stopButton.Size = new Size(62, 27);
            stopButton.TabIndex = 1;
            stopButton.Text = "Stop";
            stopButton.UseVisualStyleBackColor = true;
            stopButton.Click += button2_Click;
            // 
            // statusLabel
            // 
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(12, 9);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(66, 15);
            statusLabel.TabIndex = 2;
            statusLabel.Text = "Status: Idle";
            // 
            // transcriptTextBox
            // 
            transcriptTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            transcriptTextBox.Location = new Point(12, 168);
            transcriptTextBox.Multiline = true;
            transcriptTextBox.Name = "transcriptTextBox";
            transcriptTextBox.ReadOnly = true;
            transcriptTextBox.ScrollBars = ScrollBars.Vertical;
            transcriptTextBox.Size = new Size(776, 270);
            transcriptTextBox.TabIndex = 3;
            // 
            // modelLabel
            // 
            modelLabel.AutoSize = true;
            modelLabel.Location = new Point(12, 38);
            modelLabel.Name = "modelLabel";
            modelLabel.Size = new Size(42, 15);
            modelLabel.TabIndex = 4;
            modelLabel.Text = "Model";
            // 
            // modelComboBox
            // 
            modelComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            modelComboBox.FormattingEnabled = true;
            modelComboBox.Location = new Point(120, 34);
            modelComboBox.Name = "modelComboBox";
            modelComboBox.Size = new Size(150, 23);
            modelComboBox.TabIndex = 5;
            // 
            // temperatureLabel
            // 
            temperatureLabel.AutoSize = true;
            temperatureLabel.Location = new Point(290, 38);
            temperatureLabel.Name = "temperatureLabel";
            temperatureLabel.Size = new Size(77, 15);
            temperatureLabel.TabIndex = 6;
            temperatureLabel.Text = "Temperature";
            // 
            // temperatureNumeric
            // 
            temperatureNumeric.DecimalPlaces = 2;
            temperatureNumeric.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            temperatureNumeric.Location = new Point(373, 34);
            temperatureNumeric.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            temperatureNumeric.Name = "temperatureNumeric";
            temperatureNumeric.Size = new Size(70, 23);
            temperatureNumeric.TabIndex = 7;
            // 
            // beamLabel
            // 
            beamLabel.AutoSize = true;
            beamLabel.Location = new Point(452, 38);
            beamLabel.Name = "beamLabel";
            beamLabel.Size = new Size(63, 15);
            beamLabel.TabIndex = 8;
            beamLabel.Text = "Beam size";
            // 
            // beamSizeNumeric
            // 
            beamSizeNumeric.Location = new Point(521, 34);
            beamSizeNumeric.Maximum = new decimal(new int[] { 16, 0, 0, 0 });
            beamSizeNumeric.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            beamSizeNumeric.Name = "beamSizeNumeric";
            beamSizeNumeric.Size = new Size(60, 23);
            beamSizeNumeric.TabIndex = 9;
            beamSizeNumeric.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // bestOfLabel
            // 
            bestOfLabel.AutoSize = true;
            bestOfLabel.Location = new Point(598, 38);
            bestOfLabel.Name = "bestOfLabel";
            bestOfLabel.Size = new Size(44, 15);
            bestOfLabel.TabIndex = 10;
            bestOfLabel.Text = "Best of";
            // 
            // bestOfNumeric
            // 
            bestOfNumeric.Location = new Point(648, 34);
            bestOfNumeric.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            bestOfNumeric.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            bestOfNumeric.Name = "bestOfNumeric";
            bestOfNumeric.Size = new Size(60, 23);
            bestOfNumeric.TabIndex = 11;
            bestOfNumeric.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // translateCheckBox
            // 
            translateCheckBox.AutoSize = true;
            translateCheckBox.Location = new Point(12, 128);
            translateCheckBox.Name = "translateCheckBox";
            translateCheckBox.Size = new Size(137, 19);
            translateCheckBox.TabIndex = 12;
            translateCheckBox.Text = "Translate to English";
            translateCheckBox.UseVisualStyleBackColor = true;
            // 
            // autoDetectLanguageCheckBox
            // 
            autoDetectLanguageCheckBox.AutoSize = true;
            autoDetectLanguageCheckBox.Location = new Point(522, 67);
            autoDetectLanguageCheckBox.Name = "autoDetectLanguageCheckBox";
            autoDetectLanguageCheckBox.Size = new Size(142, 19);
            autoDetectLanguageCheckBox.TabIndex = 13;
            autoDetectLanguageCheckBox.Text = "Auto detect language";
            autoDetectLanguageCheckBox.UseVisualStyleBackColor = true;
            // 
            // languageLabel
            // 
            languageLabel.AutoSize = true;
            languageLabel.Location = new Point(200, 67);
            languageLabel.Name = "languageLabel";
            languageLabel.Size = new Size(61, 15);
            languageLabel.TabIndex = 14;
            languageLabel.Text = "Language";
            // 
            // languageTextBox
            // 
            languageTextBox.Location = new Point(267, 63);
            languageTextBox.Name = "languageTextBox";
            languageTextBox.Size = new Size(90, 23);
            languageTextBox.TabIndex = 15;
            // 
            // autodetectOnceCheckBox
            // 
            autodetectOnceCheckBox.AutoSize = true;
            autodetectOnceCheckBox.Location = new Point(522, 92);
            autodetectOnceCheckBox.Name = "autodetectOnceCheckBox";
            autodetectOnceCheckBox.Size = new Size(90, 19);
            autodetectOnceCheckBox.TabIndex = 16;
            autodetectOnceCheckBox.Text = "Detect once";
            autodetectOnceCheckBox.UseVisualStyleBackColor = true;
            // 
            // partialResultsCheckBox
            // 
            partialResultsCheckBox.AutoSize = true;
            partialResultsCheckBox.Location = new Point(12, 99);
            partialResultsCheckBox.Name = "partialResultsCheckBox";
            partialResultsCheckBox.Size = new Size(134, 19);
            partialResultsCheckBox.TabIndex = 17;
            partialResultsCheckBox.Text = "Show partial results";
            partialResultsCheckBox.UseVisualStyleBackColor = true;
            // 
            // tokenDetailsCheckBox
            // 
            tokenDetailsCheckBox.AutoSize = true;
            tokenDetailsCheckBox.Location = new Point(172, 99);
            tokenDetailsCheckBox.Name = "tokenDetailsCheckBox";
            tokenDetailsCheckBox.Size = new Size(139, 19);
            tokenDetailsCheckBox.TabIndex = 18;
            tokenDetailsCheckBox.Text = "Include token details";
            tokenDetailsCheckBox.UseVisualStyleBackColor = true;
            // 
            // vadThresholdLabel
            // 
            vadThresholdLabel.AutoSize = true;
            vadThresholdLabel.Location = new Point(320, 99);
            vadThresholdLabel.Name = "vadThresholdLabel";
            vadThresholdLabel.Size = new Size(85, 15);
            vadThresholdLabel.TabIndex = 19;
            vadThresholdLabel.Text = "VAD threshold";
            // 
            // vadThresholdNumeric
            // 
            vadThresholdNumeric.DecimalPlaces = 2;
            vadThresholdNumeric.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            vadThresholdNumeric.Location = new Point(411, 95);
            vadThresholdNumeric.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            vadThresholdNumeric.Name = "vadThresholdNumeric";
            vadThresholdNumeric.Size = new Size(60, 23);
            vadThresholdNumeric.TabIndex = 20;
            // 
            // vadGapLabel
            // 
            vadGapLabel.AutoSize = true;
            vadGapLabel.Location = new Point(477, 99);
            vadGapLabel.Name = "vadGapLabel";
            vadGapLabel.Size = new Size(53, 15);
            vadGapLabel.TabIndex = 21;
            vadGapLabel.Text = "VAD gap";
            // 
            // vadGapNumeric
            // 
            vadGapNumeric.DecimalPlaces = 2;
            vadGapNumeric.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            vadGapNumeric.Location = new Point(536, 95);
            vadGapNumeric.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            vadGapNumeric.Name = "vadGapNumeric";
            vadGapNumeric.Size = new Size(60, 23);
            vadGapNumeric.TabIndex = 22;
            // 
            // threadsLabel
            // 
            threadsLabel.AutoSize = true;
            threadsLabel.Location = new Point(12, 67);
            threadsLabel.Name = "threadsLabel";
            threadsLabel.Size = new Size(48, 15);
            threadsLabel.TabIndex = 23;
            threadsLabel.Text = "Threads";
            // 
            // threadsNumeric
            // 
            threadsNumeric.Location = new Point(120, 63);
            threadsNumeric.Maximum = new decimal(new int[] { 256, 0, 0, 0 });
            threadsNumeric.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            threadsNumeric.Name = "threadsNumeric";
            threadsNumeric.Size = new Size(60, 23);
            threadsNumeric.TabIndex = 24;
            threadsNumeric.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // promptLabel
            // 
            promptLabel.AutoSize = true;
            promptLabel.Location = new Point(172, 132);
            promptLabel.Name = "promptLabel";
            promptLabel.Size = new Size(77, 15);
            promptLabel.TabIndex = 25;
            promptLabel.Text = "Initial prompt";
            // 
            // promptTextBox
            // 
            promptTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            promptTextBox.Location = new Point(255, 128);
            promptTextBox.Name = "promptTextBox";
            promptTextBox.Size = new Size(393, 23);
            promptTextBox.TabIndex = 26;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(promptTextBox);
            Controls.Add(promptLabel);
            Controls.Add(threadsNumeric);
            Controls.Add(threadsLabel);
            Controls.Add(vadGapNumeric);
            Controls.Add(vadGapLabel);
            Controls.Add(vadThresholdNumeric);
            Controls.Add(vadThresholdLabel);
            Controls.Add(tokenDetailsCheckBox);
            Controls.Add(partialResultsCheckBox);
            Controls.Add(autodetectOnceCheckBox);
            Controls.Add(languageTextBox);
            Controls.Add(languageLabel);
            Controls.Add(autoDetectLanguageCheckBox);
            Controls.Add(translateCheckBox);
            Controls.Add(bestOfNumeric);
            Controls.Add(bestOfLabel);
            Controls.Add(beamSizeNumeric);
            Controls.Add(beamLabel);
            Controls.Add(temperatureNumeric);
            Controls.Add(temperatureLabel);
            Controls.Add(modelComboBox);
            Controls.Add(modelLabel);
            Controls.Add(transcriptTextBox);
            Controls.Add(statusLabel);
            Controls.Add(stopButton);
            Controls.Add(startButton);
            Name = "Form1";
            Text = "Live Transcribe";
            ((System.ComponentModel.ISupportInitialize)temperatureNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)beamSizeNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)bestOfNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)vadThresholdNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)vadGapNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)threadsNumeric).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button startButton;
        private Button stopButton;
        private Label statusLabel;
        private TextBox transcriptTextBox;
        private Label modelLabel;
        private ComboBox modelComboBox;
        private Label temperatureLabel;
        private NumericUpDown temperatureNumeric;
        private Label beamLabel;
        private NumericUpDown beamSizeNumeric;
        private Label bestOfLabel;
        private NumericUpDown bestOfNumeric;
        private CheckBox translateCheckBox;
        private CheckBox autoDetectLanguageCheckBox;
        private Label languageLabel;
        private TextBox languageTextBox;
        private CheckBox autodetectOnceCheckBox;
        private CheckBox partialResultsCheckBox;
        private CheckBox tokenDetailsCheckBox;
        private Label vadThresholdLabel;
        private NumericUpDown vadThresholdNumeric;
        private Label vadGapLabel;
        private NumericUpDown vadGapNumeric;
        private Label threadsLabel;
        private NumericUpDown threadsNumeric;
        private Label promptLabel;
        private TextBox promptTextBox;
    }
}
