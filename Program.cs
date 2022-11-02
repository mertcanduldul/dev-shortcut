using ClosedXML.Excel; //!!İmportant Nuget Package


//Excel => DataTable
public DataTable ExcelToDataTable(string path)
{
    try
    {
        DataTable dt = new DataTable();
        using (IXLWorkbook workBook = new XLWorkbook(path))
        {
            IXLWorksheet workSheet = workBook.Worksheet(1);
            bool isAddedColumn = false;
            foreach (IXLRow row in workSheet.Rows())
            {
                if (!isAddedColumn)
                {
                    foreach (IXLCell cell in row.Cells())
                    {
                        dt.Columns.Add(cell.Value.ToString());
                    }
                    isAddedColumn = true;
                }
                else
                {
                    dt.Rows.Add();
                    int i = 0;
                    foreach (IXLCell cell in row.Cells())
                    {
                        dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                        i++;
                    }
                }
            }
        }
        return dt;
    }
    catch (Exception ex)
    {
        throw ex;
    }
}

//DataTable => List<OneEntitiy>
public List<Entitiy> DataTableToSpesificEntityList(DataTable dt)
{
    var list = (from row in dt.AsEnumerable()
                select new Entitiy
                {
                    Id = row.Field<int>("Id"),
                    Name = row.Field<string>("Name"),
                    Age = row.Field<int>("Age"),
                    Address = row.Field<string>("Address")
                }).ToList();
    return list;
}

//DataTable => List<T>
public List<T> ListToDataTable(DataTable dt)
{
    var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToList();
    var properties = typeof(T).GetProperties();

    return dt.AsEnumerable().Select(row =>
    {
        var objT = Activator.CreateInstance<T>();
        foreach (var prop in properties)
        {
            if (columnNames.Contains(prop.Name))
            {
                if (!row.IsNull(prop.Name))
                {
                    prop.SetValue(objT, row[prop.Name], null);
                }
            }
        }
        return objT;
    }).ToList();
}

//List<T> => DataTable
public DataTable ListToDataTable(List<T> list)
{
    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
    DataTable table = new DataTable();
    foreach (PropertyDescriptor prop in properties)
        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
    foreach (T item in list)
    {
        DataRow row = table.NewRow();
        foreach (PropertyDescriptor prop in properties)
            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
        table.Rows.Add(row);
    }
    return table;
}

//DataTable => Excel
public void DataTableToExcel(DataTable dt)
{
    using (XLWorkbook wb = new XLWorkbook())
    {
        wb.Worksheets.Add(dt, "Sheet1");
        wb.SaveAs("C:\\Users\\user\\Desktop\\test.xlsx");
    }
}