using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GetHighSpeedSignal
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }


        #region 参数


        //
        List<string> AllBoardNodes = new List<string>();
        List<string> AllBoardNCNodes = new List<string>();
        List<string> AllBoardUsedNodes = new List<string>();
        //
        List<string> AllHighSpeedSignal = new List<string>();
        List<string> HighDDR = new List<string>();
        List<string> HighVRAM = new List<string>();
        List<string> HighCNV = new List<string>();
        List<string> HighSATA = new List<string>();
        List<string> HighPCIE = new List<string>();
        List<string> HighHDMI = new List<string>();
        List<string> HighDP = new List<string>();


        #endregion


        private void frmMain_Load(object sender, EventArgs e)
        {

        }

        private void txtBoard_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();

            if (openfile.ShowDialog() == DialogResult.OK)
            {
                txtBoard.Text = openfile.FileName;
                GetBoardNodes(txtBoard.Text.Trim());
            }
        }



        private void GetBoardNodes(string boardfile)
        {

            //
            AllBoardNodes.Clear();
            AllBoardNCNodes.Clear();
            AllBoardUsedNodes.Clear();
            //

            string linestr = string.Empty;
            bool IsNodesStart = false;
            System.IO.StreamReader sr = new System.IO.StreamReader(boardfile);
            while (!sr.EndOfStream )
            {
                linestr = sr.ReadLine();
                if (linestr.Trim().ToUpper() == "NODES")
                    IsNodesStart = true;
               if (IsNodesStart  &&  linestr .Trim ().ToUpper () =="DEVICES")
                   IsNodesStart = false ;
               if (IsNodesStart)
               {
                   if (!string.IsNullOrEmpty(linestr) && linestr.EndsWith(";"))
                   {
                       AllBoardNodes.Add(linestr.Trim().Replace(";", ""));
                       if ( linestr.Trim ().ToUpper ().StartsWith ("NC_"))
                           AllBoardNCNodes.Add(linestr.Trim().Replace(";", ""));
                       else
                           AllBoardUsedNodes.Add(linestr.Trim().Replace(";", ""));
                   }
               }

            }            
            sr.Close();
           lstMsg.Items.Add ("Find all nodes:" + AllBoardNodes.Count + ",NC nodes:" + AllBoardNCNodes.Count + ",Used nodes:" + AllBoardUsedNodes.Count);
        }


        private void GetHighSpeedSignal(List<string> allusednodes)
        {

        }




        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtBoard_TextChanged(object sender, EventArgs e)
        {

        }


        
    }
}
