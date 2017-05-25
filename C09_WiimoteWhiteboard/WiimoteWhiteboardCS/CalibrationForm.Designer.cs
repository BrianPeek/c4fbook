namespace WiimoteWhiteboard
{
    partial class CalibrationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.pbCalibrate = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pbCalibrate)).BeginInit();
			this.SuspendLayout();
			// 
			// pbCalibrate
			// 
			this.pbCalibrate.Location = new System.Drawing.Point(66, 63);
			this.pbCalibrate.Margin = new System.Windows.Forms.Padding(2);
			this.pbCalibrate.Name = "pbCalibrate";
			this.pbCalibrate.Size = new System.Drawing.Size(75, 41);
			this.pbCalibrate.TabIndex = 0;
			this.pbCalibrate.TabStop = false;
			// 
			// CalibrationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(219, 211);
			this.Controls.Add(this.pbCalibrate);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "CalibrationForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "CalibrationForm";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CalibrationForm_KeyDown);
			((System.ComponentModel.ISupportInitialize)(this.pbCalibrate)).EndInit();
			this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.PictureBox pbCalibrate;
    }
}