﻿namespace GLASGOW_SIMULATOR
{
    partial class GameScreen
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
            this.components = new System.ComponentModel.Container();
            this.friendlyEngine = new System.Windows.Forms.Timer(this.components);
            this.enemyEngine = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // friendlyEngine
            // 
            this.friendlyEngine.Interval = 20;
            this.friendlyEngine.Tick += new System.EventHandler(this.gameEngine_Tick);
            // 
            // enemyEngine
            // 
            this.enemyEngine.Interval = 500;
            this.enemyEngine.Tick += new System.EventHandler(this.gameEngine_Tick_1);
            // 
            // GameScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.DoubleBuffered = true;
            this.Name = "GameScreen";
            this.Size = new System.Drawing.Size(600, 700);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GameScreen_Paint);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GameScreen_KeyUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.GameScreen_PreviewKeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer friendlyEngine;
        private System.Windows.Forms.Timer enemyEngine;
    }
}
