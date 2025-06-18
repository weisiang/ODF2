using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class SlotCondition : UserControl
    {
        int cv_No = 0;
        public int Recipe
        {
            get { return Convert.ToInt32(txt_Recipe.Text.Trim()); }
        }
        public int Count
        {
            get { return Convert.ToInt32(txt_Count.Text.Trim()); }
        }

        public bool Abort
        {
            get { return cb_Abort.Text == "True" ? true : false; }
        }
        public SlotCondition(int m_No)
        {
            InitializeComponent();
            cv_No = m_No;
            this.lbl_No.Text = cv_No.ToString().PadLeft(2, '0');
        }
        public void Clear()
        {
            txt_Count.Text = "";
            txt_Recipe.Text = "";
            cb_Abort.Text = "";
        }
        public void SetContext(int m_Count , int m_Recipe , bool m_IsAbort)
        {
            this.txt_Count.Text = m_Count.ToString();
            this.txt_Recipe.Text = m_Recipe.ToString();
            this.cb_Abort.Text = m_IsAbort.ToString();
        }
    }
}
