using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProductNotes
{
    class product
    {
        public string Serial { get; set; }
        public int SerialNumberId { get; set; }
        public int ProductId { get; set; }
        public int EuiId { get; set; }
        public int Content { get; set; }
        public DateTime CreateDate { get; set; }
        public System.Nullable<System.DateTime> UpdateDate { get; set; }
        public int TesterId { get; set; }
        public string Tester { get; set; }
    }

    class product_info
    {
        public string Name;
        public string Description;
        public string Model;
    }

    public partial class AddNote : System.Web.UI.Page
    {
        static int _site_id = 2;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Find the las producted tested
                DateTime date = DateTime.Now;
                using (ManufacturingDataDataContext dc = new ManufacturingDataDataContext())
                {
                    _site_id = dc.ProductionSites.Where(p => p.Name == "Centralite").Select(p => p.Id).First<int>();

                    List<SerialNumber> ser = new List<SerialNumber>();
                    while (true)
                    {
                        ser = getSerials(date.Date, DateTime.Now);
                        if (ser.Any())
                            break;
                        date = date.AddDays(-1);
                    }

                }
                TextBox_fromDate.Text = date.Date.ToString();
                TextBox_toDate.Text = DateTime.Now.ToString();

            }

            updateData();

        }

        List<SerialNumber> getSerials(DateTime fromDate, DateTime toDate)
        {
            List<SerialNumber> serials = new List<SerialNumber>();
            ManufacturingDataDataContext dc = new ManufacturingDataDataContext();

            var ser =
            dc.SerialNumbers
            .Where(s =>
                (s.CreateDate >= fromDate || s.UpdateDate.Value.Date >= fromDate) &&
                (s.CreateDate < toDate || s.UpdateDate.Value.Date < toDate) &&
                s.EuiList.ProductionSiteId == _site_id
                )/*
            .Select(s => 
                new product{
                    Serial = string.Format("{0}{1:000000000}", s.Product.SerialNumberCode, s.Content),
                    SerialNumberId = s.SerialNumberId,
                    ProductId = s.ProductId,
                    EuiId = s.EuiId,
                    Content = s.Content,
                    CreateDate = s.CreateDate,
                    UpdateDate = s.UpdateDate.Value,
                    TesterId = s.TesterId,
                    Tester = s.Tester.Name
                })*/;

            if (ser.Any())
                serials = ser.ToList<SerialNumber>();

            return serials;
        }

        protected void ButtonPreviousDay_Click(object sender, EventArgs e)
        {
            TimeSpan oneday = new TimeSpan(1, 0, 0, 0);
            DateTime fromdate = DateTime.Parse(TextBox_fromDate.Text).Subtract(oneday);
            DateTime todate = DateTime.Parse(TextBox_toDate.Text).Subtract(oneday);

            TextBox_fromDate.Text = fromdate.ToString();
            TextBox_toDate.Text = todate.ToString();

            updateData();
        }

        void updateData()
        {
            ProductsPanel.Controls.Clear();

            DateTime fromdate = DateTime.Parse(TextBox_fromDate.Text);
            DateTime todate = DateTime.Parse(TextBox_toDate.Text);

            var groups = getSerials(fromdate, todate).OrderBy(s => s.Content).GroupBy(s => s.ProductId);

            bool color_toggle = false;
            foreach (var group in groups)
            {
                int product_id = group.Key;
                string idstr = product_id.ToString();

                Panel panel = new Panel();
                if (color_toggle)
                    panel.BackColor = System.Drawing.Color.LightGoldenrodYellow;
                else
                    panel.BackColor = System.Drawing.Color.WhiteSmoke;
                color_toggle = !color_toggle;

                Button btn = new Button();
                btn.UseSubmitBehavior = false;
                btn.Text = "+";
                btn.ID = "Button_showdetails" + idstr;
                btn.Attributes.Add("product_id", idstr);
                btn.Click += Btn_Click;
                //panel.Controls.Add(btn);

                product_info product_info = new product_info();
                using (ManufacturingDataDataContext dc = new ManufacturingDataDataContext())
                {
                    product_info = dc.Products.Where(p => p.Id == product_id).Select(
                        p => new product_info
                        {
                            Name = p.Name,
                            Description = p.Description,
                            Model = p.ModelString
                        }).First();
                }

                Table table = new Table();
                table.CellSpacing = 10;
                TableRow row = new TableRow();

                TableCell cell = new TableCell();
                cell.Controls.Add(btn);
                row.Cells.Add(cell);

                Label label = new Label();
                label.Text = product_info.Model;
                cell = new TableCell();
                cell.BorderWidth = 1;
                cell.Controls.Add(label);
                row.Cells.Add(cell);

                label = new Label();
                label.Text = product_info.Description;
                cell = new TableCell();
                cell.BorderWidth = 1;
                cell.Controls.Add(label);
                row.Cells.Add(cell);

                table.Rows.Add(row);
                panel.Controls.Add(table);


                GridView gv = getGrid(group.ToList());
                gv.Visible = false;
                gv.BackColor = System.Drawing.Color.White;
                panel.Controls.Add(gv);

                ProductsPanel.Controls.Add(panel);


            }
        }

        protected void Btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string idstr = btn.Attributes["product_id"];
            if(btn.Text == "+")
                btn.Text = "-";
            else
                btn.Text = "+";


            Control c = FindControl("Gridview" + idstr);
            if (c != null)
                c.Visible = !c.Visible;
        }


        GridView getGrid(List<SerialNumber> list_serials)
        {
            int product_id = 0;
            if (list_serials.Count > 0)
                product_id = list_serials[0].ProductId;
            string idstr = product_id.ToString();


            GridView gv = new GridView();
            gv.ID = "Gridview" + idstr;
            gv.Attributes.Add("product_id", idstr);
            gv.DataSource = list_serials;
            gv.DataBound += Gv_DataBound;
            gv.DataBind();

            return gv;
        }

        private void Gv_DataBound(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;
            if (gv.Visible)
            {

            }
        }

    }
}