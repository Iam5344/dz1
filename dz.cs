using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

class Program
{
    [STAThread]
    static void Main()
    {
        Application.Run(new Form1());
    }
}

public class Form1 : Form
{
    ListBox listProcesses = new ListBox() { Left = 10, Top = 40, Width = 400, Height = 450 };
    Label lblInfo = new Label() { Left = 420, Top = 40, Width = 350, Height = 450, AutoSize = false };
    NumericUpDown numInterval = new NumericUpDown() { Left = 130, Top = 10, Width = 80, Minimum = 1, Maximum = 60, Value = 2 };
    Button btnKill = new Button() { Text = "Завершити процес", Left = 420, Top = 500, Width = 150 };
    Timer timer = new Timer();

    public Form1()
    {
        this.Text = "Менеджер процесів";
        this.Width = 800;
        this.Height = 580;

        this.Controls.Add(new Label() { Text = "Інтервал оновлення:", Left = 10, Top = 12, Width = 120 });
        this.Controls.Add(numInterval);
        this.Controls.Add(listProcesses);
        this.Controls.Add(lblInfo);
        this.Controls.Add(btnKill);

        numInterval.ValueChanged += (s, e) =>
        {
            timer.Interval = (int)numInterval.Value * 1000;
        };

        listProcesses.SelectedIndexChanged += (s, e) => ShowInfo();

        btnKill.Click += (s, e) =>
        {
            if (listProcesses.SelectedItem == null) return;

            string selected = listProcesses.SelectedItem.ToString();
            int pid = int.Parse(selected.Split('|')[1].Trim());

            try
            {
                Process.GetProcessById(pid).Kill();
                MessageBox.Show("Процес завершено.");
                LoadProcesses();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
        };

        timer.Interval = (int)numInterval.Value * 1000;
        timer.Tick += (s, e) => LoadProcesses();
        timer.Start();

        LoadProcesses();
    }

    void LoadProcesses()
    {
        int selectedPid = -1;

        if (listProcesses.SelectedItem != null)
        {
            string sel = listProcesses.SelectedItem.ToString();
            int.TryParse(sel.Split('|')[1].Trim(), out selectedPid);
        }

        listProcesses.Items.Clear();

        foreach (var p in Process.GetProcesses().OrderBy(p => p.ProcessName))
        {
            try { listProcesses.Items.Add($"{p.ProcessName} | {p.Id}"); }
            catch { }
        }

        if (selectedPid != -1)
        {
            for (int i = 0; i < listProcesses.Items.Count; i++)
            {
                if (listProcesses.Items[i].ToString().Contains($"| {selectedPid}"))
                {
                    listProcesses.SelectedIndex = i;
                    break;
                }
            }
        }
    }

    void ShowInfo()
    {
        if (listProcesses.SelectedItem == null) return;

        string selected = listProcesses.SelectedItem.ToString();
        int pid = int.Parse(selected.Split('|')[1].Trim());

        try
        {
            Process p = Process.GetProcessById(pid);
            p.Refresh();

            string name = p.ProcessName;
            int count = Process.GetProcessesByName(name).Length;

            string info = "";
            info += $"Назва: {p.ProcessName}\n";
            info += $"PID: {p.Id}\n";

            try { info += $"Час старту: {p.StartTime}\n"; }
            catch { info += "Час старту: недоступно\n"; }

            try { info += $"Процесорний час: {p.TotalProcessorTime.TotalSeconds:F2} сек\n"; }
            catch { info += "Процесорний час: недоступно\n"; }

            try { info += $"Кількість потоків: {p.Threads.Count}\n"; }
            catch { info += "Кількість потоків: недоступно\n"; }

            info += $"Копій процесу: {count}\n";

            lblInfo.Text = info;
        }
        catch (Exception ex)
        {
            lblInfo.Text = "Помилка: " + ex.Message;
        }
    }
}
