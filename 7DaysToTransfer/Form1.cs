﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _7DllsToDie;

namespace _7DaysToTransfer
{
    public partial class Form1 : Form
    {
        public static List<WorldSaveItem> Saves = new List<WorldSaveItem>();
        public Form1()
        {
            SaveManager.PlatformUpdate();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            if (!Directory.Exists(SaveManager.SaveFolder))
            {
                MessageBox.Show("7 Days To Die saves not found.", "Save Not Found", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Application.Exit();
                Environment.Exit(1);
            }
            Saves = SaveManager.ScanSaves();
            foreach (var save in Saves)
            {
                listView1.Items.Add(save.World).SubItems.Add(save.GameName);
                listView2.Items.Add(save.World).SubItems.Add(save.GameName);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0 & listView2.SelectedIndices.Count > 0)
            {
                var FromSave = Saves[listView1.SelectedIndices[0]];
                var ToSave = Saves[listView2.SelectedIndices[0]];
                SelectID(FromSave, ToSave);
            }
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0 & listView2.SelectedIndices.Count > 0)
            {
                var FromSave = Saves[listView2.SelectedIndices[0]];
                var ToSave = Saves[listView1.SelectedIndices[0]];
                SelectID(FromSave, ToSave);
            }
        }

        private void SelectID(WorldSaveItem FromSave, WorldSaveItem ToSave)
        {
            var form = new Id_Select()
            {
                FromSave = FromSave,
                ToSave = ToSave
            };
            if (form.ShowDialog() == DialogResult.OK)
                SaveManager.CopySave(form.fromPath, form.toPath);
        }

        private void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0 & listView2.SelectedIndices.Count > 0)
            {
                button1.Enabled = true;
                button2.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
                button2.Enabled = false;
            }

                ImportLeft.Enabled = ExportLeft.Enabled = (listView1.SelectedIndices.Count > 0);
            ImportRight.Enabled = ExportRight.Enabled = (listView2.SelectedIndices.Count > 0);

        }

        private void Import_Click(object sender, EventArgs e)
        {
            var Sender = (Button)sender;
            WorldSaveItem Save;
            if (Sender.Name == "ImportLeft")
                Save = Saves[listView1.SelectedIndices[0]];
            else
                Save = Saves[listView2.SelectedIndices[0]];
            var importForm = new Import()
            {
                Save = Save
            };
            if (importForm.ShowDialog() == DialogResult.OK) 
                SaveManager.ImportSave(importForm.path, Save.Players[importForm.IdIndex].Path);
        }
        private void Export_Click(object sender, EventArgs e)
        {
            var Sender = (Button)sender;
            WorldSaveItem Save;
            if (Sender.Name == "ExportLeft")
                Save = Saves[listView1.SelectedIndices[0]];
            else
                Save = Saves[listView2.SelectedIndices[0]];
            var exportForm = new export()
            {
                Save = Save
            };
            if (exportForm.ShowDialog() == DialogResult.OK)
                SaveManager.ExportSave(Save.Players[exportForm.index].Path,exportForm.path);
        }
    }
}
