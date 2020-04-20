using System.Collections.Generic;
using System.Linq;

namespace CodeMagic.Core.Saving
{
    public class GridSaveable : ISaveable
    {
        private const string SaveKeyRows = "Rows";

        public GridSaveable(SaveData data)
        {
            var rows = data.GetObjectsCollection<GridRowSaveable>(SaveKeyRows);
            Rows = rows.Select(row => row.Cells).ToArray();
        }

        public GridSaveable(object[][] rows)
        {
            Rows = rows;
        }

        public object[][] Rows { get; }

        public SaveDataBuilder GetSaveData()
        {
            var rows = Rows.Select(row => new GridRowSaveable(row)).ToArray();
            return new SaveDataBuilder(GetType(), new Dictionary<string, object>
            {
                {SaveKeyRows, rows}
            });
        }

        public class GridRowSaveable : ISaveable
        {
            private const string SaveKeyCells = "Cells";

            public GridRowSaveable(SaveData data)
            {
                Cells = data.GetObjectsCollection(SaveKeyCells);
            }

            public GridRowSaveable(object[] cells)
            {
                Cells = cells;
            }

            public object[] Cells { get; }

            public SaveDataBuilder GetSaveData()
            {
                return new SaveDataBuilder(GetType(), new Dictionary<string, object>
                {
                    {SaveKeyCells, Cells}
                });
            }
        }
    }
}