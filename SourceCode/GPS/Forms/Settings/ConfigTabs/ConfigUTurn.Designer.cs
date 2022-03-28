namespace AgOpenGPS
{
    partial class ConfigUTurn
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblFtMUTurn = new System.Windows.Forms.Label();
            this.nudTurnDistanceFromBoundary = new System.Windows.Forms.Button();
            this.lblSmoothing = new System.Windows.Forms.Label();
            this.lblDistance = new System.Windows.Forms.Label();
            this.btnTurnSmoothingUp = new ProXoft.WinForms.RepeatButton();
            this.btnTurnSmoothingDown = new ProXoft.WinForms.RepeatButton();
            this.label59 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.lblWhenTrig = new System.Windows.Forms.Label();
            this.btnDistanceUp = new ProXoft.WinForms.RepeatButton();
            this.btnDistanceDn = new ProXoft.WinForms.RepeatButton();
            this.SuspendLayout();
            // 
            // lblFtMUTurn
            // 
            this.lblFtMUTurn.AutoSize = true;
            this.lblFtMUTurn.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFtMUTurn.ForeColor = System.Drawing.Color.Black;
            this.lblFtMUTurn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblFtMUTurn.Location = new System.Drawing.Point(165, 422);
            this.lblFtMUTurn.Name = "lblFtMUTurn";
            this.lblFtMUTurn.Size = new System.Drawing.Size(36, 25);
            this.lblFtMUTurn.TabIndex = 480;
            this.lblFtMUTurn.Text = "FF";
            // 
            // nudTurnDistanceFromBoundary
            // 
            this.nudTurnDistanceFromBoundary.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.nudTurnDistanceFromBoundary.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudTurnDistanceFromBoundary.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.nudTurnDistanceFromBoundary.Location = new System.Drawing.Point(102, 367);
            this.nudTurnDistanceFromBoundary.Name = "nudTurnDistanceFromBoundary";
            this.nudTurnDistanceFromBoundary.Size = new System.Drawing.Size(160, 52);
            this.nudTurnDistanceFromBoundary.TabIndex = 479;
            this.nudTurnDistanceFromBoundary.Text = "10";
            this.nudTurnDistanceFromBoundary.Click += new System.EventHandler(this.nudTurnDistanceFromBoundary_Click);
            this.nudTurnDistanceFromBoundary.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.nudTurnDistanceFromBoundary_HelpRequested);
            // 
            // lblSmoothing
            // 
            this.lblSmoothing.AutoSize = true;
            this.lblSmoothing.BackColor = System.Drawing.Color.Transparent;
            this.lblSmoothing.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSmoothing.ForeColor = System.Drawing.Color.Black;
            this.lblSmoothing.Location = new System.Drawing.Point(635, 308);
            this.lblSmoothing.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSmoothing.Name = "lblSmoothing";
            this.lblSmoothing.Size = new System.Drawing.Size(95, 45);
            this.lblSmoothing.TabIndex = 475;
            this.lblSmoothing.Text = "XXX";
            this.lblSmoothing.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.lblSmoothing_HelpRequested);
            // 
            // lblDistance
            // 
            this.lblDistance.AutoSize = true;
            this.lblDistance.BackColor = System.Drawing.Color.Transparent;
            this.lblDistance.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDistance.ForeColor = System.Drawing.Color.Black;
            this.lblDistance.Location = new System.Drawing.Point(409, 308);
            this.lblDistance.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDistance.Name = "lblDistance";
            this.lblDistance.Size = new System.Drawing.Size(95, 45);
            this.lblDistance.TabIndex = 470;
            this.lblDistance.Text = "XXX";
            this.lblDistance.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.lblDistance_HelpRequested);
            // 
            // btnTurnSmoothingUp
            // 
            this.btnTurnSmoothingUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnTurnSmoothingUp.FlatAppearance.BorderSize = 0;
            this.btnTurnSmoothingUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTurnSmoothingUp.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTurnSmoothingUp.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnTurnSmoothingUp.Image = global::AgOpenGPS.Properties.Resources.UpArrow64;
            this.btnTurnSmoothingUp.Location = new System.Drawing.Point(699, 364);
            this.btnTurnSmoothingUp.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnTurnSmoothingUp.Name = "btnTurnSmoothingUp";
            this.btnTurnSmoothingUp.Size = new System.Drawing.Size(59, 69);
            this.btnTurnSmoothingUp.TabIndex = 477;
            this.btnTurnSmoothingUp.UseVisualStyleBackColor = true;
            this.btnTurnSmoothingUp.Click += new System.EventHandler(this.btnTurnSmoothingUp_Click);
            // 
            // btnTurnSmoothingDown
            // 
            this.btnTurnSmoothingDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnTurnSmoothingDown.FlatAppearance.BorderSize = 0;
            this.btnTurnSmoothingDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTurnSmoothingDown.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTurnSmoothingDown.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnTurnSmoothingDown.Image = global::AgOpenGPS.Properties.Resources.DnArrow64;
            this.btnTurnSmoothingDown.Location = new System.Drawing.Point(606, 364);
            this.btnTurnSmoothingDown.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnTurnSmoothingDown.Name = "btnTurnSmoothingDown";
            this.btnTurnSmoothingDown.Size = new System.Drawing.Size(59, 69);
            this.btnTurnSmoothingDown.TabIndex = 476;
            this.btnTurnSmoothingDown.UseVisualStyleBackColor = true;
            this.btnTurnSmoothingDown.Click += new System.EventHandler(this.btnTurnSmoothingDown_Click);
            // 
            // label59
            // 
            this.label59.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label59.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label59.Image = global::AgOpenGPS.Properties.Resources.ConU_UTurnSmooth;
            this.label59.Location = new System.Drawing.Point(618, 188);
            this.label59.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(120, 120);
            this.label59.TabIndex = 478;
            this.label59.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label58
            // 
            this.label58.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label58.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label58.Image = global::AgOpenGPS.Properties.Resources.ConU_UturnDistance;
            this.label58.Location = new System.Drawing.Point(122, 224);
            this.label58.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(120, 120);
            this.label58.TabIndex = 474;
            this.label58.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblWhenTrig
            // 
            this.lblWhenTrig.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWhenTrig.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblWhenTrig.Image = global::AgOpenGPS.Properties.Resources.ConU_UturnLength;
            this.lblWhenTrig.Location = new System.Drawing.Point(399, 186);
            this.lblWhenTrig.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblWhenTrig.Name = "lblWhenTrig";
            this.lblWhenTrig.Size = new System.Drawing.Size(120, 120);
            this.lblWhenTrig.TabIndex = 473;
            this.lblWhenTrig.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnDistanceUp
            // 
            this.btnDistanceUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDistanceUp.FlatAppearance.BorderSize = 0;
            this.btnDistanceUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDistanceUp.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDistanceUp.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnDistanceUp.Image = global::AgOpenGPS.Properties.Resources.UpArrow64;
            this.btnDistanceUp.Location = new System.Drawing.Point(473, 364);
            this.btnDistanceUp.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnDistanceUp.Name = "btnDistanceUp";
            this.btnDistanceUp.Size = new System.Drawing.Size(59, 69);
            this.btnDistanceUp.TabIndex = 472;
            this.btnDistanceUp.UseVisualStyleBackColor = true;
            this.btnDistanceUp.Click += new System.EventHandler(this.btnDistanceUp_Click);
            // 
            // btnDistanceDn
            // 
            this.btnDistanceDn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDistanceDn.FlatAppearance.BorderSize = 0;
            this.btnDistanceDn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDistanceDn.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDistanceDn.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnDistanceDn.Image = global::AgOpenGPS.Properties.Resources.DnArrow64;
            this.btnDistanceDn.Location = new System.Drawing.Point(380, 364);
            this.btnDistanceDn.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnDistanceDn.Name = "btnDistanceDn";
            this.btnDistanceDn.Size = new System.Drawing.Size(59, 69);
            this.btnDistanceDn.TabIndex = 471;
            this.btnDistanceDn.UseVisualStyleBackColor = true;
            this.btnDistanceDn.Click += new System.EventHandler(this.btnDistanceDn_Click);
            // 
            // ConfigUTurn
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.lblFtMUTurn);
            this.Controls.Add(this.nudTurnDistanceFromBoundary);
            this.Controls.Add(this.lblSmoothing);
            this.Controls.Add(this.lblDistance);
            this.Controls.Add(this.btnTurnSmoothingUp);
            this.Controls.Add(this.btnTurnSmoothingDown);
            this.Controls.Add(this.label59);
            this.Controls.Add(this.label58);
            this.Controls.Add(this.lblWhenTrig);
            this.Controls.Add(this.btnDistanceUp);
            this.Controls.Add(this.btnDistanceDn);
            this.Name = "ConfigUTurn";
            this.Size = new System.Drawing.Size(890, 580);
            this.Load += new System.EventHandler(this.ConfigUTurn_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFtMUTurn;
        private System.Windows.Forms.Button nudTurnDistanceFromBoundary;
        private System.Windows.Forms.Label lblSmoothing;
        private System.Windows.Forms.Label lblDistance;
        private ProXoft.WinForms.RepeatButton btnTurnSmoothingUp;
        private ProXoft.WinForms.RepeatButton btnTurnSmoothingDown;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Label lblWhenTrig;
        private ProXoft.WinForms.RepeatButton btnDistanceUp;
        private ProXoft.WinForms.RepeatButton btnDistanceDn;
    }
}
