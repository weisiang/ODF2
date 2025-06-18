using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonData.HIRATA;
using KgsCommon;
using System.Drawing;

namespace UI
{
    public class Obj
    {
        public bool cv_IsShowUi = false;
        int _controlX;
        int _controlY;
        int _cursorX;
        int _cursorY;
        bool cv_resize;
        protected TabPage cv_Parent = null;
        public Panel cv_Panel = null;
        public UserControl cv_Ui = null;
        public int cv_Id = 0;
        public int cv_Node = 0;
        public int cv_Stage = 0;
        public int cv_TimeChartId = 0;
        public int cv_SlotCount = 0;
        public Obj(int m_Id, int m_slotCount)
        {
            cv_Id = m_Id;
            cv_SlotCount = m_slotCount;
        }
        protected virtual void InitUI()
        {
        }
        private void SetDoubleBuffering(System.Windows.Forms.Control control, bool value)
        {
            System.Reflection.PropertyInfo controlProperty = typeof(System.Windows.Forms.Control)
                .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            controlProperty.SetValue(control, value, null);
        }

        public virtual void InitPanelUI(TabPage m_Parent , KXmlItem m_Xml)
        {
            if (cv_Panel == null)
            {
                cv_Parent = m_Parent;
                cv_Panel = new Panel();
                SetDoubleBuffering(cv_Panel, true);
                cv_Panel.Parent = m_Parent;
                cv_Panel.AutoSize = false;

                cv_Ui.Dock = DockStyle.None;
                cv_Ui.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            }

            int tmp_pos = 0;
            if (int.TryParse(m_Xml.Attributes["TOP"].Trim(), out tmp_pos))
            {
                cv_Panel.Top = tmp_pos;
            }
            if (int.TryParse(m_Xml.Attributes["LEFT"].Trim(), out tmp_pos))
            {
                cv_Panel.Left = tmp_pos;
            }
            if (int.TryParse(m_Xml.Attributes["WIDTH"].Trim(), out tmp_pos))
            {
                if (tmp_pos != 0)
                    cv_Ui.Width = tmp_pos;
            }
            if (int.TryParse(m_Xml.Attributes["HEIGHT"].Trim(), out tmp_pos))
            {
                if (tmp_pos != 0)
                    cv_Ui.Height = tmp_pos;
            }
            cv_Panel.Height = cv_Ui.Size.Height + 10;
            cv_Panel.Width = cv_Ui.Size.Width + 10;
            cv_Panel.Controls.Add(cv_Ui);

            cv_Panel.MouseMove += ctrl_MouseMove;
            cv_Panel.MouseDown += ctrl_MouseDown;
            cv_Panel.AllowDrop = true;

        }
        public void ShowBackPanelColor()
        {
            cv_Panel.BackColor = Color.BlueViolet;
        }
        public void HideBackPanelColor()
        {
            cv_Panel.BackColor = Color.Transparent;
        }
        public virtual void RecoverPanelUI(KXmlItem m_Xml)
        {
            int tmp_pos = 0;
            cv_Panel.Left = Convert.ToInt32(m_Xml.Attributes["LEFT"].Trim());
            cv_Panel.Top = Convert.ToInt32(m_Xml.Attributes["TOP"].Trim());

            if (int.TryParse(m_Xml.Attributes["WIDTH"].Trim(), out tmp_pos))
            {
                if (tmp_pos != 0)
                    cv_Panel.Width = tmp_pos + 10;
            }
            if (int.TryParse(m_Xml.Attributes["HEIGHT"].Trim(), out tmp_pos))
            {
                if (tmp_pos != 0)
                    cv_Panel.Height = tmp_pos + 10;
            }
        }
        void ctrl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!UiForm.cv_Isdragdrop) return;
            Panel pan = (Panel)sender;
            if (e.Button == MouseButtons.Left)
            {
                int tx = Cursor.Position.X;
                int ty = Cursor.Position.Y;
                int _limitX = cv_Parent.Left + cv_Parent.Width;
                int _limitY = cv_Parent.Top + cv_Parent.Height;
                int _left = _controlX + (tx - _cursorX);
                int _top = _controlY + (ty - _cursorY);
                if (_left < 0 || _left > _limitX)
                    pan.Left = 0;
                else if (_left > _limitX)
                    pan.Left = _limitX;
                else
                    pan.Left = _left;

                if (_top < 0)
                    pan.Top = 0;
                else if (_top > _limitY)
                    pan.Top = _limitY;
                else
                    pan.Top = _top;
            }
            if (e.Button == MouseButtons.Right)
            {
                if (cv_resize)
                {
                    if (e.X > 50 && e.Y > 50) //set limite
                    {
                        pan.Width = e.X;
                        pan.Height = e.Y;
                    }
                }
                else
                {
                    Point pt = pan.PointToClient(Control.MousePosition);
                    if (pt.X < pan.Width - 5 && pt.Y < pan.Height - 5)
                    {
                        pan.Cursor = Cursors.SizeNWSE;
                        cv_resize = true;
                    }
                    else
                    {
                        pan.Cursor = Cursors.Default;
                        cv_resize = false;
                    }

                }
            }
        }
        private void ctrl_MouseDown(object sender, MouseEventArgs e)
        {
            if (!UiForm.cv_Isdragdrop) return;
            Panel tmp = (Panel)sender;
            _controlX = tmp.Left;
            _controlY = tmp.Top;
            _cursorX = Cursor.Position.X;
            _cursorY = Cursor.Position.Y;
        }
        public int GetTop()
        {
            int top = 0;
            if(cv_Panel != null)
            {
                top=cv_Panel.Top;
            }
            return top;
        }
        public int GetLeft()
        {
            int left = 0;
            if (cv_Panel != null)
            {
                left = cv_Panel.Left;
            }
            return left;
        }
        public int GetWidth()
        {
            int width = 0;
            if (cv_Panel != null)
            {
                width = cv_Ui.Width;
            }
            return width;
        }
        public int GetHeight()
        {
            int heigh = 0;
            if (cv_Panel != null)
            {
                heigh = cv_Ui.Height;
            }
            return heigh;
        }
        public virtual void reFresh()
        {
        }
    }
}
