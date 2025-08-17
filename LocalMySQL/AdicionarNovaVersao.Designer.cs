namespace LocalMySQL
{
    partial class AdicionarNovaVersao
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
            progressBar = new ProgressBar();
            lblProgress = new Label();
            label1 = new Label();
            comboVersions = new ComboBox();
            progressPanel = new Panel();
            btnDownload = new Button();
            SuspendLayout();
            // 
            // progressBar
            // 
            progressBar.Location = new Point(12, 207);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(330, 23);
            progressBar.TabIndex = 14;
            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.Location = new Point(12, 189);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(39, 15);
            lblProgress.TabIndex = 15;
            lblProgress.Text = "Status";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 12);
            label1.Name = "label1";
            label1.Size = new Size(99, 15);
            label1.TabIndex = 8;
            label1.Text = "Versão do MySQL";
            // 
            // comboVersions
            // 
            comboVersions.FormattingEnabled = true;
            comboVersions.Location = new Point(12, 31);
            comboVersions.Name = "comboVersions";
            comboVersions.Size = new Size(330, 23);
            comboVersions.TabIndex = 0;
            // 
            // progressPanel
            // 
            progressPanel.Location = new Point(12, 99);
            progressPanel.Name = "progressPanel";
            progressPanel.Size = new Size(330, 80);
            progressPanel.TabIndex = 5;
            // 
            // btnDownload
            // 
            btnDownload.Location = new Point(12, 60);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new Size(330, 33);
            btnDownload.TabIndex = 1;
            btnDownload.Text = "⬇️ Baixar Versão Selecionada";
            btnDownload.UseVisualStyleBackColor = true;
            btnDownload.Click += btnDownload_Click;
            // 
            // AdicionarNovaVersao
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(355, 246);
            Controls.Add(label1);
            Controls.Add(comboVersions);
            Controls.Add(progressBar);
            Controls.Add(progressPanel);
            Controls.Add(lblProgress);
            Controls.Add(btnDownload);
            HelpButton = true;
            MaximizeBox = false;
            Name = "AdicionarNovaVersao";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AdicionarNovaVersao";
            Load += AdicionarNovaVersao_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ProgressBar progressBar;
        private Label lblProgress;
        private Label label1;
        private ComboBox comboVersions;
        private Panel progressPanel;
        private Button btnDownload;
    }
}