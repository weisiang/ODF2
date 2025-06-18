using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KgsCommon;
using System.Text.RegularExpressions;
using CommonData.HIRATA;

namespace UI
{
    public partial class palette : Form
    {
        KXmlItem cv_ColorXml = new KXmlItem();
        public static Color cv_SingletonColor = new Color();
        public static Color cv_NotGlass = new Color();
        public static Color cv_NotProcess = new Color();
        public static Color cv_HasProcessed = new Color();
        public static Color cv_ResultOK = new Color();
        public static Color cv_ResultNG = new Color();
        public palette()
        {
            InitializeComponent();
            InitTable();
            SetTitleInPanel();
            InitColor();
        }

        private void SetTitleInPanel()
        {
            Label tmp = tableLayoutPanel1.GetControlFromPosition(0, 0) as Label;
            tmp.Text = "Color";
            for (int i = 1; i < tableLayoutPanel1.ColumnCount; i++)
            {
                tmp = tableLayoutPanel1.GetControlFromPosition(i, 0) as Label;
                if(i == 1)
                    tmp.Text = "Has Glass";
                if(i == 2)
                    tmp.Text = "Need Process";
                if(i == 3)
                    tmp.Text = "Had Processed";
                if(i == 4)
                    tmp.Text = "Result";
            }
            for (int k = 1; k < tableLayoutPanel1.RowCount; k++)
            {
                tmp = tableLayoutPanel1.GetControlFromPosition(0, k) as Label;
                tmp.Text = "";
                tmp.Click += new System.EventHandler(Color_click);
            }

        }
        private void InitColor()
        {
            cv_palette.BackColor = Color.FromArgb(cv_RedBar.Value, cv_GreenBar.Value, cv_BlueBar.Value);
            Color tmp_color = new Color();
            int xmlfileitem = CommonStaticData.cv_ColorXml.ItemsByName["Color"].ItemNumber;
            int red_value = 0, blue_value, green_value = 0;

            if (xmlfileitem != 5)
            {
                return;
            }
            for (int i = 0; i < CommonStaticData.cv_ColorXml.ItemsByName["Color"].ItemNumber; i++)
            {
                KXmlItem color_item = CommonStaticData.cv_ColorXml.Items[i];
                red_value = SysUtils.StrToInt(color_item.Attributes["Red"]);
                blue_value = SysUtils.StrToInt(color_item.Attributes["Blue"]);
                green_value = SysUtils.StrToInt(color_item.Attributes["Green"]);
                tmp_color = Color.FromArgb(red_value, green_value, blue_value);

                switch (i)
                {
                    case 0:
                        cv_NotGlass = tmp_color;
                        tableLayoutPanel1.GetControlFromPosition(0,1).BackColor = cv_NotGlass;
                        break;
                    case 1:
                        cv_NotProcess = tmp_color;
                        tableLayoutPanel1.GetControlFromPosition(0, 2).BackColor = cv_NotProcess;
                        break;
                    case 2:
                        cv_HasProcessed = tmp_color;
                        tableLayoutPanel1.GetControlFromPosition(0, 3).BackColor = cv_HasProcessed;
                        break;
                    case 3:
                        cv_ResultNG = tmp_color;
                        tableLayoutPanel1.GetControlFromPosition(0, 4).BackColor = cv_ResultNG; 
                        break;
                    case 4:
                        cv_ResultOK = tmp_color;
                        tableLayoutPanel1.GetControlFromPosition(0, 5).BackColor = cv_ResultOK;
                        break;
                }
            }
        }

        private void Color_click(object sender, EventArgs e)
        {
            Control tmp = sender as Control;
            tmp.BackColor = cv_SingletonColor;

            int column = tableLayoutPanel1.GetCellPosition(tmp).Column;
            int row = tableLayoutPanel1.GetCellPosition(tmp).Row;

            if ((column == 0) && (row == 1))
            {
                cv_NotGlass = tmp.BackColor;
            }
            else if ((column == 0) && (row == 2))
            {
                cv_NotProcess = tmp.BackColor;
            }
            else if ((column == 0) && (row == 3))
            {
                cv_HasProcessed = tmp.BackColor;
            }
            else if ((column == 0) && (row == 5))
            {
                cv_ResultOK = tmp.BackColor;
            }
            else if ((column == 0) && (row == 4))
            {
                cv_ResultNG = tmp.BackColor;
            }
            SaveColorToFile();
        }
        private void SaveColorToFile()
        {
            KXmlItem tmp_record_item = new KXmlItem();
            for (int i = 0; i < CommonStaticData.cv_ColorXml.ItemsByName["Color"].ItemNumber; i++)
            {
                tmp_record_item = CommonStaticData.cv_ColorXml.Items[i];
                FillColorValueToXmlItem(ref tmp_record_item, i);
            }
            CommonStaticData.cv_ColorXml.SaveToFile(CommonData.HIRATA.CommonStaticData.g_RootConfigFolderPath + "UI\\ColorRecord.xml" , false );
        }

        private void FillColorValueToXmlItem(ref KXmlItem m_Xml, int No)
        {
            switch (No)
            {
                case 0:
                    m_Xml.Attributes["Red"] = cv_NotGlass.R.ToString();
                    m_Xml.Attributes["Blue"] = cv_NotGlass.B.ToString();
                    m_Xml.Attributes["Green"] = cv_NotGlass.G.ToString();
                    break;
                case 1:
                    m_Xml.Attributes["Red"] = cv_NotProcess.R.ToString();
                    m_Xml.Attributes["Blue"] = cv_NotProcess.B.ToString();
                    m_Xml.Attributes["Green"] = cv_NotProcess.G.ToString();
                    break;
                case 2:
                    m_Xml.Attributes["Red"] = cv_HasProcessed.R.ToString();
                    m_Xml.Attributes["Blue"] = cv_HasProcessed.B.ToString();
                    m_Xml.Attributes["Green"] = cv_HasProcessed.G.ToString();
                    break;
                case 3:
                    m_Xml.Attributes["Red"] = cv_ResultNG.R.ToString();
                    m_Xml.Attributes["Blue"] = cv_ResultNG.B.ToString();
                    m_Xml.Attributes["Green"] = cv_ResultNG.G.ToString();
                    break;
                case 4:
                    m_Xml.Attributes["Red"] = cv_ResultOK.R.ToString();
                    m_Xml.Attributes["Blue"] = cv_ResultOK.B.ToString();
                    m_Xml.Attributes["Green"] = cv_ResultOK.G.ToString();
                    break;
            }
        }

        private void InitTable()
        {
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            for (int i = 0; i < 5; i++)
            {
                for (int k = 0; k < 6; k++)
                {
                    Label tmp = new Label();
                    if(i>=k)
                    tmp.Text = "X";
                    else
                    tmp.Text = "O";

                    tmp.Dock = DockStyle.Fill;
                    tmp.TextAlign = ContentAlignment.MiddleCenter;
                    tableLayoutPanel1.Controls.Add(tmp, i, k);
                }
            }
        }

        private void MixColor(object sender, EventArgs e)
        {
            cv_palette.BackColor = Color.FromArgb(cv_RedBar.Value, cv_GreenBar.Value, cv_BlueBar.Value);
            cv_SingletonColor = cv_palette.BackColor;
        }

        private void palette_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

    }
}
