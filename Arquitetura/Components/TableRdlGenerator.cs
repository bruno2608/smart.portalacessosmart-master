using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Arquitetura.Components
{
    public class TableRdlGenerator
    {
        private List<KeyValuePair<string, bool>> m_fields;

        public List<KeyValuePair<string, bool>> Fields
        {
            get { return m_fields; }
            set { m_fields = value; }
        }

        private List<KeyValuePair<string, bool>> m_headers;

        public List<KeyValuePair<string, bool>> Headers
        {
            get { return m_headers; }
            set { m_headers = value; }
        }

        public TableType CreateTable()
        {
            TableType table = new TableType();
            table.Name = "Table1";
            table.Items = new object[]
                {
                    CreateTableColumns(),
                    CreateHeader(),
                    CreateDetails(),
                };
            table.ItemsElementName = new ItemsChoiceType21[]
                {
                    ItemsChoiceType21.TableColumns,
                    ItemsChoiceType21.Header,
                    ItemsChoiceType21.Details,
                };
            return table;
        }

        private HeaderType CreateHeader()
        {
            HeaderType header = new HeaderType();
            header.Items = new object[]
                {
                    CreateHeaderTableRows(),
                };
            header.ItemsElementName = new ItemsChoiceType20[]
                {
                    ItemsChoiceType20.TableRows,
                };
            return header;
        }

        private TableRowsType CreateHeaderTableRows()
        {
            TableRowsType headerTableRows = new TableRowsType();
            headerTableRows.TableRow = new TableRowType[] { CreateHeaderTableRow() };
            return headerTableRows;
        }

        private TableRowType CreateHeaderTableRow()
        {
            TableRowType headerTableRow = new TableRowType();
            headerTableRow.Items = new object[] { CreateHeaderTableCells(), "0.25in" };
            return headerTableRow;
        }

        private TableCellsType CreateHeaderTableCells()
        {
            TableCellsType headerTableCells = new TableCellsType();
            headerTableCells.TableCell = new TableCellType[m_headers.Count];
            for (int i = 0; i < m_fields.Count; i++)
            {
                headerTableCells.TableCell[i] = CreateHeaderTableCell(m_headers[i].Key, i, m_headers[i].Value);
            }
            return headerTableCells;
        }

        private TableCellType CreateHeaderTableCell(string fieldName, int i, bool isNumeric)
        {
            TableCellType headerTableCell = new TableCellType();
            headerTableCell.Items = new object[] { CreateHeaderTableCellReportItems(fieldName, i, isNumeric) };
            return headerTableCell;
        }

        private ReportItemsType CreateHeaderTableCellReportItems(string fieldName, int i, bool isNumeric)
        {
            ReportItemsType headerTableCellReportItems = new ReportItemsType();
            headerTableCellReportItems.Items = new object[] { CreateHeaderTableCellTextbox(fieldName, i, isNumeric) };
            return headerTableCellReportItems;
        }

        private TextboxType CreateHeaderTableCellTextbox(string fieldName, int i, bool isNumeric)
        {
            TextboxType headerTableCellTextbox = new TextboxType();
            headerTableCellTextbox.Name = "hdr" + i.ToString() + "header";
            headerTableCellTextbox.Items = new object[]
                {
                    fieldName,
                    CreateHeaderTableCellTextboxStyle(isNumeric),
                    true,
                };
            headerTableCellTextbox.ItemsElementName = new ItemsChoiceType14[]
                {
                    ItemsChoiceType14.Value,
                    ItemsChoiceType14.Style,
                    ItemsChoiceType14.CanGrow,
                };
            return headerTableCellTextbox;
        }

        private StyleType CreateHeaderTableCellTextboxStyle(bool isNumeric)
        {
            StyleType headerTableCellTextboxStyle = new StyleType();
            headerTableCellTextboxStyle.Items = new object[]
                {
                    "700",
                    "14pt",
                    (isNumeric ? "Right" : "Left"),
                    "5pt",
                    "5pt",
                    "5pt",
                    "5pt",
                };
            headerTableCellTextboxStyle.ItemsElementName = new ItemsChoiceType5[]
                {
                    ItemsChoiceType5.FontWeight,
                    ItemsChoiceType5.FontSize,
                    ItemsChoiceType5.TextAlign,
                    ItemsChoiceType5.PaddingTop,
                    ItemsChoiceType5.PaddingBottom,
                    ItemsChoiceType5.PaddingLeft,
                    ItemsChoiceType5.PaddingRight,
                };
            return headerTableCellTextboxStyle;
        }

        private DetailsType CreateDetails()
        {
            DetailsType details = new DetailsType();
            details.Items = new object[] { CreateTableRows() };
            return details;
        }

        private TableRowsType CreateTableRows()
        {
            TableRowsType tableRows = new TableRowsType();
            tableRows.TableRow = new TableRowType[] { CreateTableRow() };
            return tableRows;
        }

        private TableRowType CreateTableRow()
        {
            TableRowType tableRow = new TableRowType();
            tableRow.Items = new object[] { CreateTableCells(), "0.25in" };
            return tableRow;
        }

        private TableCellsType CreateTableCells()
        {
            TableCellsType tableCells = new TableCellsType();
            tableCells.TableCell = new TableCellType[m_fields.Count];
            for (int i = 0; i < m_fields.Count; i++)
            {
                tableCells.TableCell[i] = CreateTableCell(m_fields[i].Key, m_fields[i].Value);
            }
            return tableCells;
        }

        private TableCellType CreateTableCell(string fieldName, bool isNumeric)
        {
            TableCellType tableCell = new TableCellType();
            tableCell.Items = new object[] { CreateTableCellReportItems(fieldName, isNumeric) };
            return tableCell;
        }

        private ReportItemsType CreateTableCellReportItems(string fieldName, bool isNumeric)
        {
            ReportItemsType reportItems = new ReportItemsType();
            reportItems.Items = new object[] { CreateTableCellTextbox(fieldName, isNumeric) };
            return reportItems;
        }

        private TextboxType CreateTableCellTextbox(string fieldName, bool isNumeric)
        {
            TextboxType textbox = new TextboxType();
            textbox.Name = fieldName;
            textbox.Items = new object[]
                {
                    "=Fields!" + fieldName + ".Value",
                    CreateTableCellTextboxStyle(isNumeric),
                    true,
                };
            textbox.ItemsElementName = new ItemsChoiceType14[]
                {
                    ItemsChoiceType14.Value,
                    ItemsChoiceType14.Style,
                    ItemsChoiceType14.CanGrow,
                };
            return textbox;
        }

        private StyleType CreateTableCellTextboxStyle(bool isNumeric)
        {
            StyleType style = new StyleType();
            style.Items = new object[]
                {
                    "500",
                    "10pt",
                    "=iif(RowNumber(Nothing) mod 2, \"#f9f9f9\", \"White\")",
                    (isNumeric ? "Right" : "Left"),
                    "5pt",
                    "5pt",
                    "5pt",
                    "5pt",
                };
            style.ItemsElementName = new ItemsChoiceType5[]
                {
                    ItemsChoiceType5.FontWeight,
                    ItemsChoiceType5.FontSize,
                    ItemsChoiceType5.BackgroundColor,
                    ItemsChoiceType5.TextAlign,
                    ItemsChoiceType5.PaddingTop,
                    ItemsChoiceType5.PaddingBottom,
                    ItemsChoiceType5.PaddingLeft,
                    ItemsChoiceType5.PaddingRight,
                };
            return style;
        }

        private TableColumnsType CreateTableColumns()
        {
            TableColumnsType tableColumns = new TableColumnsType();
            tableColumns.TableColumn = new TableColumnType[m_fields.Count];
            for (int i = 0; i < m_fields.Count; i++)
            {
                tableColumns.TableColumn[i] = CreateTableColumn();
            }
            return tableColumns;
        }

        private TableColumnType CreateTableColumn()
        {
            TableColumnType tableColumn = new TableColumnType();
            tableColumn.Items = new object[] { "2in" };
            return tableColumn;
        }
    }
}
