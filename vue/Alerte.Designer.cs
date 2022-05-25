
namespace Mediatek86.vue
{
    partial class Alerte
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
            this.dgvLstExpiration = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLstExpiration)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvLstExpiration
            // 
            this.dgvLstExpiration.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLstExpiration.Location = new System.Drawing.Point(27, 48);
            this.dgvLstExpiration.Name = "dgvLstExpiration";
            this.dgvLstExpiration.RowHeadersWidth = 51;
            this.dgvLstExpiration.RowTemplate.Height = 24;
            this.dgvLstExpiration.Size = new System.Drawing.Size(382, 218);
            this.dgvLstExpiration.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(22, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(404, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Liste des abonnements arrivant à expiration : ";
            // 
            // Alerte
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 296);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvLstExpiration);
            this.Name = "Alerte";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dgvLstExpiration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvLstExpiration;
        private System.Windows.Forms.Label label1;
    }
}