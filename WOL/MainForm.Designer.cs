namespace WOL
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageDefault = new System.Windows.Forms.TabPage();
            this.tabPageAdd = new System.Windows.Forms.TabPage();
            this.button_AddNew = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPageDefault);
            this.tabControl.Controls.Add(this.tabPageAdd);
            this.tabControl.Location = new System.Drawing.Point(13, 67);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(775, 371);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageDefault
            // 
            this.tabPageDefault.Location = new System.Drawing.Point(4, 22);
            this.tabPageDefault.Name = "tabPageDefault";
            this.tabPageDefault.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDefault.Size = new System.Drawing.Size(767, 345);
            this.tabPageDefault.TabIndex = 0;
            this.tabPageDefault.Text = "Default";
            this.tabPageDefault.UseVisualStyleBackColor = true;
            // 
            // tabPageAdd
            // 
            this.tabPageAdd.Location = new System.Drawing.Point(4, 22);
            this.tabPageAdd.Name = "tabPageAdd";
            this.tabPageAdd.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAdd.Size = new System.Drawing.Size(767, 345);
            this.tabPageAdd.TabIndex = 1;
            this.tabPageAdd.Text = "Add";
            this.tabPageAdd.UseVisualStyleBackColor = true;
            // 
            // button_AddNew
            // 
            this.button_AddNew.Location = new System.Drawing.Point(13, 13);
            this.button_AddNew.Name = "button_AddNew";
            this.button_AddNew.Size = new System.Drawing.Size(75, 23);
            this.button_AddNew.TabIndex = 1;
            this.button_AddNew.Text = "Add New";
            this.button_AddNew.UseVisualStyleBackColor = true;
            this.button_AddNew.Click += new System.EventHandler(this.button_AddNew_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_AddNew);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "WakeOnLan | quang.haduy@gameloft.com";
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageDefault;
        private System.Windows.Forms.TabPage tabPageAdd;
        private System.Windows.Forms.Button button_AddNew;
    }
}