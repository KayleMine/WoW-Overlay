namespace kibitka
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>


        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pidbox = new System.Windows.Forms.TextBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.open = new System.Windows.Forms.Button();
            this.ex = new System.Windows.Forms.Button();
            this.orb = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.LabL = new System.Windows.Forms.Label();
            this.OreBox = new System.Windows.Forms.CheckBox();
            this.HerbBox = new System.Windows.Forms.CheckBox();
            this.ContainerBox = new System.Windows.Forms.CheckBox();
            this.RareBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // pidbox
            // 
            this.pidbox.Location = new System.Drawing.Point(8, 49);
            this.pidbox.Name = "pidbox";
            this.pidbox.Size = new System.Drawing.Size(243, 20);
            this.pidbox.TabIndex = 1;
            this.pidbox.Text = "Pid";
            // 
            // timer
            // 
            this.timer.Interval = 20;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // open
            // 
            this.open.BackColor = System.Drawing.Color.Green;
            this.open.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.open.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.open.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.open.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.open.Location = new System.Drawing.Point(8, 75);
            this.open.Name = "open";
            this.open.Size = new System.Drawing.Size(77, 31);
            this.open.TabIndex = 0;
            this.open.Text = "Start";
            this.open.UseVisualStyleBackColor = false;
            this.open.Click += new System.EventHandler(this.open_Click);
            // 
            // ex
            // 
            this.ex.BackColor = System.Drawing.Color.Brown;
            this.ex.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ex.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ex.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ex.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.ex.Location = new System.Drawing.Point(174, 75);
            this.ex.Name = "ex";
            this.ex.Size = new System.Drawing.Size(77, 31);
            this.ex.TabIndex = 2;
            this.ex.Text = "Exit";
            this.ex.UseVisualStyleBackColor = false;
            this.ex.Click += new System.EventHandler(this.ex_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DarkCyan;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.button1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button1.Location = new System.Drawing.Point(91, 75);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(77, 31);
            this.button1.TabIndex = 3;
            this.button1.Text = "Stop";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.stop_click);
            // 
            // LabL
            // 
            this.LabL.AutoSize = true;
            this.LabL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LabL.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LabL.Image = global::kibitka.Properties.Resources.Hue_Saturation_1;
            this.LabL.Location = new System.Drawing.Point(5, 9);
            this.LabL.Name = "LabL";
            this.LabL.Size = new System.Drawing.Size(47, 15);
            this.LabL.TabIndex = 4;
            this.LabL.Text = "label1";
            // 
            // OreBox
            // 
            this.OreBox.AutoSize = true;
            this.OreBox.BackColor = System.Drawing.Color.Transparent;
            this.OreBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OreBox.Location = new System.Drawing.Point(8, 112);
            this.OreBox.Name = "OreBox";
            this.OreBox.Size = new System.Drawing.Size(46, 17);
            this.OreBox.TabIndex = 5;
            this.OreBox.Text = "Ore";
            this.OreBox.UseVisualStyleBackColor = false;
            this.OreBox.CheckedChanged += new System.EventHandler(this.OreBox_CheckedChanged);
            // 
            // HerbBox
            // 
            this.HerbBox.AutoSize = true;
            this.HerbBox.BackColor = System.Drawing.Color.Transparent;
            this.HerbBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.HerbBox.Location = new System.Drawing.Point(64, 112);
            this.HerbBox.Name = "HerbBox";
            this.HerbBox.Size = new System.Drawing.Size(53, 17);
            this.HerbBox.TabIndex = 6;
            this.HerbBox.Text = "Herb";
            this.HerbBox.UseVisualStyleBackColor = false;
            // 
            // ContainerBox
            // 
            this.ContainerBox.AutoSize = true;
            this.ContainerBox.BackColor = System.Drawing.Color.Transparent;
            this.ContainerBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ContainerBox.Location = new System.Drawing.Point(127, 112);
            this.ContainerBox.Name = "ContainerBox";
            this.ContainerBox.Size = new System.Drawing.Size(64, 17);
            this.ContainerBox.TabIndex = 7;
            this.ContainerBox.Text = "Chests";
            this.ContainerBox.UseVisualStyleBackColor = false;
            // 
            // RareBox
            // 
            this.RareBox.AutoSize = true;
            this.RareBox.BackColor = System.Drawing.Color.Transparent;
            this.RareBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.RareBox.Location = new System.Drawing.Point(194, 112);
            this.RareBox.Name = "RareBox";
            this.RareBox.Size = new System.Drawing.Size(59, 17);
            this.RareBox.TabIndex = 8;
            this.RareBox.Text = "Rares";
            this.RareBox.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::kibitka.Properties.Resources.Hue_Saturation_1;
            this.ClientSize = new System.Drawing.Size(263, 143);
            this.Controls.Add(this.RareBox);
            this.Controls.Add(this.ContainerBox);
            this.Controls.Add(this.HerbBox);
            this.Controls.Add(this.OreBox);
            this.Controls.Add(this.LabL);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ex);
            this.Controls.Add(this.pidbox);
            this.Controls.Add(this.open);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Scanner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button open;
        private System.Windows.Forms.TextBox pidbox;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button ex;
        private System.Windows.Forms.Timer orb;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label LabL;
        private System.Windows.Forms.CheckBox OreBox;
        private System.Windows.Forms.CheckBox HerbBox;
        private System.Windows.Forms.CheckBox ContainerBox;
        private System.Windows.Forms.CheckBox RareBox;
    }
}

