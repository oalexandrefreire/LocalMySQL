using LocalMySQL.Core;
using LocalMySQL.Core.dto;

namespace LocalMySQL
{
    public partial class MainForm : Form
    {
        private readonly MysqlManager _manager;
        private const string BASE_PATH = "C:\\LocalMySQL"; 


        public MainForm()
        {
            InitializeComponent();
            _manager = new MysqlManager(BASE_PATH);
            Load += MainForm_Load;
        }

        private async void MainForm_Load(object? sender, EventArgs e)
        {
            SetupDataGrid();
            await LoadInstancesAsync();

        }

        private void SetupDataGrid()
        {
            dataGridInstances.Columns.Clear();
            dataGridInstances.AutoGenerateColumns = false;
            dataGridInstances.AllowUserToAddRows = false;

            dataGridInstances.Columns.Add(new DataGridViewTextBoxColumn { Name = "Versao", HeaderText = "Version" });
            dataGridInstances.Columns.Add(new DataGridViewTextBoxColumn { Name = "Porta", HeaderText = "Port" });
            dataGridInstances.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status" });
            dataGridInstances.Columns.Add(new DataGridViewTextBoxColumn { Name = "Bancos", HeaderText = "Databases" });
            dataGridInstances.Columns.Add(new DataGridViewTextBoxColumn { Name = "Conexoes", HeaderText = "Connections" });


            var actionButtonColumn = new DataGridViewButtonColumn
            {
                Name = "Acao",
                HeaderText = "Ação",
                Text = "", 
                UseColumnTextForButtonValue = false 
            };
            dataGridInstances.Columns.Add(actionButtonColumn);

            var restartButtonColumn = new DataGridViewButtonColumn
            {
                Name = "Restart",
                HeaderText = "Restart",
                Text = "🔄",
                UseColumnTextForButtonValue = true
            };
            dataGridInstances.Columns.Add(restartButtonColumn);

            var configButtonColumn = new DataGridViewButtonColumn
            {
                Name = "Configurar",
                HeaderText = "Configurar",
                Text = "⚙️",
                UseColumnTextForButtonValue = true
            };
            dataGridInstances.Columns.Add(configButtonColumn);
        }


        private async Task LoadInstancesAsync()
        {
            dataGridInstances.Rows.Clear();

            var instances = _manager.ListInstances();

            foreach (var inst in instances)
            {
                string actionText = inst.Running ? "⏹️" : "▶️";

                int dbCount = -1;
                int connCount = -1;

                if (inst.Running)
                {
                    dbCount = await _manager.GetDatabaseCountAsync(inst);
                    connCount = await _manager.GetActiveConnectionsAsync(inst);

                    if (dbCount >= 0)
                        _manager.SaveInstanceMeta(inst.Version, new InstanceMeta { DatabaseCount = dbCount });
                }
                else
                {
                    var meta = _manager.LoadInstanceMeta(inst.Version);
                    dbCount = meta?.DatabaseCount ?? -1;
                }

                string dbText = dbCount >= 0 ? dbCount.ToString() : "";
                string connText = inst.Running && connCount >= 0 ? connCount.ToString() : "--";

                int rowIndex = dataGridInstances.Rows.Add(
                    inst.Version,
                    inst.Port,
                    inst.Running ? "🟢 Running" : "⏹️ Stopped",
                    dbText,
                    connText,
                    "",  
                    "🔄",
                    "⚙️"
                );

                var row = dataGridInstances.Rows[rowIndex];
                row.Cells["Acao"].Value = actionText;
                row.Cells["Acao"].ReadOnly = false;
                row.Cells["Acao"].Style.ForeColor = Color.Black;
            }
        }


        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadInstancesAsync();
        }

        private async void dataGridInstances_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dataGridInstances.Rows[e.RowIndex];
            var version = row.Cells["Versao"].Value.ToString();
            var columnName = dataGridInstances.Columns[e.ColumnIndex].Name;

            if (columnName == "Acao")
            {
                row.Cells["Acao"].ReadOnly = true;
                row.Cells["Acao"].Value = "⏳";
                row.Cells["Acao"].Style.ForeColor = Color.Gray;

                var currentStatus = row.Cells["Status"].Value?.ToString();
                if (currentStatus != null && currentStatus.Contains("Running"))
                {
                    _manager.StopInstance(version);
                }
                else
                {
                    await _manager.StartInstanceAsync(version);
                }

                await Task.Delay(3000);
                await LoadInstancesAsync();
            }
            else if (columnName == "Restart")
            {
                row.Cells["Restart"].ReadOnly = true;
                row.Cells["Acao"].Value = "⏳";
                _manager.StopInstance(version);
                await Task.Delay(1000);
                await _manager.StartInstanceAsync(version);
                await Task.Delay(3000);
                await LoadInstancesAsync();
            }
            else if (columnName == "Configurar")
            {
                var versionPath = Path.Combine(BASE_PATH, version);
                var iniPath = Path.Combine(versionPath, "my.ini");

                if (File.Exists(iniPath))
                {
                    var iniForm = new IniEditorForm(iniPath);
                    iniForm.ShowDialog();
                    await LoadInstancesAsync();
                }
                else
                {
                    MessageBox.Show("Arquivo my.ini não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            var adicionarNovaVersao = new AdicionarNovaVersao();
            adicionarNovaVersao.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
