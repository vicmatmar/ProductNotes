using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProductNotes
{
    public partial class ProductNotes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Text_effectiveDate.Text = DateTime.Now.ToString();
                BindGrid();
            }
        }

        private void BindGrid()
        {
            using (ManufacturingDataDataContext dc = new ManufacturingDataDataContext())
            {
                GridView_ProductNotes.DataSource = dc.ProductNotes.OrderByDescending(n => n.EffectiveDate);
                GridView_ProductNotes.DataBind();
            }
        }

        protected void Insert(object sender, EventArgs e)
        {
            if (Text_note.Text == "")
                return;

            using (ManufacturingDataDataContext dc = new ManufacturingDataDataContext())
            {
                ProductNote note = new ProductNote()
                {
                    Note = Text_note.Text,
                    EffectiveDate = DateTime.Parse(Text_effectiveDate.Text)
                };

                dc.ProductNotes.InsertOnSubmit(note);
                dc.SubmitChanges();
            }

            BindGrid();
        }
        protected void GridView_ProductNotes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            /*
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView_ProductNotes.EditIndex)
            {
                (e.Row.Cells[2].Controls[2] as Button).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
            */
        }

        protected void GridView_ProductNotes_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView_ProductNotes.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void GridView_ProductNotes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView_ProductNotes.EditIndex = -1;
            BindGrid();
        }

        protected void GridView_ProductNotes_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GridView_ProductNotes.Rows[e.RowIndex];

            int noteId = Convert.ToInt32(GridView_ProductNotes.DataKeys[e.RowIndex].Values[0]);

            string note_str = (row.FindControl("Text_note") as TextBox).Text;
            string date_str = (row.FindControl("Text_effectiveDate") as TextBox).Text;
            using (ManufacturingDataDataContext dc = new ManufacturingDataDataContext())
            {
                ProductNote note = dc.ProductNotes.Where(n => n.Id == noteId).Single();
                note.Note = note_str;
                note.EffectiveDate = DateTime.Parse(date_str);

                dc.SubmitChanges();
            }
            GridView_ProductNotes.EditIndex = -1;
            BindGrid();
        }

        protected void GridView_ProductNotes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int noteId = Convert.ToInt32(GridView_ProductNotes.DataKeys[e.RowIndex].Values[0]);
            using (ManufacturingDataDataContext dc = new ManufacturingDataDataContext())
            {
                ProductNote note = dc.ProductNotes.Where(n => n.Id == noteId).Single();
                dc.ProductNotes.DeleteOnSubmit(note);
                dc.SubmitChanges();
            }
            BindGrid();
        }

        protected void Button_expandNote_Click(object sender, EventArgs e)
        {
            Button btn_expandNote = (Button)sender;
            DataControlFieldCell cell = (DataControlFieldCell)btn_expandNote.Parent;
            GridViewRow row = (GridViewRow)cell.Parent;
            Button btn_next_day = (Button)row.Cells[0].FindControl("Button_nextDate");

            Panel panel = (Panel)cell.FindControl("Panel_noteInfo");
            panel.Visible = !panel.Visible;


            if (panel.Visible)
            {
                btn_expandNote.Text = "-";
                btn_next_day.Enabled = true;
            }
            else
            {
                btn_expandNote.Text = "+";
                btn_next_day.Enabled = false;
                return;
            }


            Label lbl = (Label)row.Cells[0].FindControl("Label_effectiveDate");
            DateTime from_date = DateTime.Parse(lbl.Text);
            btn_next_day.Attributes.Add("date", from_date.ToString());
            updateNoteWithProduct(row, from_date);

        }

        protected void Button_nextDate_Click(object sender, EventArgs e)
        {
            Button btn_next_bay = (Button)sender;
            DataControlFieldCell cell = (DataControlFieldCell)btn_next_bay.Parent;
            GridViewRow row = (GridViewRow)cell.Parent;

            DateTime from_date = DateTime.Parse(btn_next_bay.Attributes["date"]);
            from_date = from_date.AddDays(1);
            btn_next_bay.Attributes["date"] = from_date.ToString();

            bool found = updateNoteWithProduct(row, from_date);
            if (!found)
                btn_next_bay.Enabled = false;

        }

        bool updateNoteWithProduct(GridViewRow grid_row, DateTime from_date)
        {
            bool found_records = false;
            DateTime to_date = from_date.Date.AddDays(1);

            Panel note_panel = (Panel)grid_row.Cells[1].FindControl("Panel_noteInfo");

            using (ManufacturingDataDataContext dc = new ManufacturingDataDataContext())
            {
                int id1 = dc.ProductionSites.Where(p => p.Name == "Centralite").Single().Id;
                int id2 = dc.ProductionSites.Where(p => p.Name == "Centralite - SMT").Single().Id;

                //note_panel.Controls.Clear();

                while (true)
                {

                    var q = dc.SerialNumbers.Where(s =>
                        (s.CreateDate >= from_date && s.CreateDate < to_date) &&
                        (s.EuiList.ProductionSiteId == id1 || s.EuiList.ProductionSiteId == id2)
                        );

                    if (q.Any())
                    {
                        found_records = true;

                        var groups = q
                            .Select(s => new product_table
                            {
                                Serial = string.Format("{0}{1:000000000}", s.Product.SerialNumberCode, s.Content),
                                Created = s.CreateDate,
                                Updated = s.UpdateDate,
                                Tester = s.Tester.Name,
                                Content = s.Content,
                                ProductId = s.ProductId,
                                EuiId = s.EuiId,
                                SerialNumberId = s.SerialNumberId

                            })
                          .OrderBy(s => s.ProductId)
                          .ThenBy(s => s.Content)
                          .GroupBy(s => s.ProductId);

                        foreach (var group in groups)
                        {

                            int product_id = group.Key;
                            string idstr = product_id.ToString();

                            product_info product_info = new product_info();
                            product_info = dc.Products
                                .Where(p => p.Id == product_id)
                                .Select(p => new product_info
                                {
                                    Description = p.Description,
                                    Model = p.ModelString
                                })
                                 .First();

                            Panel product_panel = new Panel();
                            product_panel.BorderWidth = 1;

                            Table table = new Table();
                            table.CellPadding = 5;
                            TableRow row = new TableRow();

                            TableCell cell = new TableCell();
                            cell.Width = 30;
                            cell.HorizontalAlign = HorizontalAlign.Right;
                            cell.BackColor = System.Drawing.Color.White;

                            Label label = new Label();
                            label.Text = group.Count().ToString();
                            cell.Controls.Add(label);
                            row.Cells.Add(cell);

                            label = new Label();
                            label.Text = product_info.Model;
                            cell = new TableCell();
                            cell.BorderWidth = 1;
                            cell.Width = 150;
                            cell.BackColor = System.Drawing.Color.White;
                            cell.HorizontalAlign = HorizontalAlign.Center;
                            cell.Controls.Add(label);
                            row.Cells.Add(cell);

                            label = new Label();
                            label.Text = product_info.Description;
                            cell = new TableCell();
                            cell.Controls.Add(label);
                            row.Cells.Add(cell);

                            table.Rows.Add(row);
                            product_panel.Controls.Add(table);

                            note_panel.Controls.Add(product_panel);

                            GridView gv = new GridView();
                            gv.DataSource = group.ToList();
                            gv.DataBind();

                            gv.RowStyle.BackColor = System.Drawing.Color.LightBlue;
                            gv.RowStyle.ForeColor = System.Drawing.Color.DarkBlue;

                            gv.AlternatingRowStyle.BackColor = System.Drawing.Color.White;
                            gv.AlternatingRowStyle.ForeColor = System.Drawing.Color.DarkBlue;

                            //gv.HorizontalAlign = HorizontalAlign.Center;
                            gv.Style.Add("text-align", "center");

                            note_panel.Controls.Add(gv);

                        }

                        break;
                    }

                    to_date = to_date.AddDays(1);
                    if (to_date > DateTime.Now)
                        break;
                }
            }

            return found_records;
        }

    }
}