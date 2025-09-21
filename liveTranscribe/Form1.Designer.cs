namespace liveTranscribe
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            notifyIcon1 = new NotifyIcon(components);
            startButton = new Button();
            stopButton = new Button();
            statusLabel = new Label();
            transcriptTextBox = new TextBox();
            modelGroupBox = new GroupBox();
            modelComboBox = new ComboBox();
            modelLabel = new Label();
            languageGroupBox = new GroupBox();
            translateCheckBox = new CheckBox();
            languageComboBox = new ComboBox();
            autoDetectLanguageCheckBox = new CheckBox();
            samplingGroupBox = new GroupBox();
            patienceLabel = new Label();
            patienceNumericUpDown = new NumericUpDown();
            beamSizeNumericUpDown = new NumericUpDown();
            beamSizeLabel = new Label();
            bestOfNumericUpDown = new NumericUpDown();
            bestOfLabel = new Label();
            beamSearchRadioButton = new RadioButton();
            greedyRadioButton = new RadioButton();
            advancedGroupBox = new GroupBox();
            logProbNumericUpDown = new NumericUpDown();
            logProbLabel = new Label();
            noSpeechNumericUpDown = new NumericUpDown();
            noSpeechLabel = new Label();
            temperatureNumericUpDown = new NumericUpDown();
            temperatureLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)patienceNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)beamSizeNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bestOfNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)logProbNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)noSpeechNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)temperatureNumericUpDown).BeginInit();
            SuspendLayout();
            //
            // notifyIcon1
            //
            notifyIcon1.Text = "liveTranscribe";
            notifyIcon1.Visible = true;
            //
            // startButton
            //
            startButton.Location = new Point(20, 20);
            startButton.Name = "startButton";
            startButton.Size = new Size(140, 30);
            startButton.TabIndex = 0;
            startButton.Text = "Start Listening";
            startButton.UseVisualStyleBackColor = true;
            startButton.Click += startButton_Click;
            //
            // stopButton
            //
            stopButton.Enabled = false;
            stopButton.Location = new Point(170, 20);
            stopButton.Name = "stopButton";
            stopButton.Size = new Size(140, 30);
            stopButton.TabIndex = 1;
            stopButton.Text = "Stop";
            stopButton.UseVisualStyleBackColor = true;
            stopButton.Click += stopButton_Click;
            //
            // statusLabel
            //
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(20, 60);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(81, 15);
            statusLabel.TabIndex = 2;
            statusLabel.Text = "Status: Ready";
            //
            // transcriptTextBox
            //
            transcriptTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            transcriptTextBox.Font = new Font("Consolas", 10F, FontStyle.Regular, GraphicsUnit.Point);
            transcriptTextBox.Location = new Point(20, 90);
            transcriptTextBox.Multiline = true;
            transcriptTextBox.Name = "transcriptTextBox";
            transcriptTextBox.ReadOnly = true;
            transcriptTextBox.ScrollBars = ScrollBars.Vertical;
            transcriptTextBox.Size = new Size(540, 400);
            transcriptTextBox.TabIndex = 3;
            //
            // modelGroupBox
            //
            modelGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            modelGroupBox.Controls.Add(modelComboBox);
            modelGroupBox.Controls.Add(modelLabel);
            modelGroupBox.Location = new Point(580, 20);
            modelGroupBox.Name = "modelGroupBox";
            modelGroupBox.Size = new Size(280, 85);
            modelGroupBox.TabIndex = 4;
            modelGroupBox.TabStop = false;
            modelGroupBox.Text = "Model";
            //
            // modelComboBox
            //
            modelComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            modelComboBox.FormattingEnabled = true;
            modelComboBox.Location = new Point(20, 45);
            modelComboBox.Name = "modelComboBox";
            modelComboBox.Size = new Size(240, 23);
            modelComboBox.TabIndex = 1;
            //
            // modelLabel
            //
            modelLabel.AutoSize = true;
            modelLabel.Location = new Point(20, 25);
            modelLabel.Name = "modelLabel";
            modelLabel.Size = new Size(123, 15);
            modelLabel.TabIndex = 0;
            modelLabel.Text = "Select Whisper Model";
            //
            // languageGroupBox
            //
            languageGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            languageGroupBox.Controls.Add(translateCheckBox);
            languageGroupBox.Controls.Add(languageComboBox);
            languageGroupBox.Controls.Add(autoDetectLanguageCheckBox);
            languageGroupBox.Location = new Point(580, 115);
            languageGroupBox.Name = "languageGroupBox";
            languageGroupBox.Size = new Size(280, 135);
            languageGroupBox.TabIndex = 5;
            languageGroupBox.TabStop = false;
            languageGroupBox.Text = "Language";
            //
            // translateCheckBox
            //
            translateCheckBox.AutoSize = true;
            translateCheckBox.Location = new Point(20, 100);
            translateCheckBox.Name = "translateCheckBox";
            translateCheckBox.Size = new Size(189, 19);
            translateCheckBox.TabIndex = 2;
            translateCheckBox.Text = "Translate to English (if able)";
            translateCheckBox.UseVisualStyleBackColor = true;
            //
            // languageComboBox
            //
            languageComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            languageComboBox.FormattingEnabled = true;
            languageComboBox.Location = new Point(20, 65);
            languageComboBox.Name = "languageComboBox";
            languageComboBox.Size = new Size(240, 23);
            languageComboBox.TabIndex = 1;
            //
            // autoDetectLanguageCheckBox
            //
            autoDetectLanguageCheckBox.AutoSize = true;
            autoDetectLanguageCheckBox.Checked = true;
            autoDetectLanguageCheckBox.CheckState = CheckState.Checked;
            autoDetectLanguageCheckBox.Location = new Point(20, 30);
            autoDetectLanguageCheckBox.Name = "autoDetectLanguageCheckBox";
            autoDetectLanguageCheckBox.Size = new Size(130, 19);
            autoDetectLanguageCheckBox.TabIndex = 0;
            autoDetectLanguageCheckBox.Text = "Auto detect once";
            autoDetectLanguageCheckBox.UseVisualStyleBackColor = true;
            autoDetectLanguageCheckBox.CheckedChanged += autoDetectLanguageCheckBox_CheckedChanged;
            //
            // samplingGroupBox
            //
            samplingGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            samplingGroupBox.Controls.Add(patienceLabel);
            samplingGroupBox.Controls.Add(patienceNumericUpDown);
            samplingGroupBox.Controls.Add(beamSizeNumericUpDown);
            samplingGroupBox.Controls.Add(beamSizeLabel);
            samplingGroupBox.Controls.Add(bestOfNumericUpDown);
            samplingGroupBox.Controls.Add(bestOfLabel);
            samplingGroupBox.Controls.Add(beamSearchRadioButton);
            samplingGroupBox.Controls.Add(greedyRadioButton);
            samplingGroupBox.Location = new Point(580, 260);
            samplingGroupBox.Name = "samplingGroupBox";
            samplingGroupBox.Size = new Size(280, 160);
            samplingGroupBox.TabIndex = 6;
            samplingGroupBox.TabStop = false;
            samplingGroupBox.Text = "Sampling";
            //
            // patienceLabel
            //
            patienceLabel.AutoSize = true;
            patienceLabel.Location = new Point(40, 120);
            patienceLabel.Name = "patienceLabel";
            patienceLabel.Size = new Size(53, 15);
            patienceLabel.TabIndex = 6;
            patienceLabel.Text = "Patience";
            //
            // patienceNumericUpDown
            //
            patienceNumericUpDown.DecimalPlaces = 2;
            patienceNumericUpDown.Enabled = false;
            patienceNumericUpDown.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
            patienceNumericUpDown.Location = new Point(160, 118);
            patienceNumericUpDown.Maximum = 3m;
            patienceNumericUpDown.Minimum = 0.1m;
            patienceNumericUpDown.Name = "patienceNumericUpDown";
            patienceNumericUpDown.Size = new Size(100, 23);
            patienceNumericUpDown.TabIndex = 7;
            patienceNumericUpDown.Value = 1m;
            //
            // beamSizeNumericUpDown
            //
            beamSizeNumericUpDown.Enabled = false;
            beamSizeNumericUpDown.Location = new Point(160, 89);
            beamSizeNumericUpDown.Maximum = 10m;
            beamSizeNumericUpDown.Minimum = 1m;
            beamSizeNumericUpDown.Name = "beamSizeNumericUpDown";
            beamSizeNumericUpDown.Size = new Size(100, 23);
            beamSizeNumericUpDown.TabIndex = 5;
            beamSizeNumericUpDown.Value = new decimal(new int[] { 5, 0, 0, 0 });
            //
            // beamSizeLabel
            //
            beamSizeLabel.AutoSize = true;
            beamSizeLabel.Location = new Point(40, 91);
            beamSizeLabel.Name = "beamSizeLabel";
            beamSizeLabel.Size = new Size(86, 15);
            beamSizeLabel.TabIndex = 4;
            beamSizeLabel.Text = "Beam size (β)";
            //
            // bestOfNumericUpDown
            //
            bestOfNumericUpDown.Location = new Point(160, 60);
            bestOfNumericUpDown.Maximum = 10m;
            bestOfNumericUpDown.Minimum = 1m;
            bestOfNumericUpDown.Name = "bestOfNumericUpDown";
            bestOfNumericUpDown.Size = new Size(100, 23);
            bestOfNumericUpDown.TabIndex = 3;
            bestOfNumericUpDown.Value = new decimal(new int[] { 5, 0, 0, 0 });
            //
            // bestOfLabel
            //
            bestOfLabel.AutoSize = true;
            bestOfLabel.Location = new Point(40, 62);
            bestOfLabel.Name = "bestOfLabel";
            bestOfLabel.Size = new Size(82, 15);
            bestOfLabel.TabIndex = 2;
            bestOfLabel.Text = "Best of (γ)";
            //
            // beamSearchRadioButton
            //
            beamSearchRadioButton.AutoSize = true;
            beamSearchRadioButton.Location = new Point(20, 94);
            beamSearchRadioButton.Name = "beamSearchRadioButton";
            beamSearchRadioButton.Size = new Size(138, 19);
            beamSearchRadioButton.TabIndex = 1;
            beamSearchRadioButton.Text = "Beam search (β, ρ)";
            beamSearchRadioButton.UseVisualStyleBackColor = true;
            beamSearchRadioButton.CheckedChanged += samplingRadioButton_CheckedChanged;
            //
            // greedyRadioButton
            //
            greedyRadioButton.AutoSize = true;
            greedyRadioButton.Checked = true;
            greedyRadioButton.Location = new Point(20, 34);
            greedyRadioButton.Name = "greedyRadioButton";
            greedyRadioButton.Size = new Size(141, 19);
            greedyRadioButton.TabIndex = 0;
            greedyRadioButton.TabStop = true;
            greedyRadioButton.Text = "Greedy (best-of γ)";
            greedyRadioButton.UseVisualStyleBackColor = true;
            greedyRadioButton.CheckedChanged += samplingRadioButton_CheckedChanged;
            //
            // advancedGroupBox
            //
            advancedGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            advancedGroupBox.Controls.Add(logProbNumericUpDown);
            advancedGroupBox.Controls.Add(logProbLabel);
            advancedGroupBox.Controls.Add(noSpeechNumericUpDown);
            advancedGroupBox.Controls.Add(noSpeechLabel);
            advancedGroupBox.Controls.Add(temperatureNumericUpDown);
            advancedGroupBox.Controls.Add(temperatureLabel);
            advancedGroupBox.Location = new Point(580, 430);
            advancedGroupBox.Name = "advancedGroupBox";
            advancedGroupBox.Size = new Size(280, 120);
            advancedGroupBox.TabIndex = 7;
            advancedGroupBox.TabStop = false;
            advancedGroupBox.Text = "Advanced";
            //
            // logProbNumericUpDown
            //
            logProbNumericUpDown.DecimalPlaces = 2;
            logProbNumericUpDown.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
            logProbNumericUpDown.Location = new Point(160, 85);
            logProbNumericUpDown.Maximum = 0m;
            logProbNumericUpDown.Minimum = -5m;
            logProbNumericUpDown.Name = "logProbNumericUpDown";
            logProbNumericUpDown.Size = new Size(100, 23);
            logProbNumericUpDown.TabIndex = 5;
            logProbNumericUpDown.Value = -1m;
            //
            // logProbLabel
            //
            logProbLabel.AutoSize = true;
            logProbLabel.Location = new Point(20, 87);
            logProbLabel.Name = "logProbLabel";
            logProbLabel.Size = new Size(121, 15);
            logProbLabel.TabIndex = 4;
            logProbLabel.Text = "Log prob threshold";
            //
            // noSpeechNumericUpDown
            //
            noSpeechNumericUpDown.DecimalPlaces = 2;
            noSpeechNumericUpDown.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
            noSpeechNumericUpDown.Location = new Point(160, 55);
            noSpeechNumericUpDown.Maximum = 1m;
            noSpeechNumericUpDown.Minimum = 0m;
            noSpeechNumericUpDown.Name = "noSpeechNumericUpDown";
            noSpeechNumericUpDown.Size = new Size(100, 23);
            noSpeechNumericUpDown.TabIndex = 3;
            noSpeechNumericUpDown.Value = 0.6m;
            //
            // noSpeechLabel
            //
            noSpeechLabel.AutoSize = true;
            noSpeechLabel.Location = new Point(20, 57);
            noSpeechLabel.Name = "noSpeechLabel";
            noSpeechLabel.Size = new Size(112, 15);
            noSpeechLabel.TabIndex = 2;
            noSpeechLabel.Text = "No speech threshold";
            //
            // temperatureNumericUpDown
            //
            temperatureNumericUpDown.DecimalPlaces = 2;
            temperatureNumericUpDown.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
            temperatureNumericUpDown.Location = new Point(160, 25);
            temperatureNumericUpDown.Maximum = 1m;
            temperatureNumericUpDown.Minimum = 0m;
            temperatureNumericUpDown.Name = "temperatureNumericUpDown";
            temperatureNumericUpDown.Size = new Size(100, 23);
            temperatureNumericUpDown.TabIndex = 1;
            temperatureNumericUpDown.Value = 0.2m;
            //
            // temperatureLabel
            //
            temperatureLabel.AutoSize = true;
            temperatureLabel.Location = new Point(20, 27);
            temperatureLabel.Name = "temperatureLabel";
            temperatureLabel.Size = new Size(73, 15);
            temperatureLabel.TabIndex = 0;
            temperatureLabel.Text = "Temperature";
            //
            // Form1
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(884, 571);
            Controls.Add(advancedGroupBox);
            Controls.Add(samplingGroupBox);
            Controls.Add(languageGroupBox);
            Controls.Add(modelGroupBox);
            Controls.Add(transcriptTextBox);
            Controls.Add(statusLabel);
            Controls.Add(stopButton);
            Controls.Add(startButton);
            Name = "Form1";
            Text = "Live Transcribe";
            ((System.ComponentModel.ISupportInitialize)patienceNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)beamSizeNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)bestOfNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)logProbNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)noSpeechNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)temperatureNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NotifyIcon notifyIcon1;
        private Button startButton;
        private Button stopButton;
        private Label statusLabel;
        private TextBox transcriptTextBox;
        private GroupBox modelGroupBox;
        private ComboBox modelComboBox;
        private Label modelLabel;
        private GroupBox languageGroupBox;
        private CheckBox translateCheckBox;
        private ComboBox languageComboBox;
        private CheckBox autoDetectLanguageCheckBox;
        private GroupBox samplingGroupBox;
        private Label patienceLabel;
        private NumericUpDown patienceNumericUpDown;
        private NumericUpDown beamSizeNumericUpDown;
        private Label beamSizeLabel;
        private NumericUpDown bestOfNumericUpDown;
        private Label bestOfLabel;
        private RadioButton beamSearchRadioButton;
        private RadioButton greedyRadioButton;
        private GroupBox advancedGroupBox;
        private NumericUpDown logProbNumericUpDown;
        private Label logProbLabel;
        private NumericUpDown noSpeechNumericUpDown;
        private Label noSpeechLabel;
        private NumericUpDown temperatureNumericUpDown;
        private Label temperatureLabel;
    }
}
