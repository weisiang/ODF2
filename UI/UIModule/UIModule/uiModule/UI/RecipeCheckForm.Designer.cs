namespace UI
{
    partial class RecipeCheckForm
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_seq = new System.Windows.Forms.Button();
            this.btn_slot = new System.Windows.Forms.Button();
            this.btn_id = new System.Windows.Forms.Button();
            this.btn_recipe = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel2.Controls.Add(this.btn_seq);
            this.panel2.Controls.Add(this.btn_slot);
            this.panel2.Controls.Add(this.btn_id);
            this.panel2.Controls.Add(this.btn_recipe);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(149, 148);
            this.panel2.TabIndex = 2;
            // 
            // btn_seq
            // 
            this.btn_seq.Location = new System.Drawing.Point(35, 103);
            this.btn_seq.Name = "btn_seq";
            this.btn_seq.Size = new System.Drawing.Size(75, 23);
            this.btn_seq.TabIndex = 9;
            this.btn_seq.Text = "Foup Seq";
            this.btn_seq.UseVisualStyleBackColor = true;
            this.btn_seq.Click += new System.EventHandler(this.btn_Click);
            // 
            // btn_slot
            // 
            this.btn_slot.Location = new System.Drawing.Point(35, 74);
            this.btn_slot.Name = "btn_slot";
            this.btn_slot.Size = new System.Drawing.Size(75, 23);
            this.btn_slot.TabIndex = 8;
            this.btn_slot.Text = "Word Slot";
            this.btn_slot.UseVisualStyleBackColor = true;
            this.btn_slot.Click += new System.EventHandler(this.btn_Click);
            // 
            // btn_id
            // 
            this.btn_id.Location = new System.Drawing.Point(35, 45);
            this.btn_id.Name = "btn_id";
            this.btn_id.Size = new System.Drawing.Size(75, 23);
            this.btn_id.TabIndex = 7;
            this.btn_id.Text = "Word Id";
            this.btn_id.UseVisualStyleBackColor = true;
            this.btn_id.Click += new System.EventHandler(this.btn_Click);
            // 
            // btn_recipe
            // 
            this.btn_recipe.Location = new System.Drawing.Point(35, 16);
            this.btn_recipe.Name = "btn_recipe";
            this.btn_recipe.Size = new System.Drawing.Size(75, 23);
            this.btn_recipe.TabIndex = 6;
            this.btn_recipe.Text = "Recipe";
            this.btn_recipe.UseVisualStyleBackColor = true;
            this.btn_recipe.Click += new System.EventHandler(this.btn_Click);
            // 
            // RecipeCheckForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(149, 148);
            this.Controls.Add(this.panel2);
            this.Name = "RecipeCheckForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RecipeCheckForm";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RecipeCheckForm_FormClosing);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btn_seq;
        private System.Windows.Forms.Button btn_slot;
        private System.Windows.Forms.Button btn_id;
        private System.Windows.Forms.Button btn_recipe;
    }
}