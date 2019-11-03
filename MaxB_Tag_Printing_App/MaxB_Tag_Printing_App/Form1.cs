using MaxB_Tag_Printing_App.Model;
using MaxB_Tag_Printing_App.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaxB_Tag_Printing_App
{
    public partial class Form1 : Form
    {
        TagModel tagModel = null;
        Connection con = new Connection();
        public Form1()
        {
            InitializeComponent();
        }

        private void PrintPreviewDialog1_Load(object sender, EventArgs e)
        {
        
        }
        private string ifNullReturnDefaultMOQ(object value)
        {
            if (value == null)
            {
                return "PC";
            }
            else if (String.IsNullOrEmpty(value.ToString()))
            {
                return "PC";
            }
            return value.ToString();

        }
        private string ifNullReturnDefault_One(object value)
        {
            if (value == null)
            {
                return "1";
            }
            else if (String.IsNullOrEmpty(value.ToString()))
            {
                return "1"; 
            }
            return value.ToString();
        }
        private string ifNullReturnDefault_WhiteSpace(object value)
        {
            if (value == null)
            {
                return "";
            }
            else if (String.IsNullOrEmpty(value.ToString()))
            {
                return "";
            }
            return value.ToString();
        }

        private string ifNullReturnDefault_Zero(object value)
        {
            if (value == null)
            {
                return "0";
            }
            else if (String.IsNullOrEmpty(value.ToString()))
            {
                return "0";
            }
            return value.ToString();

        }
        private void DisplayRecords(string branchid)
        { try {
               
                string script = File.ReadAllText("SQL//DisplayRecords.sql");

                script = script.Replace("\r\n", " ");
                script = script.Replace("\t", " ");
                script = script.Replace("@BranchID", branchid);
                dataGridView1.DataSource = con.getDataTableFromDB(script);
            } catch { } }
        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.branch = BranchcomboBox.SelectedIndex;
                Properties.Settings.Default.Save();
            }
            catch { }
        
        AddLog("STARTED...!");
            StartBtn.Enabled = false;
            StopBtn.Enabled = true ;
            Timer1.Start();

          

        }
        private List<TagModel> getRecord(string branchid)
        {
            try
            {
               List< TagModel> tm = new List<TagModel>();
                if (con.con.State == ConnectionState.Open)
                { con.con.Close(); }
                con.con.Open();
                string qot = "'";
                System.Data.SqlClient.SqlDataReader sdr;
                string script = File.ReadAllText("SQL/script.sql");

                script = script.Replace("\r\n", " ");
                script = script.Replace("\t", " ");
                script = script.Replace("@BranchID", branchid);
                SqlCommand cmd = new SqlCommand(script, con.con);


                sdr = cmd.ExecuteReader();
                //  DisplayStateMessage("Loading Data Into Grid");
                while (sdr.Read())
                {
                    TagModel pmt = new TagModel()
                    {
                        LongName = sdr["LongName"].ToString(),
                        AltBarcode = sdr["AltBarcode"].ToString(),
                        MOQ = ifNullReturnDefault_One(sdr["MOQ"]),
                        MOQUnit = ifNullReturnDefaultMOQ(sdr["MOQUnit"]),
                        Barcode = sdr["Barcode"].ToString(),

                        Target = ifNullReturnDefault_Zero(sdr["MaxTarget"]),
                        L2 = sdr["L2"].ToString(),
                        BranchID= sdr["BranchID"].ToString(),
                        SaleRate= sdr["SaleRate"].ToString(),
                        Facings= sdr["Facings"].ToString(),
                        TagType=sdr["TagType"].ToString(),
                        ProductItemID = sdr["ProductItemID"].ToString(),
                       
                        ApplyPrice=ifNullReturnDefault_WhiteSpace(sdr["ApplyPrice"]),
                        Hours_Difference=ifNullReturnDefault_WhiteSpace( sdr["Hours_Difference"]),
                        BCQty=decimal.Parse(ifNullReturnDefault_One(sdr["BCQty"])),
                       
                        TagRequestId= sdr["TagRequestId"].ToString(),
                        

                    };



                    tm.Add(pmt);

                }


                con.con.Close();
                return tm;
            }
            catch (Exception ex)
            {
                AddLog(ex.Message); ; 
                return null; }
            }
        private List<string> Split(string str, int CSize)
        { //csize=25
            var tempList = new List<String>();
            if(str.Length==CSize)   //0-25
            {
                tempList.Add(str);
                return tempList;
            }
            else
            {
                var temp = str.Substring(0, CSize);
                tempList.Add(temp);

                if(str.Length==(CSize+CSize))
                {
                    var temp2 = str.Substring(CSize, (str.Length - CSize - 2));
                    tempList.Add(temp2);
                    return tempList;
                }
                else if(str.Length < (CSize + CSize))
                {
                    var temp2 = str.Substring(CSize, (str.Length - CSize - 2));
                    tempList.Add(temp2);
                    return tempList;

                }
                 if (str.Length > (CSize + CSize))
                {
                    var temp2 = str.Substring(CSize, (CSize - 1));
                    var len = str.Length - (CSize + CSize );
                    var temp3 = str.Substring(CSize + CSize,len);
                    tempList.Add(temp2);
                    tempList.Add(temp3);
                    return tempList;  
                }
                return tempList;
                


                



            }
          
        }
        private void PrintDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            try
            {



                string Product_Name = tagModel.LongName;
                string barcode = tagModel.Barcode;


                
                string rawAltBarcode = "";
                try {rawAltBarcode= tagModel.AltBarcode.Substring(1); } catch { }
                string ProductItemID = tagModel.ProductItemID;
                string L2 = tagModel.L2;
                string Target = tagModel.Target;
     

                try
                {
                    string Altbarcode1 = "";
                    string Altbarcode2 = "";
                    string Altbarcode3 = "";
                    string Altbarcode4 = "";
                    var arr = rawAltBarcode.Split(',');
                    try
                    {
                        if (arr.Length != 1)
                        {
                            rawAltBarcode = "";
                        }
                        try { Altbarcode1 =".."+arr[0].Substring(arr[0].Length - 5);
                            rawAltBarcode += Altbarcode1;
                        } catch { }
                        try { Altbarcode2 = ".." + arr[1].Substring(arr[1].Length - 5);
                            rawAltBarcode +=""+ Altbarcode2;
                        } catch { }
                        try { Altbarcode3 = ".." + arr[2].Substring(arr[2].Length - 5);
                            rawAltBarcode += "" + Altbarcode3;
                        } catch { }
                        try { Altbarcode4 = ".." + arr[3].Substring(arr[3].Length - 5);
                            rawAltBarcode += "" + Altbarcode4;
                        } catch { }

                        rawAltBarcode = ProductItemID  + rawAltBarcode;
                    }
                    catch { }

                }
                catch { }
              
                int X_Adjust = 5;
                int Y_Adjust = 0;

                Pen p = new Pen(Color.Gray);
                Pen p2 = new Pen(Color.Gray);
                Pen p3 = new Pen(Color.Red);
                p.Width = 0;
                p2.Width = 0;
                p3.Width = 0;
                e.Graphics.DrawRectangle(p, 5, 5 + Y_Adjust, 320, 140);
                try {
                    if (double.Parse(tagModel.Hours_Difference) >-1 && double.Parse(tagModel.Hours_Difference) < 13)
                    {
                
                        e.Graphics.DrawString("***", new Font("Arial Narrow ", 12, FontStyle.Bold), Brushes.Black, 240 + X_Adjust, 29 + Y_Adjust);

                    }
                    } catch { }


              
             //   e.Graphics.DrawRectangle(p, 215 + X_Adjust, 5+Y_Adjust, 110, 23);
                e.Graphics.DrawString(L2, new Font("Arial Narrow ", 12, FontStyle.Bold), Brushes.Black, 280 + X_Adjust, 7 + Y_Adjust);
                e.Graphics.DrawString(DateTime.Now.ToString("MMdd"), new Font("Arial Narrow ", 12, FontStyle.Bold), Brushes.Black, 220 + X_Adjust, 7 + Y_Adjust);

              //  e.Graphics.DrawRectangle(p, 215 + X_Adjust, 28+Y_Adjust, 110, 20);
                e.Graphics.DrawString(Target, new Font("Arial Narrow ", 12, FontStyle.Bold), Brushes.Black, 220 + X_Adjust, 29 + Y_Adjust);
               // e.Graphics.DrawString("F:"+tagModel.Facings, new Font("Arial Narrow ", 12, FontStyle.Bold), Brushes.Black, 265 + X_Adjust, 29 + Y_Adjust);

                // e.Graphics.DrawRectangle(p2, 210 + X_Adjust, 5 + Y_Adjust, 113, 140);

                string NameLine1 = "";
               
                try
                {
                    int FixDelimiter = 27;
                    if (Product_Name.Length < FixDelimiter)
                    {
                        NameLine1 = Product_Name;
                        e.Graphics.DrawString(NameLine1, new Font("Arial Narrow ", 10, FontStyle.Bold), Brushes.Black, 7 + X_Adjust, 11 + Y_Adjust);

                    }

                    else
                    {
                        var lineChunks = Split(Product_Name, FixDelimiter).ToList();
                        int temp = -8;
                        for (int i = 0; i < lineChunks.Count; i++)
                        {
                            temp += 15;
                            e.Graphics.DrawString(lineChunks[i].Trim(), new Font("Arial Narrow ", 10, FontStyle.Bold), Brushes.Black, 7 + X_Adjust, temp + Y_Adjust);
                           
                        }
                    }
                   

                        e.Graphics.DrawString(rawAltBarcode, new Font("Arial Narrow ", 10, FontStyle.Bold), Brushes.Black, 7 + X_Adjust, 125 + Y_Adjust);

                    


                }
                catch (Exception ex) { AddLog(ex.Message); }


                try
                {
                    if(decideWhichPriceToBePrint(tagModel.ApplyPrice,tagModel.Hours_Difference))
                    { e.Graphics.DrawString(Math.Round(decimal.Parse(tagModel.ApplyPrice) * tagModel.BCQty).ToString("G29"), new Font("Arial Narrow ", 25, FontStyle.Bold), Brushes.Black, 225 + X_Adjust, 70 + Y_Adjust);
                        Zen.Barcode.Code128BarcodeDraw barcodeImg = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                        Image img = barcodeImg.Draw(tagModel.ProductItemID + "," + tagModel.BCQty + "," + Math.Round(Decimal.Parse(tagModel.ApplyPrice)), 180);
                        e.Graphics.DrawImage(img, 17 + X_Adjust, 63 + Y_Adjust, 180, 60);
                    }else
                    {
                        e.Graphics.DrawString(Math.Round(decimal.Parse(tagModel.SaleRate) * tagModel.BCQty).ToString("G29"), new Font("Arial Narrow ", 25, FontStyle.Bold), Brushes.Black, 225 + X_Adjust, 70 + Y_Adjust);
                        Zen.Barcode.Code128BarcodeDraw barcodeImg = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                        Image img = barcodeImg.Draw(tagModel.ProductItemID + "," + tagModel.BCQty + "," + Math.Round(Decimal.Parse(tagModel.SaleRate)), 180);
                        e.Graphics.DrawImage(img, 17 + X_Adjust, 63 + Y_Adjust, 180, 60);
                    }
                }
                catch { }


                UpdateProductRecord("Update [mbo].[TagRequest] Set [Status]='1' Where [TagRequestId]='"+tagModel.TagRequestId+"'");
              
            } catch (Exception ex) { AddLog(ex.Message); }
            }

        private bool decideWhichPriceToBePrint(object applyPrice, string hours_Difference)
        {
            try
            {
                if (hours_Difference == null)
                { return false; }
                else if (hours_Difference == "")
                { return false; }
                else if (double.Parse(hours_Difference.ToString()) > 12)
                { return false; }
                else if (double.Parse(hours_Difference.ToString()) < 13)
                {
                    try { double.Parse(tagModel.ApplyPrice);
                        return true;
                    }
                    catch { return false; }
                }
                return true;
            }
            catch{ return false; }
        }

        public bool UpdateProductRecord(string updateString)
        {
            try
            {
                if (con.con.State == ConnectionState.Open)
                { con.con.Close(); }
                con.con.Open();
                SqlCommand cmd = new SqlCommand(updateString, con.con);
                cmd.ExecuteNonQuery();
                return true;

            }
            catch (Exception ex)
            {
              AddLog(ex.Message);
                return false;
            }
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                BranchcomboBox.DataSource = Enum.GetValues(typeof(BranchEnum));
                foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                {
                    if (!PrintercomboBox.Items.Contains(printer))
                    {
                        PrintercomboBox.Items.Add(printer);
                    }
                }

            }
            catch { }
            try
            {
              BranchcomboBox.SelectedIndex = Properties.Settings.Default.branch;
                
            }
            catch { }
            try
            {
                PrintercomboBox.SelectedIndex = Properties.Settings.Default.prinername;

            }
            catch { }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            AddLog("STOP...!");
            StartBtn.Enabled = true; 
            StopBtn.Enabled = false; 

            Timer1.Stop();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                var Branch = (BranchEnum)Enum.Parse(typeof(BranchEnum), BranchcomboBox.SelectedItem.ToString());
                string branchid = ((int)Branch).ToString();

                var rec = getRecord(branchid.Trim());
                if (rec != null)
                {
                    for (int i = 0; i < rec.Count; i++)
                    {
                        try
                        {
                            tagModel = null;
                            tagModel = rec[i];
                            AddLog("Fetching Records of " + tagModel.LongName);
                            if (validateApplyDate(tagModel.Hours_Difference))
                            {
                                AddLog("Sending Printing Command of " + tagModel.LongName);
                                if (tagModel.TagType.Trim() != "1")
                                {
                                   
                                    WHTagPrint.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("custom", 400, 200);
                                    WHTagPrint.PrinterSettings.PrinterName = PrintercomboBox.Text;
                                       WHTagPrint.Print();
                                    //printPreviewDialog1.Document = WHTagPrint;
                                    //printPreviewDialog1.ShowDialog();
                                }
                                else
                                {

                                    ShelfPriceTagPrint.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("custom", 330, 150);
                                    ShelfPriceTagPrint.PrinterSettings.PrinterName = PrintercomboBox.Text;
                                     ShelfPriceTagPrint.Print();
                                    //printPreviewDialog1.Document = ShelfPriceTagPrint;
                                    //printPreviewDialog1.ShowDialog();
                                }

                                AddLog("Ended Printing Command");
                            }
                            else
                            {
                                AddLog("Apply Time is Greater Then 12 Hours");
                            }
                                DisplayRecords(branchid.Trim());
                        }
                        catch (Exception ex) { AddLog(ex.Message); }
                    }
                }
            }
            catch (Exception ex) { AddLog(ex.Message); }
        }

        private bool validateApplyDate(object applyDate)
        {
            try
            {
                if(applyDate==null)
                { return true; }
                else if(applyDate.ToString()=="")
                { return true; }
               else if(double.Parse(applyDate.ToString())>12)
                { return false; }
                else if (double.Parse(applyDate.ToString()) < 13)
                { return true; }
                return true;
                

            }
            catch
            { return false; }
        }

        private void AddLog(string text)
        { try
            {
                   richTextBox1.BeginInvoke(new MethodInvoker(() =>
                   richTextBox1.Text = "=>" + text + Environment.NewLine + richTextBox1.Text));
            }
            catch { } }

        private void Button3_Click(object sender, EventArgs e)
        {
            
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                var branchid = BranchcomboBox.Text.Split(new[] { "--" }, StringSplitOptions.None);

                var rec = getRecord(branchid[1].Trim());
                if (rec != null)
                {
                    for (int i = 0; i < rec.Count; i++)
                    {
                        try
                        {
                            tagModel = null;
                            tagModel = rec[i];
                            AddLog("Sending Printing Command of " + tagModel.LongName);
                            if (tagModel.TagType.Trim() != "1")
                            {
                                WHTagPrint.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("custom", 330, 150);
                                printPreviewDialog1.Document = WHTagPrint;
                                printPreviewDialog1.ShowDialog();
                            }
                            else
                            {

                                ShelfPriceTagPrint.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("custom", 400, 200);
                                printPreviewDialog1.Document = ShelfPriceTagPrint;
                                printPreviewDialog1.ShowDialog();
                            }


                            AddLog("Ended Printing Command");
                        }
                        catch (Exception ex) { AddLog(ex.Message); }
                    }
                }
            }
            catch (Exception ex) { AddLog(ex.Message); }




        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void PrintPreviewDialog1_Load_1(object sender, EventArgs e)
        {
   
        }

        private void WHTagPrint_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                string L2 = tagModel.L2;
           
                string Product_Name = tagModel.LongName;
                string barcode = tagModel.Barcode;

                string Altbarcode1 = "";
                string Altbarcode2 = "";
                string Altbarcode3 = "";
                string Altbarcode4 = "";
                string rawAltBarcode = tagModel.AltBarcode;
                try
                {
                    var arr = rawAltBarcode.Split(',');
                    try
                    {
                        try { Altbarcode1 = arr[1]; } catch { }
                        try { Altbarcode2 = arr[2]; } catch { }
                        try { Altbarcode3 = arr[3]; } catch { }
                        try { Altbarcode4 = arr[4]; } catch { }


                    }
                    catch { }

                }
                catch { }
                string ProductItemID = tagModel.ProductItemID;
                string Target = tagModel.Target;
                string CtnSie = tagModel.MOQ;
                string moqUnit = tagModel.MOQUnit;
                int X_Adjust = 0;
                int Y_Adjust = 0;

                Pen p = new Pen(Color.Gray);
                Pen p2 = new Pen(Color.Gray);
                Pen p3 = new Pen(Color.Red);
                p.Width = 0;
                p2.Width = 0;
                p3.Width = 0;

                e.Graphics.DrawRectangle(p, 5 + X_Adjust, 10 + Y_Adjust, 385, 185);


                //e.Graphics.DrawRectangle(p2, 5 + X_Adjust, 10 + Y_Adjust, 290, 25);
                //e.Graphics.DrawRectangle(p2, 5 + X_Adjust, 35 + Y_Adjust, 160, 75);
                ////  e.Graphics.DrawRectangle(p3, 5 + X_Adjust, 110 + Y_Adjust, 160, 25);

                SolidBrush BlackBrush = new SolidBrush(Color.Black);

                e.Graphics.FillRectangle(BlackBrush, 295, 10, 95, 20);
                e.Graphics.DrawString(DateTime.Now.ToString("dd-MMM-yy"), new Font("Arial Narrow ", 11, FontStyle.Bold), Brushes.White, 300 + X_Adjust, 11 + Y_Adjust);



                //e.Graphics.DrawRectangle(p2, 165 + X_Adjust, 35 + Y_Adjust, 130, 25);
                //e.Graphics.DrawRectangle(p2, 165 + X_Adjust, 60 + Y_Adjust, 130, 25);
                //e.Graphics.DrawRectangle(p2, 165 + X_Adjust, 85 + Y_Adjust, 130, 25);
                //e.Graphics.DrawRectangle(p2, 165 + X_Adjust, 110 + Y_Adjust, 130, 25);



                //  e.Graphics.DrawRectangle(p2, 295 + X_Adjust, 10 + Y_Adjust, 95, 50);
                e.Graphics.DrawRectangle(p2, 295 + X_Adjust, 10 + Y_Adjust, 95, 125);

                string NameLine1 = "";
                string NameLine2 = "";
                try
                {
                    int FixDelimiter = 28;
                    if (Product_Name.Length < FixDelimiter)
                    {
                        NameLine1 = Product_Name;
                        e.Graphics.DrawString(NameLine1, new Font("Arial Narrow ", 18, FontStyle.Bold), Brushes.Black, 10 + X_Adjust, 150 + Y_Adjust);

                    }

                    else
                    {
                        NameLine1 = Product_Name.Substring(0, FixDelimiter);
                        NameLine2 = Product_Name.Substring(FixDelimiter, (Product_Name.Length - FixDelimiter));

                        e.Graphics.DrawString(NameLine1, new Font("Arial Narrow ", 16, FontStyle.Bold), Brushes.Black, 10 + X_Adjust, 140 + Y_Adjust);
                        e.Graphics.DrawString(NameLine2, new Font("Arial Narrow ", 16, FontStyle.Bold), Brushes.Black, 10 + X_Adjust, 165 + Y_Adjust);

                    }

                }
                catch (Exception ex) { }
                e.Graphics.DrawString(L2, new Font("Arial Narrow", 15, FontStyle.Bold), Brushes.Black, 10 + X_Adjust, 12 + Y_Adjust);
             

                // sd  e.Graphics.DrawString(NameLine2, new Font("Arial Narrow ", 11, FontStyle.Bold), Brushes.Black, 10 + X_Adjust, 28 + Y_Adjust);


                e.Graphics.DrawString((double.Parse(Target) / double.Parse(moqUnit)).ToString(), new Font("Arial Narrow ", 20, FontStyle.Bold), Brushes.Black, 300 + X_Adjust, 30 + Y_Adjust);
                e.Graphics.DrawString(moqUnit + " x " + CtnSie, new Font("Arial Narrow ", 11, FontStyle.Bold), Brushes.Black, 303 + X_Adjust, 60 + Y_Adjust);
                e.Graphics.DrawString(Target, new Font("Arial Narrow ", 20, FontStyle.Bold), Brushes.Black, 300 + X_Adjust, 80 + Y_Adjust);
                e.Graphics.DrawString("Pc", new Font("Arial Narrow ", 11, FontStyle.Bold), Brushes.Black, 303 + X_Adjust, 115 + Y_Adjust);
                //    

                e.Graphics.DrawString(ProductItemID, new Font("Arial Narrow ", 9, FontStyle.Bold), Brushes.Black, 200 + X_Adjust, 40 + Y_Adjust);
                e.Graphics.DrawString(Altbarcode1, new Font("Arial Narrow ", 9, FontStyle.Bold), Brushes.Black, 200 + X_Adjust, 60 + Y_Adjust);
                e.Graphics.DrawString(Altbarcode2, new Font("Arial Narrow ", 9, FontStyle.Bold), Brushes.Black, 200 + X_Adjust, 80 + Y_Adjust);
                e.Graphics.DrawString(Altbarcode3, new Font("Arial Narrow ", 9, FontStyle.Bold), Brushes.Black, 200 + X_Adjust, 100 + Y_Adjust);
                e.Graphics.DrawString(Altbarcode4, new Font("Arial Narrow ", 9, FontStyle.Bold), Brushes.Black, 200 + X_Adjust, 120 + Y_Adjust);

                Zen.Barcode.Code128BarcodeDraw barcodeImg = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                Image img = barcodeImg.Draw(barcode, 180);
                e.Graphics.DrawImage(img, 14 + X_Adjust, 43 + Y_Adjust, 170, 90);
                //   e.Graphics.DrawString(ProductItemID, new Font("Arial Narrow ", 11, FontStyle.Bold), Brushes.Black, 16 + X_Adjust, 115 + Y_Adjust);
                //    e.Graphics.DrawString("L2", new Font("Arial Narrow ", 9, FontStyle.Bold), Brushes.Black, 30 + X_Adjust, 172 + Y_Adjust);
                //          e.Graphics.DrawString("L3", new Font("Arial Narrow ", 9, FontStyle.Bold), Brushes.Black, 150 + X_Adjust, 172 + Y_Adjust);

                UpdateProductRecord("Update [mbo].[TagRequest] Set [Status]='1' Where [TagRequestId]='" + tagModel.TagRequestId + "'");
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;

                DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];

                string TID = Convert.ToString(selectedRow.Cells["TagRequestId"].Value);
              if(String.IsNullOrWhiteSpace(TID))

                {  MessageBox.Show("There is no Item to Print");
                    return; }
                tagModel = null;
                tagModel = getRecordByRequestID(TID);
                if (tagModel==null)
                {
                    MessageBox.Show("There is no Item to Print");
                    return;
                }
             
                        try
                        {
                            
                            AddLog("Sending Printing Command of " + tagModel.LongName);
                            if (tagModel.TagType.Trim() != "1")
                            {
                                WHTagPrint.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("custom", 400, 200);
                                printPreviewDialog1.Document = WHTagPrint;
                                printPreviewDialog1.ShowDialog();
                            }
                            else
                            {

                                ShelfPriceTagPrint.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("custom", 330, 150);
                                printPreviewDialog1.Document = ShelfPriceTagPrint;
                                printPreviewDialog1.ShowDialog();
                            }


                            AddLog("Ended Printing Command");
                        }
                        catch (Exception ex) { AddLog(ex.Message); }
                
            }
            catch (Exception ex) { AddLog(ex.Message); }
        }

        private TagModel getRecordByRequestID(string tID)
        {
            try
            {
               TagModel tm = new TagModel();
                if (con.con.State == ConnectionState.Open)
                { con.con.Close(); }
                con.con.Open();
                string qot = "'";
                System.Data.SqlClient.SqlDataReader sdr;
                string script = File.ReadAllText("SQL/Script_TRID.sql");

                script = script.Replace("\r\n", " ");
                script = script.Replace("\t", " ");
                script = script.Replace("@TRID", tID);
                SqlCommand cmd = new SqlCommand(script, con.con);


                sdr = cmd.ExecuteReader();
                //  DisplayStateMessage("Loading Data Into Grid");
                while (sdr.Read())
                {
                    TagModel pmt = new TagModel()
                    {
                        LongName = sdr["LongName"].ToString(),
                        AltBarcode = sdr["AltBarcode"].ToString(),
                        MOQ = ifNullReturnDefault_One(sdr["MOQ"]),
                        MOQUnit = ifNullReturnDefaultMOQ(sdr["MOQUnit"]),
                        Barcode = sdr["Barcode"].ToString(),

                        Target = ifNullReturnDefault_Zero(sdr["MaxTarget"]),
                        L2 = sdr["L2"].ToString(),
                        BranchID = sdr["BranchID"].ToString(),
                        SaleRate = sdr["SaleRate"].ToString(),
                        Facings = sdr["Facings"].ToString(),
                        TagType = sdr["TagType"].ToString(),
                        ProductItemID = sdr["ProductItemID"].ToString(),
                        
                        ApplyPrice = ifNullReturnDefault_WhiteSpace(sdr["ApplyPrice"]),
                        Hours_Difference = ifNullReturnDefault_WhiteSpace(sdr["Hours_Difference"]),
                        BCQty = decimal.Parse(ifNullReturnDefault_One(sdr["BCQty"])),

                        TagRequestId = sdr["TagRequestId"].ToString(),


                    };



                    tm = pmt;

                }


                con.con.Close();
                return tm;
            }
            catch (Exception ex)
            {
                AddLog(ex.Message); ;
                return null;
            }
        }

        private void PrintercomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Properties.Settings.Default.prinername = PrintercomboBox.SelectedIndex;
                Properties.Settings.Default.Save();
            }
            catch { }
        }

        private void Button1_Click_2(object sender, EventArgs e)
        {

        }
    }

}
