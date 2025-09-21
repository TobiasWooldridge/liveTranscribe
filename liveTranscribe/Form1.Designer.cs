using System.Drawing;
using System.Windows.Forms;

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
            settingsGroupBox = new GroupBox();
            bestOfLabel = new Label();
            beamSizeLabel = new Label();
            temperatureStepLabel = new Label();
            temperatureLabel = new Label();
            beamSearchCheckBox = new CheckBox();
            translateCheckBox = new CheckBox();
            autoDetectLanguageCheckBox = new CheckBox();
            languageTextBox = new TextBox();
            languageLabel = new Label();
            modelComboBox = new ComboBox();
            modelLabel = new Label();
            temperatureNumericUpDown = new NumericUpDown();
            temperatureStepNumericUpDown = new NumericUpDown();
            beamSizeNumericUpDown = new NumericUpDown();
            bestOfNumericUpDown = new NumericUpDown();
            settingsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)temperatureNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)temperatureStepNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)beamSizeNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)bestOfNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // notifyIcon1
            // 
            notifyIcon1.Text = "liveTranscribe";
            notifyIcon1.Visible = true;
            // 
            // startButton
            // 
            startButton.Location = new Point(408, 32);
            startButton.Name = "startButton";
            startButton.Size = new Size(150, 30);
            startButton.TabIndex = 1;
            startButton.Text = "Start transcribing";
            startButton.UseVisualStyleBackColor = true;
            startButton.Click += startButton_Click;
            // 
            // stopButton
            // 
            stopButton.Enabled = false;
            stopButton.Location = new Point(564, 32);
            stopButton.Name = "stopButton";
            stopButton.Size = new Size(120, 30);
            stopButton.TabIndex = 2;
            stopButton.Text = "Stop";
            stopButton.UseVisualStyleBackColor = true;
            stopButton.Click += stopButton_Click;
            // 
            // statusLabel
            // 
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(12, 268);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(27, 15);
            statusLabel.TabIndex = 4;
            statusLabel.Text = "Idle";
            // 
            // transcriptTextBox
            // 
            transcriptTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            transcriptTextBox.Location = new Point(12, 296);
            transcriptTextBox.Multiline = true;
            transcriptTextBox.Name = "transcriptTextBox";
            transcriptTextBox.ReadOnly = true;
            transcriptTextBox.ScrollBars = ScrollBars.Vertical;
            transcriptTextBox.Size = new Size(776, 152);
            transcriptTextBox.TabIndex = 5;
            // 
            // settingsGroupBox
            // 
            settingsGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            settingsGroupBox.Controls.Add(bestOfNumericUpDown);
            settingsGroupBox.Controls.Add(beamSizeNumericUpDown);
            settingsGroupBox.Controls.Add(temperatureStepNumericUpDown);
            settingsGroupBox.Controls.Add(temperatureNumericUpDown);
            settingsGroupBox.Controls.Add(bestOfLabel);
            settingsGroupBox.Controls.Add(beamSizeLabel);
            settingsGroupBox.Controls.Add(temperatureStepLabel);
            settingsGroupBox.Controls.Add(temperatureLabel);
            settingsGroupBox.Controls.Add(beamSearchCheckBox);
            settingsGroupBox.Controls.Add(translateCheckBox);
            settingsGroupBox.Controls.Add(autoDetectLanguageCheckBox);
            settingsGroupBox.Controls.Add(languageTextBox);
            settingsGroupBox.Controls.Add(languageLabel);
            settingsGroupBox.Controls.Add(modelComboBox);
            settingsGroupBox.Controls.Add(modelLabel);
            settingsGroupBox.Location = new Point(12, 12);
            settingsGroupBox.Name = "settingsGroupBox";
            settingsGroupBox.Size = new Size(380, 244);
            settingsGroupBox.TabIndex = 0;
            settingsGroupBox.TabStop = false;
            settingsGroupBox.Text = "Whisper options";
            // 
            // bestOfLabel
            // 
            bestOfLabel.AutoSize = true;
            bestOfLabel.Location = new Point(12, 206);
            bestOfLabel.Name = "bestOfLabel";
            bestOfLabel.Size = new Size(107, 15);
            bestOfLabel.TabIndex = 13;
            bestOfLabel.Text = "Greedy best-of (k)";
            // 
            // beamSizeLabel
            // 
            beamSizeLabel.AutoSize = true;
            beamSizeLabel.Location = new Point(12, 176);
            beamSizeLabel.Name = "beamSizeLabel";
            beamSizeLabel.Size = new Size(60, 15);
            beamSizeLabel.TabIndex = 11;
            beamSizeLabel.Text = "Beam size";
            // 
            // temperatureStepLabel
            // 
            temperatureStepLabel.AutoSize = true;
            temperatureStepLabel.Location = new Point(12, 146);
            temperatureStepLabel.Name = "temperatureStepLabel";
            temperatureStepLabel.Size = new Size(121, 15);
            temperatureStepLabel.TabIndex = 9;
            temperatureStepLabel.Text = "Temperature fallback";
            // 
            // temperatureLabel
            // 
            temperatureLabel.AutoSize = true;
            temperatureLabel.Location = new Point(12, 116);
            temperatureLabel.Name = "temperatureLabel";
            temperatureLabel.Size = new Size(72, 15);
            temperatureLabel.TabIndex = 7;
            temperatureLabel.Text = "Temperature";
            // 
            // beamSearchCheckBox
            // 
            beamSearchCheckBox.AutoSize = true;
            beamSearchCheckBox.Location = new Point(15, 92);
            beamSearchCheckBox.Name = "beamSearchCheckBox";
            beamSearchCheckBox.Size = new Size(219, 19);
            beamSearchCheckBox.TabIndex = 6;
            beamSearchCheckBox.Text = "Use beam search (highest accuracy)";
            beamSearchCheckBox.UseVisualStyleBackColor = true;
            // 
            // translateCheckBox
            // 
            translateCheckBox.AutoSize = true;
            translateCheckBox.Location = new Point(189, 67);
            translateCheckBox.Name = "translateCheckBox";
            translateCheckBox.Size = new Size(141, 19);
            translateCheckBox.TabIndex = 5;
            translateCheckBox.Text = "Translate to English";
            translateCheckBox.UseVisualStyleBackColor = true;
            // 
            // autoDetectLanguageCheckBox
            // 
            autoDetectLanguageCheckBox.AutoSize = true;
            autoDetectLanguageCheckBox.Location = new Point(15, 67);
            autoDetectLanguageCheckBox.Name = "autoDetectLanguageCheckBox";
            autoDetectLanguageCheckBox.Size = new Size(139, 19);
            autoDetectLanguageCheckBox.TabIndex = 4;
            autoDetectLanguageCheckBox.Text = "Auto detect language";
            autoDetectLanguageCheckBox.UseVisualStyleBackColor = true;
            // 
            // languageTextBox
            // 
            languageTextBox.Location = new Point(189, 38);
            languageTextBox.Name = "languageTextBox";
            languageTextBox.Size = new Size(171, 23);
            languageTextBox.TabIndex = 3;
            // 
            // languageLabel
            // 
            languageLabel.AutoSize = true;
            languageLabel.Location = new Point(12, 41);
            languageLabel.Name = "languageLabel";
            languageLabel.Size = new Size(135, 15);
            languageLabel.TabIndex = 2;
            languageLabel.Text = "Language (culture code)";
            // 
            // modelComboBox
            // 
            modelComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            modelComboBox.FormattingEnabled = true;
            modelComboBox.Location = new Point(189, 12);
            modelComboBox.Name = "modelComboBox";
            modelComboBox.Size = new Size(171, 23);
            modelComboBox.TabIndex = 1;
            // 
            // modelLabel
            // 
            modelLabel.AutoSize = true;
            modelLabel.Location = new Point(12, 15);
            modelLabel.Name = "modelLabel";
            modelLabel.Size = new Size(42, 15);
            modelLabel.TabIndex = 0;
            modelLabel.Text = "Model";
            // 
            // temperatureNumericUpDown
            // 
            temperatureNumericUpDown.Location = new Point(189, 114);
            temperatureNumericUpDown.Name = "temperatureNumericUpDown";
            temperatureNumericUpDown.Size = new Size(80, 23);
            temperatureNumericUpDown.TabIndex = 8;
            // 
            // temperatureStepNumericUpDown
            // 
            temperatureStepNumericUpDown.Location = new Point(189, 144);
            temperatureStepNumericUpDown.Name = "temperatureStepNumericUpDown";
            temperatureStepNumericUpDown.Size = new Size(80, 23);
            temperatureStepNumericUpDown.TabIndex = 10;
            // 
            // beamSizeNumericUpDown
            // 
            beamSizeNumericUpDown.Location = new Point(189, 174);
            beamSizeNumericUpDown.Name = "beamSizeNumericUpDown";
            beamSizeNumericUpDown.Size = new Size(80, 23);
            beamSizeNumericUpDown.TabIndex = 12;
            // 
            // bestOfNumericUpDown
            // 
            bestOfNumericUpDown.Location = new Point(189, 204);
            bestOfNumericUpDown.Name = "bestOfNumericUpDown";
            bestOfNumericUpDown.Size = new Size(80, 23);
            bestOfNumericUpDown.TabIndex = 14;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 460);
            Controls.Add(settingsGroupBox);
            Controls.Add(transcriptTextBox);
            Controls.Add(statusLabel);
            Controls.Add(stopButton);
            Controls.Add(startButton);
            Name = "Form1";
            Text = "liveTranscribe";
            settingsGroupBox.ResumeLayout(false);
            settingsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)temperatureNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)temperatureStepNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)beamSizeNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)bestOfNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NotifyIcon notifyIcon1;
        private Button startButton;
        private Button stopButton;
        private Label statusLabel;
        private TextBox transcriptTextBox;
        private GroupBox settingsGroupBox;
        private Label modelLabel;
        private ComboBox modelComboBox;
        private Label languageLabel;
        private TextBox languageTextBox;
        private CheckBox autoDetectLanguageCheckBox;
        private CheckBox translateCheckBox;
        private CheckBox beamSearchCheckBox;
        private Label temperatureLabel;
        private Label temperatureStepLabel;
        private Label beamSizeLabel;
        private Label bestOfLabel;
        private NumericUpDown temperatureNumericUpDown;
        private NumericUpDown temperatureStepNumericUpDown;
        private NumericUpDown beamSizeNumericUpDown;
        private NumericUpDown bestOfNumericUpDown;
    }
}
