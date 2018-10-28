using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BGayet.GIA.Utils
{

    /// <summary>
    /// Classe d'aide pour travailler avec les fichiers Excel.
    /// </summary>
    public static class ExcelSheetHelper
    {

        /// <summary>
        /// Lecture d'un fichier Excel dans une DataTable.
        /// </summary>
        /// <param name="fileName">Chemin complet du fichier.</param>
        /// <param name="hasHeader">Indique si l'entête est présent dans le fichier.</param>
        public static DataTable ReadAsDataTable(string fileName, bool hasHeader = true)
        {
            DataTable dataTable = new DataTable();
            using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();

                int indexMax = rows.ToList()
                    .Max(row => row.Descendants<Cell>().ToList()
                    .Max(cell => CellReferenceToIndex(cell)));

                DataColumn[] columnsHeader = Enumerable.Repeat("Column", indexMax).ToList()
                   .Select((x, i) => new DataColumn(string.Concat(x, i + 1)))
                   .ToArray();

                if (hasHeader)
                {
                    var rowHeader = rows.ElementAt(0);
                    rowHeader.Descendants<Cell>().ToList()
                        .ForEach(cell => columnsHeader[CellReferenceToIndex(cell) - 1] = new DataColumn(GetCellValue(spreadSheetDocument, cell)));
                }

                dataTable.Columns.AddRange(columnsHeader);

                foreach (Row row in rows)
                {
                    DataRow dataRow = dataTable.NewRow();
                    row.Descendants<Cell>().ToList()
                        .ForEach(cell => dataRow[CellReferenceToIndex(cell) - 1] = GetCellValue(spreadSheetDocument, cell));
                    dataTable.Rows.Add(dataRow);
                }

                if (hasHeader)
                    dataTable.Rows.RemoveAt(0);
            }

            return dataTable;
        }

        /// <summary>
        /// Récupère la valeur d'une cellule Excel.
        /// </summary>
        /// <param name="document">Package de document Excel.</param>
        /// <param name="cell">Cellule à traiter.</param>
        private static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                return stringTablePart.SharedStringTable.ChildElements[int.Parse(value)].InnerText;
            else
                return value;
        }

        /// <summary>
        /// Permet de convertir la position de la cellule en lettre vers nombre (ex: AA -> 27).
        /// </summary>
        /// <param name="cell">Cellule à traiter.</param>
        private static int CellReferenceToIndex(Cell cell)
        {
            string reference = new string (cell.CellReference.ToString().ToUpper().Where(char.IsLetter).ToArray());
            return reference.Select((c, i) => ((c - 'A' + 1) * ((int)Math.Pow(26, reference.Length - i - 1)))).Sum();
        }
    }
}