using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using Avalonia.Controls;
using ReactiveUI;

namespace TestDataGridVirtualMasterDetail.Models
{
    public abstract class TreeViewItemModel: ReactiveUI.ReactiveObject, IDetailed, IColumned, IPictured, IAffected, INode
    {
        private string _detailsChar = OpenChar;

        public bool DetailsVisible { get; set; } = false;
        public void ShowHideDetails()
        {
            DetailsVisible = !DetailsVisible;
            DetailsChar = DetailsVisible ? CloseChar : OpenChar;
            ShowHideViewDetails();
        }

        private static string CloseChar { get; set; } = "⇱";
        private static string OpenChar { get; set; } = "⇊";

        public string DetailsChar
        {
            get => _detailsChar;
            set => this.RaiseAndSetIfChanged(ref _detailsChar, value, nameof(DetailsChar));
        }
        public DataGridRow? BindedRow { get; set; } = null;
        public bool AlreadyFixed { get; set; }
        public double? OldHeight { get; set; }
        public void HideDetails()
        {
            DetailsVisible = false;
            DetailsChar = OpenChar;
        }

        private void ShowHideViewDetails()
        {
            if (BindedRow != null) BindedRow.AreDetailsVisible = DetailsVisible;
        }


        public int IconsColumnCount()
        {
            throw new System.NotImplementedException();
        }

        public abstract ObservableCollection<ColoredImage> Icons();
        public abstract INode GetParent();
        public abstract ObservableCollection<INode> GetChildren();
        public abstract Color GetColor();
        public abstract int ColumnsCount();
        public abstract ObservableCollection<string> ColumnValues();
        public abstract string ColumnValue(int column);

        private ColumnsMeta? _columnsMeta;
        public ref ColumnsMeta? ColumnsMeta()
        {
            return ref _columnsMeta;
        }

        private void SetColumnsMeta(ref ColumnsMeta columnsMeta)
        {
            _columnsMeta = columnsMeta;
        }

        public ObservableCollection<EventModel> Affects { get; set; }
        public MemorySafer<string> AffectColorMemory { get; set; }
        public string GetState()
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IColumned
    {
        string ColumnValue(int column);
        ref ColumnsMeta? ColumnsMeta();
    }

    public interface IPictured
    {
        int IconsColumnCount();
        ObservableCollection<ColoredImage> Icons();
    }

    public interface IAffected
    {
        ObservableCollection<EventModel> Affects
        {
            get; set;
        }

        MemorySafer<string> AffectColorMemory { get; set; }

        string GetState();
    }
    

    public interface INode
    {
        INode GetParent();
        ObservableCollection<INode> GetChildren();
        Color GetColor();
    }

    public class ColumnsMeta
    {
        public ColumnsMeta(List<string> columnNames)
        {
            ColumnNames = columnNames;
        }

        public int ColumnCount => ColumnNames.Count;
        private List<string> ColumnNames { get;}
        public List<string> GetColumnNames() => ColumnNames;
    }

    public class ColoredImage
    {
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public string? Tooltip { get; set; }
    }
    
    
    public interface IDetailed
    {
        bool DetailsVisible { get; set; }
        DataGridRow? BindedRow { get; set; }
        bool AlreadyFixed { get; set; }
        double? OldHeight { get; set; }
        public void HideDetails();
    }

    public interface ITreeViewConfiguration
    {
        
    }
}
