using PlumbingProps.CrossUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlumbingProps.Document
{
    public class WordHelper
    {
        public class CellTitles
        {
            public string Title { get; set; }

            public string Width { get; set; }

            private bool _Visible = true;

            public bool Visible
            {
                get { return _Visible; }
                set { _Visible = value; }
            }
        }

        private StringBuilder _data = new StringBuilder();

        public WordHelper(string PathFilePlantilla)
        {
            FileUtil file = new FileUtil();
            file.NameFile = PathFilePlantilla;
            _data = new StringBuilder(file.GetData());
        }

        public string GenerarDocumento(Object Source, Dictionary<string, CellTitles[]> pTitles, string PathTemp)
        {
            string vResul = CustomGuid.GetGuid();
            FileUtil file = new FileUtil();
            file.NameFile = PathTemp + "\\" + vResul + ".doc";

            try
            {
                Dictionary<string, Object> values = new Dictionary<string, object>();

                foreach (var item in Source.GetType().GetProperties())
                {
                    values.Add(item.Name, item.GetValue(Source, null));
                }

                file.createFile();

                foreach (var item in values)
                {
                    if (item.Value == null)
                    {
                        continue;
                    }
                    if (item.Value.GetType().FullName.Contains("List"))
                    {
                        CellTitles[] vCurrentTitles = pTitles.Keys.Contains(item.Key) ? pTitles[item.Key] : new CellTitles[0];
                        IList Elementos = (IList)item.Value;
                        string tabalHtml = WordParts.tagTable;
                        tabalHtml += WordParts.tagStyle;
                        tabalHtml += WordParts.tagGrid;
                        for (int i = 0; i < vCurrentTitles.Length; i++)
                        {
                            if (vCurrentTitles[i].Visible)
                            {
                                tabalHtml += String.Format(WordParts.tagGridColumn, vCurrentTitles[i].Width);
                            }
                        }
                        tabalHtml += WordParts.tagGridEnd;
                        tabalHtml += WordParts.tagGridRow;
                        for (int i = 0; i < vCurrentTitles.Length; i++)
                        {
                            if (vCurrentTitles[i].Visible)
                            {
                                tabalHtml += String.Format(WordParts.tagGridCellTitle, vCurrentTitles[i].Title);
                            }
                        }
                        tabalHtml += WordParts.tagGridRowEnd;
                        foreach (var itemElementos in Elementos)
                        {
                            tabalHtml += WordParts.tagGridRow;
                            //foreach (var itemProp in itemElementos.GetType().GetProperties())
                            for (int i = 0; i < vCurrentTitles.Length; i++)
                            {
                                if (vCurrentTitles[i].Visible)
                                {
                                    var itemProp = itemElementos.GetType().GetProperties()[i];
                                    tabalHtml += String.Format(WordParts.tagGridCellValue, Convert.ToString(itemProp.GetValue(itemElementos, null)));
                                }
                            }
                            tabalHtml += WordParts.tagGridRowEnd;
                        }

                        tabalHtml += WordParts.tagTableEnd;
                        _data.Replace(item.Key, tabalHtml);
                    }
                    else
                    {
                        _data.Replace("Obj." + item.Key, Convert.ToString(item.Value));
                    }
                }
                file.writeFile(_data.ToString());
                file.closeFile();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return file.NameFile.Replace(@"\\", @"\");
            // return vResul + ".doc";
        }
    }
}
