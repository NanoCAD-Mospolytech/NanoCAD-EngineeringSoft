namespace Loft
{
    partial class LoftForm
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
            height = new TextBox();
            width = new TextBox();
            radius = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            build = new Button();
            SuspendLayout();
            // 
            // height
            // 
            height.Location = new Point(205, 19);
            height.Name = "height";
            height.Size = new Size(117, 23);
            height.TabIndex = 0;
            // 
            // width
            // 
            width.Location = new Point(205, 48);
            width.Name = "width";
            width.Size = new Size(117, 23);
            width.TabIndex = 1;
            // 
            // radius
            // 
            radius.Location = new Point(205, 77);
            radius.Name = "radius";
            radius.Size = new Size(117, 23);
            radius.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(12, 17);
            label1.Name = "label1";
            label1.Size = new Size(135, 25);
            label1.TabIndex = 3;
            label1.Text = "Высота лофта:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(12, 46);
            label2.Name = "label2";
            label2.Size = new Size(187, 25);
            label2.TabIndex = 4;
            label2.Text = "Ширина основания:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(12, 75);
            label3.Name = "label3";
            label3.Size = new Size(175, 25);
            label3.TabIndex = 5;
            label3.Text = "Радиус основания:";
            // 
            // build
            // 
            build.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            build.Location = new Point(12, 224);
            build.Name = "build";
            build.Size = new Size(128, 75);
            build.TabIndex = 6;
            build.Text = "Построить";
            build.UseVisualStyleBackColor = true;
            build.Click += buttonBuild_Click;
            // 
            // LoftForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ButtonFace;
            ClientSize = new Size(334, 311);
            Controls.Add(build);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(radius);
            Controls.Add(width);
            Controls.Add(height);
            Name = "LoftForm";
            Text = "Параметры";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox height;
        private TextBox width;
        private TextBox radius;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button build;
    }
}