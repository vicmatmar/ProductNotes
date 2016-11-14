using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProductNotes
{
    class product_info
    {
        public string Name;
        public string Description;
        public string Model;
    }

    public partial class AddNote : System.Web.UI.Page
    {
        static int[] _sites_ids;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Find the last producted tested
                DateTime date = DateTime.Now;
                using (ManufacturingDataDataContext dc = new ManufacturingDataDataContext())
                {
                    _sites_ids = dc.ProductionSites.Where(p => p.Name == "Centralite" || p.Name == "Centralite - SMT").Select(p => p.Id).ToArray<int>();

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
            using (ManufacturingDataDataContext dc = new ManufacturingDataDataContext())
            {

                var ser =
                dc.SerialNumbers
                .Where(s =>
                    (s.CreateDate >= fromDate || s.UpdateDate.Value >= fromDate) &&
                    (s.CreateDate < toDate || s.UpdateDate.Value < toDate) &&
                    (s.EuiList.ProductionSiteId == _sites_ids[0] || s.EuiList.ProductionSiteId == _sites_ids[1])
                    );

                serials = ser.ToList<SerialNumber>();
            }

            return serials;
        }

        protected void ButtonPreviousDay_Click(object sender, EventArgs e)
        {
            DateTime fromdate = DateTime.Parse(TextBox_fromDate.Text).AddDays(-1);
            //DateTime todate = DateTime.Parse(TextBox_toDate.Text).AddDays(-1);
            DateTime todate = fromdate.AddDays(1);

            TextBox_fromDate.Text = fromdate.ToString();
            TextBox_toDate.Text = todate.ToString();

            updateData();
        }

        protected void ButtonNextDay_Click(object sender, EventArgs e)
        {
            DateTime fromdate = DateTime.Parse(TextBox_fromDate.Text).AddDays(+1);
            DateTime todate = fromdate.AddDays(1);

            TextBox_fromDate.Text = fromdate.ToString();
            TextBox_toDate.Text = todate.ToString();

            updateData();
        }

        void updateData()
        {
            ProductsPanel.Controls.Clear();

            DateTime fromdate = DateTime.Parse(TextBox_fromDate.Text);
            DateTime todate = DateTime.Parse(TextBox_toDate.Text);

            var groups = getSerials(fromdate, todate)
                .OrderBy(s => s.ProductId).
                ThenBy(s => s.Content).
                GroupBy(s => s.ProductId);

            bool color_toggle = false;
            foreach (var group in groups)
            {
                int product_id = group.Key;
                string idstr = product_id.ToString();

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

                Panel panel = new Panel();
                if (color_toggle)
                    panel.BackColor = System.Drawing.Color.LightGray;
                else
                    panel.BackColor = System.Drawing.Color.WhiteSmoke;
                color_toggle = !color_toggle;


                Table table = new Table();
                table.CellSpacing = 10;
                TableRow row = new TableRow();

                TableCell cell = new TableCell();

                Button btn = new Button();
                btn.UseSubmitBehavior = false;
                btn.Text = "+";
                btn.ID = "Button_" + idstr;
                btn.Attributes.Add("product_id", idstr);
                btn.Click += Btn_Click;

                Label label = new Label();
                label.Text = "[" + group.Count().ToString() + "]";

                cell.Controls.Add(btn);
                cell.Controls.Add(label);
                row.Cells.Add(cell);

                label = new Label();
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
                gv.AllowSorting = true;
                gv.HorizontalAlign = HorizontalAlign.Justify;

                gv.RowStyle.BackColor = System.Drawing.Color.LightBlue;
                gv.RowStyle.ForeColor = System.Drawing.Color.DarkBlue;

                gv.AlternatingRowStyle.BackColor = System.Drawing.Color.White;
                gv.AlternatingRowStyle.ForeColor = System.Drawing.Color.DarkBlue;

                //gv.AlternatingRowStyle.BorderColor = System.Drawing.Color.WhiteSmoke;

                panel.Controls.Add(gv);

                ProductsPanel.Controls.Add(panel);


            }
        }

        protected void Btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string idstr = btn.Attributes["product_id"];
            if (btn.Text == "+")
                btn.Text = "-";
            else
                btn.Text = "+";


            Control c = FindControl("Gridview" + idstr);
            if (c != null)
            {
                if (btn.Text == "+")
                    c.Visible = false;
                else
                    c.Visible = true;
            }
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