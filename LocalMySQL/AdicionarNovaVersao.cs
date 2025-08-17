using LocalMySQL.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LocalMySQL
{
    public partial class AdicionarNovaVersao : Form
    {
        private readonly MysqlManager _manager;
        private const string BASE_PATH = "C:\\LocalMySQL";

        public AdicionarNovaVersao()
        {
            InitializeComponent();
            _manager = new MysqlManager(BASE_PATH);
        }

        private async void AdicionarNovaVersao_Load(object sender, EventArgs e)
        {
            await LoadVersionsAsync();
        }

        private async Task LoadVersionsAsync()
        {
            comboVersions.Items.Clear();
            var versions = _manager.GetVersions();
            foreach (var version in versions)
                comboVersions.Items.Add(version);

            comboVersions.SelectedIndex = 0;
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            if (comboVersions.SelectedItem is not string version)
            {
                MessageBox.Show("Selecione uma versão!");
                return;
            }

            progressBar.Value = 0;
            lblProgress.Text = $"Iniciando download da versão {version}...";
            progressPanel.Visible = true;

            var progress = new Progress<double>(p =>
            {
                progressBar.Value = (int)p;
                lblProgress.Text = $"Progresso: {p:0}%";
            });

            var result = await _manager.DownloadMysqlAsync(version, progress);

            if (result.Success)
            {
                lblProgress.Text = "✅ Download concluído!";
            }
            else
            {
                lblProgress.Text = $"❌ Erro: {result.ErrorMessage}";
            }

            await Task.Delay(3000);
            progressPanel.Visible = false;
        }
    }
}
