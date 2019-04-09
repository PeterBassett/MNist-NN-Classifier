namespace MNistClassifier
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            this.trainingImageDisplay = new System.Windows.Forms.PictureBox();
            this.lblPredictionPercent = new System.Windows.Forms.Label();
            this.userImageCanvas = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.userPrediction = new System.Windows.Forms.Label();
            this.lblTrainingImagesExaminedText = new System.Windows.Forms.Label();
            this.lblTrainingImagesExamined = new System.Windows.Forms.Label();
            this.btnSaveState = new System.Windows.Forms.Button();
            this.btnLoadState = new System.Windows.Forms.Button();
            this.saveFileDlg = new System.Windows.Forms.SaveFileDialog();
            this.openFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.bntClear = new System.Windows.Forms.Button();
            this.standardisedImage = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rdoSimple = new System.Windows.Forms.RadioButton();
            this.rdoComplex = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtLayerCounts = new System.Windows.Forms.TextBox();
            this.tmrIteration = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.trainingImageDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.userImageCanvas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.standardisedImage)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // trainingImageDisplay
            // 
            this.trainingImageDisplay.Location = new System.Drawing.Point(424, 25);
            this.trainingImageDisplay.Name = "trainingImageDisplay";
            this.trainingImageDisplay.Size = new System.Drawing.Size(200, 200);
            this.trainingImageDisplay.TabIndex = 0;
            this.trainingImageDisplay.TabStop = false;
            // 
            // lblPredictionPercent
            // 
            this.lblPredictionPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPredictionPercent.AutoSize = true;
            this.lblPredictionPercent.Location = new System.Drawing.Point(543, 228);
            this.lblPredictionPercent.Name = "lblPredictionPercent";
            this.lblPredictionPercent.Size = new System.Drawing.Size(35, 13);
            this.lblPredictionPercent.TabIndex = 1;
            this.lblPredictionPercent.Text = "label1";
            // 
            // userImageCanvas
            // 
            this.userImageCanvas.Location = new System.Drawing.Point(12, 25);
            this.userImageCanvas.Name = "userImageCanvas";
            this.userImageCanvas.Size = new System.Drawing.Size(200, 200);
            this.userImageCanvas.TabIndex = 2;
            this.userImageCanvas.TabStop = false;
            this.userImageCanvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.userImageCanvas_MouseDown);
            this.userImageCanvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.userImageCanvas_MouseMove);
            this.userImageCanvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.userImageCanvas_MouseUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(421, 228);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Accuracy Of Prediction";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(215, 228);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "This is a : ";
            // 
            // userPrediction
            // 
            this.userPrediction.AutoSize = true;
            this.userPrediction.Location = new System.Drawing.Point(266, 228);
            this.userPrediction.Name = "userPrediction";
            this.userPrediction.Size = new System.Drawing.Size(13, 13);
            this.userPrediction.TabIndex = 4;
            this.userPrediction.Text = "_";
            // 
            // lblTrainingImagesExaminedText
            // 
            this.lblTrainingImagesExaminedText.AutoSize = true;
            this.lblTrainingImagesExaminedText.Location = new System.Drawing.Point(421, 245);
            this.lblTrainingImagesExaminedText.Name = "lblTrainingImagesExaminedText";
            this.lblTrainingImagesExaminedText.Size = new System.Drawing.Size(110, 13);
            this.lblTrainingImagesExaminedText.TabIndex = 6;
            this.lblTrainingImagesExaminedText.Text = "Training Images Used";
            // 
            // lblTrainingImagesExamined
            // 
            this.lblTrainingImagesExamined.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTrainingImagesExamined.AutoSize = true;
            this.lblTrainingImagesExamined.Location = new System.Drawing.Point(543, 245);
            this.lblTrainingImagesExamined.Name = "lblTrainingImagesExamined";
            this.lblTrainingImagesExamined.Size = new System.Drawing.Size(38, 13);
            this.lblTrainingImagesExamined.TabIndex = 7;
            this.lblTrainingImagesExamined.Text = "label1 ";
            // 
            // btnSaveState
            // 
            this.btnSaveState.Location = new System.Drawing.Point(335, 13);
            this.btnSaveState.Name = "btnSaveState";
            this.btnSaveState.Size = new System.Drawing.Size(75, 23);
            this.btnSaveState.TabIndex = 8;
            this.btnSaveState.Text = "Save NN";
            this.btnSaveState.UseVisualStyleBackColor = true;
            this.btnSaveState.Click += new System.EventHandler(this.btnSaveState_Click);
            // 
            // btnLoadState
            // 
            this.btnLoadState.Location = new System.Drawing.Point(254, 13);
            this.btnLoadState.Name = "btnLoadState";
            this.btnLoadState.Size = new System.Drawing.Size(75, 23);
            this.btnLoadState.TabIndex = 9;
            this.btnLoadState.Text = "Load NN";
            this.btnLoadState.UseVisualStyleBackColor = true;
            this.btnLoadState.Click += new System.EventHandler(this.btnLoadState_Click);
            // 
            // saveFileDlg
            // 
            this.saveFileDlg.Filter = "Xml Files|*.xml|All files|*.*";
            // 
            // openFileDlg
            // 
            this.openFileDlg.FileName = "openFileDialog1";
            this.openFileDlg.Filter = "Xml Files|*.xml|All files|*.*";
            // 
            // bntClear
            // 
            this.bntClear.Location = new System.Drawing.Point(12, 231);
            this.bntClear.Name = "bntClear";
            this.bntClear.Size = new System.Drawing.Size(75, 23);
            this.bntClear.TabIndex = 10;
            this.bntClear.Text = "Clear";
            this.bntClear.UseVisualStyleBackColor = true;
            this.bntClear.Click += new System.EventHandler(this.bntClear_Click);
            // 
            // standardisedImage
            // 
            this.standardisedImage.Location = new System.Drawing.Point(218, 25);
            this.standardisedImage.Name = "standardisedImage";
            this.standardisedImage.Size = new System.Drawing.Size(200, 200);
            this.standardisedImage.TabIndex = 11;
            this.standardisedImage.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Drawing Canvas";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(215, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Centered Image";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(421, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(114, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Current Training Image";
            // 
            // rdoSimple
            // 
            this.rdoSimple.AutoSize = true;
            this.rdoSimple.Checked = true;
            this.rdoSimple.Location = new System.Drawing.Point(6, 19);
            this.rdoSimple.Name = "rdoSimple";
            this.rdoSimple.Size = new System.Drawing.Size(56, 17);
            this.rdoSimple.TabIndex = 15;
            this.rdoSimple.TabStop = true;
            this.rdoSimple.Text = "Simple";
            this.rdoSimple.UseVisualStyleBackColor = true;
            this.rdoSimple.CheckedChanged += new System.EventHandler(this.rdoSimple_CheckedChanged);
            // 
            // rdoComplex
            // 
            this.rdoComplex.AutoSize = true;
            this.rdoComplex.Location = new System.Drawing.Point(68, 19);
            this.rdoComplex.Name = "rdoComplex";
            this.rdoComplex.Size = new System.Drawing.Size(65, 17);
            this.rdoComplex.TabIndex = 16;
            this.rdoComplex.Text = "Complex";
            this.rdoComplex.UseVisualStyleBackColor = true;
            this.rdoComplex.CheckedChanged += new System.EventHandler(this.rdoSimple_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtLayerCounts);
            this.groupBox1.Controls.Add(this.rdoComplex);
            this.groupBox1.Controls.Add(this.rdoSimple);
            this.groupBox1.Controls.Add(this.btnLoadState);
            this.groupBox1.Controls.Add(this.btnSaveState);
            this.groupBox1.Location = new System.Drawing.Point(208, 261);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(416, 46);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Network Type";
            // 
            // txtLayerCounts
            // 
            this.txtLayerCounts.Location = new System.Drawing.Point(139, 13);
            this.txtLayerCounts.Name = "txtLayerCounts";
            this.txtLayerCounts.Size = new System.Drawing.Size(109, 20);
            this.txtLayerCounts.TabIndex = 17;
            // 
            // tmrIteration
            // 
            this.tmrIteration.Interval = 1;
            this.tmrIteration.Tick += new System.EventHandler(this.tmrIteration_Tick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 313);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.standardisedImage);
            this.Controls.Add(this.bntClear);
            this.Controls.Add(this.lblTrainingImagesExamined);
            this.Controls.Add(this.lblTrainingImagesExaminedText);
            this.Controls.Add(this.userPrediction);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.userImageCanvas);
            this.Controls.Add(this.lblPredictionPercent);
            this.Controls.Add(this.trainingImageDisplay);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trainingImageDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.userImageCanvas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.standardisedImage)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox trainingImageDisplay;
        private System.Windows.Forms.Label lblPredictionPercent;
        private System.Windows.Forms.PictureBox userImageCanvas;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label userPrediction;
        private System.Windows.Forms.Label lblTrainingImagesExaminedText;
        private System.Windows.Forms.Label lblTrainingImagesExamined;
        private System.Windows.Forms.Button btnSaveState;
        private System.Windows.Forms.Button btnLoadState;
        private System.Windows.Forms.SaveFileDialog saveFileDlg;
        private System.Windows.Forms.OpenFileDialog openFileDlg;
        private System.Windows.Forms.Button bntClear;
        private System.Windows.Forms.PictureBox standardisedImage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rdoSimple;
        private System.Windows.Forms.RadioButton rdoComplex;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtLayerCounts;
        private System.Windows.Forms.Timer tmrIteration;
    }
}

