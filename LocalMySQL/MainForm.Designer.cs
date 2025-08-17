namespace LocalMySQL
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            flowInstances = new FlowLayoutPanel();
            imageList1 = new ImageList(components);
            groupBox2 = new GroupBox();
            dataGridInstances = new DataGridView();
            Versão = new DataGridViewTextBoxColumn();
            Porta = new DataGridViewTextBoxColumn();
            Status = new DataGridViewTextBoxColumn();
            Iniciar = new DataGridViewButtonColumn();
            Parar = new DataGridViewButtonColumn();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            pictureBox2 = new PictureBox();
            label3 = new Label();
            label4 = new Label();
            pictureBox3 = new PictureBox();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridInstances).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // flowInstances
            // 
            flowInstances.AutoScroll = true;
            flowInstances.AutoSize = true;
            flowInstances.Location = new Point(15, 80);
            flowInstances.Name = "flowInstances";
            flowInstances.Size = new Size(0, 0);
            flowInstances.TabIndex = 6;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageSize = new Size(16, 16);
            imageList1.TransparentColor = Color.Transparent;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dataGridInstances);
            groupBox2.Controls.Add(flowInstances);
            groupBox2.Location = new Point(12, 96);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(710, 346);
            groupBox2.TabIndex = 14;
            groupBox2.TabStop = false;
            groupBox2.Text = "Servidores locais";
            // 
            // dataGridInstances
            // 
            dataGridInstances.AllowUserToAddRows = false;
            dataGridInstances.AllowUserToDeleteRows = false;
            dataGridInstances.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridInstances.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridInstances.Columns.AddRange(new DataGridViewColumn[] { Versão, Porta, Status, Iniciar, Parar });
            dataGridInstances.Location = new Point(15, 22);
            dataGridInstances.MultiSelect = false;
            dataGridInstances.Name = "dataGridInstances";
            dataGridInstances.ReadOnly = true;
            dataGridInstances.Size = new Size(689, 318);
            dataGridInstances.TabIndex = 7;
            dataGridInstances.CellClick += dataGridInstances_CellClick;
            // 
            // Versão
            // 
            Versão.HeaderText = "Versão";
            Versão.Name = "Versão";
            Versão.ReadOnly = true;
            // 
            // Porta
            // 
            Porta.HeaderText = "Porta";
            Porta.Name = "Porta";
            Porta.ReadOnly = true;
            // 
            // Status
            // 
            Status.HeaderText = "Status";
            Status.Name = "Status";
            Status.ReadOnly = true;
            // 
            // Iniciar
            // 
            Iniciar.HeaderText = "Iniciar";
            Iniciar.Name = "Iniciar";
            Iniciar.ReadOnly = true;
            // 
            // Parar
            // 
            Parar.HeaderText = "Parar";
            Parar.Name = "Parar";
            Parar.ReadOnly = true;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 14);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(50, 50);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 15;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(65, 18);
            label2.Name = "label2";
            label2.Size = new Size(174, 40);
            label2.TabIndex = 16;
            label2.Text = "LocalMySQL";
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(632, 25);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(30, 30);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 17;
            pictureBox2.TabStop = false;
            pictureBox2.Click += btnRefresh_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(664, 31);
            label3.Name = "label3";
            label3.Size = new Size(62, 17);
            label3.TabIndex = 18;
            label3.Text = "REFRESH";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.Black;
            label4.Location = new Point(532, 31);
            label4.Name = "label4";
            label4.Size = new Size(87, 17);
            label4.TabIndex = 20;
            label4.Text = "NEW SERVER";
            label4.Click += label4_Click;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = (Image)resources.GetObject("pictureBox3.Image");
            pictureBox3.Location = new Point(501, 25);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(29, 29);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.TabIndex = 19;
            pictureBox3.TabStop = false;
            pictureBox3.Click += pictureBox3_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(733, 458);
            Controls.Add(label4);
            Controls.Add(pictureBox3);
            Controls.Add(label3);
            Controls.Add(pictureBox2);
            Controls.Add(label2);
            Controls.Add(pictureBox1);
            Controls.Add(groupBox2);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Local MySQL";
            Load += MainForm_Load;
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridInstances).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private FlowLayoutPanel flowInstances;
        private ImageList imageList1;
        private GroupBox groupBox2;
        private DataGridView dataGridInstances;
        private DataGridViewTextBoxColumn Versão;
        private DataGridViewTextBoxColumn Porta;
        private DataGridViewTextBoxColumn Status;
        private DataGridViewButtonColumn Iniciar;
        private DataGridViewButtonColumn Parar;
        private PictureBox pictureBox1;
        private Label label2;
        private PictureBox pictureBox2;
        private Label label3;
        private Label label4;
        private PictureBox pictureBox3;
    }
}