using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Capture
{
    public partial class Select : Form
    {
        public RecData recData
        {
            get
            {
                return new RecData(this.Left,this.Top,this.Width,this.Height);
            }
           
        }
        
       
        public Select()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        public Select(RecData r)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = r.pos;
            this.Width = r.width;
            this.Height = r.height;
            this.TopMost = true;
        }
       
       
        private void Select_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button==System.Windows.Forms.MouseButtons.Left)
            {
                if (e.X>4&&e.Y>4&&e.X<Width-4&&e.Y<Height-4)
                {
                    lastClick = new Point(e.X, e.Y);
                    methods.Add(move);
                    MouseMove += new MouseEventHandler(move);
                }
                
                if (e.X>this.Width-4)
                {
                    methods.Add(ExpandRight);
                    MouseMove += new MouseEventHandler(ExpandRight);
                    
                }
                if (e.X<4)
                {
                    methods.Add(ExpandLeft);
                    MouseMove += new MouseEventHandler(ExpandLeft);
                }
                 if (e.Y > this.Height-4)
                {
                    methods.Add(ExpandDown);
                    MouseMove += new MouseEventHandler(ExpandDown);
                }
                 if (e.Y < 4)
                 {
                     methods.Add(ExpandUp);
                     MouseMove += new MouseEventHandler(ExpandUp);
                 }
                
            }
        }
        Point lastClick;
        void ExpandRight(object sender, MouseEventArgs e)
        {
            this.Width = e.X;
        }
        void ExpandLeft(object sender, MouseEventArgs e)
        {
            this.Width -= e.X;
            this.Left += e.X;
        }
        void ExpandDown(object sender, MouseEventArgs e)
        {
            this.Height = e.Y;
            
        }
        void ExpandUp(object sender, MouseEventArgs e)
        {
            this.Height -= e.Y;
            this.Top += e.Y;

        }
        void move(object sender, MouseEventArgs e)
        {
           
                this.Left += e.X - this.lastClick.X;
                this.Top += e.Y - this.lastClick.Y;
            
        }
        List<MouseEventHandler> methods = new List<MouseEventHandler>();
        private void Select_MouseUp(object sender, MouseEventArgs e)
        {
            foreach (var item in methods)
            {
                MouseMove -= item;
            }
            methods.Clear();
           
            
        }

        private void Select_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.X > this.Width - 4 && e.Y < 4) || (e.Y > this.Height - 4 && e.X < 4))
            {
                this.Cursor = Cursors.SizeNESW;

            }
            else if ((e.X < 4 && e.Y < 4) || (e.Y > this.Height - 4 && e.X > this.Width-4))
            {
                this.Cursor = Cursors.SizeNWSE;

            }
            else if (e.X > this.Width - 4 || e.X < 4)
            {
               this.Cursor=Cursors.SizeWE;

            }
            
            else if (e.Y > this.Height - 4 || e.Y < 4)
            {
               this.Cursor=Cursors.SizeNS;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void Select_DoubleClick(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void Select_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Escape)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
        }

        
    }
}
