namespace LocalMySQL
{
    public partial class IniEditorForm : Form
    {
        private readonly string _iniPath;
        private string? _originalBasedir;
        private string? _originalDatadir;

        public IniEditorForm(string iniPath)
        {
            InitializeComponent();
            _iniPath = iniPath;
            Load += IniEditorForm_Load;
        }

        private void IniEditorForm_Load(object? sender, EventArgs e)
        {
            if (File.Exists(_iniPath))
            {
                var lines = File.ReadAllLines(_iniPath);

                _originalBasedir = lines.FirstOrDefault(l => l.StartsWith("basedir", StringComparison.OrdinalIgnoreCase));
                _originalDatadir = lines.FirstOrDefault(l => l.StartsWith("datadir", StringComparison.OrdinalIgnoreCase));

                txtIniContent.Text = string.Join(Environment.NewLine, lines);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var newLines = txtIniContent.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                var newBasedir = newLines.FirstOrDefault(l => l.StartsWith("basedir", StringComparison.OrdinalIgnoreCase));
                var newDatadir = newLines.FirstOrDefault(l => l.StartsWith("datadir", StringComparison.OrdinalIgnoreCase));

                if (!string.Equals(_originalBasedir, newBasedir, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Você não pode alterar o 'basedir' por segurança.");
                    return;
                }

                if (!string.Equals(_originalDatadir, newDatadir, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Você não pode alterar o 'datadir' por segurança.");
                    return;
                }

                File.WriteAllText(_iniPath, txtIniContent.Text);
                MessageBox.Show("Arquivo salvo com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }


}
