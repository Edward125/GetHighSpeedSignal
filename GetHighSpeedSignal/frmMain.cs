using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
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
            this.Text = "Get High Seep Signal from board,Ver:" + Application.ProductVersion;
        }

        private void txtBoard_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();

            if (openfile.ShowDialog() == DialogResult.OK)
            {
                txtBoard.Text = openfile.FileName;
                GetBoardNodes(txtBoard.Text.Trim());
                GetHighSpeedSignal( AllBoardUsedNodes);
            }
        }



        private void GetBoardNodes(string boardfile)
        {

            //
            AllBoardNodes.Clear();
            AllBoardNCNodes.Clear();
            AllBoardUsedNodes.Clear();
            lstMsg.Items.Clear();
            //

            lstMsg.Items.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " start to read board");

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

            //
            AllHighSpeedSignal.Clear();
            HighDDR.Clear();
            HighVRAM.Clear();
            HighCNV.Clear();
            HighSATA.Clear();
            HighPCIE.Clear();
            HighHDMI.Clear();
            HighDP.Clear();
           
            //
            lstMsg.Items.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " start to read high speed signals");
            foreach (string  item in allusednodes)
            {
                string RegexStr = string.Empty;
                //DDR
                RegexStr = @"^DIMM[1-9]*[_A-Z]*[0-9]";
                if (Regex.IsMatch(item.ToUpper(), RegexStr))
                {
                    AllHighSpeedSignal.Add(item);
                    HighDDR.Add(item);
                }
                RegexStr = @"^M_[A-Z]*[_A-Z0-9]+";
                if (Regex.IsMatch(item.ToUpper(), RegexStr))
                {
                    AllHighSpeedSignal.Add(item);
                    HighDDR.Add(item);
                }

                //VRAM
                RegexStr = @"^FB[_A-Z0-9]+";
                if (Regex.IsMatch(item.ToUpper(), RegexStr))
                {
                    AllHighSpeedSignal.Add(item);
                    HighVRAM.Add(item);
                }
                //CNV
                RegexStr = @"^CNV[_A-Z0-9]+";
                if (Regex.IsMatch(item.ToUpper(), RegexStr))
                {
                    AllHighSpeedSignal.Add(item);
                    HighCNV.Add(item);
                }

                //SATA
                RegexStr = @"^(SSD|HDD)_SATA_(RX|TX)[_A-Z0-9]+";
                if (Regex.IsMatch(item.ToUpper(), RegexStr))
                {
                    AllHighSpeedSignal.Add(item);
                    HighSATA.Add(item);
                }

                //PCIE
                RegexStr = @"^GFX_CLK[_A-Z0-9]+";
                if (Regex.IsMatch(item.ToUpper(), RegexStr))
                {
                    AllHighSpeedSignal.Add(item);
                    HighPCIE.Add(item);
                }

                RegexStr = @"^GFX_PCIE_(RX|TX)[_A-Z0-9]+";
                if (Regex.IsMatch(item.ToUpper(), RegexStr))
                {
                    AllHighSpeedSignal.Add(item);
                    HighPCIE.Add(item);
                }
                // HDMI

                RegexStr = @"^HDMI[_A-Z]*(_RX|TX)[_A-Z0-9]*";
                if (Regex.IsMatch(item.ToUpper(), RegexStr))
                {
                    AllHighSpeedSignal.Add(item);
                    HighHDMI.Add(item);
                }

                // DP
                RegexStr = @"^DP[0-9]+_AUX[_A-Z0-9]*";
                if (Regex.IsMatch(item.ToUpper(), RegexStr))
                {
                    AllHighSpeedSignal.Add(item);
                    HighDP.Add(item);
                }
                RegexStr = @"^DP[0-9]+_DDI_TX[_A-Z0-9]*";
                if (Regex.IsMatch(item.ToUpper(), RegexStr))
                {
                    AllHighSpeedSignal.Add(item);
                    HighDP.Add(item);
                }
            }


            lstMsg.Items.Add("All high speed signal:" + AllHighSpeedSignal.Count + ",rate:" + ((double)AllHighSpeedSignal.Count / (double)allusednodes.Count).ToString ("0.00%"));
            lstMsg.Items.Add("DDR high speed signal:" + HighDDR.Count+",rate:" + ((double)HighDDR.Count  / (double)allusednodes.Count).ToString ("0.00%"));
            lstMsg.Items.Add("VRAM high speed signal:" + HighVRAM.Count + ",rate:" + ((double)HighVRAM.Count  / (double)allusednodes.Count).ToString("0.00%"));
            lstMsg.Items.Add("CNV high speed signal:" + HighCNV.Count + ",rate:" + ((double)HighCNV .Count  / (double)allusednodes.Count).ToString("0.00%"));
            lstMsg.Items.Add("SATA high speed signal:" + HighSATA.Count + ",rate:" + ((double)HighSATA.Count  / (double)allusednodes.Count).ToString("0.00%"));
            lstMsg.Items.Add("HIMI high speed signal:" + HighHDMI.Count + ",rate:" + ((double)HighHDMI.Count  / (double)allusednodes.Count).ToString("0.00%"));
            lstMsg.Items.Add("DP high speed signal:" + HighDP.Count + ",rate:" + ((double)HighDP.Count  / (double)allusednodes.Count).ToString("0.00%"));
            lstMsg.Items.Add("PCIE high speed signal:" + HighPCIE.Count + ",rate:" + ((double)HighPCIE.Count  / (double)allusednodes.Count).ToString("0.00%"));

            writeLog(AllHighSpeedSignal, "ALLHIGHSPEED");
            writeLog(HighCNV, "CNV");
            writeLog(HighDDR, "DDR");
            writeLog(HighDP, "DP");
            writeLog(HighHDMI, "HDMI");
            writeLog(HighPCIE, "PCIE");
            writeLog(HighSATA, "SATA");
            writeLog(HighVRAM, "VRAM");


        }



        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtBoard_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnGO_Click(object sender, EventArgs e)
        {

            if (System.IO.File.Exists (txtBoard.Text.Trim ()))
            {
                GetBoardNodes(txtBoard.Text.Trim());
                GetHighSpeedSignal(AllBoardUsedNodes);
            }
        }



        private void writeLog(List<string> nodes, string filename)
        {

            if (System.IO.File.Exists(filename))
                System.IO.File.Delete(filename);


            System.IO.StreamWriter sw = new System.IO.StreamWriter(filename);

            sw.WriteLine(filename);

            foreach (string  item in nodes )
            {
                sw.WriteLine(item);
            }

            sw.Close();

        }

        
    }
}
