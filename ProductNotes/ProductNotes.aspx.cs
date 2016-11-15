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
                BindGrid();
            }
        }

        private void BindGrid()
        {
            using (ManufacturingDataDataContext dc = new ManufacturingDataDataContext())
            {
                GridView_ProductNotes.DataSource = dc.ProductNotes;
                GridView_ProductNotes.DataBind();
            }
        }

        protected void Insert(object sender, EventArgs e)
        {
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

            this.BindGrid();
        }
        protected void GridView_ProductNotes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GridView_ProductNotes.EditIndex)
            {
                (e.Row.Cells[2].Controls[2] as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
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
            string date = (row.FindControl("Text_effectiveDate") as TextBox).Text;
            using (ManufacturingDataDataContext dc = new ManufacturingDataDataContext())
            {
            }
            GridView_ProductNotes.EditIndex = -1;
            BindGrid();
        }

        protected void GridView_ProductNotes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        int id = Convert.ToInt32(GridView_ProductNotes.DataKeys[e.RowIndex].Values[0]);
        using (ManufacturingDataDataContext ctx = new ManufacturingDataDataContext())
        {
        }
        this.BindGrid();
    }
    }
}